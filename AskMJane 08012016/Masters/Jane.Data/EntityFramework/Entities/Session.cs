using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jane.Data.EntityFramework.Entities
{
    public class Session :BaseEntity
    {
        [Required]
        public string Token { get; set; }
        public DateTimeOffset LastSeen { get; set; }
        [NotMapped]
        public new bool IsDeleted
        {
            get;
            set;
        }
    }
    
}
