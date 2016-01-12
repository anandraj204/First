using AutoMapper;
using Jane.API.Infrastructure.Common;
using Jane.Core.Models;
using Jane.Data.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Jane.API.Controllers
{
    [System.Web.Http.RoutePrefix("api/Dispensaries")]
    public class DispensariesController : BaseApiController
    {
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("InRangeOfZipcode")]
        public async Task<HttpResponseMessage> InRangeOfZipcode([FromUri] string zipcode)
        {
            var entities = await HGContext.Dispensaries.Where(x => x.ApprovalZipCodes.Any(z => z.Code.Equals(zipcode)) && !x.IsDeleted).ToListAsync();
            var dlist = entities.Select(d => Mapper.Map<DispensaryModel>(d)).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, dlist);
        }

        [System.Web.Http.Authorize(Roles = "GlobalAdmin")]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("HideDispensary")]
        public async Task<HttpResponseMessage> HideDispensary([FromUri] int id)
        {
            try
            {
                var dispensary = await HGContext.Dispensaries.SingleOrDefaultAsync(d => d.Id == id && !d.IsDeleted);
                dispensary.IsHidden = true;
                HGContext.Entry(dispensary).State = EntityState.Modified;
                await HGContext.SaveChangesAsync();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [System.Web.Http.Authorize(Roles = "GlobalAdmin")]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("ShowDispensary")]
        public async Task<HttpResponseMessage> ShowDispensary([FromUri] int id)
        {
            try
            {
                var dispensary = await HGContext.Dispensaries.SingleOrDefaultAsync(d => d.Id == id && !d.IsDeleted);
                dispensary.IsHidden = false;
                HGContext.Entry(dispensary).State = EntityState.Modified;
                await HGContext.SaveChangesAsync();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [Route("get")]
        public HttpResponseMessage Get(DispensaryDataTableParams filter)
        {
            IQueryable<Dispensary> query = null;
            if (User.IsInRole("GlobalAdmin"))
            {
                query = from d in HGContext.Dispensaries.Include(d => d.Address) where !d.IsDeleted select d;
            }
            else
            {
                query = from d in HGContext.Dispensaries.Include(d => d.Address) where !d.IsHidden && !d.IsDeleted select d;
            }

            if (!String.IsNullOrEmpty(filter.name))
                query = query.Where(i => i.Name.Contains(filter.name));
            if (!String.IsNullOrEmpty(filter.state))
                query = query.Where(i => i != null && i.Address.State != null && i.Address.State.Contains(filter.state));

            if (filter.iSortCol_0 == 0)
                query = filter.sSortDir_0 == "desc" ? query.OrderByDescending(d => d.Name) : query.OrderBy(d => d.Name);
            if (filter.iSortCol_0 == 1)
                query = filter.sSortDir_0 == "desc" ? query.OrderByDescending(d => d.Address.State) : query.OrderBy(d => d.Address.State);
            if (filter.iSortCol_0 == 2)
                query = filter.sSortDir_0 == "desc" ? query.OrderByDescending(d => d.Type) : query.OrderBy(d => d.Type);
            if (filter.iSortCol_0 == 3)
                query = filter.sSortDir_0 == "desc" ? query.OrderByDescending(d => d.EmailAddress) : query.OrderBy(d => d.EmailAddress);
            if (filter.iSortCol_0 == 4)
                query = filter.sSortDir_0 == "desc" ? query.OrderByDescending(d => d.PhoneNumber) : query.OrderBy(d => d.PhoneNumber);

            var dispensaries = query.AsNoTracking().Skip(filter.iDisplayStart).Take(filter.iDisplayLength).Select(d => new DispensaryModel()
            {
                Id = d.Id,
                Address = new AddressModel()
                {
                    Name = d.Address.Name,
                    Address1 = d.Address.Address1,
                    Address2 = d.Address.Address2,
                    Country = d.Address.Country,
                    City = d.Address.City,
                    FormattedAddress = d.Address.FormattedAddress,
                    Id = d.Address.Id,
                    Latitude = d.Address.Latitude,
                    Longitude = d.Address.Longitude,
                    PhoneNumber = d.Address.PhoneNumber,
                    State = d.Address.State,
                    Zip = d.Address.Zip,
                },
                Description = d.Description,
                EmailAddress = d.EmailAddress,
                PhoneNumber = d.PhoneNumber,
                HasDelivery = d.HasDelivery,
                HasPickup = d.HasPickup,
                HasStorefront = d.HasStorefront,
                HasScheduledDelivery = d.HasScheduledDelivery,
                HoursAndInfo = d.HoursAndInfo,
                IsHidden = d.IsHidden,
                IsPrivate = d.IsPrivate,
                Name = d.Name,
                Slug = d.Slug,
                Type = d.Type,
                HoursOfOperationString = d.HoursOfOperation,
                ApprovalZipCodesCollection = d.ApprovalZipCodes.Select(z => z.Code),
                DeliveryZipCodesCollection = d.DeliveryZipCodes.Select(z => z.Code)
            }).ToList();

            foreach (DispensaryModel dispensary in dispensaries)
            {
                if (dispensary.ApprovalZipCodesCollection != null)
                    dispensary.ApprovalZipCodes = String.Join(", ", dispensary.ApprovalZipCodesCollection);
                if (dispensary.DeliveryZipCodesCollection != null)
                    dispensary.DeliveryZipCodes = String.Join(", ", dispensary.DeliveryZipCodesCollection);

                dispensary.HoursOfOperation = Mapper.Map<List<HoursOfOperation>>(dispensary.HoursOfOperationString);
            }

            var count = query.AsNoTracking().Count();

            GridModel<DispensaryModel> gritItems = new GridModel<DispensaryModel>()
            {
                aaData = dispensaries,
                iTotalDisplayRecords = count,
                iTotalRecords = count,
                sEcho = filter.sEcho
            };

            var response = Request.CreateResponse(HttpStatusCode.OK, gritItems);
            return response;
        }

        [HttpGet]
        [Route("getShortDispensaries")]
        public HttpResponseMessage GetShortDispensaries()
        {
            IQueryable<Dispensary> query = null;
            if (User.IsInRole("GlobalAdmin"))
            {
                query = from d in HGContext.Dispensaries where !d.IsDeleted select d;
            }
            else
            {
                query = from d in HGContext.Dispensaries where !d.IsHidden && !d.IsDeleted select d;
            }
            var dispensaries = query.Select(d => new DispensaryModel { Id = d.Id, Name = d.Name }).AsNoTracking().ToList();
            var dlist = dispensaries.ToList();
            var response = Request.CreateResponse(HttpStatusCode.OK, dlist);
            return response;
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("GetMarkets")]
        public HttpResponseMessage GetMarkets()
        {
            var query = HGContext.Orders.Where(x => x.IsCheckedOut == true && x.IsDeleted == false).Select(o => o.UserId);
            var patients = HGContext.Users.Include(p => p.Address).Where(u => query.Contains(u.Id));
            List<String> states = new List<string>();

            foreach (var patient in patients)
            {
                if (patient.Address != null)
                {
                    if (!String.IsNullOrEmpty(patient.Address.State))
                        states.Add(patient.Address.State);
                }
            }


            return Request.CreateResponse(HttpStatusCode.OK, states);
        }

        // GET: api/Dispensaries/5
        public HttpResponseMessage Get(int id)
        {
            var dispensary = HGContext.Dispensaries.AsNoTracking().FirstOrDefault(x => x.Id == id && !x.IsDeleted);
            var mapped = Mapper.Map<DispensaryModel>(dispensary);
            return Request.CreateResponse(mapped);
        }

        // POST: api/Dispensaries
        public HttpResponseMessage Post([FromBody]DispensaryModel dispensary)
        {
            try
            {
                var entity = Mapper.Map<Dispensary>(dispensary);
                var id = AddDispensary(dispensary, entity);
                return Request.CreateResponse(HGContext.Dispensaries.FirstOrDefault(x => x.Id == id));
            }
            catch (Exception e)
            {
                _logger.Error("Dispensaries.Post", e);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        private int AddDispensary(DispensaryModel dispensary, Dispensary entity)
        {
            if (dispensary.ApprovalZipCodes != null && dispensary.ApprovalZipCodes.Any())
            {
                foreach (string zip in dispensary.ApprovalZipCodes.Split(','))
                {
                    string trimmedZip = zip.Trim();
                    if (!String.IsNullOrEmpty(trimmedZip))
                        entity.ApprovalZipCodes.Add(HGContext.ZipCodes.Add(new ZipCode() { Code = trimmedZip }));
                }
            }
            if (dispensary.DeliveryZipCodes != null && dispensary.DeliveryZipCodes.Any())
            {
                foreach (string zip in dispensary.DeliveryZipCodes.Split(','))
                {
                    string trimmedZip = zip.Trim();
                    if (!String.IsNullOrEmpty(trimmedZip))
                        entity.DeliveryZipCodes.Add(HGContext.ZipCodes.Add(new ZipCode() { Code = trimmedZip }));
                }
            }

            HGContext.Dispensaries.Add(entity);
            var id = HGContext.SaveChanges();
            return id;
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("Update")]
        public async Task<HttpResponseMessage> Update([FromBody]DispensaryModel dispensary)
        {
            try
            {
                var entity = await HGContext.Dispensaries.FirstOrDefaultAsync(x => x.Id == dispensary.Id);
                entity.Name = dispensary.Name;
                entity.Slug = dispensary.Name;
                entity.Type = dispensary.Type;
                entity.PhoneNumber = dispensary.PhoneNumber;
                entity.LeaflySlug = dispensary.LeaflySlug ?? dispensary.Name;
                entity.Description = dispensary.Description;
                entity.HoursOfOperation = Mapper.Map<string>(dispensary.HoursOfOperation);
                entity.HoursAndInfo = dispensary.HoursAndInfo;
                entity.EmailAddress = dispensary.EmailAddress;
                entity.HasDelivery = dispensary.HasDelivery;
                entity.HasPickup = dispensary.HasPickup;
                entity.HasStorefront = dispensary.HasStorefront;
                entity.IsCaregiver = dispensary.IsCaregiver;
                entity.IsPrivate = dispensary.IsPrivate;

                if (dispensary.Address != null && (!string.IsNullOrEmpty(dispensary.Address.Zip) || !string.IsNullOrEmpty(dispensary.Address.City) ||
                    !string.IsNullOrEmpty(dispensary.Address.State)))
                {
                    if (entity.Address == null)
                    {
                        entity.Address = new Address();
                    }

                    entity.Address.Address1 = dispensary.Address.Address1;
                    entity.Address.Address2 = dispensary.Address.Address2;
                    entity.Address.City = dispensary.Address.City;
                    entity.Address.State = dispensary.Address.State;
                    entity.Address.Zip = dispensary.Address.Zip;
                    entity.Address.Name = dispensary.Name;
                    entity.Address.Latitude = dispensary.Address.Latitude;
                    entity.Address.Longitude = dispensary.Address.Longitude;
                    entity.Address.FormattedAddress = dispensary.Address.FormattedAddress;
                }

                HGContext.ZipCodes.RemoveRange(entity.ApprovalZipCodes);

                if (dispensary.ApprovalZipCodes != null)
                {
                    foreach (string zip in dispensary.ApprovalZipCodes.Split(','))
                    {
                        string trimmedZip = zip.Trim();
                        if (!String.IsNullOrEmpty(trimmedZip))
                            entity.ApprovalZipCodes.Add(HGContext.ZipCodes.Add(new ZipCode() { Code = trimmedZip }));
                    }
                }

                HGContext.ZipCodes.RemoveRange(entity.DeliveryZipCodes);

                if (dispensary.DeliveryZipCodes != null)
                {
                    foreach (string zip in dispensary.DeliveryZipCodes.Split(','))
                    {
                        string trimmedZip = zip.Trim();
                        if (!String.IsNullOrEmpty(trimmedZip))
                            entity.DeliveryZipCodes.Add(HGContext.ZipCodes.Add(new ZipCode() { Code = trimmedZip }));
                    }
                }

                var id = await HGContext.SaveChangesAsync();
                if (id > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Unable to save dispensary");
                }
            }
            catch (Exception e)
            {
                _logger.Error("Dispensaries.Post", e);
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        [Authorize(Roles = "GlobalAdmin")]
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("Delete")]
        public void Delete([FromUri]int id)
        {
            Dispensary dispensary = HGContext.Dispensaries.Include(d => d.DispensaryProducts).FirstOrDefault(d => d.Id == id);
            foreach (DispensaryProduct product in dispensary.DispensaryProducts)
            {
                product.IsDeleted = true;
            }
            dispensary.IsDeleted = true;

            HGContext.SaveChanges();
        }
    }

    public class DispensaryDataTableParams : jQueryDataTableParams
    {
        public string state { get; set; }
        public string name { get; set; }
    }
}
