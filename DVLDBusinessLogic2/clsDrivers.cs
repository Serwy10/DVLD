using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLDDataAccess;

namespace DVLDBusinessLogic2
{
    public static class clsDrivers
    {
        public static DataTable driversTable()
        {
            return manageDriversData.DriversTable();
        }

    }
}
