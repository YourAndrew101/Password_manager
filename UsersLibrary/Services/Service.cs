using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Services;

namespace UsersLibrary.Services
{
    public class Service : BaseModel
    {
        private int _id;
        private string _name;
        private string _login;
        private string _password;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));

            }
        }
        public ImageSource LogoPath
        {
            get
            {
                ImageSourceConverter converter = new ImageSourceConverter();

                return (ImageSource)converter.ConvertFromString(LogoService.GetLogo(Name));
            }
        }
        public string Name 
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Login 
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }
        public string Password 
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public Service(string name, string login, string password)
        {
            Name = name;
            Login = login;
            Password = password;
        }
    }
}
