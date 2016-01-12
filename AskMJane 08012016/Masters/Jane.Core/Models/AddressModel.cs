using NpgsqlTypes;

namespace Jane.Core.Models
{
    public class AddressModel :BaseModel
    {
        public AddressModel() { }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public NpgsqlPoint? LatLong { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string FormattedAddress { get; set; }
    }
}