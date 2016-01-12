using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AskMJane.LeaflyScraper.Models
{
    public class LeaflyReview
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string StrainDisplayName { get; set; }
        public string LeaflySlug { get; set; }
        public string Category { get; set; }
        public string UserName { get; set; }
        public string UserUrl { get; set; }
        public decimal Rating { get; set; }
        public string Review { get; set; }
        public List<string> Effects { get; set; }
        public List<string> Flavors { get; set; }
        public List<string> Forms { get; set; }
        public string AcquiredFrom { get; set; }
        public string AcquiredFromUrl { get; set; }

 
    }
}
