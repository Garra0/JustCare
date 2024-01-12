using JustCare_MB.Dtos.User;
using JustCare_MB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustCare_MB.Dtos
{
    public class GenderDto
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string EnglishType { get; set; } //  = null!;
        [StringLength(50)]
        public string ArabicType { get; set; }

        // Relations
        public ICollection<User.UserLoginRequestDto> Users { get; set; } //= new List<User>();


    }
}
