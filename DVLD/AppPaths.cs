using System;
using System.IO;

namespace DVLD
{
    internal static class AppPaths
    {
        public static string DataDirectory => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DVLD");
        public static string PhotosDirectory => Path.Combine(DataDirectory, "Photos");
        public static string Asset(string name) => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", name);
    }
}
