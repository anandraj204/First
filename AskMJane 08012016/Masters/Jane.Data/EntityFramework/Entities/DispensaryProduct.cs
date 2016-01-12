using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jane.Data.EntityFramework.Entities
{
    public class DispensaryProduct:BaseEntity
    {
        public DispensaryProduct(){
            this.DispensaryProductVariants = new HashSet<DispensaryProductVariant>();
        }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public bool IsAvailable { get; set; }
        [Required]
        public bool IsDiscounted { get; set; }
        [Required]
        public bool IsPopular { get; set; }
        [Required]
        public string Slug { get; set; }
        public string LeaflySlug { get; set; }
        public List<File> Photos { get; set; }
        public string ProductAttributes { get; set; }

        public string YouTubeVideoUrl { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ICollection<Effect> Effects { get; set; }
        public virtual ICollection<Symptom> Symptoms { get; set; }

        // Relationships
        
        public int ProductId { get; set; }
        [Required]
        public int DispensaryId { get; set; }
        public virtual Product Product { get; set; }
        public virtual Dispensary Dispensary { get; set; }
        public virtual ICollection<DispensaryProductVariant> DispensaryProductVariants {get;set;}

    }
}
