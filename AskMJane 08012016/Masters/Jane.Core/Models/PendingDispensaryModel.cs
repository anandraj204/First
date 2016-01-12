using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jane.Core.Models
{
    public class PendingDispensaryModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public int AddressId { get; set; }
        [Required]
        public string Type { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Website { get; set; }

        public string DriversLicenseImageUrl { get; set; }
        public string RecommendationImageUrl { get; set; }

        public string IdNumber { get; set; }

        public Nullable<DateTime> ExperationDate { get; set; }

        public PendingDispensaryStatus PendingDispensaryStatus { get; set; }
        [ForeignKey("AddressId")]
        public AddressModel Address { get; set; }
    }

    public enum PendingDispensaryStatus
    {
        Approved,
        Rejected,
        WaitingForApprove,
        Initilized,
    }
}
