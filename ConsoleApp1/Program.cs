using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using FluentEmail.Core;
using FluentEmail.Smtp;

namespace ConsoleApp1
{
    class Program
    {
        static void  Main(string[] args)
        {
            CultureInfo.CurrentCulture = new CultureInfo("ru-RU", false);

            CultureInfo ci = CultureInfo.CurrentCulture;

            Console.WriteLine("Default Language Info:");
            Console.WriteLine("* Name: {0}", ci.Name);
            Console.WriteLine("* Display Name: {0}", ci.DisplayName);
            Console.WriteLine("* English Name: {0}", ci.EnglishName);
            Console.WriteLine("* 2-letter ISO Name: {0}", ci.TwoLetterISOLanguageName);
            Console.WriteLine("* 3-letter ISO Name: {0}", ci.ThreeLetterISOLanguageName);
            Console.WriteLine("* 3-letter Win32 API Name: {0}", ci.ThreeLetterWindowsLanguageName);

            Console.Read();
        }
    }
}
