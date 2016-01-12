using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jane.Core.Models
{
    public class EffectModel : BaseModel
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [NotMapped]
        public new bool IsDeleted { get; set; }
        [NotMapped]
        public new DateTime CreatedAt { get; set; }
        [NotMapped]
        public new DateTime UpdatedAt { get; set; }
    }
}