using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLibrary
{
    public class ThemesService
    {
        public enum Themes
        {
            Dark = 0,
            Light = 1
        }

        public static Themes GetSystemTheme()
        {
            object registerTheme = Microsoft.Win32.Registry
                .GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", "1");
            return (Themes)registerTheme;
        }
    }
}
