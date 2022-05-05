using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NY.Framework.Infrastructure.Utilities
{
    public class PasswordHashHelper
    {
        public PasswordHashHelper()
        {
            
        }
        private static string GetRandomSalt()
        {
            return BCrypt.BCryptHelper.GenerateSalt(12);
        }

        public static string HashPassword(string password)
        {
          return  System.Web.Helpers.Crypto.HashPassword(password);
            //return BCrypt.BCryptHelper.HashPassword(password, GetRandomSalt());
        }

        public static bool ValidatePassword(string password, string correctHash)
        {
            return System.Web.Helpers.Crypto.VerifyHashedPassword(correctHash,password);
            //return BCrypt.BCryptHelper.CheckPassword(password, correctHash);
        }
    }
}
