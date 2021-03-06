using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLibrary
{
    public class EMailService
    {
        private readonly int _confirmationCodeLength = 6;

        private const string _smtpClientHost = "127.0.0.1";
        private const int _smtpClientPort = 25;

        public string ConfirmationCode { get; set; }
        public string ConfirmationMessage { get; set; }
        public string Email { get; set; }

        MailMessage _mailMessage;

        public EMailService(string email)
        {
            Email = email;

            GenerateConfirmationCode();
            GetConfirmationMessage();
            PrepareConfirmationMessage();
        }

        private void PrepareConfirmationMessage()
        {
            _mailMessage = new MailMessage(new MailAddress("pswdmngr.supp0rt@gmail.com"), new MailAddress(Email))
            {
                Subject = "Action confirmation",
                Body = ConfirmationMessage.Replace("_CONFIRMATIONCODE_", ConfirmationCode),
                IsBodyHtml = true
            };
        }

        private void GenerateConfirmationCode()
        {
            Random rand = new Random();
            StringBuilder temp = new StringBuilder();

            for (int _ = 0; _ < _confirmationCodeLength; _++)
                temp.Append(rand.Next(0, 9).ToString());

            ConfirmationCode = temp.ToString();
        }

        private void GetConfirmationMessage()
        {
            using (StreamReader streamReader = new StreamReader("../../../Services/Assets/ActionConfirmationMessage.txt"))
                ConfirmationMessage = streamReader.ReadToEnd();
        }

        public void SendConfirmationMessage()
        {
            string confirmationMailLogin = ConfigurationManager.AppSettings["ConfirmationMailLogin"];
            string confirmationMailPassword = ConfigurationManager.AppSettings["ConfirmationMailPassword"];

            SmtpClient _smtpClient = new SmtpClient(_smtpClientHost, _smtpClientPort)
            {
                Credentials = new NetworkCredential(confirmationMailLogin, confirmationMailPassword),
                EnableSsl = false
            };

            _smtpClient.Send(_mailMessage);
        }
        public async Task SendAsyncConfirmationMessage()
        {
            await Task.Run(() => SendConfirmationMessage());
        }
    }
}
