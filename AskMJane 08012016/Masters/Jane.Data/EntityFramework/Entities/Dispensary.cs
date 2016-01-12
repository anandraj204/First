using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jane.Data.EntityFramework.Entities
{
    public class Dispensary : BaseEntity
    {
        public Dispensary()
        {
            this.DispensaryProducts = new HashSet<DispensaryProduct>();
            this.DispensaryInvites = new HashSet<DispensaryInvite>();
            this.DispensaryStaff = new HashSet<DispensaryStaff>();
            this.ApprovalZipCodes = new List<ZipCode>();
            this.DeliveryZipCodes = new List<ZipCode>();
            Guid = System.Guid.NewGuid().ToString();
        }

        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string HoursAndInfo { get; set; }
        [Required]
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        [Required]
        public bool HasDelivery { get; set; }
        [Required]
        public bool HasPickup { get; set; }
        [Required]
        public bool HasScheduledDelivery { get; set; }
        [Required]
        public bool HasStorefront { get; set; }
        [Required]
        public bool IsCaregiver { get; set; }
        [Required]
        public bool IsPrivate { get; set; }
        [Required]
        public bool IsHidden { get; set; }
        [Required]
        public string Slug { get; set; }
        public string LeaflySlug { get; set; }

        public string Type { get; set; }

        public string Guid { get; set; }
        public string Photos { get; set; }
        public string HoursOfOperation { get; set; }
        public string DeliveryZones { get; set; }
        public string[] ScheduledDeliveryZipCodes { get; set; }
        public string OnfleetMerchantId { get; set; }

        public int? UserId { get; set; }

        public int AddressId { get; set; }
        public virtual Address Address { get; set; }

        public virtual ICollection<DispensaryProduct> DispensaryProducts { get; private set; }
        public virtual ICollection<DispensaryInvite> DispensaryInvites { get; private set; }
        public virtual ICollection<DispensaryStaff> DispensaryStaff { get; private set; }

        public virtual ICollection<ZipCode> ApprovalZipCodes { get; private set; }

        public virtual ICollection<ZipCode> DeliveryZipCodes { get; private set; }

        [ForeignKey("UserId")]
        public virtual User Owner { get; set; }
    }
}
