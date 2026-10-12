using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLDDataAccess;

namespace DVLDBusinessLogic2
{
    public class clsInternationalLicense
    {
        public static bool IssueForLocalLicense(int localLicenseID, int userID, out int applicationID, out int internationalID)
        {
            var result = LicenseWorkflowData.IssueInternational(localLicenseID, userID);
            applicationID = result == null ? -1 : result.ApplicationID;
            internationalID = result == null ? -1 : result.LicenseID;
            return result != null;
        }

        public int internationalLicenseID { get; private set; }
        public int applicationID {  get; set; }
        public int DriverID {  get; set; }
        public int localLicenseID {  get; set; }
        public DateTime Issuedate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool isActive {  get; set; }
        public int createdBy {  get; set; }


        public bool AddNewInternationalLicense()
        {
            int interID = -1;

            if(ManageInternationalLicenseData.saveNewInternationalLicense(ref interID,applicationID,DriverID,localLicenseID,Issuedate,ExpirationDate,isActive
                , createdBy))
            {
                internationalLicenseID = interID;

            }
            return interID != -1;

        }

        public static int isthatLicenseALreayHasInternationalLicense(int localLicenseID)
        {
            return ManageInternationalLicenseData.isthatLicenseHasAlreadyInternationalLicenseAndNotExpired(localLicenseID);
        }

        public static DataTable InternationalLicensesTable()
        {
            return ManageInternationalLicenseData.internationalLicensesTable();
        }

       public static int getPersonID(int internationalID)
        {
            return ManageInternationalLicenseData.getPersonID(internationalID);
        }

        public static bool isThatDriverHasInternationalLicenseAndNotExpired(int driverID)
        {
            return ManageInternationalLicenseData.checkIfPersonHaveINTLicenseAndNotExpired(driverID);
        }
    }

    public static class internationalLicenseDTO
    {
        public static string Name { get; private set; }
        public static int IntLicenseID { get; private set; }
        public static int LicensesID { get; private set; }
        public static string nationaNo { get; private set; }
        public static string gender { get; private set; }
        public static DateTime IssueDate { get; private set; }
        public static DateTime ExpirationDate { get;private set; }
        public static int ApplicationID { get; private set; }
        public static int DriverID { get;private set; }
        public static string IsActive {  get; private set; }
        public static DateTime dateOfBirth {  get; private set; }

        public static string imagePath { get; private set; }
        public static bool findINTDriverLicenseInfo(int INTLicenseID)
        {
            string Name = string.Empty;
            int LicensesID = -1;
            string nationaNo = string.Empty;
            string gender =string.Empty;
            DateTime IssueDate = DateTime.MinValue;
            DateTime ExpirationDate=DateTime.MinValue;
            int ApplicationID = -1;
            int DriverID = -1;
            string IsActive = string.Empty;
            DateTime dateOfBirth = DateTime.MinValue;
            string imagePath = string.Empty;

            if(ManageInternationalLicenseData.findInternationalDriverLicenseInfo(INTLicenseID,ref Name,ref LicensesID,ref nationaNo,ref gender,ref IssueDate
                ,ref ApplicationID,ref IsActive,ref dateOfBirth,ref DriverID,ref ExpirationDate,ref imagePath))
            {
                IntLicenseID = INTLicenseID;
                internationalLicenseDTO.nationaNo = nationaNo;
                internationalLicenseDTO.Name = Name;
                internationalLicenseDTO.gender = gender;
                internationalLicenseDTO.dateOfBirth = dateOfBirth;
                internationalLicenseDTO.DriverID = DriverID;
                internationalLicenseDTO.ApplicationID = ApplicationID;
                internationalLicenseDTO.ExpirationDate = ExpirationDate;
                internationalLicenseDTO.IssueDate = IssueDate;
                internationalLicenseDTO.IsActive = IsActive;
                internationalLicenseDTO.LicensesID = LicensesID;
                internationalLicenseDTO.imagePath = imagePath;

                return true;

            }
            return false;

        }
    }
}
