using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harvestgeek.Data.EntityFramework.Models
{
    [Table("public.backer_infos")]
    public partial class backer_infos
    {
        public int id { get; set; }

        public int? kickstarter_id { get; set; }

        public string name { get; set; }

        public string email { get; set; }

        public decimal? amount { get; set; }

        public DateTime? pledged_at { get; set; }

        public string pledge_status { get; set; }

        public string notes { get; set; }

        public string ks_token { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        public string shipping { get; set; }

        public int? kickstarter_reward_level_id { get; set; }
    }
}
