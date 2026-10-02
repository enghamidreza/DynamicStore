using DynamicStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DynamicStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly AppDbContext _context;
        //برای اپلود تصویر
        private readonly IWebHostEnvironment _environment;
        public ProductsController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                  .Include(p => p.Category)
                  //ردیابی یا track نکردن
                  .AsNoTracking()
                  .ToListAsync();

            return View(products);
        }
        // 2. فرم ایجاد محصول
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories.AsNoTracking().ToListAsync() ?? new List<Category>();

            ViewBag.Categories = new SelectList(categories, "Id", "Title");

            try
            {
                var test = categories;
                var model = new ProductCreateViewModel
                {
                    AttributeValues = new List<ProductAttributeValueInputModel>()
                };
                return View(model);

            }
            catch (Exception ex)
            {
                return View();
            }

        }
        // ۳. متد AJAX برای لود ویژگی‌های دسته انتخابی
        [HttpGet]
        public async Task<IActionResult> GetCategoryAttributes(int categoryId)
        {
            var attributes = await _context.CategoryAttributes
                .Where(a => a.CategoryId == categoryId)
                .Select(a => new { a.Id, a.Title, a.DataType })
                .ToListAsync();

            return Json(attributes);
        }
        // ۴. ثبت محصول (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(await _context.Categories.AsNoTracking().ToListAsync(), "Id", "Name", model.CategoryId);
                return View(model);
            }
            // ذخیره تصویر در صورت وجود
            string? uniqueFileName = null;

            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(
                    _environment.WebRootPath,
                    "images",
                    "products"
                );

                Directory.CreateDirectory(uploadsFolder);

                uniqueFileName = Guid.NewGuid().ToString() + "_" +
                                 Path.GetFileName(model.ImageFile.FileName);

                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);

                await model.ImageFile.CopyToAsync(fileStream);
            }
            // ایجاد موجودیت Product
            var product = new Product
            {
                CategoryId = model.CategoryId,
                Title = model.Title,
                Code = model.Code,
                Price = model.Price,
                Description = model.Description,
                ImageName = uniqueFileName,
                IsActive = true
            };
            var testAtProduct = model.AttributeValues;
            // اضافه کردن مقادیر ویژگی‌های داینامیک EAV
            if (model.AttributeValues != null)
            {
                foreach (var attr in model.AttributeValues.Where(a => !string.IsNullOrWhiteSpace(a.Value)))
                {
                    product.AttributeValues.Add(new ProductAttributeValue
                    {
                        CategoryAttributeId = attr.CategoryAttributeId,
                        Value = attr.Value!
                    });
                }
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.AttributeValues)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound();

            var model = new ProductEditViewModel
            {
                Id = product.Id,
                Name = product.Title,
                Price = product.Price,
                CategoryId = product.CategoryId,
                ExistingImageUrl = product.ImageName,
                AttributeValues = product.AttributeValues.Select(av => new ProductAttributeValueInputModel
                {
                    CategoryAttributeId = av.CategoryAttributeId,
                    Value = av.Value
                }).ToList()
            };

            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(model);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductEditViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var product = await _context.Products
                    .Include(p => p.AttributeValues)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null) return NotFound();

                // 1. آپدیت فیلدهای اصلی
                product.Title = model.Name;
                product.Price = model.Price;
                product.CategoryId = model.CategoryId;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {


                    // حذف تصویر قبلی
                    if (!string.IsNullOrEmpty(product.ImageName))
                    {
                        var oldPath = Path.Combine(
                            _environment.WebRootPath,
                            "images",
                            "products",
                            Path.GetFileName(product.ImageName)
                        );

                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }

                    // مسیر پوشه تصاویر
                    var uploadsFolder = Path.Combine(
                        _environment.WebRootPath,
                        "images",
                        "products"
                    );

                    Directory.CreateDirectory(uploadsFolder);

                    // نام فایل جدید
                    var uniqueFileName =
                        Guid.NewGuid().ToString() + "_" +
                        Path.GetFileName(model.ImageFile.FileName);

                    var filePath = Path.Combine(
                        uploadsFolder,
                        uniqueFileName
                    );

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }

                    // فقط نام فایل در DB
                    product.ImageName = uniqueFileName;
                }

                // 3. آپدیت ویژگی‌های EAV (حذف مقادیر قبلی و درج مقادیر جدید)
                _context.ProductAttributeValues.RemoveRange(product.AttributeValues);

                if (model.AttributeValues != null && model.AttributeValues.Any())
                {
                    foreach (var attr in model.AttributeValues.Where(a => !string.IsNullOrWhiteSpace(a.Value)))
                    {
                        product.AttributeValues.Add(new ProductAttributeValue
                        {
                            ProductId = product.Id,
                            CategoryAttributeId = attr.CategoryAttributeId,
                            Value = attr.Value.Trim()
                        });
                    }
                }

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "محصول با موفقیت ویرایش شد.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = await _context.Categories.ToListAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products
                .Include(p => p.AttributeValues)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            // 1. حذف فایل تصویر از هاست در صورت وجود
            if (!string.IsNullOrEmpty(product.ImageName))
            {
                var imagePath = Path.Combine(_environment.WebRootPath, "images", "products", product.ImageName);
                if (System.IO.File.Exists(imagePath))
                {
                    try
                    {
                        System.IO.File.Delete(imagePath);
                    }
                    catch (Exception ex)
                    {
                        // در صورت قفل بودن فایل یا خطا لاگ می‌شود
                    }
                }
            }

            // 2. حذف ویژگی‌های داینامیک متصل به محصول
            if (product.AttributeValues != null && product.AttributeValues.Any())
            {
                _context.ProductAttributeValues.RemoveRange(product.AttributeValues);
            }

            // 3. حذف خود محصول
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "محصول با موفقیت حذف شد.";
            return RedirectToAction(nameof(Index));
        }

    }
}
