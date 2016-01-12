using System.Linq;
using System.Net;
using System.Net.Http;
using AutoMapper;
using Jane.API.Infrastructure.Common;
using Jane.Core.Models;

namespace Jane.API.Controllers
{
    [System.Web.Http.RoutePrefix("api/symptoms")]
    public class SymptomsController : BaseApiController
    {
        public HttpResponseMessage Get()
        {
            var query = from d in HGContext.Symptoms select d;
            var symptoms = query.OrderBy(s => s.Name).ToList();
            var dlist = symptoms.Select(d => Mapper.Map<SymptomModel>(d)).ToList();
            var response = Request.CreateResponse(HttpStatusCode.OK, dlist);
            return response;
        }
    }
}
