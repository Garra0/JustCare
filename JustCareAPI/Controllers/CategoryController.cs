using JustCare_MB.Dtos.Category;
using JustCare_MB.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JustCareAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("CategoryAPI")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _CategoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _CategoryService = categoryService;
        }

        [HttpGet("GetAllCategorys")]
        public async Task<IActionResult> GetAllCategorys()
        {
            IEnumerable<CategoryDto> categories = await _CategoryService.GetAllCategories();
            return Ok(categories);
        }

        //Task<Category> getCategoryById(int id)
    }
}
