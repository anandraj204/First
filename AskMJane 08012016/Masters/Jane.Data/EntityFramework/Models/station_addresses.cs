using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harvestgeek.Data.EntityFramework.Models
{
    [Table("public.station_addresses")]
    public partial class station_addresses
    {
        public int id { get; set; }

        public int station_id { get; set; }

        [Required]
        [StringLength(255)]
        public string ip { get; set; }

        [Required]
        [StringLength(255)]
        public string mac { get; set; }

        public int pid { get; set; }

        public bool? is_current { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public bool is_deleted { get; set; }

        public virtual station station { get; set; }
    }
}
