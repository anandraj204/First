using System;

namespace Jane.Core.Models
{
    public class DispensaryStaffModel :BaseModel
    {
        public string JobRole { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime HiredDate { get; set; }
        public int UserId { get; set; }
        public int DispensaryId { get; set; }

    }
}
