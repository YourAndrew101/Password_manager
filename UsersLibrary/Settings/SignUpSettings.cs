using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersLibrary.Settings
{
    public class SignUpSettings
    {
        public string Email { get; private set; }
        public string Password { get; private set; }

        public SignUpSettings(User user)
        {
            Email = user.Email;
            Password = user.AuthPassword;
        }

        public SignUpSettings() { }

        public SignUpSettings(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
