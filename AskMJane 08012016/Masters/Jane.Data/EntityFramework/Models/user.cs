using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Harvestgeek.Data.EntityFramework.Models
{
    [Table("public.users")]
    public partial class user
    {
        public user()
        {
            dashboards = new HashSet<dashboard>();
            stations = new HashSet<station>();
            users_roles = new HashSet<users_roles>();
            widgets = new HashSet<widget>();
        }

        public int Id { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string encrypted_password { get; set; }

        [StringLength(255)]
        public string reset_password_token { get; set; }

        public DateTime? reset_password_sent_at { get; set; }

        public DateTime? remember_created_at { get; set; }

        public int? sign_in_count { get; set; }

        public DateTime? current_sign_in_at { get; set; }

        public DateTime? last_sign_in_at { get; set; }

        [StringLength(255)]
        public string current_sign_in_ip { get; set; }

        [StringLength(255)]
        public string last_sign_in_ip { get; set; }

        public DateTime created_at { get; set; }

        public DateTime updated_at { get; set; }

        [Required]
        public string UserName { get; set; }

        public int? ship_address_id { get; set; }

        public int? bill_address_id { get; set; }

        public string firstname { get; set; }

        public string lastname { get; set; }

        public bool is_deleted { get; set; }

        public string location { get; set; }

        public string website { get; set; }

        public string bio { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string Discriminator { get; set; }

        public string PhoneNumber { get; set; }

        public bool? PhoneNumberConfirmed { get; set; }

        public bool? TwoFactorEnabled { get; set; }

        public DateTime? LockoutEndDateUtc { get; set; }

        public bool? LockoutEnabled { get; set; }

        public string UserId { get; set; }

        public int AccessFailedCount { get; set; }

        public virtual ICollection<dashboard> dashboards { get; set; }

        public virtual ICollection<station> stations { get; set; }

        public virtual ICollection<users_roles> users_roles { get; set; }

        public virtual ICollection<widget> widgets { get; set; }
    }
}
