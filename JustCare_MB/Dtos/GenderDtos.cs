using JustCare_MB.Dtos.User;
using System.ComponentModel.DataAnnotations;

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
        public ICollection<UserLoginRequestDto> Users { get; set; } //= new List<User>();


    }
}
