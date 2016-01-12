using Jane.Data.EntityFramework.Contexts;
using Jane.Data.EntityFramework.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Jane.Data.EntityFramework.Managers
{
    public class HgRoleStore : RoleStore<Role, int, UserRole>
    {
        public HgRoleStore(HGContext context)
            : base(context)
        {
        }
    } 
}