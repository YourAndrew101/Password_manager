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
using System.Web.Security;

namespace ConsoleApp1
{
    class Program
    {
        public static void Main()
        {
            string s = Membership.GeneratePassword(16, 16 / 4);
        }
    }
}
