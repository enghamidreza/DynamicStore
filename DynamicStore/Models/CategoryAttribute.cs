using DynamicStore.Enum;
using System.ComponentModel.DataAnnotations;

namespace DynamicStore.Models
{
    //جدول تعریف خصوصیات برای هر دسته
    public class CategoryAttribute
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }

        [MaxLength(100)]
        public required string Title { get; set; } // مثلا: "وزن" یا "دارای گارانتی"

        public AttributeDataType DataType { get; set; } // Text, Number, Boolean

        // Relation
        public Category? Category { get; set; }
    }
}
