using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Jane.Core.Models;

namespace Jane.Data.EntityFramework.Entities
{
    public class PendingDispensary : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        public int AddressId { get; set; }
        [Required]
        public string Type { get; set; }
        public string Website { get; set; }

        public string Password { get; set; }

        public string IdNumber { get; set; }

        public DateTime? ExperationDate { get; set; }

        public string DriversLicenseImageUrl { get; set; }
        public string RecommendationImageUrl { get; set; }

        [ForeignKey("AddressId")]
        public Address Address { get; set; }

        public PendingDispensaryStatus PendingDispensaryStatus { get; set; }
    }
}
