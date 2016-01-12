using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harvestgeek.Data.EntityFramework.Models
{
    [Table("public.spree_countries")]
    public partial class spree_countries
    {
        public int id { get; set; }

        [StringLength(255)]
        public string iso_name { get; set; }

        [StringLength(255)]
        public string iso { get; set; }

        [StringLength(255)]
        public string iso3 { get; set; }

        [StringLength(255)]
        public string name { get; set; }

        public int? numcode { get; set; }

        public bool? states_required { get; set; }
    }
}
