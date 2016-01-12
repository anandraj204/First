using System.ComponentModel.DataAnnotations;
using NpgsqlTypes;

namespace Jane.Data.EntityFramework.Entities
{
    public class DispensaryStaff : BaseEntity
    {
        public string JobRole { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public NpgsqlTimeStampTZ HiredDate { get; set; }


        [Required]
        public int UserId { get; set; }
        [Required]
        public int DispensaryId { get; set; }
        public virtual User User { get; set; }
        public virtual Dispensary Dispensary { get; set; }
    }
}
