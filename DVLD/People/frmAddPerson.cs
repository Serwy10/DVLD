using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Schema;
using DVLDBusinessLogic2;


namespace DVLD
{
    public partial class frmAddPerson : Form
    {
        public delegate void SendDataBack(int id);
        public event SendDataBack _DataBack;

        public delegate void PersonRefreshed();
        public event PersonRefreshed _Refresh;

        public delegate void delLoadPeopleTable();
        public event delLoadPeopleTable delLoadPeople;
        enum _enMode { Addnew=1,Update=2}
        private _enMode mode = _enMode.Addnew;
        string destenation = AppPaths.PhotosDirectory;
        private int _personID = -1;
        public frmAddPerson(int personID=-1)
        {
            InitializeComponent();
            dateTimePicker1.MaxDate = DateTime.Today.AddYears(-18);
            _personID = personID;
        }
        private string gender()
        {
            if (rbMale.Checked)
                return "Male";
            else
                return "Female";
        }
        private void textBox_Validating(object sender, CancelEventArgs e)
        {
            if (((TextBox)sender).Text == string.Empty)
            {
                e.Cancel = true;
                errorProvider1.SetError((TextBox)sender, "please enter data");
            }

            else
            {
                e.Cancel = false;
                errorProvider1.SetError((TextBox)sender, "");

            }
        }

