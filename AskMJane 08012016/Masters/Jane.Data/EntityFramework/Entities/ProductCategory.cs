using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jane.Data.EntityFramework.Entities
{
    public class ProductCategory : BaseEntity
    {
        public ProductCategory() 
        {
            this.Products = new HashSet<Product>(); 
        } 

        [Required]
        public string Name { get; set; }
        [Required]
        public string Color { get; set; }
        [Required]
        public string PhotoUrl { get; set; }

        [NotMapped]
        public new bool IsDeleted { get; set; }
        [NotMapped]
        public new DateTime CreatedAt { get; set; }
        [NotMapped]
        public new DateTime UpdatedAt { get; set; }
        


        public virtual ICollection<Product> Products { get; private set; } 


    }
}
