using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jane.Data.EntityFramework.Entities
{
    public class ZipCode : BaseEntity
    {
        public string Code { get; set; }

        [NotMapped]
        public new bool IsDeleted { get; set; }
        [NotMapped]
        public new DateTime CreatedAt { get; set; }
        [NotMapped]
        public new DateTime UpdatedAt { get; set; }
    }
}
