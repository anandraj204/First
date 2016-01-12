using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jane.Data.EntityFramework.Entities
{
    public class DispensaryProductVariantOrder :BaseEntity
    {
        [Required]
        public bool IsPricedByWeight{get;set;}
        public decimal Weight { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }


        [Required]
        public decimal Quantity { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }



        [Required]
        public int DispensaryProductVariantId { get; set; }
        [Required]
        public int OrderId { get; set; }

        public virtual DispensaryProductVariant DispensaryProductVariant { get; set; }
        public virtual Order Order { get; set; }

        [NotMapped]
        public int VariantPriceId { get; set; }
    }
}
