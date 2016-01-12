using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harvestgeek.Data.EntityFramework.Models
{
    [Table("public.claims")]
    public partial class claim
    {
        public int id { get; set; }

        public int? user_id { get; set; }

        public int? backer_info_id { get; set; }

        [StringLength(255)]
        public string phone { get; set; }

        public string token { get; set; }

        public int? spree_addresses_id { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public string status { get; set; }

        public string email_status { get; set; }
    }
}
