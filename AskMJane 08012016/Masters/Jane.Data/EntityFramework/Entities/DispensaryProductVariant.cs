using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jane.Data.EntityFramework.Entities
{
    public class DispensaryProductVariant : BaseEntity
    {
        public DispensaryProductVariant()
        {
            this.DispensaryProductVariantOrders = new HashSet<DispensaryProductVariantOrder>();

            Guid = System.Guid.NewGuid().ToString();
        }

        public string Name { get; set; }
        [Required]
        public bool IsPricedByWeight { get; set; }
        public decimal VariantQuantity { get; set; }
        public decimal Price { get; set; }
        public string Slug { get; set; }
        public string LeaflySlug { get; set; }
        public string Guid { get; set; }
        [Required]
        public int DisplayOrder { get; set; }
        public List<File> Photos { get; set; }
        public string VariantAttributes { get; set; }
        [Required]
        public string VariantPricing { get; set; }

        public int DispensaryProductId { get; set; }
        public bool IsMasterVariant { get; set; }
        public virtual DispensaryProduct DispensaryProduct { get; set; }

        public virtual ICollection<DispensaryProductVariantOrder> DispensaryProductVariantOrders { get; private set; }
    }

}
