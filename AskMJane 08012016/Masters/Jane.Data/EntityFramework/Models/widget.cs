using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harvestgeek.Data.EntityFramework.Models
{
    [Table("public.widgets")]
    public partial class widget
    {
        public widget()
        {
            chart_configs = new HashSet<chart_configs>();
        }

        public int id { get; set; }

        public int dashboard_id { get; set; }

        public int user_id { get; set; }

        [Required]
        public string title { get; set; }

        [Required]
        public string type { get; set; }

        public int sizeX { get; set; }

        public int sizeY { get; set; }

        public int col { get; set; }

        public int row { get; set; }

        public virtual ICollection<chart_configs> chart_configs { get; set; }

        public virtual dashboard dashboard { get; set; }

        public virtual user user { get; set; }
    }
}
