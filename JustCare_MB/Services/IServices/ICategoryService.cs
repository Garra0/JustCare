using JustCare_MB.Dtos.Category;

namespace JustCare_MB.Services.IServices
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategories();
        //Task<Category> getCategoryById(int id);
        Task CreateCategory(CreateCategoryDto createCategoryDto);    }
}
