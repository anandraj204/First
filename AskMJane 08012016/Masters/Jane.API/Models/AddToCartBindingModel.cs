namespace Jane.API.Models
{
    public class AddToCartBindingModel
    {
        public int UserId { get; set; }
        public int VariantId { get; set; }
        public int VariantPriceId { get; set; }
    }
}