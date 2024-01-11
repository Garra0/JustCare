using AutoMapper;
using JustCare_MB.Data;
using JustCare_MB.Dtos.Category;
using JustCare_MB.Helpers;
using JustCare_MB.Models;
using JustCare_MB.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace JustCare_MB.Services
{
    public class CategoryService : ICategoryService
    {
        //for the images (to get the path)
        private readonly IHostEnvironment _hostEnvironment;
        private readonly JustCareContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;
        public CategoryService(JustCareContext context
            , IMapper mapper, IHostEnvironment hostEnvironment
            , ILogger<CategoryService> logger)
        {
            _context = context;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        public async Task CreateCategory(CreateCategoryDto createCategoryDto)
        {
            _logger.LogInformation(
              $"Create new Category {JsonConvert.SerializeObject(createCategoryDto)}"
              );

            if (await _context.Categories.AnyAsync(x => x.EnglishName.ToLower()
            == createCategoryDto.EnglishName.ToLower()))
                throw new ExistsException("Category is exists");



            string imageName = createCategoryDto.EnglishName + ".jpg";
            string imagePath = _hostEnvironment.ContentRootPath
                + "\\Images\\Categories\\" + imageName;
            // the next 2 lines eqaule to the above
            //string im = Path.Combine(_hostEnvironment.ContentRootPath
            //    + "Images" + "Categories" + imageName);
            imagePath = imagePath.Replace("JustCareAPI", "JustCare_MB"); // Replace from "JustCareAPI" to "JustCare_MB"  

            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await stream.WriteAsync(createCategoryDto.Image);
            }

            Category category = _mapper.Map<Category>(createCategoryDto);

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }


        //string imagePath = "C:\\Users\\Smail_-\\Desktop\\JustCare\\JustCare_MB\\Images\\Categories\\" + imageName + ".jpg";

        public async Task<IEnumerable<CategoryDto>> GetAllCategories()
        {
            _logger.LogInformation(
              $"Get all Categories"
              );

            IEnumerable<Category> categories = await _context.Categories.ToListAsync();
            IEnumerable<CategoryDto> categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            foreach (var category in categoryDtos)
            {
                string imageName = category.EnglishName;
                //var imagePath = Path.Combine("Images", "Categories", $"{imageName}.jpg");
                string imagePath = _hostEnvironment.ContentRootPath + "\\Images\\Categories\\" + imageName + ".jpg";
                imagePath = imagePath.Replace("JustCareAPI", "JustCare_MB"); // Replace from "JustCareAPI" to "JustCare_MB"  
                category.Image = await File.ReadAllBytesAsync(imagePath);
            }
            return categoryDtos;

        }


        //public async Task<Category> getCategoryById(int id)
        //{
        //    Category category = await _context.Categories.
        //        FirstAsync(c => c.Id == id);
        //    return category;
        //} 
    }

}



