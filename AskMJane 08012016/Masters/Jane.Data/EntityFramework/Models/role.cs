using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harvestgeek.Data.EntityFramework.Models
{
    [Table("public.roles")]
    public partial class role
    {
        public role()
        {
            users_roles = new HashSet<users_roles>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public virtual ICollection<users_roles> users_roles { get; set; }
    }
}
