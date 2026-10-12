using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLDDataAccess;

namespace DVLDBusinessLogic2
{
    public class clsTest
    {
       public int testID {  get; private set; }
        public int testAppointmentID {  get;  set; }
        public bool TestResult {  get;  set; }
        public string Notes {  get; set; }
        public int CreatedBy {  get; set; }


        public clsTest()
        {
            testID = -1;
            testAppointmentID = -1;
            TestResult = false;
            Notes = string.Empty;
            CreatedBy = -1;
        }

        public bool Save()
        {
            int id = -1;
            if (!ManageTestsData.saveNewTest(ref id, testAppointmentID, TestResult, Notes, CreatedBy)) return false;
            testID = id;
            return true;
        }

        private clsTest(int testID,int appointmentID,bool result,string notes,int createdBy)
        {
            this.testID = testID;
            this.CreatedBy = createdBy;
            this.Notes = notes;
            this.testAppointmentID = appointmentID;
            this.TestResult = result;
        }

        public static clsTest Find(int appointmentID)
        {
            int testID = -1;bool result = false;string note = string.Empty;int createdby = -1;
            if (ManageTestsData.findTest(appointmentID, ref testID, ref result, ref note, ref createdby))
                return new clsTest(testID, appointmentID, result, note, createdby);
            else
                return null;

        }
    }
}
