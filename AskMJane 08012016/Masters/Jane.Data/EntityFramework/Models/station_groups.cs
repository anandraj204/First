using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harvestgeek.Data.EntityFramework.Models
{
    [Table("public.station_groups")]
    public partial class station_groups
    {
        public station_groups()
        {
            station_station_groups = new HashSet<station_station_groups>();
        }

        public int id { get; set; }

        [Required]
        public string name { get; set; }

        public string description { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public bool is_deleted { get; set; }

        public virtual ICollection<station_station_groups> station_station_groups { get; set; }
    }
}
