using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using L2L.WebApi.Models;
using System;
using System.Web.Mail;
using System.Net.Mail;
using SendGrid;
using System.Net;

namespace L2L.WebApi
{

    // Send Grid API Key: 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
    public class EmailService : IIdentityMessageService
    {
        private const string _emailAccount = "support";
        private const string sendGridKey = "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";
        public Task SendAsync(IdentityMessage message)
        {
            try
            {
                //System.Net.Mail.MailMessage email = new System.Net.Mail.MailMessage();
                //SmtpClient mailClient = new SmtpClient("smtp.gmail.com", 587)
                //{
                //    Credentials = new System.Net.NetworkCredential(_emailAccount + "@quizzocake.com", "RanchRush8765"),
                //    EnableSsl = true
                //};

                //email.From = new MailAddress("\"QuizzOCake Support\" <" + _emailAccount + "@quizzocake.com>");
                //email.To.Add(message.Destination);
                //email.Subject = message.Subject;
                //email.Body = message.Body;
                //email.IsBodyHtml = true;

                //return mailClient.SendMailAsync(email);

                // Create the email object first, then add the properties.
                SendGridMessage sendGridMessage = new SendGridMessage();
                sendGridMessage.AddTo(message.Destination);
                sendGridMessage.From = new MailAddress(_emailAccount + "@quizzocake.com", "QuizzOCake Support");
                sendGridMessage.Subject = message.Subject;

                sendGridMessage.Html = message.Body;

                // Create an Web transport for sending email.
                var transportWeb = new Web(sendGridKey);

                // Send the email, which returns an awaitable task.
                return transportWeb.DeliverAsync(sendGridMessage);
            }
            catch (Exception)
            {
                return Task.FromResult(0);
            }
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
                //RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 4,
                //RequireNonLetterOrDigit = true,
                //RequireDigit = true,
                //RequireLowercase = true,
                //RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
}
