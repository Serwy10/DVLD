using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLDDataAccess;

namespace DVLDBusinessLogic2
{
    public class clsUser
    {
        enum enMode { Addnew=0, Update=1}
        public int userID { get; private set; }
        public int personID { get; set; }
        public string username { get; set; }

        public string password { get; set; }
        public bool IsActive {  get; set; }
        enMode _mode {  get; set; }= enMode.Addnew;

        public static DataTable UsersTable()
        {
            return UsersAccessData.UsersTable();
        }

        private clsUser(int userID,int personID,string userName,bool isActive)
        {
            this.userID = userID;
            this.personID = personID;
            this.username = userName;
            this.IsActive = isActive;
            _mode = enMode.Update;
        }

        private clsUser(int userID,string UserName,string password,bool isActive)
        {
            this.userID = userID;
            this.username=UserName;
            this.password = password;
            this.IsActive = isActive;
            _mode = enMode.Update;
        }

        public clsUser()
        {
            this.userID = -1;
            this.personID = -1;
            this.username = string.Empty;
            this.IsActive=false;
            _mode = enMode.Addnew;
        }

        public static bool isValidLogin(string username,string password)
        {
            return UsersAccessData.isValidLogin(username, password);
        }

        public static bool isUserActive(string username)
        {
            return UsersAccessData.isUserActive(username);
        }

        public static clsUser Find(string username)
        {
            bool isActive=false;
            int userID = -1;
            int personID = -1;

            if (UsersAccessData.Find(username, ref userID, ref personID, ref isActive))
                return new clsUser(userID, personID, username, isActive);
            else
                return null;

        }

        public static clsUser Find(int UserID)
        {
            string userName = string.Empty;
            string Password=string.Empty;
            bool isActive=false;

            if(UsersAccessData.Find(UserID,ref userName,ref Password,ref isActive))
                return new clsUser(UserID,userName,Password,isActive);
            else
                return null;
        }

        public static clsUser FindbyPersonID(int personID)
        {
            int userID = -1;
            string userName = string.Empty;
            bool isactive=false;

            if (UsersAccessData.Find(personID, ref userID, ref userName, ref isactive))
                return new clsUser(userID, personID, userName, isactive);
            else
                return null;

        }

        public static bool saveNewPassword(string userNmae,string Password)
        {
            return UsersAccessData.saveNewPassword(userNmae, Password);
        }

        public static DataView viewFilterdBy(string category,string filter)
        {
            return TableFilter.Number(UsersTable(), category, filter);
        }

        public static DataView viewFilteredByStrings(string category, string filter) {
            return TableFilter.Text(UsersTable(), category, filter);
        }

        public static bool IsUserNameExsted(string username)
        {
            return UsersAccessData.checkIfUserNameExisted(username);
        }

        private bool _AddNewUser()
        {
            int ID = -1;
            UsersAccessData.AddNewUser(ref ID, this.personID, this.username, this.password, this.IsActive);
            this.userID = ID;
            return(userID!=-1);
        }

        private bool _UpdateUser()
        {
            return UsersAccessData.UpdateUser(this.userID,this.username,this.password,this.IsActive);
        }

        public bool Save()
        {
            switch (_mode)
            {
                case enMode.Addnew:
                    if (!_AddNewUser()) return false;
                    _mode = enMode.Update;
                    return true;

                    case enMode.Update:
                    return _UpdateUser();

                default:
                    return false;
            }

        }

        public static bool isThisPersonHasUserAcc(int personID)
        {
            return UsersAccessData.checkIfthatPersonHasUserAccount(personID);
        }

        public static int getPersonIdOfUser(int userID) {

            return UsersAccessData.getPersonIDOfUser(userID);
        }

        public static bool Delete(int UserID)
        {
            return UsersAccessData.DeleteUser(UserID);
        }

    }




    public static class CurrentUser
    {
        public static int userID { get; set;}
        public static string username { get; set;}
        public static int PersonID {  get; set;}
        public static bool isActive { get; set;}
    }
}
