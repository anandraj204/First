using System.Collections.Generic;

namespace Jane.Core.Models
{
    public class ProductReviewModel :BaseModel
    {
        public ProductReviewModel() { }
        public string Review { get; set; }
        public int Rating { get; set; }
        public ReviewedTypeEnum ReviewedType { get; set; }
        public int ReviewerUserId { get; set; }
        public int ProductId { get; set; }
        public int DispensaryProductId { get; set; }
        public int PurchasedFromDispensaryId { get; set; }
        public ICollection<EffectModel> Effects { get; set; }
        public ICollection<FlavorModel> Flavors { get; set; }
    }

    public enum ReviewedTypeEnum
    {
        Product = 1,
        DispensaryProduct=2
    }
}