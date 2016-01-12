using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AutoMapper;
using Jane.API.Infrastructure.Common;
using Jane.Core.Models;

namespace Jane.API.Controllers
{
    public class EffectsController : BaseApiController
    {
        public HttpResponseMessage Get()
        {
            var query = from d in HGContext.Effects select d;
            var effects = query.OrderBy(e => e.Name).ToList();
            var dlist = new List<EffectModel>();
            foreach (var d in effects)
            {
                var dmap = Mapper.Map<EffectModel>(d);
                dlist.Add(dmap);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK, dlist);
            return response;
        }
    }
}
