using DynamicStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DynamicStore.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        public CategoryService(AppDbContext context)
        {
            _context = context;
        }
        public  async Task<IEnumerable<SelectListItem>> GetCategoriesSelectListAsync(int? selectedId = null)
        {
            return await _context.Categories
              .Where(c => !c.Isdeleted)
              .OrderBy(c => c.Title)
              .Select(c => new SelectListItem
              {
                  Value = c.Id.ToString(),
                  Text = c.Title,
                  Selected = selectedId.HasValue && c.Id == selectedId.Value
              })
              .ToListAsync();
        }
    }
}
