using DynamicStore.Models;
using Microsoft.AspNetCore.Identity;

namespace DynamicStore.Data
{
    public static class IdentityDataSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // 1. ساخت نقش Admin در صورت عدم وجود
            string adminRole = "Admin";
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            // 2. ساخت یک کاربر ادمین پیش‌فرض
            string adminEmail = "admin@store.com";
            var existingAdmin = await userManager.FindByEmailAsync(adminEmail);

            if (existingAdmin == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "مدیر کل سیستم",
                    EmailConfirmed = true
                };

                // ساخت کاربر با پسورد دلخواه (طبق قوانین پسورد Identity: حروف بزرگ، کوچک، عدد و کاراکتر خاص)
                var result = await userManager.CreateAsync(newAdmin, "Admin@123456");

                if (result.Succeeded)
                {
                    // انتساب نقش ادمین به این کاربر
                    await userManager.AddToRoleAsync(newAdmin, adminRole);
                }
            }
        }
    }
}
