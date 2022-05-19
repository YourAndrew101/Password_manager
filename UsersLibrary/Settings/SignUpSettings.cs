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
        public string AuthPassword { get; private set; }

        //TODO йобаний рот
        public SignUpSettings(User user)
        {
            Email = user.Email;
            AuthPassword = user.AuthPassword;
            CryptoPassword = user.
        }

        public SignUpSettings() { }

        public SignUpSettings(string email, string password)
        {
            Email = email;
            AuthPassword = password;
        }
    }
}
