using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Jane.Data.EntityFramework.Entities
{
    public class User : IdentityUser<int, UserLogin, UserRole, UserClaim>
    {
        private DateTimeOffset _lastLogin;
        private DateTimeOffset _updatedAt;
        private DateTimeOffset _createdAt;

        public User()
        {
            this.DispensaryStaff = new HashSet<DispensaryStaff>();
            this.Orders = new HashSet<Order>();
            this.UserSessions = new HashSet<UserSession>();
        }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Zipcode { get; set; }
        public DateTime? Birthday { get; set; }
        public DateTimeOffset LastLogin
        {
            get { return _lastLogin; }
            set
            {
                if ((_lastLogin == null && value == null) || (_lastLogin == DateTimeOffset.MinValue && value == DateTimeOffset.MinValue))
                {
                    _lastLogin = DateTimeOffset.UtcNow;
                }
                else
                {
                    _lastLogin = value;
                }
            }
        }

        public string OnfleetRecipientId { get; set; }
        public string CurrentIp { get; set; }
        public string LastIp { get; set; }
        public int? SignInCount { get; set; }

        public DateTimeOffset CreatedAt
        {
            get { return _createdAt; }
            set
            {
                if ((_createdAt == null && value == null) || (_createdAt == DateTimeOffset.MinValue && value == DateTimeOffset.MinValue))
                {
                    _createdAt = DateTimeOffset.UtcNow;
                }
                else
                {
                    _createdAt = value;
                }
            }
        }

        public DateTimeOffset UpdatedAt
        {
            get { return _updatedAt; }
            set
            {
                if ((_updatedAt == null && value == null) ||
                    (_updatedAt == DateTimeOffset.MinValue && value == DateTimeOffset.MinValue))
                {
                    _updatedAt = DateTimeOffset.UtcNow;
                }
                else
                {
                    _updatedAt = value;
                }
            }
        }
        [Required]
        public string Guid { get; set; }

        // Foreign Keys
        public int? PatientInfoId { get; set; }
        public int? AddressId { get; set; }
        public int? DeliveryAddressId { get; set; }
        public int? BillingAddressId { get; set; }
        public int? WalletId { get; set; }

        // Mapped Tables
        public virtual Wallet Wallet { get; set; }
        public virtual PatientInfo PatientInfo { get; set; }
        public virtual Address Address { get; set; }
        [ForeignKey("DeliveryAddressId")]
        public virtual Address DeliveryAddress { get; set; }
        [ForeignKey("BillingAddressId")]
        public virtual Address BillingAddress { get; set; }

        public HashSet<Order> Orders { get; set; }
        public HashSet<UserSession> UserSessions { get; set; } 
        public HashSet<DispensaryStaff> DispensaryStaff { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User, int> manager,string authenticationType)
        {
            // Note the authenticationType must match the one defined in
            // CookieAuthenticationOptions.AuthenticationType 
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here 
            return userIdentity;
        }
    }
}
