using DynamicStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DynamicStore.Controllers
{
    // فقط کاربران لاگین شده (مدیر) دسترسی دارند
    [Authorize(Roles = "Admin")]
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

        // نمایش فرم ویرایش کامل دسته‌بندی
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Attributes)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return NotFound();

            return View(category);
        }

        // ذخیره تغییرات دسته‌بندی و ویژگی‌های داینامیک
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var existingCategory = await _context.Categories
                    .Include(c => c.Attributes)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (existingCategory == null)
                    return NotFound();

                // 1. آپدیت نام دسته
                existingCategory.Title = category.Title;

                // پاکسازی ویژگی‌های خالی ارسال شده
                var submittedAttributes = category.Attributes?
                    .Where(a => !string.IsNullOrWhiteSpace(a.Title)).ToList() ?? new List<CategoryAttribute>();

                // 2. حذف ویژگی‌هایی که در دیتابیس هستند اما ادمین در فرم پاک کرده است
                var submittedAttrIds = submittedAttributes.Select(a => a.Id).ToList();
                var attributesToRemove = existingCategory.Attributes
                    .Where(a => !submittedAttrIds.Contains(a.Id))
                    .ToList();

                // در EF Core اگر موجودیتی را از کالکشن حذف کنید (و آبجکت‌های وابسته باشند)، باید آنها را از کانتکست هم ریمو کنید
                foreach (var attr in attributesToRemove)
                {
                    existingCategory.Attributes.Remove(attr);
                    _context.Remove(attr);
                }

                // 3. آپدیت ویژگی‌های موجود و اضافه کردن ویژگی‌های جدید
                foreach (var attr in submittedAttributes)
                {
                    var existingAttr = existingCategory.Attributes.FirstOrDefault(a => a.Id == attr.Id && attr.Id != 0);
                    if (existingAttr != null)
                    {
                        // آپدیت ویژگی موجود
                        existingAttr.Title = attr.Title;
                    }
                    else
                    {
                        // افزودن ویژگی جدید
                        existingCategory.Attributes.Add(new CategoryAttribute
                        {
                            Title = attr.Title
                        });
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
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
