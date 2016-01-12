using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harvestgeek.Data.EntityFramework.Models
{
    [Table("public.station_type_sensors")]
    public partial class station_type_sensors
    {
        public int id { get; set; }

        public int station_type_id { get; set; }

        public int sensor_id { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public bool is_deleted { get; set; }

        public virtual sensor sensor { get; set; }

        public virtual station_types station_types { get; set; }
    }
}
