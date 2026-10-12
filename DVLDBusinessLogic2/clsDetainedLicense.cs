using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLDDataAccess;

namespace DVLDBusinessLogic2
{
    public class clsDetainedLicense
    {
        private clsDetainedLicense(int detainID, int licenseID, DateTime detainDate, decimal fineFees, int createdByUserID, bool isRealesed
            , DateTime releasedDate
            , int releasedByUserID, int releasedApplicationID)
        {
            this.detainID = detainID;
            this.licenseID = licenseID;
            this.isRealesed=isRealesed;
            this.releasedDate = releasedDate;
            this.fineFees = fineFees;
            this.createdByUserID = createdByUserID;
            this.detainDate = detainDate;
            this.releasedApplicationID = releasedApplicationID;
            this.releasedByUserID = releasedByUserID;


        }
        public static bool ReleaseLicense(int licenseID, int userID, out int applicationID)
        {
            var result = LicenseWorkflowData.Release(licenseID, userID);
            applicationID = result == null ? -1 : result.ApplicationID;
            return result != null;
        }

        public int detainID {  get; private set; }
        public int licenseID { get; set; }

        public DateTime detainDate { get; set; }

        public decimal fineFees { get; set; }

        public int createdByUserID {  get; set; }

        public bool isRealesed { get; set; }

        public DateTime releasedDate {  get; set; }

        public int releasedByUserID { get; set; }

        public int releasedApplicationID {  get; set; }

        public clsDetainedLicense() { }
        public static bool isLicenseDetained(int licenseID)
        {
            return manageDetainLicenseData.isLicenseDetained(licenseID);
        }

        public bool saveNewDetainLicense()
        {
            int ID = -1;

            if(manageDetainLicenseData.saveNewDetainedLicense(ref ID, licenseID, detainDate, fineFees, createdByUserID))
                detainID = ID;

            return ID != -1;
        }



        public static clsDetainedLicense findDetainedLicense(int licenseID)
        {
            int detainID = -1; DateTime detainDate = DateTime.MinValue; decimal fineFees = decimal.MinValue;
            int createdByUserID = -1; bool isRealesed = false; DateTime releasedDate= DateTime.MinValue;
            int releasedByUserID = -1; int releasedApplicationID = -1;

            if(manageDetainLicenseData.findDetainedLicense(licenseID,ref detainID,ref detainDate,ref fineFees,ref createdByUserID,ref isRealesed,
                ref releasedDate,ref releasedByUserID
                ,ref releasedApplicationID))

                return new clsDetainedLicense(detainID,licenseID, detainDate, fineFees,createdByUserID, isRealesed,releasedDate,releasedByUserID
                    ,releasedApplicationID);
            return null;


        }

        public bool release()
        {
            if(manageDetainLicenseData.releaseLicense(this.detainID, this.releasedDate, this.releasedByUserID, this.releasedApplicationID))
            {
                if (clsLicense.markApplicationAsCompleted(releasedApplicationID))
                    return true;

            }
            return false;
        }

        public static int isThatLicenseStillDetained(int detainID)
        {
            return manageDetainLicenseData.isThatLicenseStillDetained(detainID);
        }

        public static DataTable DetainedLicensesTable()
        {
            return manageDetainLicenseData.detainedLicensesTable();
        }
    }
}
