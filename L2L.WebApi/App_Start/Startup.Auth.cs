using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;
using L2L.WebApi.Providers;
using L2L.WebApi.Models;
using Microsoft.Owin.Security.Facebook;
using System.Security.Claims;
using System.Threading.Tasks;

namespace L2L.WebApi
{
    public partial class Startup
    {
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        public static string PublicClientId { get; private set; }

        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions());
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configure the application for OAuth based flow
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");


            ConfigureFacebook(app);
            //app.UseFacebookAuthentication(
            //   appId: Config.FbAppId,
            //   appSecret: Config.FbAppSecret);

            ConfigureGoogle(app);
            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "931357503970-4as9br19iit2km7u8n83b7rai1mu554t.apps.googleusercontent.com",
            //    ClientSecret = "0fMEVXn-Y-gjs1mjFVzGE1Lo"
            //});
        }

        private static void ConfigureFacebook(IAppBuilder app)
        {
//#if DEBUG
//            var appId = Config.FbAppTestId;
//            var appSecret = Config.FbAppTestSecret;
//#else
            var appId = Config.FbAppId;
            var appSecret = Config.FbAppSecret;
//#endif

            var options = new FacebookAuthenticationOptions()
            {
                AppId = appId,
                AppSecret = appSecret,
                Provider = new FacebookAuthenticationProvider
                {
                    OnAuthenticated = (context) =>
                    {
                        // Add the email id to the claim
                        context.Identity.AddClaim(new System.Security.Claims.Claim("FacebookAccessToken", context.AccessToken));
                        //context.Identity.AddClaim(new Claim(ClaimTypes.Email, context.Email));
                        return Task.FromResult(0);
                    }
                }
            };

            options.Scope.Add("email");
            app.UseFacebookAuthentication(options);
        }

        private static void ConfigureGoogle(IAppBuilder app)
        {
            var options = new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "931357503970-4as9br19iit2km7u8n83b7rai1mu554t.apps.googleusercontent.com",
                ClientSecret = "0fMEVXn-Y-gjs1mjFVzGE1Lo",
                Provider = new GoogleOAuth2AuthenticationProvider
                {
                    OnAuthenticated = (context) =>
                    {
                        // Add the email id to the claim
                        context.Identity.AddClaim(new Claim(ClaimTypes.Email, context.Email));
                        return Task.FromResult(0);
                    }
                }
            };

            options.Scope.Add("email");

            app.UseGoogleAuthentication(options);
        }
    }
}
