using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harvestgeek.Data.EntityFramework.Models
{
    [Table("public.series_data")]
    public partial class series_data
    {
        public int id { get; set; }

        public int? series_id { get; set; }

        public decimal? last { get; set; }

        public decimal? min { get; set; }

        public decimal? max { get; set; }

        public decimal? sum { get; set; }

        public int? count { get; set; }

        public DateTime? start_time { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public DateTime? minmax_date { get; set; }

        public DateTime? min_timestamp { get; set; }

        public DateTime? max_timestamp { get; set; }
    }
}
