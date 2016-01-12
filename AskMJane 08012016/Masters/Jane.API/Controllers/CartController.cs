using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Jane.API.Infrastructure.Common;
using Jane.API.Models;
using Jane.Core.Models;
using Jane.Core.Services;
using Jane.Data.EntityFramework.Entities;
using Newtonsoft.Json;

namespace Jane.API.Controllers
{
    [System.Web.Http.RoutePrefix("api/Cart")]
    public class CartController : BaseApiController
    {
        [HttpGet]
        [System.Web.Http.Route("GetByUserId")]
        public async Task<HttpResponseMessage> GetByUserId(int userid)
        {
            var entity = await GetUserCart(userid);
            var mapped = Mapper.Map<OrderModel>(entity);
            return Request.CreateResponse(HttpStatusCode.OK, mapped);
        }

        [HttpGet]
        [System.Web.Http.Route("GetByUserToken")]
        public async Task<HttpResponseMessage> GetByUserToken([FromUri]string usertoken)
        {
            var userEntity = await HGContext.Users.FirstOrDefaultAsync(x => x.Guid == usertoken);
            var cartEntity = await GetUserCart(userEntity.Id);
            var mappedCart = Mapper.Map<OrderModel>(cartEntity);
            return Request.CreateResponse(HttpStatusCode.OK, mappedCart);
        }

        [HttpGet]
        [System.Web.Http.Route("GetCountByUserToken")]
        public async Task<HttpResponseMessage> GetCountByUserToken([FromUri]string usertoken)
        {
            var userEntity = await HGContext.Users.FirstOrDefaultAsync(x => x.Guid == usertoken);
            var cartEntity =
                HGContext.Orders.OrderByDescending(x => x.UpdatedAt)
                    .Where(x => x.UserId == userEntity.Id && x.IsCheckedOut == false).Sum(o => o.DispensaryProductVariantOrders.Count);
            
            return Request.CreateResponse(HttpStatusCode.OK, cartEntity);
        }

        private async Task<Order> GetUserCart(int userid)
        {
            return await HGContext.Orders.OrderByDescending(x => x.UpdatedAt).FirstOrDefaultAsync(x => x.UserId == userid && x.IsCheckedOut == false);
        }

        [HttpPost]
        [System.Web.Http.Route("Checkout")]
        public async Task<HttpResponseMessage> Checkout([FromBody] CheckoutBindingModel model)
        {
            var user = await HGContext.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);
            var cart = await HGContext.Orders.FirstOrDefaultAsync(x => x.Id == model.OrderId);

            if (model.TransactionAmount > 0)
            {
                StripeService stripe = new StripeService();
                var success = await stripe.ChargeCustomer(model.StripeToken, model.TransactionAmount);
                if (!success)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Payment has been failed");
                }

                var wallet = await HGContext.Wallets.FirstOrDefaultAsync(x => x.Id == model.WalletId);
                if (wallet != null && user.WalletId == wallet.Id)
                {
                    wallet.Transactions.Add(new Transaction()
                    {
                        TransactedAt = DateTimeOffset.UtcNow,
                        TransactionAmount = model.TransactionAmount/100M,
                        TransactionCharge = 0M,
                        TransactionStatus = TransactionStatusEnum.CLEARED,
                        TransactionType = TransactionTypeEnum.DEPOSIT
                    });
                }
            }

