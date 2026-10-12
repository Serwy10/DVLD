using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DVLDDataAccess;

namespace DVLDBusinessLogic2
{
    public class clsLicense
    {
        public enum enMode {NewLicense=1,Renew=2 }
        public enMode Mode;
        public int licenseID { get; private set; }
        public int applicationID { get; set; }
        public int driverID { get; set; }

        public int licenseClass { get; set; }
        public DateTime issueDate { get; set; }
        public DateTime expirationDate { get; set; }

        public string Notes { get; set; }

        public bool isActive { get; set; }

        public int issueReason { get; set; }

        public decimal paidFees { get; set; }

        public int createdBY { get; set; }

        public int _oldLicenseID {  get; set; }



        public bool SaveNewLicense(enMode mode)
        {
            if (mode != enMode.NewLicense) return false;
            var result = LicenseWorkflowData.IssueFirst(clsApplication.getPersonID(applicationID), applicationID,
                licenseClass, issueDate, expirationDate, Notes, paidFees, createdBY);
            if (result == null) return false;
            licenseID = result.LicenseID;
            driverID = result.DriverID;
            return true;
        }

        public static int isPersonHasAlreadyLicenseFromThatClass(int driverID, string className)
        {
            return manageLicensesData.isPersonHasAlreadyLicenseFromThatClass(driverID, className);
        }
        public static bool deActivateLisence(int lisenceID)
        {
            return manageLicensesData.DeActivateLisence(lisenceID);
        }
        public static bool markApplicationAsCompleted(int applicationID)
        {
            return manageLicensesData.markApplicationAsCompleted(applicationID);
        }
        public clsLicense() { licenseID = -1; driverID = -1; }

        public static bool RenewOrReplace(int oldLicenseID, int applicationType, int userID, string notes, out int applicationID, out int newLicenseID)
        {
            var result = LicenseWorkflowData.RenewOrReplace(oldLicenseID, applicationType, userID, notes);
            applicationID = result == null ? -1 : result.ApplicationID;
            newLicenseID = result == null ? -1 : result.LicenseID;
            return result != null;
        }
        private clsLicense(int licenseID,int applicationID,int driverID,int licenseClass,DateTime issueDate,DateTime expirationDate,string notes,
            decimal paidFees,bool isactive,int issueReason,int createdBy)
        {
            this.createdBY = createdBy;
            this.licenseID = licenseID;
            this.licenseClass = licenseClass;
            this.isActive  = isactive;
            this.issueDate = issueDate;
            this.expirationDate = expirationDate;
            this.applicationID = applicationID;
            this.driverID= driverID;
            this.issueReason = issueReason;
            this.Notes = notes;
            this.paidFees = paidFees;

        }

        public static clsLicense findLicense(int LicenseID)
        {
            int applictionID = -1;int driverid=-1;int licenseClass=-1;DateTime issuedate = DateTime.MinValue; DateTime expirationDate= DateTime.MinValue;
            string notes = string.Empty;decimal paidFees =decimal.MinValue; bool isactive = false; int issuereason=-1; int createdBy=-1;

            if(manageLicensesData.FIndLicense(LicenseID,ref applictionID,ref driverid,ref licenseClass,ref issuedate,ref expirationDate,ref notes
                ,ref paidFees,ref isactive,ref issuereason,ref createdBy))
                return new clsLicense(LicenseID,applictionID,driverid,licenseClass,issuedate,expirationDate,notes,paidFees,isactive,issuereason,createdBy);
            else
                return null;

        }
        public static int getValidityLegnth(string className)
        {
            return manageLicensesData.getValidityLengthForLicenseClass(className);
        }

        public static decimal getLicenseClassFees(string className)
        {
            return manageLicensesData.getLicenseClassFees(className);
        }

        public static int getApplicationTypeID(string applicationTypeName)
        {
            return manageLicensesData.getApplicationTypeID(applicationTypeName);
        }

        public static DataTable localDrivingLicensesTable(int personID)
        {
            return manageLicensesData.localLicensesTable(personID);
        }

        public static DataTable InternationalLicensesTable(int personID)
        {
            return manageLicensesData.internationalLicensesTable(personID);
        }

        public static int getLocalDrivingID(int licenseID)
        {
            return manageLicensesData.getLocalDrivingID(licenseID);
        }

        public static bool isThatLicenseExisted(int licenseID)
        {
            return manageLicensesData.isThatLicenseIDExisted(licenseID);
        }

