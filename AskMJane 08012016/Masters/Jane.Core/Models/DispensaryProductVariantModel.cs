using System.Collections.Generic;
using System.Web.Script.Serialization;
using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Jane.Core.Models
{
    public class DispensaryProductVariantModel : BaseModel
    {
        //private string _photoUrl;

        public DispensaryProductVariantModel()
        {
            Guid = System.Guid.NewGuid().ToString();
        }
        public string Name { get; set; }
        public bool IsPricedByWeight { get; set; }
        public decimal VariantQuantity { get; set; }
        public decimal Price { get; set; }
        public string Slug { get; set; }
        public string Guid { get; set; }
        public int DisplayOrder { get; set; }
        public IEnumerable<FileModel> Photos { get; set; }
        public JObject VariantAttributes { get; set; }
        public List<VariantPricing> VariantPricing { get; set; }
        public int DispensaryProductId { get; set; }
        public ThinDispensaryProductModel DispensaryProduct { get; set; }
        public bool IsMasterVariant { get; set; }

        [ScriptIgnore]
        public string VariantPricingJSON { get; set; }
    }

    public class VariantPricing
    {
        public decimal Price { get; set; }
        public decimal Weight { get; set; }
        public string DisplayWeight { get; set; }
    }
 

    public class VariantPricingConverter : ITypeConverter<List<VariantPricing>, string>
    {
        public string Convert(ResolutionContext context)
        {
            return JsonConvert.SerializeObject(context.SourceValue);
        }
    }
    public class StringToVariantPricingConverter : ITypeConverter<string,List<VariantPricing>>
    {
        public List<VariantPricing> Convert(ResolutionContext context)
        {
            return JsonConvert.DeserializeObject<List<VariantPricing>>(context.SourceValue.ToString());
        }
    }

}