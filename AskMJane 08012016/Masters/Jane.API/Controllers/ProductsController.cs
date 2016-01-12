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

namespace Jane.API.Controllers
{
    [RoutePrefix("api/products")]
    public class ProductsController : BaseApiController
    {
        [HttpGet]
        public HttpResponseMessage Get(int? categoryId)
        {
            var query = HGContext.Products.AsQueryable().Where(product => product.IsDeleted == false);
            if (categoryId.HasValue)
            {
                query = query.Where(product => product.ProductCategoryId == categoryId.Value);
            }

            var products = query.Select(product => new {Id = product.Id, Name = product.Name});

            return Request.CreateResponse(HttpStatusCode.OK, products);
        }

        [HttpGet]
        public HttpResponseMessage Get(int id)
        {
            Product product = HGContext.Products.Include(p => p.Photos).FirstOrDefault(prod => prod.Id == id && prod.IsDeleted == false);
            var result = Mapper.Map<ProductModel>(product);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("list")]
        public HttpResponseMessage List(int categoryId, int page)
        {
            var pageSize = 20;
            var products =
                HGContext.Products.Where(prod => prod.IsDeleted == false)
                    .Include(prod => prod.Photos)
                    .OrderBy(prod => prod.Name)
                    .Skip((page - 1)*pageSize)
                    .Take(pageSize);
            var result = Mapper.Map<List<ProductModel>>(products);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<HttpResponseMessage> Delete([FromUri] int id)
        {
            var entity = await HGContext.Products.FirstOrDefaultAsync(x => x.Id == id);
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



        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("Update")]
        public async Task<HttpResponseMessage> Update([FromBody]ProductModel model)
        {
            try
            {
                var entity =
                    await
                        HGContext.Products.Include(prod => prod.Photos).FirstOrDefaultAsync(x => x.Id == model.Id);

                entity.Name = model.Name;
                entity.Slug = model.Slug;
                entity.YouTubeVideoUrl = YouTubeVideoHelper.TransformYouTubeVideo(model.YouTubeVideoUrl);
                entity.LeaflySlug = model.LeaflySlug ?? model.Name;
                entity.Description = model.Description;
                entity.UpdatedAt = DateTimeOffset.UtcNow;
                entity.ProductCategory = HGContext.ProductCategories.FirstOrDefault(c => c.Id == model.ProductCategoryId);

                List<int> ids = model.Effects.Select(e => e.Id).ToList();
                entity.Effects.Clear();
                entity.Effects = HGContext.Effects.Where(e => ids.Contains(e.Id)).ToList();

                ids = model.Symptoms.Select(e => e.Id).ToList();
                entity.Symptoms.Clear();
                entity.Symptoms = HGContext.Symptoms.Where(e => ids.Contains(e.Id)).ToList();

                ids = model.Photos.Select(p => p.Id).ToList();
                entity.Photos.Clear();
                entity.Photos = HGContext.Files.Where(i => ids.Contains(i.Id)).ToList();

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

        public async Task<HttpResponseMessage> Post([FromBody]ProductModel product)
        {
            try
            {
                var entity = Mapper.Map<Product>(product);

                List<int> ids;

                ids = entity.Effects.Select(e => e.Id).ToList();
                entity.Effects =
                    HGContext.Effects.Where(e => ids.Contains(e.Id)).ToList();

                entity.ProductCategory =
                    HGContext.ProductCategories.FirstOrDefault(c => c.Id == product.ProductCategoryId);

                ids = product.Photos.Select(p => p.Id).ToList();
                entity.Photos.Clear();
                entity.Photos = HGContext.Files.Where(i => ids.Contains(i.Id)).ToList();

                HGContext.Products.Add(entity);
                var id = await HGContext.SaveChangesAsync();
                if (id > 0)
                {
                    return Request.CreateResponse(Mapper.Map<ProductModel>(HGContext.Products.FirstOrDefault(x => x.Id == id)));
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, product);
                }
            }
            catch (Exception e)
            {
                _logger.Error("Products.Post", e);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

    }
}
