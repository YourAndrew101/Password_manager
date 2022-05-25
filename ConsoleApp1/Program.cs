using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using static System.Net.Mime.MediaTypeNames;
using ServicesLibrary;
using System.Threading;

namespace ConsoleApp1
{
    class Program
    {
        public static void Main()
        {
            for (int i = 0; i < 100; i++)
            {
                EMailService eMailService = new EMailService("test@gmail.com");
                eMailService.SendConfirmationMessage();
                Thread.Sleep(10);
            }
        }
    }
}
