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
    public class clsTestAppointment
    {
        private enum _enmode { addNew=1,Edit=2}
        private _enmode _mode;
        public int AppointmentID {  get; private set; }
        public int testTypeID {  get;  set; }

        public int localDrivingID {  get;  set; }

        public DateTime AppointmentDate {  get; set; }

        public decimal paidFees {  get; set; }

        public int createdBy {  get; set; }

        public bool isLocked {  get; set; }

        public clsTestAppointment()
        {
            AppointmentID = -1;
            testTypeID = -1;
            localDrivingID = -1;
            paidFees = -1;
            createdBy = -1;
            isLocked = false;
            AppointmentDate= DateTime.MinValue;
            _mode=_enmode.addNew;
        }

        public static DataTable testAppointmentsTable(string appointmentType,int localDrivingID)
        {
            return ManageTestAppointmentsData.GetTestAppointmentTable(appointmentType,localDrivingID);
        }

        public static bool checkIfApplicantHasAppointmentAndNotLocked(int localDrivingID,string testType)
        {
            return ManageTestAppointmentsData.ifApplicantHasAppointmentAndNotLocked(localDrivingID,testType);
        }

        public static bool checkIfPassedThatTestType(int localDrivingID,string TestType)
        {
            return ManageTestAppointmentsData.checkApplicantPassedThatTest(localDrivingID,TestType);
        }

        public static decimal getTestTypeFees(string testType)
        {
            return ManageTestAppointmentsData.GetTestTypeFees(testType);
        }

        public static bool checkIfApplicantFailedThatTestBefore(int localDrivingID,string testType)
        {
            return ManageTestAppointmentsData.checkIfApplicantHasFailedOnThatTest(localDrivingID,testType);
        }

        public static int getTestTypeID(string testType)
        {
            return ManageTestAppointmentsData.getTestTypeID(testType);
        }

        private bool AddNewTestAppointment()
        {
            int ID = -1;
            ManageTestAppointmentsData.saveNewTestAppointment(ref ID, this.localDrivingID, this.AppointmentDate, this.testTypeID, this.paidFees,
                this.createdBy, this.isLocked);
            this.AppointmentID = ID;
            return (ID != -1);
        }

        private bool updateTestAppointment()
        {
            return (ManageTestAppointmentsData.updateAppointmentDate(this.AppointmentID, this.AppointmentDate));
        }

        public bool Save()
        {
            switch (_mode)
            {
                case _enmode.addNew:
                    if (!AddNewTestAppointment()) return false;
                    this._mode = _enmode.Edit;
                    return true;

                case _enmode.Edit:
                    return updateTestAppointment();
            }
            return false;
        }

        private clsTestAppointment(int testAppointmentID,int testTypeID,int localDrivingID,DateTime AppDate,decimal paidFees,int createdBy,bool isLocked)
        {
            this.AppointmentID= testAppointmentID;
            this.AppointmentDate= AppDate;
            this.localDrivingID= localDrivingID;
            this.testTypeID= testTypeID;
            this.createdBy= createdBy;
            this.isLocked=isLocked;
            this.paidFees= paidFees;
            _mode = _enmode.Edit;

        }

        public static clsTestAppointment Find(int AppointmentID) {

            int testTypeID = -1;int localDrivingID = -1;DateTime AppDate=DateTime.MinValue;decimal PaidFees=Decimal.MinValue;int createdBy = -1;
            bool isLocked=false;

            if(ManageTestAppointmentsData.findAppointment(AppointmentID,ref testTypeID,ref localDrivingID,ref  AppDate,ref PaidFees,ref createdBy,ref isLocked))
            return new clsTestAppointment(AppointmentID,testTypeID,localDrivingID,AppDate,PaidFees,createdBy,isLocked);

            return null;



        }

        public static int countApplicantTestTrials(string testType,int localDrivingID)
        {
            return ManageTestAppointmentsData.countApplicantTestTrials(testType,localDrivingID);
        }

        public static bool checkIfAppointmentIsLocked(int AppointmentID)
        {
            return ManageTestAppointmentsData.checkIfAppointmentLocked(AppointmentID);
        }

        public static int countPassedTests(int localDrivingID)
        {
            return ManageTestAppointmentsData.countPassedTests(localDrivingID);
        }
    }

}
