using System;

namespace Jane.Data.EntityFramework.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
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
        public bool IsDeleted { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }
    }
}
