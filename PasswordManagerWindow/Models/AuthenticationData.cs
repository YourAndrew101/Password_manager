using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManagerWindow.Models
{
    public class AuthenticationData : BaseModel
    {

        private string resource;
        private string login;
        private string password;

      


        public string Resource
        {
            get { return resource; }
            set { resource = value;
                OnPropertyChanged(nameof(Resource));
            }
        }
        public string Login
        {
            get { return login; }
            set { login = value;
                OnPropertyChanged(nameof(Login));
            }
        }
        public string Password
        {
            get { return password; }
            set { password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public AuthenticationData() { }
        public AuthenticationData(string Resource, string Login, string Password)
        {
            this.Resource = Resource;
            this.Login = Login;
            this.Password = Password;
        }
    }
}
