using UsersLibrary.Settings;

namespace ServicesLibrary.SettingsService
{
    public class SignUpSettingsService : ISettingsService
    {
        public bool IsSavedSettings
        {
            get => !string.IsNullOrEmpty(((SignUpSettings)GetSettings()).Email);
        }

        public void SaveSettings(ISettings settings)
        {
            if (settings is SignUpSettings newSettings)
            {
                Properties.Settings.Default.Email = newSettings.Email;
                Properties.Settings.Default.Password = newSettings.AuthPassword;
                Properties.Settings.Default.Save();
            }
        }

        public void ClearSettings()
        {
            Properties.Settings.Default.Email = "";
            Properties.Settings.Default.Password = "";
            Properties.Settings.Default.Save();
        }

        public ISettings GetSettings()
        {
            string email = Properties.Settings.Default.Email;
            string password = Properties.Settings.Default.Password;

            return new SignUpSettings(email, password);
        }
    }
}
