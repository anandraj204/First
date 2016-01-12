using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jane.Data.EntityFramework.Entities
{
    public enum PaymentTypeEnum
    {
        CASH = 1,
        CREDIT = 2,
        DEBIT = 3,
        ACH = 4,
        OTHER = 5
    }
    public enum DeliveryTypeEnum
    {
        DELIVERY = 1,
        PICKUP = 2,
        SCHEDULED_DELIVERY = 3
    }

    public class Order : BaseEntity
    {

        public Order()
        {
            this.DispensaryProductVariantOrders = new HashSet<DispensaryProductVariantOrder>();
        }

        [Required]
        public PaymentTypeEnum? PaymentType { get; set; }
        [Required]
        public DeliveryTypeEnum? DeliveryType { get; set; }

        [Required]
        public bool IsCheckedOut { get; set; }
        [Required]
        public bool IsConfirmed { get; set; }
        [Required]
        public bool IsReceived { get; set; }
        [Required]
        public string OrderReferenceId { get; set; }
        public string OnfleetDestinationId { get; set; }
        public string OnfleetTaskId { get; set; }
        public string OnfleetTrackingURL { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTimeOffset? CheckedOutAt { get; set; }
        public decimal DeliveryCharge { get; set; }

        public int? DispensaryId { get; set; }
        public virtual Dispensary Dispensary { get; set; }
        public int? BillingAddressId { get; set; }
        public int? DeliveryAddressId { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        [ForeignKey("BillingAddressId")]
        public virtual Address BillingAddress { get; set; }
        [ForeignKey("DeliveryAddressId")]
        public virtual Address DeliveryAddress { get; set; }
        public virtual ICollection<DispensaryProductVariantOrder> DispensaryProductVariantOrders { get; private set; }
    }
}
