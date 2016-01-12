using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harvestgeek.Data.EntityFramework.Models
{
    [Table("public.chart_configs")]
    public partial class chart_configs
    {
        public int id { get; set; }

        public int widget_id { get; set; }

        public DateTime start_timestamp { get; set; }

        public DateTime end_timestamp { get; set; }

        public DateTime nav_start_timestamp { get; set; }

        public DateTime nav_end_timestamp { get; set; }

        public virtual widget widget { get; set; }
    }
}
