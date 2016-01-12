using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harvestgeek.Data.EntityFramework.Models
{
    [Table("public.tags")]
    public partial class tag
    {
        public tag()
        {
            stations_tags = new HashSet<stations_tags>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string value { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public bool is_deleted { get; set; }

        public virtual ICollection<stations_tags> stations_tags { get; set; }
    }
}
