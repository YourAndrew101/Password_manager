using UsersLibrary.Settings;

namespace ServicesLibrary.SettingsService
{
    public interface ISettingsService
    {
        bool IsSavedSettings { get; }
        void SaveSettings(ISettings settings);
        void ClearSettings();
        ISettings GetSettings();
    }
}
