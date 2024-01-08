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

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            await _CategoryService.CreateCategory(createCategoryDto);
            return Ok();
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
