using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harvestgeek.Data.EntityFramework.Models
{
    [Table("public.claims_products")]
    public partial class claims_products
    {
        public int id { get; set; }

        public int? claim_id { get; set; }

        public int? spree_product_id { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }
    }
}
