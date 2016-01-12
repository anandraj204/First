using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jane.Data.EntityFramework.Entities
{
    public class ProductReview :BaseEntity
    {
     
        [Required]
        public string Review { get; set; }
        [Required]
        public int Rating { get; set; }

        [Required]
        public int ReviewedType { get; set; }


        [Required]
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int DispensaryProductId { get; set; }
        public int PurchasedFromDispensaryId { get; set; }

        public virtual Product Product { get; set; }
        public virtual DispensaryProduct DispensaryProduct{get;set;    }
        public virtual User User { get; set; }
        [ForeignKey("PurchasedFromDispensaryId")]
        public virtual Dispensary Dispensary{get;set;}


    }
}
