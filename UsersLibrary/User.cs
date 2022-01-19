using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersLibrary
{
    public class User
    {
        public string _connectionString = PasswordManager.Default.BayrueConnectionString;

        public int Id { get; private set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }f
    }

}
