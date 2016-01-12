using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harvestgeek.Data.EntityFramework.Models
{
    [Table("public.series")]
    public partial class series
    {
        public series()
        {
            series_tags = new HashSet<series_tags>();
        }

        public int id { get; set; }

        [Required]
        public string series_key { get; set; }

        public int station_id { get; set; }

        public int sensor_id { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public bool is_deleted { get; set; }

        public virtual sensor sensor { get; set; }

        public virtual station station { get; set; }

        public virtual ICollection<series_tags> series_tags { get; set; }
    }
}
