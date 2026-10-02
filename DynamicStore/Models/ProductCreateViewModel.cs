using System.ComponentModel.DataAnnotations;

namespace DynamicStore.Models
{
    public class ProductCreateViewModel
    {
        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "وارد کردن نام محصول الزامی است")]
        [StringLength(200, ErrorMessage = "نام محصول نمی‌تواند بیش از ۲۰۰ کاراکتر باشد")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "قیمت (تومان)")]
        [Range(0, double.MaxValue, ErrorMessage = "قیمت باید بزرگ‌تر یا مساوی صفر باشد")]
        public decimal Price { get; set; }

        [Display(Name = "دسته‌بندی")]
        [Required(ErrorMessage = "انتخاب دسته‌بندی الزامی است")]
        public int CategoryId { get; set; }

        [Display(Name = "کد محصول")]
        [Required(ErrorMessage = "لطفاً {0} را وارد کنید")]
        public string Code { get; set; } = string.Empty;

        [Display(Name = "توضیحات")]
        [MaxLength(500)]
        public string? Description { get; set; }
        public List<ProductAttributeValueInputModel> AttributeValues { get; set; } = new();

        // این فیلد برای دریافت فایل از فرم است و در دیتابیس ذخیره نمی‌شود
        [Display(Name = "تصویر محصول")]
        public IFormFile? ImageFile { get; set; }


    }
}
