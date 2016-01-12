using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harvestgeek.Data.EntityFramework.Models
{
    [Table("public.spree_products")]
    public partial class spree_products
    {
        public int id { get; set; }

        [Required]
        [StringLength(255)]
        public string name { get; set; }

        public string description { get; set; }

        public DateTime? available_on { get; set; }

        public DateTime? deleted_at { get; set; }

        [StringLength(255)]
        public string permalink { get; set; }

        public string meta_description { get; set; }

        [StringLength(255)]
        public string meta_keywords { get; set; }

        public int? tax_category_id { get; set; }

        public int? shipping_category_id { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }
    }
}
