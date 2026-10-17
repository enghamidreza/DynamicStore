using DynamicStore.Models;

namespace DynamicStore.ViewModel
{
    public class ShopIndexViewModel
    {
        public List<Product> Products { get; set; } = new();
        public List<Category> Categories { get; set; } = new();

        // پارامترهای فیلتر (جهت حفظ مقادیر درون فرم پس از ارسال)
        public string? Search { get; set; }
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? SortBy { get; set; }
    }
}
