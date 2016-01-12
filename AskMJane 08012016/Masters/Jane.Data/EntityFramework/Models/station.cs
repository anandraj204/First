using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harvestgeek.Data.EntityFramework.Models
{
    [Table("public.stations")]
    public partial class station
    {
        public station()
        {
            series = new HashSet<series>();
            station_addresses = new HashSet<station_addresses>();
            station_station_groups = new HashSet<station_station_groups>();
            stations_tags = new HashSet<stations_tags>();
        }

        public int id { get; set; }

        public int? user_id { get; set; }

        public int station_type_id { get; set; }

        public string display_name { get; set; }

        public string display_location { get; set; }

        public DateTime? last_active { get; set; }

        public string guid { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public bool is_deactivated { get; set; }

        public string status { get; set; }

        public bool is_deleted { get; set; }

        public virtual ICollection<series> series { get; set; }

        public virtual ICollection<station_addresses> station_addresses { get; set; }

        public virtual ICollection<station_station_groups> station_station_groups { get; set; }

        public virtual station_types station_types { get; set; }

        public virtual ICollection<stations_tags> stations_tags { get; set; }

        public virtual user user { get; set; }
    }
}
