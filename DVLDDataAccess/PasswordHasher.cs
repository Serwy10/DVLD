using System;
using System.Globalization;
using System.Security.Cryptography;

namespace DVLDDataAccess
{
    public static class PasswordHasher
    {
        private const int Iterations = 600000;
        private const string Prefix = "PBKDF2-SHA256";

        public static string Hash(string password)
        {
            if (string.IsNullOrEmpty(password)) throw new ArgumentException("A password is required.", nameof(password));
            byte[] salt = new byte[16];
            using (var random = RandomNumberGenerator.Create()) random.GetBytes(salt);
            using (var derive = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
                return Prefix + "$" + Iterations.ToString(CultureInfo.InvariantCulture) + "$" +
                    Convert.ToBase64String(salt) + "$" + Convert.ToBase64String(derive.GetBytes(32));
        }

        public static bool IsHash(string value)
        {
            return value != null && value.StartsWith(Prefix + "$", StringComparison.Ordinal);
        }

        public static bool Verify(string password, string stored)
        {
            if (password == null || !IsHash(stored)) return false;
            try
            {
                string[] parts = stored.Split('$');
                int iterations;
                if (parts.Length != 4 || !int.TryParse(parts[1], out iterations) ||
                    iterations < 100000 || iterations > 2000000) return false;
                byte[] salt = Convert.FromBase64String(parts[2]);
                byte[] expected = Convert.FromBase64String(parts[3]);
                if (salt.Length != 16 || expected.Length != 32) return false;
                using (var derive = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
                {
                    byte[] actual = derive.GetBytes(expected.Length);
                    int difference = 0;
                    for (int i = 0; i < actual.Length; i++) difference |= actual[i] ^ expected[i];
                    return difference == 0;
                }
            }
            catch (FormatException) { return false; }
        }
    }
}
