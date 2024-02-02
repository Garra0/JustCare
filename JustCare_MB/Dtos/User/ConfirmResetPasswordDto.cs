using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustCare_MB.Dtos.User
{
    public class ConfirmResetPasswordDto
    {
        public string Email { get; set; }
        [Required]
        [StringLength(100)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }
        [Required]
        [StringLength(100)]
        [Compare("Password", ErrorMessage = "Confirmation password must be sammeler to the first password")]
        [MinLength(6)]
        public string ConfirmationPassword { get; set; }
    }
}
