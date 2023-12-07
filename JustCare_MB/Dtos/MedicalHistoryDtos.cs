using JustCare_MB.Models;
using System.ComponentModel.DataAnnotations;

namespace JustCare_MB.Dtos
{
    public class MedicalHistoryDto
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [StringLength(150)]
        public string EnglishDisease { get; set; }
        [StringLength(150)]
        public string ArabicDisease { get; set; }

        // relations
        public ICollection<MedicalHistoryStatus> MedicalHistoryStatuses { get; set; }


    }
}
