using Microsoft.AspNetCore.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CustomPasswordHasher
{
    /// <summary>
    /// A drop-in replacement for the standard Identity hasher to be backwards compatible with existing md5 hashes
    /// New passwords will be hashed with Identity V3
    /// </summary>
    public class Md5PasswordHasher<TUser> : PasswordHasher<TUser> where TUser : class
    {
        public const byte Md5FormatByte = 0xF0;

        public override PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
        {
            if (hashedPassword == null) { throw new ArgumentNullException(nameof(hashedPassword)); }
            if (providedPassword == null) { throw new ArgumentNullException(nameof(providedPassword)); }

            byte[] decodedHashedPassword = Convert.FromBase64String(hashedPassword);

            // read the format marker from the hashed password
            if (decodedHashedPassword.Length == 0)
            {
                return PasswordVerificationResult.Failed;
            }

            // ASP.NET Core uses 0x00 and 0x01, so we start at the other end
            if (decodedHashedPassword[0] == Md5FormatByte)
            {
                // replace the 0xF0 prefix in the stored password with 0x01 (ASP.NET Core Identity V3) and convert back
                decodedHashedPassword[0] = 0x01;
                var storedPassword = Convert.ToBase64String(decodedHashedPassword);

                // md5 hash the provided password
                var md5ProvidedPassword = GetM5Hash(providedPassword);

                // call the base implementation with the new values
                var result = base.VerifyHashedPassword(user, storedPassword, md5ProvidedPassword);

                return result == PasswordVerificationResult.Success
                    ? PasswordVerificationResult.SuccessRehashNeeded
                    : result;
            }

            return base.VerifyHashedPassword(user, hashedPassword, providedPassword);
        }

        public static string GetM5Hash(string input)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                var bytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                return Convert.ToBase64String(bytes);
            }
        }
    }
}