            cart.TotalPrice = 0;
            foreach (var v in cart.DispensaryProductVariantOrders)
            {
                cart.TotalPrice = cart.TotalPrice + v.Quantity * v.UnitPrice;
            }
            if (model.DeliveryTypeId == DeliveryTypeEnumModel.DELIVERY &&
                (model.DeliveryAddress == null || string.IsNullOrWhiteSpace(model.DeliveryAddress.FormattedAddress)))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Address");
            }
            if (model.DeliveryTypeId == DeliveryTypeEnumModel.DELIVERY)
            {
                cart.DeliveryType = DeliveryTypeEnum.DELIVERY;
                if (cart.DeliveryAddress == null)
                {
                    cart.DeliveryAddress = new Address();
                }
                if (user.DeliveryAddress == null)
                {
                    user.DeliveryAddress = new Address();
                }
                cart.DeliveryAddress.Address1 = model.DeliveryAddress.Address1;
                cart.DeliveryAddress.Address2 = model.DeliveryAddress.Address2;
                cart.DeliveryAddress.City = model.DeliveryAddress.City;
                cart.DeliveryAddress.State = model.DeliveryAddress.State;
                cart.DeliveryAddress.Zip = model.DeliveryAddress.Zip;
                cart.DeliveryAddress.Country = "USA";
                cart.DeliveryAddress.FormattedAddress = model.DeliveryAddress.FormattedAddress;
                cart.DeliveryAddress.Latitude = model.DeliveryAddress.Latitude;
                cart.DeliveryAddress.Longitude = model.DeliveryAddress.Longitude;
                //user address
                user.DeliveryAddress.Address1 = model.DeliveryAddress.Address1;
                user.DeliveryAddress.Address2 = model.DeliveryAddress.Address2;
                user.DeliveryAddress.City = model.DeliveryAddress.City;
                user.DeliveryAddress.State = model.DeliveryAddress.State;
                user.DeliveryAddress.Zip = model.DeliveryAddress.Zip;
                user.DeliveryAddress.Country = "USA";
                user.DeliveryAddress.FormattedAddress = model.DeliveryAddress.FormattedAddress;
                user.DeliveryAddress.Latitude = model.DeliveryAddress.Latitude;
                user.DeliveryAddress.Longitude = model.DeliveryAddress.Longitude;
                cart.DeliveryCharge = 0;

                // Do Onfleet Integration here

                var onfleet = new OnfleetService();
                OnfleetRecipient recipient = null;
                if (string.IsNullOrWhiteSpace(user.OnfleetRecipientId))
                {
                    recipient = onfleet.FindRecipientByPhone(user.PhoneNumber);
                    if (recipient == null)
                    {
                        recipient = onfleet.CreateRecipient(user.FirstName + " " + user.LastName, user.PhoneNumber,
                            "", true, true);
                    }
                    if (recipient == null)
                    {
                        _logger.ErrorFormat("Unable to create onfleet recipient for order");
                        return Request.CreateResponse(HttpStatusCode.BadRequest,
                            "Unable to process delivery orders at this time.");
                    }
                    user.OnfleetRecipientId = recipient.id;
                }

                //var pickupdestination = onfleet.CreateDestination(Mapper.Map<AddressModel>(cart.Dispensary.Address));
                //if (pickupdestination == null)
                //{
                //  return Request.CreateResponse(HttpStatusCode.BadRequest, "Unable to process delivery orders at this time.");
                //}
                var destination = onfleet.CreateDestination(Mapper.Map<AddressModel>(cart.DeliveryAddress));
                if (destination == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        "Unable to process delivery orders at this time.");
                }
                cart.OnfleetDestinationId = destination.id;
                var recipientid = user.OnfleetRecipientId ?? recipient.id;
                var task = onfleet.CreateTask(onfleet.GetOnfleetOrganization(), destination, recipientid, "", false,
                    null,
                    false);
                if (task == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        "Unable to process delivery orders at this time.");
                }
                cart.OnfleetTaskId = task.id;
                cart.OnfleetTrackingURL = task.trackingURL;
            }
            else
            {
                cart.DeliveryCharge = 0;
                cart.DeliveryType = DeliveryTypeEnum.PICKUP;
            }

            cart.DispensaryId =
                cart.DispensaryProductVariantOrders.FirstOrDefault()
                    .DispensaryProductVariant.DispensaryProduct.DispensaryId;


            if (model.PaymentTypeId == PaymentTypeEnumModel.CASH)
            {
                cart.PaymentType = PaymentTypeEnum.CASH;
            }
            else
            {
                var total = cart.TotalPrice;
                if (model.UseCredits)
                {
                    if (total >= user.Wallet.Credit)
                    {
                        total -= user.Wallet.Credit;
                        user.Wallet.Credit = 0;
                    }
                    else
                    {
                        user.Wallet.Credit -= total;
                        total = 0;
                    }
                }

                if (total - model.TransactionAmount != 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Sorry, we have a billing error. Contact support, please.");


                cart.PaymentType = PaymentTypeEnum.CREDIT;
                user.Wallet.Transactions.Add(new Transaction() { TransactionAmount = total });
            }

            cart.CheckedOutAt = DateTimeOffset.UtcNow;
            cart.IsCheckedOut = true;

            var result = HGContext.SaveChanges();
            if (result == 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Unable to process order at this time");
            }
            //var sendgrind = new SendGridEmailService();
            //if (cart.DeliveryType == DeliveryTypeEnum.DELIVERY)
            //{
            //    sendgrind.SendEmailAsync(user.Email,
            //        String.Format("AskMJane Order  #{0} Confirmation", cart.Id),
            //        "Your order has been confirmed and will be on it's way soon.");
            //}
            //else
            //{
            //    sendgrind.SendEmailAsync(user.Email,
            //        String.Format("AskMJane Order #{0} Confirmation", cart.Id),
            //        String.Format(
            //            "Your order has been confirmed and will be waiting for you at {0}\r\n{1}\r\n{2}, {3} {4}",
            //            cart.Dispensary.Name, cart.Dispensary.Address.Address1, cart.Dispensary.Address.City,
            //            cart.Dispensary.Address.State, cart.Dispensary.Address.Zip));
            //}
            return Request.CreateResponse(HttpStatusCode.OK);
        }



        [
            HttpPost]
        [System.Web.Http.Route("RemoveFromCart")]
        public async Task<HttpResponseMessage> RemoveFromCart(int id)
        {
            var entity = await HGContext.DispensaryProductVariantOrders.FirstOrDefaultAsync(x => x.Id == id);
            var removeEntity = HGContext.DispensaryProductVariantOrders.Remove(entity);
            var orderentity = await HGContext.Orders.FirstOrDefaultAsync(x => x.Id == entity.OrderId);
            orderentity.UpdatedAt = DateTimeOffset.UtcNow;
            var response = await HGContext.SaveChangesAsync();
            if (response > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, id);
            }
        }

        [HttpPost]
        [System.Web.Http.Route("UpdateQuantity")]
        public async Task<HttpResponseMessage> UpdateQuantity(int id, int qty)
        {
            var entity = await HGContext.DispensaryProductVariantOrders.FirstOrDefaultAsync(x => x.Id == id);
            // var removeEntity = HGContext.DispensaryProductVariantOrders.Remove(entity);
            entity.Quantity = qty;
            entity.TotalPrice = qty * entity.UnitPrice;
            entity.UpdatedAt = DateTimeOffset.UtcNow;
            var response = await HGContext.SaveChangesAsync();
            var orderentity = await HGContext.Orders.FirstOrDefaultAsync(x => x.Id == entity.OrderId);
            orderentity.UpdatedAt = DateTimeOffset.UtcNow;
            var response2 = await HGContext.SaveChangesAsync();

            if (response > 0 && response2 > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, id);
            }
        }
        [HttpPost]
        [System.Web.Http.Route("AddToCart")]
        public async Task<HttpResponseMessage> AddToCart(AddToCartBindingModel model)
        {
            var cartEntity = await GetUserCart(model.UserId);
            if (cartEntity == null)
            {
                cartEntity = new Order()
                {
                    UserId = model.UserId,
                    IsCheckedOut = false,
                    IsConfirmed = false,
                    IsReceived = false,
                    OrderReferenceId = Guid.NewGuid().ToString(),
                    TotalPrice = 0,
                    PaymentType = PaymentTypeEnum.CREDIT,
                    DeliveryType = DeliveryTypeEnum.DELIVERY,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                };
                HGContext.Orders.Add(cartEntity);
                var addcartresult = await HGContext.SaveChangesAsync();
                if (addcartresult == 0)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, model);
                }
            }

            // Add check to make sure product is in stock
            var variant = await HGContext.DispensaryProductVariants.FirstOrDefaultAsync(x => x.Id == model.VariantId && x.IsDeleted == false);
            if (variant == null)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, "Product is no longer available.");
                //return BadRequest("Product is no longer available.");
            }

            var addedToCart = false;
            if (cartEntity.DispensaryProductVariantOrders.Count > 0)
            {
                //Search through existing products in cart to see if product and packaging (weight/units) already exists
                foreach (var vo in cartEntity.DispensaryProductVariantOrders)
                {
                    if (vo.DispensaryProductVariantId == model.VariantId)
                    {
                        //Same Variant and Packaging -- Increase Quantity
                        if (vo.VariantPriceId == model.VariantPriceId)
                        {
                            vo.Quantity++;
                            vo.TotalPrice = vo.Quantity * vo.UnitPrice;
                            vo.UpdatedAt = DateTimeOffset.UtcNow;
                            addedToCart = true;
                            break;
                        }
                    }
                }
            }

            //if wasn't found above add to cart
            if (!addedToCart)
            {

                var variantpricing = JsonConvert.DeserializeObject<List<VariantPricing>>(variant.VariantPricing);
                var variantToAdd = new DispensaryProductVariantOrder()
                {
                    Order = cartEntity,
                    IsPricedByWeight = variant.IsPricedByWeight,
                    Weight = variantpricing[model.VariantPriceId].Weight,
                    UnitPrice = variantpricing[model.VariantPriceId].Price,
                    Quantity = 1,
                    TotalPrice = variantpricing[model.VariantPriceId].Price,
                    DispensaryProductVariantId = model.VariantId,
                    VariantPriceId = model.VariantPriceId,
                    CreatedAt = DateTimeOffset.UtcNow,
                    UpdatedAt = DateTimeOffset.UtcNow
                };
                HGContext.DispensaryProductVariantOrders.Add(variantToAdd);
            }
            cartEntity.UpdatedAt = DateTimeOffset.UtcNow;

            var result = await HGContext.SaveChangesAsync();
            if (result > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Unable to add product to cart");
                //return BadRequest("Unable to add product to cart");
            }
        }


    }

    public class CheckoutBindingModel
    {
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public DeliveryTypeEnumModel DeliveryTypeId { get; set; }
        public PaymentTypeEnumModel PaymentTypeId { get; set; }
        public AddressModel DeliveryAddress { get; set; }
        public bool UseCredits { get; set; }
        public int WalletId { get; set; }
        public int TransactionAmount { get; set; }
        public string StripeToken { get; set; }
    }
}




