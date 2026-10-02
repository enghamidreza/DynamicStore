using Microsoft.AspNetCore.Mvc.Rendering;

namespace DynamicStore.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<SelectListItem>> GetCategoriesSelectListAsync(int? selectedId = null);
    }
}
