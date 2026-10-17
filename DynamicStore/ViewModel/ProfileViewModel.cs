using System.ComponentModel.DataAnnotations;

namespace DynamicStore.ViewModel
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "نام و نام خانوادگی الزامی است")]
        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "ایمیل")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "شماره موبایل الزامی است")]
        [RegularExpression(@"^09\d{9}$", ErrorMessage = "شماره موبایل نامعتبر است")]
        [Display(Name = "شماره موبایل")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "آدرس پستی الزامی است")]
        [Display(Name = "آدرس تحویل سفارش")]
        public string Address { get; set; } = string.Empty;

        [Display(Name = "کد پستی")]
        public string? PostalCode { get; set; }
    }
}