        public static bool isThatLicenseActive(int licenseID)
        {
            return manageLicensesData.isThatLicenseActive(licenseID);
        }

        public static bool isThatLicenseNotExpired(int licenseID)
        {
            return manageLicensesData.isThatLicenseNotExpired(licenseID);
        }
        public static int getLicenseID(int applicationID)
        {
            return manageLicensesData.getLicenseID(applicationID);
        }

        public static bool isthatLicenseFromOrginaryLicenseClass(int localLicenseID)
        {
            return manageLicensesData.isThatLicenseFromOrdinaryLicenseClass(localLicenseID);
        }

        public static bool isThatLicenseExpired(int licenseID)
        {
            return manageLicensesData.isThatLicenseExpired(licenseID);
        }

        public static DateTime Expirationdate(int licenseID)
        {
            return manageLicensesData.ExpirationDate(licenseID);
        }

        public static int getPersonID(int licenseID)
        {
            return manageLicensesData.getPersonID(licenseID);
        }

        public static int getLicenseClassID(int licenseID)
        {
            return manageLicensesData.getLicenseClassID(licenseID);
        }
    }

    public static class DriverLicenseDTO
    {
        public static int licenseID {  get; set; }
        public static string className { get; set; }
        public static string FullName { get; set; }

        public static string nationalNo { get; set; }=string.Empty;

        public static string Gendor {  get; set; }

        public static DateTime issueDate {  get; set; }
        public static DateTime expirationDate { get; set; }
        public static string issueReason { get; set; }
        public static string Notes { get; set; }
        public static string isActive {  get; set; }

        public static DateTime dateOfBirth {  get; set; }

        public static int driverID {  get; set; }

        public static string isDetained {  get; set; }

        public static string imageLocation { get; set; }



        public static bool findDriverLicenseInfo(int licenseID)
        {

            reset();
            DriverLicenseDTO.licenseID = licenseID;
            string className = string.Empty;
            string fullName=string.Empty; string nationalNO=string.Empty;string gender=string.Empty;DateTime issueDate=DateTime.MinValue;DateTime expirationDate=DateTime.MinValue;
            string issueReason=string.Empty; string notes=string.Empty;string isActive=string.Empty; DateTime dateOdBirth=DateTime.MinValue;
            string isDetained=string.Empty; int driverID = -1; string imageLocation = "";

            if(manageLicensesData.findDriverLicenseInfo(licenseID,ref className,ref  fullName,ref nationalNO,ref gender,
                ref issueDate,ref expirationDate,ref issueReason,ref notes,ref isActive,ref driverID,ref dateOdBirth,ref isDetained,ref imageLocation))
            {
                DriverLicenseDTO.className=className;
                DriverLicenseDTO.FullName=fullName;
                DriverLicenseDTO.Gendor=gender;
                DriverLicenseDTO.nationalNo=nationalNO;
                DriverLicenseDTO.issueDate=issueDate;
                DriverLicenseDTO.expirationDate=expirationDate;
                DriverLicenseDTO.issueReason=issueReason;
                DriverLicenseDTO.dateOfBirth=dateOdBirth;
                DriverLicenseDTO.driverID=driverID;
                DriverLicenseDTO.isActive=isActive;
                DriverLicenseDTO.isDetained=isDetained;
                DriverLicenseDTO.Notes=notes;
                DriverLicenseDTO.imageLocation=imageLocation;
                return true;

            }
            return false;

        }

        public static void reset()
        {
            DriverLicenseDTO.licenseID = -1;
            DriverLicenseDTO.className = string.Empty;
            DriverLicenseDTO.FullName = string.Empty;
            DriverLicenseDTO.Gendor=string.Empty;
            DriverLicenseDTO.nationalNo = string.Empty;
            DriverLicenseDTO.issueDate = DateTime.MinValue;
        DriverLicenseDTO.expirationDate=DateTime.MinValue;
            DriverLicenseDTO.issueReason = string.Empty;

            DriverLicenseDTO.dateOfBirth = DateTime.MinValue;

            DriverLicenseDTO.driverID = -1;

            DriverLicenseDTO.isActive = string.Empty;

            DriverLicenseDTO.isDetained = string.Empty;

        DriverLicenseDTO.Notes= string.Empty;
            DriverLicenseDTO.imageLocation = string.Empty;
    }

    }
}
