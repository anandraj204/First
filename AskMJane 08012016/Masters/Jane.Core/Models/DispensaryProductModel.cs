using System.Collections.Generic;

namespace Jane.Core.Models
{
    public class DispensaryProductModel : BaseModel
    {
        private int? _categoryId;

        public DispensaryProductModel()
        {
            
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDiscounted { get; set; }
        public bool IsPopular { get; set; }
        public string Slug { get; set; }
        public string LeaflySlug { get; set; }
        public string ProductAttributes { get; set; }
        public int DispensaryId { get; set; }
        public int ProductId { get; set; }
        public string YouTubeVideoUrl { get; set; }

        public IEnumerable<DispensaryProductVariantModel> DispensaryProductVariants { get; set; }

        private DispensaryProductVariantModel _masterVariant;
        public DispensaryProductVariantModel MasterVariant
        {
            get
            {
                if (_masterVariant == null)
                {
                    foreach (var v in DispensaryProductVariants)
                    {
                        if (v.IsMasterVariant)
                        {
                            _masterVariant = v;
                        }
                    }
                }
                return _masterVariant;
            }
            set
            {
                _masterVariant = value;
            }
        }

        public ProductModel Product { get; set; }
        public ThinDispensaryModel Dispensary { get; set; }

        public IEnumerable<EffectModel> Effects { get; set; }
        public IEnumerable<SymptomModel> Symptoms { get; set; }

        public int ProductCategoryId
        {
            get
            {
                if (!_categoryId.HasValue)
                    return ProductCategory == null ? 0 : ProductCategory.Id;
                else
                    return _categoryId.Value;
            }
            set { _categoryId = value; }
        }

        public ProductCategoryModel ProductCategory { get; set; }
    }

    public class ThinDispensaryProductModel :BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDiscounted { get; set; }
        public bool IsPopular { get; set; }
        public string Slug { get; set; }
        public string LeaflySlug { get; set; }
        public string Photos { get; set; }
        public string ProductAttributes { get; set; }
        public int DispensaryId { get; set; }
        public int ProductId { get; set; }

        public ProductModel Product { get; set; }
        public ThinDispensaryModel Dispensary { get; set; }
    }

  
  
}