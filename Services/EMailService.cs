﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Services
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
        SmtpClient _smtpClient;


        public EMailService(string email)
        {
            Email = email;

            GenerateConfirmationCode();
            GetConfirmationMessage();
            PrepareConfirmationMessage();
        }

        private void PrepareConfirmationMessage()
        {
            _mailMessage = new MailMessage(new MailAddress("pswdmngr.supp0rt@gmail.com"), new MailAddress(Email));
            _mailMessage.Subject = "Email confirmation";
            _mailMessage.Body = ConfirmationMessage.Replace("_CONFIRMATIONCODE_", ConfirmationCode);
            _mailMessage.IsBodyHtml = true;
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
            using (StreamReader streamReader = new StreamReader("/Auth/Assets/EmailConfirmationMessage.txt"))
                ConfirmationMessage = streamReader.ReadToEnd();
        }

        public async Task SendConfirmationMessage()
        {
            string confirmationMailLogin = ConfigurationManager.AppSettings["ConfirmationMailLogin"];
            string confirmationMailPassword = ConfigurationManager.AppSettings["ConfirmationMailPassword"];

            _smtpClient = new SmtpClient(_smtpClientHost, _smtpClientPort);
            _smtpClient.Credentials = new NetworkCredential(confirmationMailLogin, confirmationMailPassword);
            _smtpClient.EnableSsl = false;

            await _smtpClient.SendMailAsync(_mailMessage);
        }
    }
}
