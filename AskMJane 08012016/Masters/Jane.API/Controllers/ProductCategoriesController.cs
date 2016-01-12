using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AutoMapper;
using Jane.API.Infrastructure.Common;
using Jane.Core.Models;

namespace Jane.API.Controllers
{
    public class ProductCategoriesController : BaseApiController
    {
        public HttpResponseMessage Get()
        {
            var query = from d in HGContext.ProductCategories select d;
            var categories = query.OrderBy(c => c.Name).ToList();
            var dlist = new List<ProductCategoryModel>();
            foreach (var d in categories)
            {
                var dmap = Mapper.Map<ProductCategoryModel>(d);
                dlist.Add(dmap);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK, dlist);
            return response;
        }
        
    }
}
