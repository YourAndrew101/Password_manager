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
            get => !string.IsNullOrEmpty(GetSettings().Email);
        }

        public static void SaveSettings(Settings settings)
        {
            PasswordManager.Properties.Settings.Default.Email = settings.Email;
            PasswordManager.Properties.Settings.Default.Password = settings.Password;
            PasswordManager.Properties.Settings.Default.Salt = settings.Salt;
            PasswordManager.Properties.Settings.Default.Save();
        }

        public static Settings GetSettings()
        {
            string email = PasswordManager.Properties.Settings.Default.Email;
            string password = PasswordManager.Properties.Settings.Default.Password;
            string salt = PasswordManager.Properties.Settings.Default.Salt;
            return new Settings(email, password, salt);
        }

        public static void SaveEmptySettings()
        {
            PasswordManager.Properties.Settings.Default.Email = "";
            PasswordManager.Properties.Settings.Default.Password = "";
            PasswordManager.Properties.Settings.Default.Salt = "";
            PasswordManager.Properties.Settings.Default.Save();
        }
    }
}
