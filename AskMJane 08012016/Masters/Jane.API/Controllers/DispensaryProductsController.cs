using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Jane.API.Helpers;
using Jane.API.Infrastructure.Common;
using Jane.Core.Models;
using Jane.Data.EntityFramework.Entities;
using WebGrease.Css.Extensions;

namespace Jane.API.Controllers
{
    [RoutePrefix("api/DispensaryProducts")]
    public class DispensaryProductsController : BaseApiController
    {
        // GET: api/DispensaryProducts/
        [HttpGet]
        public async Task<HttpResponseMessage> Get()
        {
            var query = from d in HGContext.DispensaryProducts select d;
            var products = await query.AsNoTracking().Where(x => x.IsDeleted == false && !x.Dispensary.IsDeleted && !x.Dispensary.IsPrivate && !x.Dispensary.IsHidden).ToListAsync();
            var dlist = products.Select(d => Mapper.Map<DispensaryProductModel>(d));
            var response = Request.CreateResponse(HttpStatusCode.OK, dlist);
            return response;
        }


        [HttpGet]
        public async Task<HttpResponseMessage> Get(int id)
        {
            var entity = await HGContext.DispensaryProducts.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && !x.Dispensary.IsDeleted && !x.Dispensary.IsPrivate && !x.Dispensary.IsHidden);

            if (entity == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            var mapped = Mapper.Map<DispensaryProductModel>(entity);
            return Request.CreateResponse(HttpStatusCode.OK, mapped);
        }

        [System.Web.Http.Route("getProducts")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get([FromUri]ProductsFilter filter)
        {
            if (filter == null)
                return Request.CreateResponse(HttpStatusCode.OK);

            // select all to create query
            var query =
                HGContext.DispensaryProducts.AsNoTracking()
                    .Include(p => p.DispensaryProductVariants.Select(v => v.Photos))
                    .Where(p => !p.IsDeleted && !p.Dispensary.IsDeleted && !p.Dispensary.IsPrivate && !p.Dispensary.IsHidden && p.Dispensary.ApprovalZipCodes.Any(z => z.Code.Equals(filter.zip)));

            if (filter.productCategories != null)
                query = query.Where(p => filter.productCategories.Any(c => c == p.ProductCategory.Id));

            if (filter.effects != null)
                query = query.Where(p => p.Effects.Any(e => filter.effects.Any(fe => fe == e.Id)));

            if (filter.symptoms != null)
                query = query.Where(p => p.Symptoms.Any(e => filter.symptoms.Any(fe => fe == e.Id)));

            var products = query.Take(15 * (filter.page + 1)).Select(p => new DispensaryProductModel()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Dispensary = new ThinDispensaryModel() { Name = p.Dispensary.Name, Id = p.Dispensary.Id },
                Effects = p.Effects.Select(e => new EffectModel() { Name = e.Name, Description = e.Description }),
                Symptoms = p.Symptoms.Select(e => new SymptomModel() { Name = e.Name }),
                ProductCategory = p.ProductCategory == null ? null : new ProductCategoryModel()
                {
                    Name = p.ProductCategory.Name,
                    Color = p.ProductCategory.Color
                },
                DispensaryProductVariants = p.DispensaryProductVariants.Select(v => new DispensaryProductVariantModel()
                    {
                        Id = v.Id,
                        IsMasterVariant = v.IsMasterVariant,
                        DisplayOrder = v.DisplayOrder,
                        Name = v.Name,
                        Photos = v.Photos.Select(ph => new FileModel() { Id = ph.Id }),
                        IsPricedByWeight = v.IsPricedByWeight,
                        VariantQuantity = v.VariantQuantity,
                        VariantPricingJSON = v.VariantPricing
                    }),

            }).ToList();

            foreach (DispensaryProductModel product in products)
            {
                foreach (DispensaryProductVariantModel variant in product.DispensaryProductVariants)
                {
                    variant.VariantPricing = Mapper.Map<List<VariantPricing>>(variant.VariantPricingJSON);
                }
            }

            var response = Request.CreateResponse(HttpStatusCode.OK, products);
            return response;
        }

