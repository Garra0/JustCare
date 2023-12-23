using JustCare_MB.Models;
using System.ComponentModel.DataAnnotations;

namespace JustCare_MB.Dtos.Category
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string EnglishName { get; set; }
        public string ArabicName { get; set; }



    }
}
