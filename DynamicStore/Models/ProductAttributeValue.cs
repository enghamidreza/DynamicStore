namespace DynamicStore.Models
{
    //جدول مقادیر خصوصیات برای هر محصول
    public class ProductAttributeValue
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CategoryAttributeId { get; set; }

        // مقدار همیشه به صورت رشته ذخیره می‌شود، 
        // سمت کلاینت یا کد بر اساس DataType تبدیل می‌شود (Cast)
        public required string Value { get; set; }

        // Relation
        public Product? Product { get; set; }
        public CategoryAttribute? CategoryAttribute { get; set; }
    }
}
