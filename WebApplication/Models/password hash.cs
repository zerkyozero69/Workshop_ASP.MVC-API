using SimplePassword;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication.Models
{
    public class password_hash
    {
        //hash password
        public static string hash(string password)
        {
            var passwordHash = new SaltedPasswordHash(password,20);
            return passwordHash.Hash + ":" + passwordHash.Salt;     

        }

        //verify password hash method
        public static bool Verify(string password, string passwordhash)
        { string[] passwordhashes = passwordhash.Split(  ':'  );
            if (passwordhashes.Length==2)
            {
                var passwordHash = new SaltedPasswordHash(passwordhashes[0], passwordhashes[1]);
                return passwordHash.Verify(password);
            }
            return false;
        }
    }
}