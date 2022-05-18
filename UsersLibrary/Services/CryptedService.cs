using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersLibrary.Services
{
    public class CryptedService : Service
    {
        public CryptedService(int id, string name, string login, string password) : base(name, login, password)
        {
            Id = id;
        }
    }
}
