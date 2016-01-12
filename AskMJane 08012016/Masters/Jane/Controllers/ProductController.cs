using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using AutoMapper;
using Jane.Core.Models;
using Jane.Data.EntityFramework.Contexts;
using Jane.Web.Infrastructure;

namespace Jane.Web.Controllers
{
    public class ProductController : BaseController
    {
        [System.Web.Http.Route("product/details")]
        [System.Web.Http.HttpGet]
        public ViewResult Details([FromUri] [Required] int id)
        {

            using (var context = new HGContext())
            {
                DispensaryProductModel product = context.DispensaryProducts.AsNoTracking()
                    .Include(p => p.DispensaryProductVariants.Select(v => v.Photos))
                    .Where(p => !p.IsDeleted && !p.Dispensary.IsDeleted && p.Id == id)
                    .Select(item => new DispensaryProductModel()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description,
                        Dispensary = new ThinDispensaryModel()
                        {
                            Name = item.Dispensary.Name,
                            Id = item.Dispensary.Id,
                            Slug = item.Dispensary.Slug
                        },
                        ProductCategory =
                            new ProductCategoryModel() { Id = item.ProductCategory.Id, Name = item.ProductCategory.Name },
                        DispensaryProductVariants =
                            item.DispensaryProductVariants.Select(v => new DispensaryProductVariantModel()
                            {
                                Id = v.Id,
                                IsMasterVariant = v.IsMasterVariant,
                                DisplayOrder = v.DisplayOrder,
                                Name = v.Name,
                                Photos = v.Photos.Select(p => new FileModel() { Id = p.Id }),
                                IsPricedByWeight = v.IsPricedByWeight,
                                VariantQuantity = v.VariantQuantity,
                                VariantPricingJSON = v.VariantPricing
                            }),
                        Effects = item.Effects.Select(e => new EffectModel() { Id = e.Id, Name = e.Name, Description = e.Description }),
                        Symptoms = item.Symptoms.Select(e => new SymptomModel() { Id = e.Id, Name = e.Name, Description = e.Description }),
                        IsAvailable = item.IsAvailable,
                        IsDiscounted = item.IsDiscounted,
                        IsPopular = item.IsPopular,
                        Slug = item.Slug,
                        YouTubeVideoUrl = item.YouTubeVideoUrl
                    }).FirstOrDefault();


                if (product == null)
                    throw new HttpException(404, "Can't find product");

                foreach (DispensaryProductVariantModel variant in product.DispensaryProductVariants)
                {
                    variant.VariantPricing = Mapper.Map<List<VariantPricing>>(variant.VariantPricingJSON);
                }


                return View(product);
            }


        }
    }
}