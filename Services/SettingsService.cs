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
        public static void SaveSignUpSettings(Settings settings)
        {
            PasswordManager.Properties.Settings.Default.Email = settings.Email;
            PasswordManager.Properties.Settings.Default.Password = settings.Password;
            PasswordManager.Properties.Settings.Default.Save();
        }

        public static Settings GetSignUpSettings()
        {
            return new Settings()
            {
                Email = PasswordManager.Properties.Settings.Default.Email,
                Password = PasswordManager.Properties.Settings.Default.Password
            };
        }
    }
}
