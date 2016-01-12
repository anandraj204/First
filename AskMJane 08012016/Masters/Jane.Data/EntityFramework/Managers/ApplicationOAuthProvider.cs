using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;

namespace Jane.Data.EntityFramework.Managers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
#pragma warning disable 1998
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
#pragma warning restore 1998

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (var UserManager = context.OwinContext.GetUserManager<HgUserManager>())
            {
                var user = await UserManager.FindAsync(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect.");
                    return;
                }

                //if (!user.EmailConfirmed)
                //{
                //    context.SetError("invalid_grant", "User did not confirm email.");
                //    return;
                //}

                user.LastLogin = DateTime.UtcNow;
                user.LastIp = context.Request.RemoteIpAddress;
                var result = await UserManager.UpdateAsync(user);

                ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager, "JWT");

                var ticket = new AuthenticationTicket(oAuthIdentity, null);

                context.Validated(ticket);
            }
        }




        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
    }
}
