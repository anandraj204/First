using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harvestgeek.Data.EntityFramework.Models
{
    [Table("public.station_station_groups")]
    public partial class station_station_groups
    {
        public int id { get; set; }

        public int station_id { get; set; }

        public int station_group_id { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public bool is_deleted { get; set; }

        public virtual station_groups station_groups { get; set; }

        public virtual station station { get; set; }
    }
}
