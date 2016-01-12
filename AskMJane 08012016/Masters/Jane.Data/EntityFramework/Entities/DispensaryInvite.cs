using System.ComponentModel.DataAnnotations;

namespace Jane.Data.EntityFramework.Entities
{
    public class DispensaryInvite : BaseEntity
    {
        [Required]
        public string DispensaryInviteCode { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }



        [Required]
        public int UserId { get; set; }
        [Required]
        public int DispensaryId { get; set; }
        public virtual User User { get; set; }
        public virtual Dispensary Dispensary { get; set; }
    }
}
