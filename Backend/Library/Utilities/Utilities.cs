using System;
using System.Security.Cryptography;
using System.Text;

namespace Library.Utilities
{
    static class Utilities
    {
        /// <summary>
        /// Simple SHA1 hash function
        /// </summary>
        /// <param name="input"></param>
        /// <returns>A SHA1 hashed string from the input string</returns>
        public static string HashString(string input)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
                return BitConverter.ToString(hash).Replace("-", string.Empty);
            }
        }

        /// <summary>
        /// Extension method on the DateTime Type to allow conversion to a UNIX timestamp
        /// </summary>
        /// <returns>Timestamp of the date this method is called on</returns>
        public static long ToTimestamp(this DateTime date)
        {
            return (long)(date - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        /// Creates a DateTime from a UNIX timestamp
        /// </summary>
        /// <param name="timestamp">Timestamp to convert</param>
        /// <returns>DateTime representing the given timestamp</returns>
        public static DateTime FromTimestamp(long timestamp)
        {
            return new DateTime(1970, 1, 1).AddSeconds(timestamp);
        }
    }
}
