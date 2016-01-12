using System;
using System.Configuration;
using System.Threading.Tasks;
using Jane.Data.EntityFramework.Contexts;
using Jane.Data.EntityFramework.Managers;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Microsoft.Owin.Security.Jwt;
using Microsoft.Owin.Security.OAuth;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
using Jane.Data.EntityFramework.Entities;
using Microsoft.Owin.Security.Google;

namespace Jane.Web
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(HGContext.Create);
            app.CreatePerOwinContext<HgUserManager>(HgUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);




            var issuer = "askmjane.com";
            string audienceId = ConfigurationManager.AppSettings["as:AudienceId"];
            byte[] audienceSecret = TextEncodings.Base64Url.Decode(ConfigurationManager.AppSettings["as:AudienceSecret"]);

            // Api controllers with an [Authorize] attribute will be validated with JWT
            app.UseJwtBearerAuthentication(
                new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    AllowedAudiences = new[] { audienceId },
                    IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    {
                        new SymmetricKeyIssuerSecurityTokenProvider(issuer, audienceSecret)
                    },
                    Provider = new OAuthBearerAuthenticationProvider
                    {
                        OnApplyChallenge = context =>
                        {
                            context.Response.Redirect("/Login");
                            return Task.FromResult<object>(null);
                        },
                        OnRequestToken = context =>
                        {
                            if (!String.IsNullOrEmpty(context.Request.Cookies["access_token"]))
                            {
                                context.Token = context.Request.Cookies["access_token"];
                            }
                            return Task.FromResult<object>(null);
                        },
                    }
                });


            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                //LoginPath = new PathString("/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<HgUserManager, User, int>(
                   validateInterval: TimeSpan.FromMinutes(30),
                        //regenerateIdentityCallback: (manager, user) => user.GenerateUserIdentityAsync(manager),
                          regenerateIdentityCallback: (manager, user) => user.GenerateUserIdentityAsync((HgUserManager)manager, "JWT"),
                   getUserIdCallback: (id) => (id.GetUserId<int>()))
                }
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            // app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            //app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            app.UseFacebookAuthentication(
               appId: "209374059402667",
               appSecret: "1e5a951a99e8cecbe5c16547b7bc2754");

            app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "684957714095-ecuqbu81nhgen7pramrd2rp7vhtfdfed.apps.googleusercontent.com",
                ClientSecret = "LLTT6NunV3JIMsg-yi9f7BeT"
            });
        }
    }
}