        private void tbNationalNO_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbNationalNO.Text) || (ClsPerson.checkIfNationalNoExisted(tbNationalNO.Text) && ClsPerson.Find(tbNationalNO.Text)?.PersonID != _personID))
            {
                e.Cancel = true;
                errorProvider1.SetError(tbNationalNO, "this number existed");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(tbNationalNO, "");

            }
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMale.Checked && !linkLabelRemove.Visible)
                ImagePicBox.ImageLocation = AppPaths.Asset("male.png");
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFemale.Checked && !linkLabelRemove.Visible)
                ImagePicBox.ImageLocation = AppPaths.Asset("male-and-female.png");
        }

        private bool isValidMail(string newmail)
        {
            MailAddress mail = null;
            try
            {
                mail = new MailAddress(newmail);
            }
            catch
            {
                return false;
            }
            return mail != null;
        }

        private void tbEmail_Validating(object sender, CancelEventArgs e)
        {
            if (tbEmail.Text == string.Empty)
                return;

            else if (!isValidMail(tbEmail.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(tbEmail, "enter valid Mail");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(tbEmail, "");
            }
        }

        private void frmAddPerson_Load(object sender, EventArgs e)
        {
            foreach (DataRow country in ClsPerson.CountriesList().Rows)
            {
                cbCountries.Items.Add(country["countryname"]);
            }
            cbCountries.SelectedItem = "Egypt";
            ImagePicBox.ImageLocation = AppPaths.Asset("male.png");

            if (whichMode() == _enMode.Update)
            {
                lbTitle.Text = "Update Person";
                loadPersonDataToControls(ClsPerson.Find(_personID));
            }


        }

        private _enMode whichMode()
        {
            if (_personID == -1)
          mode = _enMode.Addnew;

            else
                mode= _enMode.Update;

            return mode;
        }

        private void linkLabelSet_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Title = "Choose image";
                dialog.Filter = "Images|*.png;*.jpg;*.jpeg;*.bmp";
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                using (var image = Image.FromFile(dialog.FileName)) { }
                ImagePicBox.ImageLocation = dialog.FileName;
                linkLabelRemove.Visible = true;
            }
        }

        private void linkLabelRemove_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (rbMale.Checked)
                ImagePicBox.ImageLocation = AppPaths.Asset("male.png");

            else
                ImagePicBox.ImageLocation = AppPaths.Asset("male-and-female.png");

            linkLabelRemove.Visible = false;
        }

        private bool checkValidData()
        {
            return (tbFirst.Text != "" && tbLast.Text != string.Empty && tbSecond.Text != string.Empty && tbPhone.Text != string.Empty
                 && tbAddress.Text != string.Empty && tbNationalNO.Text != string.Empty);
        }

        private void savePersonImageToTheFile(string source,string dest)
        {
            File.Copy(source,dest,true);
        }

        private void addNewPerson()
        {
            ClsPerson person = new ClsPerson();
            if (saveNewData(person)) {

            if (person.Save())
            {

                ImagePicBox.ImageLocation = string.IsNullOrEmpty(person.ImagePath) ? AppPaths.Asset("male.png") : person.ImagePath;
                lbPersonID.Text = person.PersonID.ToString();
                mode = _enMode.Update;
                _personID=person.PersonID;
                lbTitle.Text = "Update Person";
                MessageBox.Show("Person Added Successfully");
            }
            else
                MessageBox.Show("Something Wrong ! Person not Saved");
            }
        }
        private void _updatePerson()
        {

            ClsPerson person=ClsPerson.Find(_personID);
            string oldPhoto = person?.ImagePath;
            if (person != null)
                if (saveNewData(person))
                {

                    if (person.Save())
                    {
                        ImagePicBox.ImageLocation = string.IsNullOrEmpty(person.ImagePath) ? AppPaths.Asset("male.png") : person.ImagePath;
                        if (!string.IsNullOrEmpty(oldPhoto) && oldPhoto != person.ImagePath &&
                            Path.GetFullPath(oldPhoto).StartsWith(Path.GetFullPath(AppPaths.PhotosDirectory) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                        {
                            try { File.Delete(oldPhoto); } catch (IOException) { } catch (UnauthorizedAccessException) { }
                        }
                        MessageBox.Show("Person information updated successfully.");
                    }
                    else
                    {
                        MessageBox.Show("SomeThing Went Wrong And Nothing Updated :(");
                    }
                }
        }

        private bool saveNewData(ClsPerson person)
        {
            string destinationPath = string.Empty;
            if (!checkValidData() || !ValidateChildren())
            {
                foreach (Control ctrl in panel1.Controls)
                {

                    if (ctrl as TextBox != null && ctrl.Text == string.Empty)
                        errorProvider1.SetError(ctrl, "enter valid data");

                }
                return false;
            }

            person.NationalNum = tbNationalNO.Text;
            person.FirstName = tbFirst.Text;
            person.SecondName = tbSecond.Text;
            person.ThirdName = tbThird.Text;
            person.LastName = tbLast.Text;
            person.Phone = tbPhone.Text;
            person.Address = tbAddress.Text;
            person.Email = tbEmail.Text;
            person.Gendor = gender();
            person.DateOfBirth = dateTimePicker1.Value;
            person.Nationality = cbCountries.SelectedItem.ToString();

            string selected = ImagePicBox.ImageLocation;
            if (selected == AppPaths.Asset("male.png") || selected == AppPaths.Asset("male-and-female.png") || string.IsNullOrEmpty(selected))
                person.ImagePath = string.Empty;
            else if (!string.Equals(selected, person.ImagePath, StringComparison.OrdinalIgnoreCase))
            {
                Directory.CreateDirectory(destenation);
                destinationPath = Path.Combine(destenation, Guid.NewGuid().ToString("N") + Path.GetExtension(selected));
                savePersonImageToTheFile(selected, destinationPath);
                person.ImagePath = destinationPath;
            }
            return true;
        }

        private void loadPersonDataToControls(ClsPerson person)
        {

            if (person != null)
            {
                lbPersonID.Text=person.PersonID.ToString();
                tbFirst.Text = person.FirstName;
                tbSecond.Text = person.SecondName;
                tbThird.Text = person.ThirdName;
                tbLast.Text = person.LastName;
                tbAddress.Text = person.Address;
                tbPhone.Text = person.Phone;
                tbEmail.Text = person.Email;
                dateTimePicker1.Value = person.DateOfBirth;

                if(person.Gendor.ToLower()=="male")
                    rbMale.Checked = true;
                else
                    rbFemale.Checked =true;

                tbNationalNO.Text = person.NationalNum;
                cbCountries.SelectedItem=person.Nationality;

                if (person.ImagePath != string.Empty)
                {
                    ImagePicBox.ImageLocation = person.ImagePath;
                    linkLabelRemove.Visible=true;
                }

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            switch (mode) {
                case _enMode.Addnew:
            addNewPerson();
                    break;
                case _enMode.Update:
                    _updatePerson();
                    break;
        }
            delLoadPeople?.Invoke();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (int.TryParse(lbPersonID.Text,out int personID))
                _DataBack?.Invoke(Convert.ToInt32(personID));

            _Refresh?.Invoke();

                this.Close();
        }
    }
}
