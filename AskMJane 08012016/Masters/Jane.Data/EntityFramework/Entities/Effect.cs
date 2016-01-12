using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jane.Data.EntityFramework.Entities
{
    public class Effect : BaseEntity
    {
        public Effect()
        {
            this.ProductReviewEffects = new HashSet<ProductReviewEffect>();
        }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [NotMapped]
        public new bool IsDeleted { get; set; }
        [NotMapped]
        public new DateTime CreatedAt { get; set; }
        [NotMapped]
        public new DateTime UpdatedAt { get; set; }

        public virtual ICollection<ProductReviewEffect> ProductReviewEffects { get; private set; }
    }
}
