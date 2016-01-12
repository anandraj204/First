using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harvestgeek.Data.EntityFramework.Models
{
    [Table("public.kickstarter_reward_levels")]
    public partial class kickstarter_reward_levels
    {
        public int id { get; set; }

        public string name { get; set; }

        public string description { get; set; }

        public decimal? price { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }
    }
}
