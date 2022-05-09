using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersLibrary.Settings;

namespace ServicesLibrary
{
    public class SettingsService
    {
        public static bool IsSavedUser
        {
            get => !string.IsNullOrEmpty(GetSignUpSettings().Email);
        }

        public static void SaveSignUpSettings(SignUpSettings settings)
        {
            PasswordManager.Properties.Settings.Default.Email = settings.Email;
            PasswordManager.Properties.Settings.Default.Password = settings.Password;
            PasswordManager.Properties.Settings.Default.Save();
        }

        public static SignUpSettings GetSignUpSettings()
        {
            string email = PasswordManager.Properties.Settings.Default.Email;
            string password = PasswordManager.Properties.Settings.Default.Password;
            return new SignUpSettings(email, password);
        }

        public static void SaveEmptySignUpSettings()
        {
            PasswordManager.Properties.Settings.Default.Email = "";
            PasswordManager.Properties.Settings.Default.Password = "";
            PasswordManager.Properties.Settings.Default.Save();
        }
    }
}
