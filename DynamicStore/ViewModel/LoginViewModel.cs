using System.ComponentModel.DataAnnotations;

namespace DynamicStore.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "ایمیل یا نام کاربری الزامی است")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "کلمه عبور الزامی است")]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "مرا به خاطر بسپار")]
        public bool RememberMe { get; set; }
        //اگر کاربر لاگین کرد دوباره به صفحه مورد نظر اش بیاد
        public string? ReturnUrl { get; set; }
    }
}
