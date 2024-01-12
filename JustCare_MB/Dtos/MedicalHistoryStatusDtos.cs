using JustCare_MB.Dtos.User;
using JustCare_MB.Models;
using System.ComponentModel.DataAnnotations;

namespace JustCare_MB.Dtos
{
    public class MedicalHistoryStatusDto
    {
        [Required]
        [Key]
        public int Id { get; set; }
        public bool Status { get; set; }

        // Relations:
        // User
        public int UserId { get; set; }
        public UserLoginRequestDto User { get; set; }
        // MedicalHistory
        public int MedicalHistoryId { get; set; }
        public MedicalHistory MedicalHistory { get; set; }

    }
}
