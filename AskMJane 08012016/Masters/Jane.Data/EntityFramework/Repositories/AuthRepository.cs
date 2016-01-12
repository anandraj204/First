using System;
using System.Threading.Tasks;
using Harvestgeek.Data.EntityFramework.Contexts;
using Harvestgeek.Data.EntityFramework.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Harvestgeek.Data.EntityFramework.Repositories
{
    public class AuthRepository :IDisposable
    {
        private HGContext _ctx;

        private UserManager<User,int> _userManager;

        public AuthRepository()
        {
            _ctx = new HGContext();
            _userManager = new UserManager<User,int>(new UserStore<User,int>(_ctx));
        }

        public async Task<IdentityResult> RegisterUser(string username, string password)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = username
            };

            var result = await _userManager.CreateAsync(user, password);

            return result;
        }

     

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();

        }
    }
}
