using AutoMapper;
using JustCare_MB.Data;
using JustCare_MB.Dtos;
using JustCare_MB.Dtos.Category;
using JustCare_MB.Helpers;
using JustCare_MB.Models;
using JustCare_MB.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace JustCare_MB.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IHostEnvironment _hostEnvironment;

        private readonly JustCareContext _context;
        private readonly IMapper _mapper;
        public CategoryService(JustCareContext context
            , IMapper mapper, IHostEnvironment hostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
        }

        public async Task CreateCategory(CreateCategoryDto createCategoryDto)
        {
            if (await _context.Categories.AnyAsync(x => x.EnglishName.ToLower()
            == createCategoryDto.EnglishName.ToLower()))
                throw new ExistsException("Category is exists");

            

            string imageName = createCategoryDto.EnglishName + ".jpg";
            string imagePath = _hostEnvironment.ContentRootPath + "\\Images\\Categories\\" + imageName;
            imagePath = imagePath.Replace("JustCareAPI", "JustCare_MB"); // Replace from "JustCareAPI" to "JustCare_MB"  
            //category.Image = await File.ReadAllBytesAsync(imagePath);
            //var imagePath = Path.Combine("Images", "Categories", imageName);

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
            IEnumerable<Category> categories = await _context.Categories.ToListAsync();
            IEnumerable<CategoryDto> categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            foreach (var category in categoryDtos)
            {
                string imageName = category.EnglishName;
                //var imagePath = Path.Combine("Images", "Categories", $"{imageName}.jpg");
                string imagePath = _hostEnvironment.ContentRootPath+ "\\Images\\Categories\\" + imageName+".jpg";
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

        //public async Task UploadCategoryImage(byte[] imageData, string imageName)
        //{
        //    imageName = imageName + ".jpg";
        //    var imagePath = Path.Combine("Images", "Categories", imageName);

        //    using (var stream = new FileStream(imagePath, FileMode.Create))
        //    {
        //        await stream.WriteAsync(imageData);
        //    }


        //}

        //public async Task<byte[]> GetImageAsync(string imageName)
        //{
        //    var imagePath = Path.Combine("wwwroot", "Images", imageName);
        //    return await File.ReadAllBytesAsync(imagePath);
        //    //catch (FileNotFoundException)
        //    //{
        //    //    // Handle the exception as needed (e.g., log, return a default image, etc.)
        //    //    throw;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    // Handle the exception as needed
        //    //    throw;
        //    //}
        //}


    }

}



