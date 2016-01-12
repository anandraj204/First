using System;
using System.Data.Entity;
using System.Threading.Tasks;
using Jane.Core.Services;
using Jane.Data.EntityFramework.Contexts;
using Jane.Data.EntityFramework.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Jane.Data.EntityFramework.Managers
{
 
    public class HgUserManager : UserManager<User, int>
    {
        public HgUserManager(IUserStore<User, int> store)
            : base(store)
        {
       
        }
        public static HgUserManager Create(IdentityFactoryOptions<HgUserManager> options,
            IOwinContext context)
        {
            var manager = new HgUserManager(new HgUserStore(context.Get<HGContext>()));

             manager.UserValidator = new UserValidator<User,int>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
                
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false, 
                RequireDigit = true,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 10;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            //manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            //{
            //    MessageFormat = "Your security code is {0}"
            //});
            //manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            //{
            //    Subject = "Security Code",
            //    BodyFormat = "Your security code is {0}"
            //});
            //manager.EmailService = new EmailService();
            manager.EmailService = new SendGridIdentityEmailService();

            //manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<User, int>(dataProtectionProvider.Create("ASP.NET Identity"))
                    {
                        TokenLifespan = TimeSpan.FromHours(6),
                    };
            }
            return manager;
        }

        public async Task<User> FindByGuidAsync(string guid)
        {
            return await this.Users.FirstOrDefaultAsync(u => u.Guid == guid);
        }

    }
}
