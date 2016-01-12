using System.ComponentModel.DataAnnotations;

namespace Jane.Data.EntityFramework.Entities
{
    public class Flavor :BaseEntity
    {
        [Required]
        public string Name { get; set; }

    }
}
