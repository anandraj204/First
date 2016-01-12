using System;
using System.Threading.Tasks;
using Stripe;

namespace Jane.Core.Services
{
    public class StripeService
    {
        public StripeService()
        {
            StripeConfiguration.SetApiKey("sk_test_aQDuRlZpjns2bSMpA1VwYDLo");
            //StripeConfiguration.SetApiKey("sk_live_io4AvywcsPKASkzGHFfe34PL");

        }

        public async Task<bool> ChargeCustomer(string tokenId, int amount)
        {
            return await System.Threading.Tasks.Task.Run(() =>
            {
                try
                {

                    var myCharge = new StripeChargeCreateOptions
                    {
                        Amount = amount,
                        Currency = "usd",
                        Description = "Fund Digital Wallet",
                        Source = new StripeSourceOptions() {TokenId = tokenId},
                        Capture = true,
                    };

                    var chargeService = new StripeChargeService();
                    var stripeCharge = chargeService.Create(myCharge);
                    if (stripeCharge.Status == "succeeded" && stripeCharge.Paid == true)
                    {
                        return true;
                    }
                    return false;

                }
                catch (Exception)
                {
                    //log exception here
                    return false;
                }
            });
        }

        public void StripeCharge(string token,int amount)
        {
            var myCharge = new StripeChargeCreateOptions();

            // always set these properties
            myCharge.Amount = amount;
            myCharge.Currency = "usd";

            // set this if you want to
            myCharge.Description = "Digital Wallet fund";

            // setting up the card
            myCharge.Source = new StripeSourceOptions()
            {
                // set this property if using a token
                TokenId = token
            };

            // set this property if using a customer
            //myCharge.CustomerId = *customerId*;

            // set this if you have your own application fees (you must have your application configured first within Stripe)
           // myCharge.ApplicationFee = 25;

            // (not required) set this to false if you don't want to capture the charge yet - requires you call capture later
            myCharge.Capture = true;

            var chargeService = new StripeChargeService();
            StripeCharge stripeCharge = chargeService.Create(myCharge);
        }

    }
}
