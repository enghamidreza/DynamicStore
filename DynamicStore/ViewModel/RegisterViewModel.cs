using System.ComponentModel.DataAnnotations;

namespace DynamicStore.ViewModel
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "نام و نام خانوادگی الزامی است")]
        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "ایمیل الزامی است")]
        [EmailAddress(ErrorMessage = "فرمت ایمیل نامعتبر است")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "شماره موبایل الزامی است")]
        [RegularExpression(@"^09\d{9}$", ErrorMessage = "فرمت شماره موبایل صحیح نیست (مثال: 09123456789)")]
        [Display(Name = "شماره موبایل")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "کلمه عبور الزامی است")]
        [StringLength(100, ErrorMessage = "{0} باید حداقل {2} کاراکتر باشد", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور")]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "تکرار کلمه عبور")]
        [Compare("Password", ErrorMessage = "کلمه عبور با تکرار آن یکسان نیست")]
        public string ConfirmPassword { get; set; } = string.Empty;

        //[Required(ErrorMessage = "آدرس تحویل سفارش الزامی است")]
        //[Display(Name = "آدرس کامل پستی")]
        //public string Address { get; set; } = string.Empty;

        //[Display(Name = "کد پستی")]
        //public string? PostalCode { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