        [System.Web.Http.Route("search")]
        [System.Web.Http.HttpGet]
        public HttpResponseMessage Search([FromUri]string term, [FromUri]string zip)
        {
            // select all to create query
            var query = HGContext.DispensaryProducts.AsNoTracking().Where(p => !p.IsDeleted && !p.Dispensary.IsDeleted && !p.Dispensary.IsPrivate
                && !p.Dispensary.IsHidden && p.Dispensary.ApprovalZipCodes.Any(z => z.Code.Equals(zip)) && p.Name.Contains(term));

            var products = query.Take(10).ToList();
            var mappedlist = new List<AutocompeteModel>();
            foreach (var p in products)
            {
                mappedlist.Add(new AutocompeteModel() { label = p.Name, value = p.Id.ToString() });
            }
            var response = Request.CreateResponse(HttpStatusCode.OK, mappedlist);
            return response;
        }

        [HttpPost]
        [Route("getByDispensary")]
#pragma warning disable 1998
        public async Task<HttpResponseMessage> GetByDispensary(InventoryDataTableParams filter)
        {
            var query =
                HGContext.DispensaryProducts.AsNoTracking()
                    .Include(p => p.DispensaryProductVariants.Select(v => v.Photos))
                    .Where(x => !x.IsDeleted && !x.Dispensary.IsDeleted);

            if (filter.dispensaryId > 0)
                query = query.Where(i => i.DispensaryId == filter.dispensaryId);

            if (filter.categoryId > 0)
                query = query.Where(i => i.ProductCategory.Id == filter.categoryId);

            if (!String.IsNullOrEmpty(filter.name))
                query = query.Where(i => i.Name.Contains(filter.name));


            if (filter.iSortCol_0 == 0)
                query = filter.sSortDir_0 == "desc" ? query.OrderByDescending(d => d.Product.Name) : query.OrderBy(d => d.Product.Name);
            if (filter.iSortCol_0 == 1)
                query = filter.sSortDir_0 == "desc" ? query.OrderByDescending(d => d.Product.ProductCategory.Name) : query.OrderBy(d => d.Product.ProductCategory.Name);
            if (filter.iSortCol_0 == 2)
                query = filter.sSortDir_0 == "desc" ? query.OrderByDescending(d => d.Product.Description) : query.OrderBy(d => d.Product.Description);


            var list = query.Skip(filter.iDisplayStart).Take(filter.iDisplayLength);
            var count = query.Count();
            var mappedlist = list.Select(item => new DispensaryProductModel()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                ProductCategory = item.ProductCategory == null ? null : new ProductCategoryModel() { Id = item.ProductCategory.Id, Name = item.ProductCategory.Name },
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
                Effects = item.Effects.Select(e => new EffectModel() { Id = e.Id, Name = e.Name }),
                Symptoms = item.Symptoms.Select(e => new SymptomModel() { Id = e.Id, Name = e.Name }),
                IsAvailable = item.IsAvailable,
                IsDiscounted = item.IsDiscounted,
                IsPopular = item.IsPopular,
                Slug = item.Slug,
                YouTubeVideoUrl = item.YouTubeVideoUrl
            }).ToList();

            foreach (DispensaryProductModel product in mappedlist)
            {
                foreach (DispensaryProductVariantModel variant in product.DispensaryProductVariants)
                {
                    variant.VariantPricing = Mapper.Map<List<VariantPricing>>(variant.VariantPricingJSON);
                }
            }

            GridModel<DispensaryProductModel> gritItems = new GridModel<DispensaryProductModel>()
            {
                aaData = mappedlist,
                iTotalDisplayRecords = count,
                iTotalRecords = count,
                sEcho = filter.sEcho
            };

            return Request.CreateResponse(HttpStatusCode.OK, gritItems);
        }
#pragma warning restore 1998

