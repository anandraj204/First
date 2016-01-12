using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harvestgeek.Data.EntityFramework.Models
{
    [Table("public.station_types")]
    public partial class station_types
    {
        public station_types()
        {
            station_type_sensors = new HashSet<station_type_sensors>();
            stations = new HashSet<station>();
        }

        public int id { get; set; }

        [Required]
        public string name { get; set; }

        public string description { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public virtual ICollection<station_type_sensors> station_type_sensors { get; set; }

        public virtual ICollection<station> stations { get; set; }
    }
}
