using System;
using System.Security.Cryptography;
using System.Text;

namespace TypicalTools.DataAccess
{
    public static class PasswordHasher
    {
        /// <summary>
        /// hashing method to convert passwords more securely
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ConvertStringToHash(string input)
        {
            using (SHA256 algo = SHA256.Create())
            {
                byte[] hash = algo.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Convert.ToBase64String(hash);                
            }
        }

    }
}