        [HttpPost]
        [Route("create")]
        // POST: api/Dispensaries
        public async Task<HttpResponseMessage> Post([FromBody]DispensaryProductModel product)
        {
            try
            {
                foreach (var v in product.DispensaryProductVariants)
                {
                    foreach (var p in v.VariantPricing)
                    {
                        if (v.IsPricedByWeight)
                        {
                            var oz = p.Weight / 28M;
                            if (1 / 28M == oz)
                            {
                                p.DisplayWeight = "Gram";
                            }
                            else if (.125M == oz)
                            {
                                p.DisplayWeight = "Eighth";
                            }
                            else if (.25M == oz)
                            {
                                p.DisplayWeight = "Quarter";
                            }
                            else if (.5M == oz)
                            {
                                p.DisplayWeight = "Half";
                            }
                            else if (1M == oz)
                            {
                                p.DisplayWeight = "Ounce";
                            }
                            else
                            {
                                p.DisplayWeight = String.Format("{0}g", p.Weight);
                            }
                        }
                        else
                        {
                            if (1M == p.Weight)
                            {
                                p.DisplayWeight = "Each";
                            }
                            else
                            {
                                p.DisplayWeight = String.Format("{0}-pack", p.Weight);
                            }
                        }
                    }
                }
                var entity = Mapper.Map<DispensaryProduct>(product);

                List<int> ids;
                foreach (DispensaryProductVariant variant in entity.DispensaryProductVariants)
                {
                    ids = variant.Photos.Select(p => p.Id).ToList();
                    variant.Photos = HGContext.Files.Where(i => ids.Contains(i.Id)).ToList();
                }

                ids = entity.Effects.Select(e => e.Id).ToList();
                entity.Effects =
                    HGContext.Effects.Where(e => ids.Contains(e.Id)).ToList();
                ids = entity.Symptoms.Select(e => e.Id).ToList();
                entity.Symptoms =
                    HGContext.Symptoms.Where(e => ids.Contains(e.Id)).ToList();


                entity.YouTubeVideoUrl = YouTubeVideoHelper.TransformYouTubeVideo(entity.YouTubeVideoUrl);

                entity.ProductCategory =
                    HGContext.ProductCategories.FirstOrDefault(cat => cat.Id == product.ProductCategoryId);

                HGContext.DispensaryProducts.Add(entity);
                var id = await HGContext.SaveChangesAsync();
                if (id > 0)
                {
                    return Request.CreateResponse(Mapper.Map<DispensaryProductModel>(HGContext.DispensaryProducts.FirstOrDefault(x => x.Id == id)));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, product);
                }
            }
            catch (Exception e)
            {
                _logger.Error("Dispensaries.Post", e);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("Update")]
        public async Task<HttpResponseMessage> Update([FromBody]DispensaryProductModel model)
        {
            try
            {
                foreach (var v in model.DispensaryProductVariants)
                {
                    foreach (var p in v.VariantPricing)
                    {
                        if (v.IsPricedByWeight)
                        {
                            var oz = p.Weight / 28M;
                            if (1 / 28M == oz)
                            {
                                p.DisplayWeight = "Gram";
                            }
                            else if (.125M == oz)
                            {
                                p.DisplayWeight = "Eighth";
                            }
                            else if (.25M == oz)
                            {
                                p.DisplayWeight = "Quarter";
                            }
                            else if (.5M == oz)
                            {
                                p.DisplayWeight = "Half";
                            }
                            else if (1M == oz)
                            {
                                p.DisplayWeight = "Ounce";
                            }
                            else
                            {
                                p.DisplayWeight = String.Format("{0}g", p.Weight);
                            }
                        }
                        else
                        {
                            if (1M == p.Weight)
                            {
                                p.DisplayWeight = "Each";
                            }
                            else
                            {
                                p.DisplayWeight = String.Format("{0}-pack", p.Weight);
                            }
                        }
                    }
                }
                var entity =
                    await
                        HGContext.DispensaryProducts.Include(product => product.DispensaryProductVariants)
                            .Include(product => product.DispensaryProductVariants.Select(variant => variant.Photos))
                            .FirstOrDefaultAsync(x => x.Id == model.Id);

                entity.Name = model.Name;
                entity.Slug = model.Slug;
                entity.LeaflySlug = model.LeaflySlug ?? model.Name;
                entity.Description = model.Description;
                entity.UpdatedAt = DateTimeOffset.UtcNow;
                entity.YouTubeVideoUrl = YouTubeVideoHelper.TransformYouTubeVideo(model.YouTubeVideoUrl);
                entity.ProductCategory = HGContext.ProductCategories.FirstOrDefault(c => c.Id == model.ProductCategoryId);

                List<int> ids = model.Effects.Select(e => e.Id).ToList();
                entity.Effects.Clear();
                entity.Effects = HGContext.Effects.Where(e => ids.Contains(e.Id)).ToList();

                ids = model.Symptoms.Select(e => e.Id).ToList();
                entity.Symptoms.Clear();
                entity.Symptoms = HGContext.Symptoms.Where(e => ids.Contains(e.Id)).ToList();

                entity.DispensaryProductVariants.ForEach(v => v.IsDeleted = true);

                foreach (DispensaryProductVariantModel variant in model.DispensaryProductVariants)
                {
                    DispensaryProductVariant variantEntity =
                        entity.DispensaryProductVariants.FirstOrDefault(v => v.Id == variant.Id);
                    if (variantEntity != null)
                    {
                        variantEntity.IsDeleted = false;

                        ids = variant.Photos.Select(p => p.Id).ToList();
                        List<File> images =
                            HGContext.Files.Where(i => ids.FirstOrDefault(p => p == i.Id) != 0).ToList();

                        if (variantEntity.Photos == null)
                            variantEntity.Photos = new List<File>();

                        variantEntity.Photos.Clear();
                        variantEntity.Photos.AddRange(images);

                        variantEntity.VariantPricing = Mapper.Map<string>(variant.VariantPricing);
                    }
                }

                var statusCode = await HGContext.SaveChangesAsync();
                if (statusCode > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unable to save product");
                }
            }
            catch (Exception e)
            {
                _logger.Error("DispensaryProduct.Update", e);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("Delete")]
        public async Task<HttpResponseMessage> Delete([FromUri] int id)
        {
            var entity = await HGContext.DispensaryProducts.FirstOrDefaultAsync(x => x.Id == id);
            entity.IsDeleted = true;
            entity.UpdatedAt = DateTimeOffset.UtcNow;
            var res = await HGContext.SaveChangesAsync();
            if (res > 0)
            {

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unable to remove product");
            }

        }

    }

