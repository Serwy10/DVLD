using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLDDataAccess;

namespace DVLDBusinessLogic2
{
    enum _Mode { AddNew=0,Update=1}

    public class ClsPerson
    {
        _Mode mode = _Mode.AddNew;
        public int PersonID { get; private set; }
        public string NationalNum { set; get; }
        public string FirstName { set; get; }
        public string SecondName { set; get; }
        public string ThirdName { set; get; }
        public string LastName { set; get; }
        public DateTime DateOfBirth { set; get; }
        public string Gendor { set; get; }
        public string Nationality { set; get; }
        public string Address { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }
        public string ImagePath { set; get; }

        public string FullName()
        {
            return FirstName + ' ' + LastName;
        }

        public ClsPerson()
        {
            NationalNum = string.Empty;
            FirstName = string.Empty;
            SecondName = string.Empty;
            ThirdName = string.Empty;
            LastName = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
            ImagePath = string.Empty;
            Address = string.Empty;
            DateOfBirth = DateTime.MinValue;
            Gendor = string.Empty;
            Nationality = string.Empty;

            mode=_Mode.AddNew;

        }

        private ClsPerson(int personid, string nationalno, string firstname, string secondname, string thirdname, string lastname,
            string phone, string email, string imagepath, string address, DateTime dateofbirth, string gendor, string nationality)
        {
            this.PersonID = personid;
            this.NationalNum = nationalno;
            this.FirstName = firstname;
            this.SecondName = secondname;
            this.ThirdName = thirdname;
            this.LastName = lastname;
            this.Phone = phone;
            this.Email = email;
            this.ImagePath = imagepath;
            this.Address = address;
            this.DateOfBirth = dateofbirth;
            this.Gendor = gendor;
            this.Nationality = nationality;

            mode = _Mode.Update;

        }

        public static ClsPerson Find(int personID)
        {
            string NationalN = string.Empty;
            string FirstName = string.Empty;
            string SecondNam = string.Empty;
            string ThirdName = string.Empty;
            string LastName = string.Empty;
            DateTime DateOfBirth = DateTime.MinValue;
            string Gendor = string.Empty;
            string Nationality = string.Empty;
            string Address = string.Empty;
            string Phone = string.Empty;
            string Email = string.Empty;
            string ImagePath = string.Empty;

            if (PersonsData.getPersonByID(personID, ref NationalN, ref FirstName, ref SecondNam, ref ThirdName, ref LastName, ref Phone
                , ref Email, ref ImagePath, ref Address, ref DateOfBirth, ref Gendor, ref Nationality))

                return new ClsPerson(personID, NationalN, FirstName, SecondNam, ThirdName, LastName, Phone, Email, ImagePath, Address, DateOfBirth
                    , Gendor, Nationality);
            else
                return null;


        }

        public static ClsPerson Find(string NationalNO) {

            int personID = -1;
            string FirstName = string.Empty;
            string SecondNam = string.Empty;
            string ThirdName = string.Empty;
            string LastName = string.Empty;
            DateTime DateOfBirth = DateTime.MinValue;
            string Gendor = string.Empty;
            string Nationality = string.Empty;
            string Address = string.Empty;
            string Phone = string.Empty;
            string Email = string.Empty;
            string ImagePath = string.Empty;

            if (PersonsData.getPersonByNationalNo(NationalNO, ref personID, ref FirstName, ref SecondNam, ref ThirdName, ref LastName, ref Phone
                , ref Email, ref ImagePath, ref Address, ref DateOfBirth, ref Gendor, ref Nationality))

                return new ClsPerson(personID, NationalNO, FirstName, SecondNam, ThirdName, LastName, Phone, Email, ImagePath, Address, DateOfBirth
                    , Gendor, Nationality);
            else
                return null;
        }


        public static DataView PeopleList()
        {

            return (PersonsData.PeopleList()).DefaultView;
        }

        public static DataTable CountriesList()
        {
            return PersonsData.CountriesList();
        }

        private bool _AddNewPerson()
        {
            int id = -1;
            if (PersonsData.AddNewPerson(ref id, this.NationalNum, this.FirstName, this.SecondName, this.ThirdName, this.LastName
                , this.DateOfBirth, this.Nationality, this.Gendor, this.Phone, this.Email, this.Address, this.ImagePath)) {
                this.PersonID = id;
                return true;
            }
            else
                return false;
        }

        private bool _updatePerson()
        {
            return (PersonsData.updatePerson(this.PersonID, this.NationalNum, this.FirstName, this.SecondName, this.ThirdName, this.LastName,
                this.DateOfBirth, this.Nationality, this.Gendor, this.Phone, this.Email, this.Address, this.ImagePath));
        }

        public bool Delete()
        {
            return(PersonsData.DeletePerson(this.PersonID));
        }

        public bool checkIfPersonExistedByID(int personID)
        {
            return (PersonsData.checkIfPersonExistedByID(personID));
        }

        public bool checkIfPersonExistedByNayionalNO(string nationalNO)
        {
            return (PersonsData.checkIfPersonExistedByNationalNO(nationalNO));
        }

        public bool Save()
        {
            switch (mode)
            {
                case _Mode.AddNew:
                    if (!_AddNewPerson()) return false;
                    mode = _Mode.Update;
                    return true;
                    case _Mode.Update:
                    return _updatePerson();

            }
            return false;
        }

       public static DataView PeopleTableViewFilteredBy(string filter)
        {
            DataView dv = new DataView(PersonsData.PeopleList());
           dv.RowFilter = filter;

            return dv;
        }

        public static bool checkIfNationalNoExisted(string nationalNO) {

            return PersonsData.checkIfPersonExistedByNationalNO(nationalNO);
        }

        public static int getPersonID(string nationalNO)
        {
            return PersonsData.getPersonID(nationalNO);
        }

    }

}
