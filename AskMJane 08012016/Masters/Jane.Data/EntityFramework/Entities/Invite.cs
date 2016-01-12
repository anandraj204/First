using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jane.Data.EntityFramework.Entities
{
    public class Invite :BaseEntity
    {
        public string InviteCode { get; set; }
        public decimal? InviterCredit { get; set; }
        public decimal? InviteeCredit { get; set; }

        [Required]
        public int InviterId { get; set; }
        public int? InviteeId { get; set; }

        public string InviteeName { get; set; }
        public string InviteeEmail { get; set; }
        public string InviteePhone { get; set; }


        [ForeignKey("InviterId")]
        public virtual User Inviter { get; set; }
        [ForeignKey("InviteeId")]
        public virtual User Invitee { get; set; }

         
    }
}
