using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Jane.Core.Models
{
    public class ProductModel : BaseModel
    {
        public new int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Slug { get; set; }
        public string LeaflySlug { get; set; }
        public List<FileModel> Photos { get; set; }
        public JObject Attributes { get; set; }
        public List<EffectModel> Effects { get; set; }
        public List<SymptomModel> Symptoms { get; set; }
        public int ProductCategoryId { get; set; }
        public ProductCategoryModel ProductCategory { get; set; }
        public string YouTubeVideoUrl { get; set; }
    }
}