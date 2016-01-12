using Microsoft.AspNet.Identity.EntityFramework;

namespace Jane.Data.EntityFramework.Entities
{
    public class Role : IdentityRole<int,UserRole>
    {
        public const string GlobalAdmin = "GlobalAdmin";
        public const string DispensaryManager = "Dispensary Manager";
        public Role() { } 

        public Role(string name) { Name = name; } 

    }
}
