using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace UsersLibrary
{
    public class User
    {
        public int Id { get; private set; }
        public string Email { get; set; }
        private string _password;
        public string Password
        {
            get => _password; set
            {
                Random random = new Random();
                StringBuilder salt = new StringBuilder();
                for (int i = 0; i < 20; i++)
                {
                    salt.Append((char)random.Next(65, 90));
                }
                _password = string.Concat(_password, salt);
                using (SHA256 sHA256 = SHA256.Create())
                {
                    byte[] hash = sHA256.ComputeHash(Encoding.UTF8.GetBytes(_password));
                    StringBuilder sb = new StringBuilder();
                    foreach (var item in hash)
                    {
                        sb.Append(item.ToString("x2"));
                    }
                    _password = sb.ToString();
                }
            }
        }
        public string Salt { get; set; }
    }

}
