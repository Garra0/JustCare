using JustCare_MB.Models;
using System.ComponentModel.DataAnnotations;

namespace JustCare_MB.Dtos
{
    public class UserTypeDto
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string EnglishType { get; set; } // admin , paitent , dental
        [StringLength(50)] 
        public string ArabicType { get; set; }


        // Relations
        public ICollection<UserLoginDto> Users { get; set; }

    }
}
