using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersLibrary.Services
{
    public class Service
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public Service(string name, string login, string password)
        {
            Name = name;
            Login = login;
            Password = password;
        }
    }
}
