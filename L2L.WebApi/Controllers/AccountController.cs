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
using L2L.WebApi.Models;
using L2L.WebApi.Providers;
using L2L.WebApi.Results;
using System.Net;
using System.Data.Entity;
using System.Linq;
using Facebook;

namespace L2L.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : BaseApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public async Task<UserInfoViewModel> GetUserInfo()
        {
            var isEmailUsed = false;
            RegisterExternalBindingModel model = new RegisterExternalBindingModel();
            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info != null)
            {
                GetExternalLoginInfo(model, info);
                isEmailUsed = SvcContainer.UserSvc.IsEmailUsed(model.Email);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null,
                IsEmailUsed = isEmailUsed
            };
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            SvcContainer.UserSvc.ResetStoredUser();
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ForgotPassword")]
        public async Task<IHttpActionResult> ForgotPassword([FromBody]ForgotPasswordModel model)
        {
            string code;
            string callbackUrl;
            string unsubscribeUrl;
            string messageTitle;
            string messageBody;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string mainUserId;
            string subUserId;
            if (SvcContainer.UserSvc.GetMainUserId(model.EmailOrUserName, out mainUserId, out subUserId) == false)
                return NotFound();

            var mainUser = await UserManager.FindByIdAsync(mainUserId);
            var subUser = await UserManager.FindByIdAsync(subUserId);

            if (mainUser == null || subUser == null)
                return BadRequest();

            if (mainUser.EmailConfirmed == false)
            {
                code = await UserManager.GenerateEmailConfirmationTokenAsync(mainUser.Id);

                callbackUrl = "http://quizzocake.com" +
                   "/#/account/confirm-email?userId=" + mainUser.Id + "&code=" + code + "";
                //callbackUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                //   "/#/account/confirm-email?userId=" + mainUser.Id + "&code=" + code + "";
                unsubscribeUrl = "http://quizzocake.com";
                var user = Uow.Users.GetAll()
                    .Where(u => u.LocalAuthUserId == mainUser.Id)
                    .Include(u => u.Profile)
                    .FirstOrDefault();
                var registeredUser = new RegisterStandardModel { Email=user.Email, FirstName=user.Profile.FirstName };

                messageTitle = "Please Confirm Your QuizzOCake Account";
                messageBody = ConfirmEmailBody(registeredUser, callbackUrl, unsubscribeUrl);
               
            }
            else
            {
                string userName = model.EmailOrUserName.Contains("@") ? mainUser.UserName : model.EmailOrUserName;

                code = await UserManager.GeneratePasswordResetTokenAsync(subUser.Id);
                callbackUrl = "http://quizzocake.com" +
                    "/#/account/reset-password?userName=" + userName + "&code=" + code + "";
                unsubscribeUrl = "http://quizzocake.com";
                //callbackUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                //    "/#/account/reset-password?userName=" + userName + "&code=" + code + "";
                messageTitle = "QuizzOCake Password Reset";
                messageBody = ForgotPasswordEmailBody(userName, callbackUrl, unsubscribeUrl);
            }

            await UserManager.SendEmailAsync(mainUser.Id, messageTitle, messageBody);

            return Ok();
        }

        private string ForgotPasswordEmailBody(string userName, string callbackUrl, string unsubscribeUrl)
        {
            var companyName = "<span style='color:#ff6666;'>Quizz</span><span style='color:#3c2d1a;'>O</span><span style='color:#d2691e;'>Cake</span>";
            var emailBody = 
                "<div style='font-family: sans-serif; background-color:#f1f1f1; padding: 30px;'>" +
                    "<div style='max-width: 600px; margin: 0 auto; background-color:#e53d0e; height:52px;'>" +
                        "<div style='max-width: 600px;'>" +
                            "<div style='font-weight: 700; background-color: white; display: inline-block; margin: 6px 0 6px 6px; padding: 10px 20px;'>" +
                                companyName +
                            "</div>" +
                        "</div>" +
                    "</div>" +
                    "<div style='max-width: 600px; margin: 0 auto; background-color: white;'>" +
                        "<div style='padding:20px;'>" +
                            "<div style='color:#5bc0de; font-weight:bolder; font-size:large;'>" +
                                "Thank you for using " + companyName + "!" +
                            "</div>" +
                            "<p>" +
                                "We received a request to reset the password for your account." +
                            "</p>" +
                            "<p>" +
                                "The following username is associated with this email address<br />" +
                                "Username: " + "<strong>" + userName + "</strong>" +
                            "</p>" +
                            "<p>" +
                                "Please click the button below and follow the prompts to reset the password for <strong>" + userName + "</strong>." +
                            "</p>" +
                            "<div style='width: 200px; height: 22px; margin: auto; background-color: #f07a18; text-align: center; padding: 10px 20px; border-radius: 5px; cursor: pointer;'>" +
                                "<a style='text-decoration:none; font-weight: bold; font-size: 18px; color: white;' href=\"" + callbackUrl + "\">Reset My Password</a>" +
                             "</div>" +
                            "<p>" +
                                "Your privacy is important to us. If you feel you have received this message in error, " +
                                "please contact " +
                                "<a href='mailto:support@quizzocake.com?Subject=Question' target='_top'>" +
                                    "support@quizzocake.com" +
                                "</a>" +
                            "</p>" +
                            "<p>" +
                                "Thank you for being a part of our learning community,<br />" +
                                "The " + companyName + " Team" +
                            "</p>" +
                            "<hr style='background-color: lightgray; height: 1px; border: 0;' />" +
                            "<p style='color:gray; font-size:x-small'>" +
                                "This communication contains proprietary information and may be confidential. " +
                                "If you are not the intended recipient, the reading, copying, disclosure " +
                                "or other use of the contents of this email is strictly prohibited " +
                                "and you are instructed to please delete this email immediately." +
                            "</p>" +
                            "<p style='color:gray; font-size:x-small'>" +
                                "You can manage your notifications by clicking <a href=\"" + unsubscribeUrl + "\">unsubscribe</a>." +
                            "</p>" +
                        "</div>" +
                    "</div>" +                
                "</div>";

            return emailBody;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("ResetPassword")]
        public async Task<IHttpActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await UserManager.FindByNameAsync(model.UserName);

            if (user == null)
                return BadRequest(ModelState);
            string code = model.Code.Replace(' ', '+');
            var result = await UserManager.ResetPasswordAsync(user.Id, code, model.Password);
            if (result.Succeeded)
                return Ok();

            return BadRequest();
        }

        // POST api/Account/AddExternalLogin
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin

        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                   OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("RegisterStandard")]
        public async Task<IHttpActionResult> RegisterStandard(RegisterStandardModel model)
        {
            if (!ModelState.IsValid)
            {
                var message = "Internal Server Error. Please try again later.";
                if (ModelState.Values.FirstOrDefault() != null && ModelState.Values.FirstOrDefault().Errors.FirstOrDefault() != null)
                    message = ModelState.Values.FirstOrDefault().Errors.FirstOrDefault().ErrorMessage;

                return BadRequest(message);
            }

            if (SvcContainer.UserSvc.IsEmailUsed(model.Email))
                return BadRequest("Email address already in use");

            if (SvcContainer.UserSvc.IsUserNameUsed(model.UserName))
                return BadRequest("Username already in use");

            var user = new ApplicationUser() { UserName = model.UserName, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);
            //IdentityResult result = UserManager.Create(user, model.Password);

            if (!result.Succeeded)
            {
                //var message = "Internal Server Error. Please try again later.";
                return BadRequest(result.Errors.FirstOrDefault());
            }

            string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            //var callbackUrl = Url.Link("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
            //string callbackUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
            //   "/#/account/confirm-email?userId=" + user.Id + "&code=" + code + "";
            string callbackUrl = "http://quizzocake.com" + "/#/account/confirm-email?userId=" + user.Id + "&code=" + code + "";
            string unsubscribeUrl = "http://quizzocake.com";
            await UserManager.SendEmailAsync(user.Id, "Please Confirm Your QuizzOCake Account", ConfirmEmailBody(model, callbackUrl, unsubscribeUrl));

            var userModel = SvcContainer.UserSvc.CreateNewStandardUser(user.Id, model.UserName, model.Email, model);

            return Ok(userModel);
        }

        private string ConfirmEmailBody(RegisterStandardModel model, string callbackUrl, string unsubscribeUrl)
        {
            var companyName = "<span style='color:#ff6666;'>Quizz</span><span style='color:#3c2d1a;'>O</span><span style='color:#d2691e;'>Cake</span>";
            var emailBody = 
            "<div style='font-family:sans-serif; background-color:#f1f1f1; padding:30px;'>" +
                "<div style='max-width:600px; margin: 0px auto; background-color:#e53d0e; height:52px;'>" +
                    "<div style='max-width:600px;'>" +
                        "<div style='font-weight:700; background-color:white; display:inline-block; margin: 6px 0px 6px 6px; padding:10px 20px;'>" +
                            companyName +
                        "</div>" +
                    "</div>" +
                "</div>" +
                "<div style='max-width:600px; margin: 0px auto; background-color:white;'>" +
                    "<div style='padding:20px;'>" +
                        "<div style='color:#5bc0de; font-weight:bolder; font-size:large;'>" +
                            "Welcome to the " +
                            companyName +
                            " learning community, " + model.FirstName + "!" +
                        "</div>" +
                        "<p>To finish setting up your account, please verify your email address(" + model.Email + ") by clicking this button:</p>" +
                        "<div style='width:160px; height:22px; margin:auto; background-color:#f07a18; text-align:center; padding:10px 20px; border-radius:5px;'>" +
                            "<a style='text-decoration:none; font-weight:bold; font-size:18px; color:white;' href=\"" + callbackUrl + "\">Confirm My Email</a>" +
                        "</div>" +
                        "<p>With your " +
                            companyName +
                            " account, you will be able to:</p>" +
                        "<ul>" +
                            "<li>Create and Publish Quizzes, Flashcards, and Reviewer Notes</li>" +
                            "<li>Search For and Take Available Quizzes</li>" +
                            "<li>Register Quizzlings(Children or Dependents), Assign Quizzes, Monitor Their Progress and Activities</li>" +
                            "<li>Open or Enroll in Quizzrooms, Our Virtual Classrooms</li>" +
                            "<li>Connect with Quizzmates</li>" +
                            "<li>And Many More…</li>" +
                        "</ul>" +
                        "<p>If you have any questions, please email us at " +
                            "<a href='mailto:support@quizzocake.com?Subject=Question' target='_top'>" +
                                "support@quizzocake.com" +
                            "</a></p>" +
                        "<p>Cheers,<br/>The " + companyName + " Team</p>" +
                        "<hr style='background-color: lightgray; height: 1px; border: 0;' />" +
                        "<p style='color:gray; font-size:x-small'>" +
                            "You received this email because you registered on quizzocake.com with the email address " + model.Email + "." +
                            "You can manage your notifications by clicking <a href=\"" + unsubscribeUrl + "\">unsubscribe</a>." +
                        "</p>" +
                    "</div>" +
                "</div>" +
            "</div>";

            return emailBody;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return BadRequest();
            }
            var newCode = code.Replace(' ', '+');
            var result = await UserManager.ConfirmEmailAsync(userId, newCode);
            if (result.Succeeded)
                return Ok();
            else
                return BadRequest();
        }

        [Route("RegisterDependent")]
        public async Task<IHttpActionResult> RegisterDependent(RegisterDependentModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (SvcContainer.UserSvc.IsUserNameUsed(model.UserName))
                return BadRequest("Username already in use");

            var currUserModel = SvcContainer.UserSvc.GetCurrentUserModel();
            var user = new ApplicationUser() { UserName = model.UserName, Email = currUserModel.Email };
            user.EmailConfirmed = true;

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            var userModel = SvcContainer.UserSvc.CreateNewDependentUser(user.Id, user.UserName, user.Email, model);
            userModel.DependentPermission.User = null;
            SvcContainer.UserSvc.ResetStoredUser();

            return Ok(userModel);
        }

        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            GetExternalLoginInfo(model, info);

            var user = new ApplicationUser() { UserName = model.UserName, Email = model.Email };
            user.EmailConfirmed = true;

            var idx = 1;
            while (SvcContainer.UserSvc.IsUserNameUsed(user.UserName))
            {
                user.UserName = user.UserName + idx++;
            }

            if (SvcContainer.UserSvc.IsEmailUsed(user.Email))
            {

            }

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            var userModel = SvcContainer.UserSvc.CreateNewStandardUser(user.Id, model.UserName, model.Email, model);

            return Ok(userModel);
        }

        private void GetExternalLoginInfo(RegisterExternalBindingModel model, ExternalLoginInfo info)
        {
            if (info.Login.LoginProvider == "Facebook")
            {
                var identity = Authentication.GetExternalIdentity(DefaultAuthenticationTypes.ExternalCookie);
                var access_token = identity.FindFirstValue("FacebookAccessToken");
                var fb = new FacebookClient(access_token);
                dynamic myInfo = fb.Get("/me?fields=first_name,last_name,email"); // specify the email field
                info.Email = myInfo.email;
                model.Email = info.Email;
                model.FirstName = myInfo.first_name;
                model.LastName = myInfo.last_name;
            }
            else
            {
                var externalIdentity = HttpContext.Current.GetOwinContext().Authentication.GetExternalIdentityAsync(DefaultAuthenticationTypes.ExternalCookie);
                var firstName = externalIdentity.Result.Claims.Where(c => c.Type == ClaimTypes.GivenName).SingleOrDefault();
                var lastName = externalIdentity.Result.Claims.Where(c => c.Type == ClaimTypes.Surname).SingleOrDefault();
                var email = externalIdentity.Result.Claims.Where(c => c.Type == ClaimTypes.Email).FirstOrDefault();

                model.FirstName = firstName != null ? firstName.Value : "QuizzO";
                model.LastName = lastName != null ? lastName.Value : "Cake";
                model.Email = email != null ? email.Value : "quizz.o.cake@gmail.com";
            }

            model.UserName = model.FirstName + " " + model.LastName;
            model.BirthDate = new DateTime(1980, 1, 1);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        [Route("GetUser")]
        [AllowAnonymous]
        public HttpResponseMessage GetUser(string clientToday)
        {
            var userModel = SvcContainer.UserSvc.GetCurrentUserModel();
            if (userModel.Id == 0) // not logged in
                return Request.CreateResponse(HttpStatusCode.OK, userModel);

            //if (userModel == null)
            //    return Request.CreateResponse(HttpStatusCode.BadRequest, "Not Logged In");

            var user = UserManager.FindById(userModel.LocalAuthUserId);
            if (user == null)
                Request.CreateResponse(HttpStatusCode.NotFound, userModel);

            userModel.IsThirdPartyLogin = user.Logins.Count != 0;
            userModel.IsEmailConfirmed = user.EmailConfirmed;

            SvcContainer.UserSvc.SetClientToday(clientToday);

            return Request.CreateResponse(HttpStatusCode.OK, userModel);
        }

        [HttpGet]
        [Route("IsUserNameAvailable")]
        public HttpResponseMessage IsUserNameAvailable(string userName)
        {
            var user = UserManager.FindByName(userName);

            if (user == null)
                return Request.CreateResponse(HttpStatusCode.OK, new { isAvail = true });

            return Request.CreateResponse(HttpStatusCode.OK, new { isAvail = false });
        }

        [HttpPost]
        [Route("ChangeUserName")]
        public HttpResponseMessage ChangeUserName([FromBody] QuizzerModel model)
        {
            try
            {
                if(HasUpdatePermission(model) == false)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                var userEntity = Uow.Users.GetById(model.Id);
                var user = UserManager.FindById(userEntity.LocalAuthUserId);
                user.UserName = model.UserName;

                UserManager.Update(user);

                userEntity.UserName = user.UserName;
                Uow.Users.Update(userEntity);
                Uow.SaveChanges();
                SvcContainer.UserSvc.ResetStoredUser();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                SvcContainer.LoggingSvc.Log(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        private bool HasUpdatePermission(QuizzerModel model)
        {
            if (model.Id == SvcContainer.UserSvc.GetCurrentUser().Id)
                return true;
            if (SvcContainer.UserSvc.IsDependent(model.Id))
                return true;

            return false;
        }

        private void Test()
        {
            ApplicationDbContext context = ApplicationDbContext.Create();
            // Usermanager.update
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
