using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustCare_MB.Dtos.Category
{
    public class CreateCategoryDto
    {
        public string EnglishName { get; set; }
        public string ArabicName { get; set; }
        public byte[] Image { get; set; }

    }
}
