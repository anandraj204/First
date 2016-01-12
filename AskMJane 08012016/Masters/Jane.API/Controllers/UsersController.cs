using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Jane.API.Infrastructure.Common;
using Microsoft.AspNet.Identity;

namespace Jane.API.Controllers
{

    [System.Web.Http.RoutePrefix("api/Users")]
    public class UsersController : BaseApiController
    {
   


        // GET api/values
        public HttpResponseMessage Get()
        {
            try
            {
                return Request.CreateResponse(HGContext.Users);
            }
            catch (Exception e)
            {
                _logger.Error("Users.Get", e);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        // GET api/values/5
        public HttpResponseMessage Get(int id)
        {
            try
            {
                return Request.CreateResponse(HGContext.Users.FirstOrDefault(x => x.Id == id));
            }
            catch (Exception e)
            {
                _logger.Error("Users.Get", e);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        [HttpGet]
        public HttpResponseMessage CurrentUser()
        {
            try
            {
                var userId = User.Identity.GetUserId<int>();
                return Request.CreateResponse(HGContext.Users.FirstOrDefault(x => x.Id == userId));
            }
            catch (Exception e)
            {
                _logger.Error("Users.Get", e);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }
      
    }
}
