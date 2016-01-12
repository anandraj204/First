using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Jane.API.Infrastructure.Common;
using Jane.Data.EntityFramework.Entities;
using File = Jane.Data.EntityFramework.Entities.File;

namespace Jane.API.Controllers
{
    [System.Web.Http.RoutePrefix("api/images")]
    public class ImagesController : BaseApiController
    {


        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("upload")]
        public async Task<HttpResponseMessage> Upload(HttpRequestMessage request)
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                return Request.CreateResponse(HttpStatusCode.UnsupportedMediaType);
            }

            HttpFileCollection files = HttpContext.Current.Request.Files;

            if (files.Count != 0)
            {
                byte[] content;
                using (MemoryStream ms = new MemoryStream())
                {
                    files[0].InputStream.CopyTo(ms);
                    content = ms.ToArray();
                }

                File image = new File()
                {
                    ContentType = files[0].ContentType,
                    Content = content
                };

                image = HGContext.Files.Add(image);

                await HGContext.SaveChangesAsync();

                return Request.CreateResponse(HttpStatusCode.OK, image.Id);
            }

            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get([FromUri] int id)
        {
            var etag = Request.Headers.IfNoneMatch;

            if (etag.Any())
                return Request.CreateResponse(HttpStatusCode.NotModified);

            var data = from image in HGContext.Files
                       where image.Id == id
                       select image;

            File img = data.AsNoTracking().SingleOrDefault();

            if (img == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new ByteArrayContent(img.Content);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(img.ContentType);
            response.Content.Headers.ContentLength = img.Content.Length;
            response.Content.Headers.LastModified = DateTime.Now.AddDays(-2).ToUniversalTime();

            response.Headers.ETag = new EntityTagHeaderValue("\"" + img.Content.GetHashCode() + "\"");


            return response;
        }
    }
}
