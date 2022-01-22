using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PasswordManagerMainWindow.Auth
{
    /// <summary>
    /// Логика взаимодействия для EmailConfirmation.xaml
    /// </summary>
    public partial class EmailConfirmation : Page
    {
        Random random = new Random();
        int code;
        string _login;
        string _password;
        public EmailConfirmation(string login, string password)
        {
            _login = login;
            _password = password;
            InitializeComponent();
            
            //code = random.Next(100000, 999999);
            //MailMessage message = new MailMessage(new MailAddress("pswdmanager@yahoo.com"), new MailAddress(_login));
            //message.Subject = "Email confirmation";
            //message.Body = "<h2>Test</h2>";
            //message.IsBodyHtml = true;
            //SmtpClient smtp = new SmtpClient("smtp.mail.yahoo.com", 465);
            //smtp.Credentials = new NetworkCredential("pswdmanager@yahoo.com", "f;j7Ne_2uy4ZVR!");
            //smtp.EnableSsl = true;
            //smtp.Send(message);



        }
        //
        //pswdmanager @ukr.net

        //f;j7Ne_2uy4ZVR!
        //xC2W74QD3GzxwHUx imap
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            //if(code=)
        } 
    }
}
