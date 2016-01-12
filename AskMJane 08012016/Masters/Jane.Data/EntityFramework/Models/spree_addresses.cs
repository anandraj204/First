using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harvestgeek.Data.EntityFramework.Models
{
    [Table("public.spree_addresses")]
    public partial class spree_addresses
    {
        public int id { get; set; }

        [StringLength(255)]
        public string firstname { get; set; }

        [StringLength(255)]
        public string lastname { get; set; }

        [StringLength(255)]
        public string address1 { get; set; }

        [StringLength(255)]
        public string address2 { get; set; }

        [StringLength(255)]
        public string city { get; set; }

        [StringLength(255)]
        public string zipcode { get; set; }

        [StringLength(255)]
        public string phone { get; set; }

        [StringLength(255)]
        public string state_name { get; set; }

        [StringLength(255)]
        public string alternative_phone { get; set; }

        [StringLength(255)]
        public string company { get; set; }

        public int? state_id { get; set; }

        public int? country_id { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }
    }
}
