using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersLibrary.Settings
{
    public class Settings
    {
        private User _user;
        public string Email { get; set; }
        public string Password { get; set; }

        public Settings(User user)
        {
            Email = user.Email;
            Password = user.AuthPassword;
        }
        public Settings() { }
        public Settings(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
