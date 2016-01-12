using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harvestgeek.Data.EntityFramework.Models
{
    [Table("public.stations_tags")]
    public partial class stations_tags
    {
        public int id { get; set; }

        public int station_id { get; set; }

        public int tag_id { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public bool is_deleted { get; set; }

        public virtual station station { get; set; }

        public virtual tag tag { get; set; }
    }
}
