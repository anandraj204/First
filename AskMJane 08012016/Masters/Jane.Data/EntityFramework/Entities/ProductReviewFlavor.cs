using System.ComponentModel.DataAnnotations;

namespace Jane.Data.EntityFramework.Entities
{
    public class ProductReviewFlavor :BaseEntity
    {
        [Required]
        public int ProductReviewId { get; set; }
        [Required]
        public int FlavorId { get; set; }

        public virtual ProductReview ProductReview { get; set; }
        public virtual Flavor Flavor { get; set; }


    }
}
