using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Jane.API.Infrastructure.Common;
using Jane.Core.Services;
using Jane.Data.EntityFramework.Entities;

namespace Jane.API.Controllers
{
    [System.Web.Http.RoutePrefix("api/Wallet")]
    public class WalletController : BaseApiController
    {
        [HttpPost]
        [System.Web.Http.Route("Fund")]
        public async Task<IHttpActionResult> Fund([FromBody] FundWalletBindingModel model)
        {
            StripeService stripe = new StripeService();
            var response =  await stripe.ChargeCustomer(model.StripeToken, model.TransactionAmount);
            if (response)
            {
                var user = await HGContext.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);
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
                    
                    var result = HGContext.SaveChanges();
                    if (result == 0)
                    {
                        return BadRequest("Something went wrong");
                    }
                    return Ok();
                }
                return BadRequest("Something went wrong");
            }
            else
            {
                _logger.ErrorFormat("Failed to finalize payment user {0} amount {1}", model.UserId, model.TransactionAmount);
                return BadRequest("Something went wrong");
            }
            //return Ok(response);
        }


        [HttpPost]
        [Route("EditCredits")]
        [Authorize]

        public IHttpActionResult EditCredits([FromBody]EditCredit credit)
        {
            var wallet = HGContext.Users.Where(u => u.Id == credit.userId).Select(u => u.Wallet).FirstOrDefault();

            if (wallet != null)
                wallet.Credit = credit.amount;
            else
                return NotFound();

            HGContext.SaveChanges();

            return Ok();
        }
    }

    public class FundWalletBindingModel
    {
        public int UserId { get; set; }
        public int WalletId { get; set; }
        public int TransactionAmount { get; set; }
        public string StripeToken { get; set; }
    }

    public class EditCredit
    {
        public int userId { get; set; }
        public int amount { get; set; }
    }
}
