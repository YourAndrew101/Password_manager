using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersLibrary.Settings
{
    public class WindowSettings : ISettings
    {
        public enum Languges
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

        public Languges Languge { get; set; }
        public Themes Theme { get; set; }
        public bool ToTrey { get; set; }

        public WindowSettings() { }

        public WindowSettings(Languges languge, Themes theme, bool toTrey)
        {
            Languge = languge;
            Theme = theme;
            ToTrey = toTrey;
        }
    }
}
