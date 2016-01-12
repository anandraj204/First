using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harvestgeek.Data.EntityFramework.Models
{
    [Table("public.sessions")]
    public partial class session
    {
        public int id { get; set; }

        public string session_token { get; set; }

        public int? user_id { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }
    }
}
