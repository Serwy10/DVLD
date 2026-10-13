using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DVLDDataAccess;

namespace DVLDBusinessLogic2
{
    public class clsApplication
    {
        private enum _enmode { addNew=0,Update=1 }
        _enmode _mode = _enmode.addNew;
        public int _applicationID { get; private set; }
        public int _ApplicationPersonID { get; set; }
        public DateTime _ApplicationStartDate { get; set; }

        public int _TypeID {  get; set; }

        public string _ApplicationStatus {  get; set; }

        public DateTime lastStatusDate {  get; set; }
        public decimal paidFees {  get; set; }

        public int createdByUserID {  get; set; }

        public int _LicenseClassID {  get; set; }

        public clsApplication()
        {
            _applicationID = -1;
            _ApplicationPersonID = -1;
            _ApplicationStatus = "New";
            lastStatusDate = DateTime.MinValue;
            paidFees = 0;
            createdByUserID = -1;
            _ApplicationStartDate = DateTime.MinValue;
            _TypeID = -1;
            _LicenseClassID = -1;
            _mode = _enmode.addNew;
        }

        public static DataTable localDrivingApplicationsTable()
        {
            return ManageApplications.LocalDrivingApplicationsTable();
        }

        private clsApplication(int applicationID,int applicationPersonID,string AppStatus,DateTime applicationDate,DateTime lastStatusDate,
            decimal paidFees,int CreatedByUserID,int typeID,int licenseClassiD)
        {
            _applicationID = applicationID;
            _ApplicationPersonID= applicationPersonID;
            _ApplicationStatus = AppStatus;
            _ApplicationStartDate= applicationDate;
            _TypeID = typeID;
            _LicenseClassID = licenseClassiD;
            this.lastStatusDate = lastStatusDate;
            this.paidFees = paidFees;
            this.createdByUserID = createdByUserID;
            _mode=_enmode.Update;
        }

        public static clsApplication FindApplication(int ApplicationID)
        {
            int applicationPersonID = -1; string AppStatus = ""; DateTime applicationDate = DateTime.MinValue; DateTime lastStatusDate=DateTime.MinValue;
            decimal paidFees = 0; int CreatedByUserID = -1; int typeID = -1; int licenseClassiD= -1;

            if (ManageApplications.FindApplication(ApplicationID, ref applicationPersonID, ref applicationDate, ref typeID, ref AppStatus, ref lastStatusDate
                , ref paidFees, ref CreatedByUserID, ref licenseClassiD))
                return new clsApplication(ApplicationID,applicationPersonID,AppStatus,applicationDate,lastStatusDate,paidFees,CreatedByUserID,typeID
                    ,licenseClassiD);
            return null;

        }


        public static decimal getFeesForApplication(int ApplicationTypeID)
        {
            return ManageApplications.GetApplicationFees(ApplicationTypeID);
        }

        public static int getLicenseClassID(string className)
        {
            return ManageApplications.getLicenseClassID(className);
        }

        public static DataTable licensesClasses()
        {
            return ManageApplications.LicensesClasses();
        }

        public static bool IsThisPersonHasApplicationFromThatCategory(int personID,string className, int excludedApplicationID = -1)
        {
            return ManageApplications.PersonAlreadyHasApplicationOfThatClass(personID,className,excludedApplicationID);
        }

        private bool AddNewApplication()
        {
            int tempAppID = -1;
            bool saved = false;
            saved = ManageApplications.SaveNewApplication(ref tempAppID, this._ApplicationPersonID, this._ApplicationStartDate, this._TypeID
                 , this._ApplicationStatus, this.lastStatusDate, this.paidFees, this.createdByUserID, this._LicenseClassID);

            this._applicationID = tempAppID;
            return saved;
        }

