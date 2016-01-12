using System;

namespace Jane.Core.Models
{
    public class DispensaryProductVariantOrderModel : BaseModel
    {
        private string _displayQuantity;
        public bool IsPricedByWeight { get; set; }
        public decimal Weight { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public string DisplayQuantity
        {
            get
            {
                if (_displayQuantity == null)
                {
                    if (IsPricedByWeight)
                    {
                        var oz = Weight/28M;
                        if (.125M == oz)
                        {
                            return "Eigth";
                        }
                        if (.25M == oz)
                        {
                            return "Quarter";
                        }
                        if (.5M == oz)
                        {
                            return "Half";
                        }
                        if (1M == oz)
                        {
                            return "Ounce";
                        }
                        return String.Format("{0}g", Weight);
                    }
                    else
                    {
                        if (1M == Weight)
                        {
                            return "Each";
                        }
                        else
                        {
                            return String.Format("{0}pack", Weight);
                        }
                    }
                }
                return _displayQuantity;
            }
            set
            {
                _displayQuantity = value;
            }
        }
        public int DispensaryProductVariantId { get; set; }
        public int OrderId { get; set; }
        public DispensaryProductVariantModel DispensaryProductVariant { get; set; }

    }
}
