using System.ComponentModel.DataAnnotations.Schema;

namespace Harvestgeek.Data.EntityFramework.Models
{
    [Table("public.users_roles")]
    public partial class users_roles
    {
        public int user_id { get; set; }

        public int role_id { get; set; }

        public int id { get; set; }

        public virtual role role { get; set; }

        public virtual user user { get; set; }
    }
}
