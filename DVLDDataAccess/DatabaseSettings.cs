using System;
using System.Configuration;

namespace DVLDDataAccess
{
    public static class DatabaseSettings
    {
        public static string ConnectionString
        {
            get
            {
                string value = Environment.GetEnvironmentVariable("DVLD_CONNECTION_STRING");
                if (!string.IsNullOrWhiteSpace(value)) return value;
                var setting = ConfigurationManager.ConnectionStrings["DVLD"];
                if (setting == null || string.IsNullOrWhiteSpace(setting.ConnectionString))
                    throw new ConfigurationErrorsException("Set the DVLD connection string in App.config or DVLD_CONNECTION_STRING.");
                return setting.ConnectionString;
            }
        }
    }
}
