using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DynamicStore.Models
{
    public class Category
    {
        public int Id { get; set; }
        //public bool Isdeleted { get; set; }

        [MaxLength(100)]
        public required string Title { get; set; } // استفاده از required برای دات نت مدرن

        // Relation (Navigation Properties)
        // هر دسته بندی شامل چندین محصوله رابطه 1 ب چند 
        public ICollection<Product> Products { get; set; } = []; // استفاده از [] به جای new List
        //// Relation (Navigation Properties)
        ///هر دسته بندی شامل چند ویژگی است رابطه 1 ب چند 
        public ICollection<CategoryAttribute> Attributes { get; set; } = [];
        public bool Isdeleted { get; internal set; }
    }
}
