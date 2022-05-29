using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersLibrary.Settings;

namespace ServicesLibrary.SettingsService
{
    public interface ISettingsService
    {
        bool IsSavedSettings { get; }
        void Save(ISettings settings);
        void Clear();
        ISettings GetSettings();
    }
}
