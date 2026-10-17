using DynamicStore.Models;
using System.ComponentModel.DataAnnotations;

namespace DynamicStore.ViewModel
{
    public class ProductEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "نام محصول الزامی است")]
        [Display(Name = "نام محصول")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "قیمت الزامی است")]
        [Range(0, double.MaxValue, ErrorMessage = "قیمت باید عددی مثبت باشد")]
        [Display(Name = "قیمت")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "انتخاب دسته‌بندی الزامی است")]
        [Display(Name = "دسته‌بندی")]
        public int CategoryId { get; set; }

        public string? ExistingImageUrl { get; set; }

        [Display(Name = "تصویر جدید محصول (اختیاری)")]
        public IFormFile? ImageFile { get; set; }

        // ویژگی‌های داینامیک
        public List<ProductAttributeValueInputModel> AttributeValues { get; set; } = new();
    }
}
