using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DynamicStore.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            // اگر کاربر قبلاً لاگین کرده باشد، مستقیم به صفحه اصلی برود
            if (User.Identity is { IsAuthenticated: true })
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // در اینجا برای تست، نام کاربری و رمز عبور را به صورت ثابت admin در نظر گرفته‌ایم
            // در پروژه‌های واقعی این مقادیر از دیتابیس خوانده می‌شوند
            if (username == "admin" && password == "admin")
            {
                var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Admin")
            };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true // تیک "مرا به خاطر بسپار"
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "نام کاربری یا رمز عبور اشتباه است.";
            return View();
        }

        //[HttpPost]
        public async Task<IActionResult> Logout()
        {
            // این دستور کوکی احراز هویت را از مرورگر کاربر پاک می‌کند
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // بعد از خروج، کاربر را به صفحه لاگین هدایت می‌کنیم
            return RedirectToAction("Login", "Account");
        }

    }
}
