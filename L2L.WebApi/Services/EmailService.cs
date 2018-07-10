using L2L.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using L2L.Entities;
using System.Data.Entity;
using System.Linq;

namespace L2L.WebApi.Services
{
    public class EmailService : BaseService
    {
        public EmailService(BaseApiController controller)
            : base(controller)
        {
        }

        public void SendQuizzmateRequest(FriendRequest request)
        {
            string subjectAdd = " sent you a Quizzmate request";
            QuizzmateRequestEmail(request, subjectAdd);
        }

        public void ReSendQuizzmateRequest(FriendRequest request)
        {
            string subjectAdd = "'s Quizzmate request is awaiting your approval";
            QuizzmateRequestEmail(request, subjectAdd);
        }

        private void QuizzmateRequestEmail(FriendRequest request, string subjectAdd)
        {
            var fromUser = _uow.Users.GetAll()
                .Where(u => u.Id == request.RequestFromId)
                .Include(u => u.Profile)
                .FirstOrDefault();
            var toUser = _uow.Users.GetAll()
                .Where(u => u.Id == request.RequestToId)
                .Include(u => u.Profile)
                .FirstOrDefault();

            var fromUserFullName = fromUser.Profile.FirstName + " " + fromUser.Profile.LastName;
            var toUserFullName = toUser.Profile.FirstName + " " + toUser.Profile.LastName;
            string acceptUrl = "http://quizzocake.com/#/relationship-notifications";
            string unsubscribeUrl = "http://quizzocake.com";

            string emailSubject = fromUserFullName + subjectAdd;
            string body =
                "<div style='font-family: sans-serif; background-color:#f1f1f1; padding: 30px;'>" +
                    "<div style='max-width: 600px; margin: 0 auto; background-color:#e53d0e; height:52px;'>" +
                        "<div style='max-width: 600px;'>" +
                            "<div style='font-weight: 700; background-color: white; display: inline-block; margin: 6px 0 6px 6px; padding: 10px 20px;'>" +
                                "<span style='color:#ff6666;'>Quizz</span><span>O</span><span style='color:#d2691e;'>Cake</span>" +
                            "</div>" +
                        "</div>" +
                    "</div>" +
                    "<div style='max-width: 600px; margin: 0 auto; background-color: white;'>" +
                        "<div style='padding:20px;'>" +
                            "<div style='color:#5bc0de; font-weight:bolder; font-size:large;'>" +
                                "You received a Quizzmate Request!" +
                            "</div>" +
                            "<p>" +
                                "Quizzer <strong>" + fromUser.UserName + "</strong> (" + fromUserFullName +
                                ") sent <strong>" + toUser.UserName + "</strong> (" + toUserFullName + ") a Quizzmate request." +
                            "</p>" +
                            "<p style='font-style:italic; margin-left:50px;'>" +
                                "\"" + request.Message + "\"" +
                            "</p>" +
                            "<p>" +
                                "If you are <strong>" + toUser.UserName + "</strong>, you could accept by clicking this button:" +
                            "</p>" +
                            "<div style='width: 160px; height: 22px; margin: auto; background-color: #f07a18;" +
                                "text-align: center; padding: 10px 20px; border-radius: 5px;" +
                                "cursor: pointer;'>" +
                                "<a style='text-decoration:none; font-weight: bold; font-size:18px; color: white;' href='" + acceptUrl +
                                "'>Accept Quizzmate</a>" +
                            "</div>" +
                            "<p>" +
                                "Cheers,<br />" +
                                "The <span style='color:#ff6666;'>Quizz</span><span style='color:#3c2d1a;'>O</span><span style='color:#d2691e;'>Cake</span> Team" +
                            "</p>" +
                            "<hr style='background-color: lightgray; height: 1px; border: 0;' />" +
                            "<p style='color:gray; font-size:x-small'>" +
                                "You received this email because you registered on quizzocake.com with the email address " + toUser.Email +
                                ". You can manage your notifications by clicking <a href=\"" + unsubscribeUrl + "\">unsubscribe</a>. " +
                                "For questions, you may email us at " +
                                "<a href='mailto:support@quizzocake.com?Subject=Question' target='_top'>" +
                                    "support@quizzocake.com" +
                                "</a>." +
                            "</p>" +
                        "</div>" +
                    "</div>" +
                "</div>";

            SendEmail(toUser.LocalAuthUserId, emailSubject, body);
        }

        public void SendEmail(string toUserId, string subject, string body)
        {
            _userManager.SendEmail(toUserId, subject, body);
        }

        private ApplicationUserManager __userManager;
        public ApplicationUserManager _userManager
        {
            get
            {
                return __userManager ?? _controller.Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                __userManager = value;
            }
        }
    }
}