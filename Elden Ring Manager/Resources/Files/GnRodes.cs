using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Elden_Ring_Manager.Resources.Files
{
    public class GnRodes
    {
        private static string GenerateCode(string key, string timeIdentifier)
        {
            using (var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(timeIdentifier));
                return BitConverter.ToString(hash).Replace("-", "").Substring(0, 8); // take the first 8 chars
            }
        }


        public static string GenerateDailyCode(string key)
        {
            string timeIdentifier = DateTime.UtcNow.ToString("yyyyMMdd");
            return GenerateCode(key, timeIdentifier);
        }

        public static string GenerateHourlyCode(string key)
        {
            string timeIdentifier = DateTime.UtcNow.ToString("yyyyMMddHH");
            return GenerateCode(key, timeIdentifier);
        }

        public static string GenerateMinuteCode(string key)
        {
            string timeIdentifier = DateTime.UtcNow.ToString("yyyyMMddHHmm");
            return GenerateCode(key, timeIdentifier);
        }
    }
}
