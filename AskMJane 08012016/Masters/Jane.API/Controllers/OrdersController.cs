using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Jane.API.Infrastructure.Common;
using Jane.Core.Models;
using Jane.Data.EntityFramework.Entities;

namespace Jane.API.Controllers
{
    [System.Web.Http.RoutePrefix("api/Orders")]
    public class OrdersController : BaseApiController
    {
        [HttpGet]
        [System.Web.Http.Route("dispensary/{id}")]
        public async Task<HttpResponseMessage> GetByDispensary(int? id)
        {
            IQueryable<Order> query = null;
            if (id.HasValue && id.Value > 0)
            {
                query =
                    HGContext.Orders.Include(o => o.Dispensary).Where(x => x.IsCheckedOut == true && x.DispensaryId == id && x.IsDeleted == false);
            }
            else
            {
                query =
                    HGContext.Orders.Include(o => o.Dispensary).Where(x => x.IsCheckedOut == true && x.IsDeleted == false);
            }
            var orders = await query.ToListAsync();
            var list = orders.Select(o => Mapper.Map<OrderModel>(o)).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, list);

        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetByMarket(string stateName)
        {
            IQueryable<Order> query = null;
            List<Order> orders = new List<Order>();
            if (string.IsNullOrEmpty(stateName))
            {
                query =
                    HGContext.Orders.Include(o => o.Dispensary).Where(x => x.IsCheckedOut == true && x.IsDeleted == false);
                orders = await query.ToListAsync();
            }
            else
            {
                query =
                    HGContext.Orders.Include(o => o.Dispensary).Where(x => x.IsCheckedOut == true && x.IsDeleted == false);
                foreach (var order in query)
                {
                    if (order.User.Address != null && order.User.Address.State == stateName)
                    {
                        orders.Add(order);
                    }
                }
            }

            var list = orders.Select(o => Mapper.Map<OrderModel>(o)).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, list);

        }
    }
}
