using JustCare_MB.Models;
using System.ComponentModel.DataAnnotations;

namespace JustCare_MB.Dtos
{
    public class UserDto
    {
        [StringLength(50)]
        [DisplayFormat(DataFormatString = "User Name")]
        [Required]
        public string FullName { get; set; }
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(\+9627[7-9][0-9]{7})|^([0][7][7-9][0-9]{7})$", ErrorMessage = "Invalid Jordanian phone number.")]
        public string PhoneNumber { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? BirthDay { get; set; } //
        public int? NationalId { get; set; } // 30818134
    }
}