    public class AdminProductsFilter
    {
        public int dispensaryId { get; set; }
        public int page { get; set; }
        public int category { get; set; }
        public string name { get; set; }
    }

    public class ProductsFilter
    {
        public List<int> productCategories { get; set; }
        public List<int> symptoms { get; set; }
        public List<int> effects { get; set; }

        public int page { get; set; }

        public string zip { get; set; }
    }

    public class InventoryDataTableParams : jQueryDataTableParams
    {
        public int dispensaryId { get; set; }
        public int categoryId { get; set; }
        public string name { get; set; }
    }

    /// <summary>
    /// Class that encapsulates most common parameters sent by DataTables plugin
    /// </summary>
    public class jQueryDataTableParams
    {
        /// <summary>
        /// Request sequence number sent by DataTable,
        /// same value must be returned in response
        /// </summary>       
        public string sEcho { get; set; }

        /// <summary>
        /// Text used for filtering
        /// </summary>
        public string sSearch { get; set; }

        /// <summary>
        /// Number of records that should be shown in table
        /// </summary>
        public int iDisplayLength { get; set; }

        /// <summary>
        /// First record that should be shown(used for paging)
        /// </summary>
        public int iDisplayStart { get; set; }

        /// <summary>
        /// Number of columns in table
        /// </summary>
        public int iColumns { get; set; }

        /// <summary>
        /// Number of columns that are used in sorting
        /// </summary>
        public int iSortingCols { get; set; }

        /// <summary>
        /// Comma separated list of column names
        /// </summary>
        public string sColumns { get; set; }

        /// <summary>
        /// Пользовательские данные
        /// </summary>
        public string CustomData { get; set; }

        public int iSortCol_0 { get; set; }

        public string sSortDir_0 { get; set; }
    }

}
