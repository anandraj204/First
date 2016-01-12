using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harvestgeek.Data.EntityFramework.Models
{
    [Table("public.spree_states")]
    public partial class spree_states
    {
        public int id { get; set; }

        [StringLength(255)]
        public string name { get; set; }

        [StringLength(255)]
        public string abbr { get; set; }

        public int? country_id { get; set; }
    }
}
