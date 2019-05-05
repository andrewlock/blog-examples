using CustomPasswordHasher.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CustomPasswordHasher
{
    public static class UserManagerExtensions
    {
        public static async Task<IdentityResult> SetMd5PasswordForUser(this UserManager<IdentityUser> userManager, IdentityUser user, string md5Password)
        {
            var reHashedPassword = userManager.PasswordHasher.HashPassword(user, md5Password);

            var passwordToStore = ReplaceFormatMarker(reHashedPassword, Md5PasswordHasher<IdentityUser>.Md5FormatByte);

            user.PasswordHash = passwordToStore;

            await userManager.UpdateSecurityStampAsync(user);

            return await userManager.UpdateAsync(user);
        }

        static string ReplaceFormatMarker(string passwordHash, byte formatMarker)
        {
            var bytes = Convert.FromBase64String(passwordHash);
            bytes[0] = formatMarker;
            return Convert.ToBase64String(bytes);
        }
    }
}
