using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace L2L.WebApi.Services.external
{
    public class EmailService
    {
        public EmailService()
        {

        }

        public bool Send()
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient server = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new System.Net.NetworkCredential("quizzinator.new", "RanchRush8765"),
                    EnableSsl = true
                };

                mail.From = new MailAddress("quizzinator.new@gmail.com");
                mail.To.Add("edwin.lingad@gmail.com");
                mail.Subject = "Test Mail";
                mail.Body = "Test mail body";

                server.Send(mail);
                return true;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public bool SendSF()
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient server = new SmtpClient("mail.stfrancisacademyofpampanga.com", 465)
                {
                    Credentials = new System.Net.NetworkCredential("noreply", "PvZombies8765"),
                    EnableSsl = true
                };

                mail.From = new MailAddress("noreply@stfrancisacademyofpampanga.com");
                mail.To.Add("edwin.lingad@gmail.com");
                mail.Subject = "Test Mail";
                mail.Body = "Test mail body";

                server.Send(mail);
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}