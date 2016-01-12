using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Jane.API.Infrastructure.Common;
using Jane.Core.Models;
using Jane.Data.EntityFramework.Entities;

namespace Jane.API.Controllers
{
    [System.Web.Http.RoutePrefix("api/dispensaryproductvariants")]
    public class DispensaryProductVariantsController : BaseApiController
    {
        [HttpPost]
        [Route("updatephotos")]
        public async Task<HttpResponseMessage> UpdatePhotos([FromBody] UpdatePhotoBindingModel model)
        {
            var entity = await HGContext.DispensaryProductVariants.Include(v => v.Photos).FirstOrDefaultAsync(x => x.Id == model.Id);

            if (entity == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            List<int> ids = model.Photos.Select(p => p.Id).ToList();
            List<File> images = HGContext.Files.Where(i => ids.FirstOrDefault(p => p == i.Id) != 0).ToList();

            if (entity.Photos != null && entity.Photos.Any())
            {
                ids = entity.Photos.Select(p => p.Id).ToList();
                images.RemoveAll(i => ids.FirstOrDefault(id => id == i.Id) != 0);
            }

            if (!images.Any())
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            if (entity.Photos == null)
                entity.Photos = new List<File>();

            entity.Photos.AddRange(images);
            var res = await HGContext.SaveChangesAsync();
            if (res > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }
    }

    public class UpdatePhotoBindingModel
    {
        public int Id { get; set; }
        public List<FileModel> Photos { get; set; }
    }
}
