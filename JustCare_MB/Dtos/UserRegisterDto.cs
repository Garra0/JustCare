using JustCare_MB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustCare_MB.Dtos
{
    public class UserRegisterDto
    {
        

        [Required]
        [StringLength(50)]
        [DisplayFormat(DataFormatString = "User Name")]
        public string FullName { get; set; }
        [Required]
        [StringLength(100)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }
        [Required]
        [StringLength(100)]
        [Compare("Password", ErrorMessage = "Confirmation password must be sammeler to the first password")]
        [MinLength(6)]
        public string ConfirmationPassword { get; set; }
        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email address.\n Delete spaces before and after the email if there are some")]
        public string Email { get; set; }
        [Required]
        //https://www.regextester.com/99724 RegularExpression tester 
        [RegularExpression(@"^(\+9627[7-9][0-9]{7})|^([0][7][7-9][0-9]{7})$", ErrorMessage = "Invalid Jordanian phone number.")]
        public string PhoneNumber { get; set; }
        [Required]
        [Range(3,120)]
        public int Age { get; set; } //
        public int? NationalId { get; set; } // 30818134




        // UserType
        //[Required]
        //public int UserTypeId { get; set; }
        //public UserType UserType { get; set; }
        // Gender
        [Required]
        public int GenderId { get; set; }
        //public Gender? Gender { get; set; }
        // MedicalHistoryStatus
        //public ICollection<MedicalHistoryStatus> MedicalHistoryStatuses { get; set; }

        

    }
}
