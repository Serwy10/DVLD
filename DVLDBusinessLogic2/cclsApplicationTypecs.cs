using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLDDataAccess;

namespace DVLDBusinessLogic2
{
    public class cclsApplicationTypecs
    {

      public  int _applicationID {  get; }
        public string _applicationName {  get; set; }
        public decimal _fees {  get; set; }


        public static DataTable applicationTypesTable()
        {
            return ManageApplicationTypes.applicationTypesTable();
        }

        private cclsApplicationTypecs(int applicationID, string applicationName, decimal fees)
        {
            _applicationID = applicationID;
            _applicationName = applicationName;
            _fees = fees;
        }

        public static cclsApplicationTypecs Find(int appliCationID) {

            string applicationName=string.Empty;
            decimal fees = 0;

            if (ManageApplicationTypes.findApplicationType(appliCationID, ref applicationName, ref fees))
                return new cclsApplicationTypecs(appliCationID,applicationName,fees);
            else
                return null;

        }

        public bool Update()
        {
            if(ManageApplicationTypes.updateApplicationTypeInfo(this._applicationID,this._applicationName,this._fees))
                return true;
            return false;
        }
    }
}
