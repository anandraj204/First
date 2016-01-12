namespace Jane.Core.Models
{
    public class ProductCategoryModel :BaseModel
    {
        public ProductCategoryModel()
        {
            
        }

        public string Name
        {
            get; set;
        }
        public string Color { get; set; }
        public string PhotoUrl { get; set; }

    }
}