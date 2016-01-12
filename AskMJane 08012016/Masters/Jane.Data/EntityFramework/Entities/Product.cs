using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jane.Data.EntityFramework.Entities
{
    public class Product :BaseEntity
    {
        public Product()
        {
            this.DispensaryProducts = new HashSet<DispensaryProduct>();
        }
        [Required]
        public string Name{ get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Slug { get; set; }
        public string LeaflySlug { get; set; }
        public List<File> Photos { get; set; }
        public string Attributes { get; set; }
        public string YouTubeVideoUrl { get; set; }


        public int ProductCategoryId { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ICollection<DispensaryProduct> DispensaryProducts { get; private set; } 
        public virtual ICollection<Effect> Effects { get; set; } 
        public virtual ICollection<Symptom> Symptoms { get; set; }
    }
}
