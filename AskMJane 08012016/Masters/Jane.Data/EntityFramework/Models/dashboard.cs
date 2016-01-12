using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harvestgeek.Data.EntityFramework.Models
{
    [Table("public.dashboards")]
    public partial class dashboard
    {
        public dashboard()
        {
            widgets = new HashSet<widget>();
        }

        public int id { get; set; }

        [Required]
        public string title { get; set; }

        public int user_id { get; set; }

        public bool is_current { get; set; }

        public virtual user user { get; set; }

        public virtual ICollection<widget> widgets { get; set; }
    }
}
