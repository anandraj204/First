using Jane.Data.EntityFramework.Contexts;
using Jane.Data.EntityFramework.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Jane.Data.EntityFramework.Managers
{
    public class HgRoleManager: RoleManager<Role,int>
    {
        public HgRoleManager(IRoleStore<Role, int> roleStore)
            : base(roleStore)
        {
        }

        public static HgRoleManager Create(IdentityFactoryOptions<HgRoleManager> options, IOwinContext context)
        {
            var appRoleManager = new HgRoleManager(new HgRoleStore(context.Get<HGContext>()));
 
            return appRoleManager;
        }
    }
}
