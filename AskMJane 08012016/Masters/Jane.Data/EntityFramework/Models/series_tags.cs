using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harvestgeek.Data.EntityFramework.Models
{
    [Table("public.series_tags")]
    public partial class series_tags
    {
        public int id { get; set; }

        public int series_id { get; set; }

        public int tag_id { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public bool is_deleted { get; set; }

        public virtual series series { get; set; }
    }
}
