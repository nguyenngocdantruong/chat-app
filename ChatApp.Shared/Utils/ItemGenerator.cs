using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Shared.Utils
{
    public static class ItemGenerator
    {
        public static string GenerateOtp(int length = 6)
        {
            if (length <= 0)
            {
                throw new ArgumentException("Length must be a positive integer.");
            }
            var random = new Random();
            var otp = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                otp.Append(random.Next(0, 10)); 
            }
            return otp.ToString();
        }
        public static string GenerateKey(string prefix, string userId, string? additionalInfo = null)
        {
            if (string.IsNullOrWhiteSpace(prefix) || string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("Prefix and userId cannot be null or empty.");
            }
            var keyBuilder = new StringBuilder(prefix);
            keyBuilder.Append(":").Append(userId);
            if (!string.IsNullOrWhiteSpace(additionalInfo))
            {
                keyBuilder.Append(":").Append(additionalInfo);
            }
            return keyBuilder.ToString();
        }

        public static string GenerateRandom()
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
