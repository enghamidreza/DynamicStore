using DynamicStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynamicStore.Controllers
{
    [Authorize] // فقط کاربران لاگین شده (مدیر) دسترسی دارند
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

       
        // نمایش لیست دسته‌بندی‌ها
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                //include میاد یک جوین همراه با جدول CategoryAttribute میزنه ک کتگوری همراه با ویژگی هاش رو ب ما نشون بده 
                .Include(c => c.Attributes)
                // نتیجه ب دیتابیس ارسال میشه و در متغیر category گذاشته میشه 
                .ToListAsync();
            return View(categories);
        }
        // نمایش فرم ایجاد دسته‌بندی
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // ذخیره دسته‌بندی و ویژگی‌های داینامیک آن
        [HttpPost]
        //برای مسائل امنیتی 
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            // اطلاعات معتبر هستند
            if (ModelState.IsValid)
            {
                if (category.Attributes != null)
                {
                    category.Attributes = category.Attributes
                        .Where(a => !string.IsNullOrWhiteSpace(a.Title))
                        .ToList();
                }
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> QuickEdit(int id, string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Json(new { success = false, message = "عنوان دسته‌بندی نمی‌تواند خالی باشد." });

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return Json(new { success = false, message = "دسته‌بندی یافت نشد." });

            category.Title = title;
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "ویرایش با موفقیت انجام شد." });
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Attributes)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return Json(new { success = false, message = "دسته‌بندی یافت نشد." });

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }


    }
}
