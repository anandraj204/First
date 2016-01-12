using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harvestgeek.Data.EntityFramework.Models
{
    [Table("public.rewards_products")]
    public partial class rewards_products
    {
        public int id { get; set; }

        public int? kickstarter_reward_level_id { get; set; }

        public int? spree_product_id { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }
    }
}
