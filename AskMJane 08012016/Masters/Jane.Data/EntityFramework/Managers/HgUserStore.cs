using System.Data.Entity;
using Jane.Data.EntityFramework.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Jane.Data.EntityFramework.Managers
{
    public class HgUserStore : UserStore<User,Role,int,UserLogin,UserRole,UserClaim>
    {
        public HgUserStore(DbContext context) : base(context)
        {
            
        }
     
    }
}
