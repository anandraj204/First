using System;

namespace Jane.Core.Models
{
    public class BaseModel
    {
		
        public BaseModel()
        {
            if (CreatedAt == DateTimeOffset.MinValue)
            {
                CreatedAt = DateTimeOffset.UtcNow;
            }
            if (UpdatedAt == DateTimeOffset.MinValue)
            {
                UpdatedAt = DateTimeOffset.UtcNow;
            }
        }
        public int Id { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }

    }

}