using System;
using System.Collections.Generic;

namespace Jane.Core.Models
{
    public enum PaymentTypeEnumModel
    {
        CASH = 1,
        CREDIT = 2,
        DEBIT = 3,
        ACH = 4,
        OTHER = 5
    }
    public enum DeliveryTypeEnumModel
    {
        DELIVERY = 1,
        PICKUP = 2,
        SCHEDULED_DELIVERY = 3
    }
    public enum OrderStatusEnumModel
    {
        INCOMPLETE =0,
        AWAITING_PREPARATION =1,
        PREPARED_AWAITING_DELIVERY = 2,
        PREPARED_AWAITING_PICKUP = 3,
        IN_TRANSIT = 4,
        RECEIVED = 5
    }

    public class OrderModel : BaseModel
    {
        public PaymentTypeEnumModel? PaymentType { get; set; }
        public DeliveryTypeEnumModel? DeliveryType { get; set; }
        public OrderStatusEnumModel? OrderStatus { get; set; }
        public bool IsCheckedOut { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsReceived { get; set; }
        public string OrderReferenceId { get; set; }
        public string OnfleetDestinationId { get; set; }
        public string OnfleetTaskId { get; set; }
        public string OnfleetTrackingURL { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTimeOffset? CheckedOutAt { get; set; }
        public decimal DeliveryCharge { get; set; }
        public int BillingAddressId { get; set; }
        public int DeliveryAddressId { get; set; }
        public int UserId { get; set; }
        public int DispensaryId { get; set; }
        public ThinUserModel User { get; set; }
        public ThinDispensaryModel Dispensary { get; set; }
        public AddressModel BillingAddress { get; set; }
        public AddressModel DeliveryAddress { get; set; }
        public List<DispensaryProductVariantOrderModel> DispensaryProductVariantOrders { get; set; }

        public string DeliveryTypeName
        {
            get
            {
                switch (DeliveryType)
                {
                    case DeliveryTypeEnumModel.DELIVERY:
                        return "Delivery";
                    case DeliveryTypeEnumModel.PICKUP:
                        return "Pickup";
                    case DeliveryTypeEnumModel.SCHEDULED_DELIVERY:
                        return "Scheduled Delivery";

                }
                return "";
            }
        }
    }

    public class ThinOrderModel : BaseModel
    {
        public PaymentTypeEnumModel? PaymentType { get; set; }
        public DeliveryTypeEnumModel? DeliveryType { get; set; }
        public bool IsCheckedOut { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsReceived { get; set; }
        public string OrderReferenceId { get; set; }
        public string OnfleetDestinationId { get; set; }
        public string OnfleetTaskId { get; set; }
        public string OnfleetTrackingURL { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTimeOffset? CheckedOutAt { get; set; }
        public decimal DeliveryCharge { get; set; }
        public int BillingAddressId { get; set; }
        public int DeliveryAddressId { get; set; }
        public int UserId { get; set; }
        public int DispensaryId { get; set; }
    }
}
