using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DynamicStore.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }

        [MaxLength(150)]
        public required string Title { get; set; }

        [MaxLength(50)]
        public required string Code { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; } // Nullable (?)

        public bool IsActive { get; set; } = true;

        [MaxLength(200)]
        public string? ImageName { get; set; }
        [Precision(18, 2)]
        public required decimal Price { get; set; }


        // Relation
        //محصول برای کدام CATEGORY است 
        public Category? Category { get; set; }
        //هر محصول می‌تواند چندین مقدار (Value) برای ویژگی‌های مختلف داشته باشد
        public ICollection<ProductAttributeValue> AttributeValues { get; set; } = [];
    }
}
