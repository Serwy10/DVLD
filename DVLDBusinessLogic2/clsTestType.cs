using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLDDataAccess;

namespace DVLDBusinessLogic2
{
    public class clsTestType
    {
        public int TestID { get; }
        public string TestTitle {  get; set; }
        public string TestDescription { get; set; }
        public decimal TestFees {  get; set; }


        public static DataTable TestTypesTable()
        {
            return ManageTestTypes.TestTypesTable();
        }

        private clsTestType(int testID,string testTitle,string testDescription,decimal testFees)
        {
            TestID = testID;
            TestTitle = testTitle;
            TestDescription = testDescription;
            TestFees = testFees;

        }

        public static clsTestType Find(int testID)
        {
            string testTitle=string.Empty;
            string testDescription=string.Empty;
            decimal testFees = 0;

            if(ManageTestTypes.FindTest(testID,ref testTitle,ref testDescription,ref testFees))
                return new clsTestType(testID,testTitle,testDescription,testFees);
            else
                return null;
        }

        public bool Update()
        {
            return ManageTestTypes.UpdateTestType(TestID,TestTitle,TestDescription,TestFees);
        }
    }
}
