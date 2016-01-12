using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harvestgeek.Data.EntityFramework.Models
{
    [Table("public.sensors")]
    public partial class sensor
    {
        public sensor()
        {
            series = new HashSet<series>();
            station_type_sensors = new HashSet<station_type_sensors>();
        }

        public int id { get; set; }

        [Required]
        public string sensor_type { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        [Required]
        public string display_name { get; set; }

        public bool is_deleted { get; set; }

        public virtual ICollection<series> series { get; set; }

        public virtual ICollection<station_type_sensors> station_type_sensors { get; set; }
    }
}
