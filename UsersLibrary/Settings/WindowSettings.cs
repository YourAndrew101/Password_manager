using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersLibrary.Settings
{
    public class WindowSettings : ISettings
    {
        public enum Languages
        {
            English,
            Ukrainian,
            Russian
        }

        public enum Themes
        {
            Dark = 0,
            Light = 1
        }

        public Languages Language { get; set; }
        public Themes Theme { get; set; }
        public bool ToTrey { get; set; }

        public WindowSettings() { }

        public WindowSettings(Languages language, Themes theme, bool toTrey)
        {
            Language = language;
            Theme = theme;
            ToTrey = toTrey;
        }
    }
}