        private bool UpdateApplication()
        {
            return ManageApplications.UpdateApplicationInfo(this._applicationID,this.lastStatusDate,this._LicenseClassID);
        }
        public bool Save()
        {
            switch (_mode)
            {
                case _enmode.addNew:
                    if (!AddNewApplication()) return false;
                    this._mode = _enmode.Update;
                    return true;
                case _enmode.Update:
                    return UpdateApplication();

            }
            return false;

        }

        public static DataView getApplicationsViewForText(string filter,string condition)
        {
            return TableFilter.Text(localDrivingApplicationsTable(), filter, condition);
        }

        public static DataView getappLicationsViewForNumeric(string filter, string condition) {
            return TableFilter.Number(localDrivingApplicationsTable(), filter, condition);
        }

        public static bool cancelApplication(int applicationID)
        {
            return ManageApplications.CancelApplication(applicationID);
        }

        public static int getApplicationID(int LocalApplicationID) {

            return ManageApplications.getApplicationID(LocalApplicationID);
        }

        public static int getPersonID(int ApplicationID)
        {
            return ManageApplications.getPersonID(ApplicationID);
        }

        public static string getClassName(int licenseClassID)
        {
            return ManageApplications.getClassName(licenseClassID);
        }

        public static string getUserName(int userID)
        {
            return ManageApplications.getUserName(userID);
        }

        public static bool isApplicationNew(int LocalapplicationID) {

            return ManageApplications.isApplicationNew(LocalapplicationID);
        }

        public static bool isSucessededInThatTest(int testTypeID,int localAppID)
        {
            return ManageApplications.IsSuccededInThatTest(testTypeID,localAppID);
        }

        public static bool isApplicantPassedAllTests(int localDrivingID)
        {
            return ManageApplications.isApplicantPassedAllTests(localDrivingID);
        }

        public static decimal getApplicationFees(int applicationTypeID)
        {
            return ManageApplications.GetApplicationFees(applicationTypeID);
        }

        public static bool isApplicationCompleted(int localDrivingID)
        {
            return ManageApplications.isApplicationCompleted(localDrivingID);
        }

        public static bool isApplicationHasLicense(int localDrivingID)
        {
            return ManageApplications.isAPplicationHasLicense(localDrivingID);
        }

        public static bool DeleteApplication(int localID)
        {
            return ManageApplications.deleteApplication(localID);
        }
    }

    public static class ApplicationDetailsDTO
    {
        public static int _localDrivingAppID {  get; set; }
        public static string _className {  get; set; }
        public static int _passedTests {  get; set; }
        public static int _AppID {  get; set; }
        public static string _status {  get; set; }
        public static decimal _paidFees {  get; set; }
        public static string _AppType {  get; set; }
        public static string _applicantName {  get; set; }
        public static DateTime _AppDate {  get; set; }
        public static DateTime _lastStatusDate { get; set; }
        public static string _createdBy { get; set; }
        public static int _personID {  get; set; }

        public static bool findApplicationInfo(int localID)
        {
            string className = string.Empty;int passedTests = 0;int AppID = -1;string status = string.Empty;decimal paidFees = 0;
            string AppType=string.Empty;string applicantName = string.Empty; DateTime appDate= DateTime.MinValue; DateTime lastStatusDate = DateTime.MinValue;
            string createdBy = string.Empty; int personID = -1;

            if (ManageApplications.FindApplicationInfo(localID, ref className, ref passedTests, ref AppID,
                ref status, ref paidFees, ref AppType, ref applicantName, ref appDate, ref lastStatusDate
                , ref createdBy,ref personID))
            {
                _localDrivingAppID=localID;
                _className = className;
                _applicantName = applicantName;
                _passedTests = passedTests;
                _AppID = AppID;
                _status = status;
                _createdBy = createdBy;
                _paidFees = paidFees;
                _AppType = AppType;
                _AppDate=appDate;
                _lastStatusDate = lastStatusDate;
                _personID=personID;
                return true;
            }
            else
                return false;
        }
    }
}
