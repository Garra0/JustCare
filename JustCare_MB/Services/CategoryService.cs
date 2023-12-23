using AutoMapper;
using JustCare_MB.Data;
using JustCare_MB.Dtos.Category;
using JustCare_MB.Models;
using JustCare_MB.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustCare_MB.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly JustCareContext _context;
        private readonly IMapper _mapper;
        public CategoryService(JustCareContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CategoryDto>> GetAllCategories()
        {
            IEnumerable<Category> categories = await _context.Categories.ToListAsync();
            IEnumerable<CategoryDto> categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
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
