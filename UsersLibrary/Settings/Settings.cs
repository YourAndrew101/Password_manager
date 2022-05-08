using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersLibrary.Settings
{
    public class Settings
    {
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string Salt { get; private set; }

        public Settings(User user)
        {
            Email = user.Email;
            Salt = user.Salt;
            Password = user.AuthPassword;
        }

        public Settings() { }

        public Settings(string email, string password, string salt)
        {
            Email = email;
            Password = password;
            Salt = salt;
        }
    }
}
