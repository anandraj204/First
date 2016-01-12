using System.ComponentModel.DataAnnotations;

namespace Jane.Data.EntityFramework.Entities
{
    public class ProductReviewEffect : BaseEntity
    {
        [Required]
        public int EffectId { get; set; }
        [Required]
        public int ProductReviewId { get; set; }

        public virtual ProductReview ProductReview { get; set; }
        public virtual Effect Effect { get; set; }
    }
}
