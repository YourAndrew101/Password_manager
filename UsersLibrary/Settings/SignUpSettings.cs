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

        public SignUpSettings() { }

        public SignUpSettings(string email, string password)
        {
            Email = email;
            AuthPassword = password;
        }
    }
}
