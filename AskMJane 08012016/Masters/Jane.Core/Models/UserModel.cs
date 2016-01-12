using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Jane.Core.Models
{
    public class UserModel : BaseModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

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



        [Required]
        public string Guid { get; set; }
        public string Zipcode { get; set; }
        public DateTimeOffset? LastLogin { get; set; }
        public DateTime? Birthday { get; set; }
        public string OnfleetRecipientId { get; set; }
        public string CurrentIp { get; set; }
        public string LastIp { get; set; }
        public int SignInCount { get; set; }
        public int? PatientInfoId { get; set; }
        public int? DeliveryAddressId { get; set; }
        public int BillingAddressId { get; set; }
        public int WalletId { get; set; }
        public WalletModel Wallet { get; set; }
        //public List<DispensaryStaff> DispensaryStaff{get;set;}
        public ThinDispensaryModel Dispensary { get; set; }
        public PatientInfoModel PatientInfo { get; set; }
        public AddressModel Address { get; set; }
        public AddressModel DeliveryAddress { get; set; }
        public AddressModel BillingAddress { get; set; }
        public List<OrderModel> Orders { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }

        public string ApprovalStatusString
        {
            get
            {
                if (PatientInfo == null)
                {
                    return null;
                }

                switch (PatientInfo.ApprovalStatus)
                {
                    case ApproalStatusModel.ACCEPTED:
                        return "Approved";
                    case ApproalStatusModel.APPLIED:
                        return "Pending";
                    case ApproalStatusModel.NOTAPPLIED:
                        return "Incomplete";
                    case ApproalStatusModel.OTHER:
                        return "Other";
                    case ApproalStatusModel.REJECTED:
                        return "Rejected";
                }

                return "";
            }
        }
    }
    public class ThinUserModel : BaseModel
    {
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Guid { get; set; }
        public string Zipcode { get; set; }
        public DateTimeOffset? LastLogin { get; set; }
        public DateTime? Birthday { get; set; }
        public string OnfleetRecipientId { get; set; }
        public string CurrentIp { get; set; }
        public string LastIp { get; set; }
        public int SignInCount { get; set; }
        public int? PatientInfoId { get; set; }
        public int? DeliveryAddressId { get; set; }
        public int BillingAddressId { get; set; }
        public int WalletId { get; set; }
        public WalletModel Wallet { get; set; }
        public PatientInfoModel PatientInfo { get; set; }
        public AddressModel Address { get; set; }
        public AddressModel DeliveryAddress { get; set; }
        public AddressModel BillingAddress { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
    }

    public class UserWithRolesModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public List<RoleModel> RolesList { get; set; }
    }
}