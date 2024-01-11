using JustCare_MB.Models;
using System.ComponentModel.DataAnnotations;

namespace JustCare_MB.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        [DisplayFormat(DataFormatString = "User Name")]
        public string FullName { get; set; }// first and last name
        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(\+9627[7-9][0-9]{7})|^([0][7][7-9][0-9]{7})$", ErrorMessage = "Invalid Jordanian phone number.")]
        public string PhoneNumber { get; set; }
        [Required]
        [Range(3, 120)]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Age { get; set; }
        public int? NationalId { get; set; } // 30818134
         
         
    }
}
