using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JustCare_MB.Models
{
    public class MedicalHistory
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Database will generate the ID
        public int Id { get; set; }
        [StringLength(150)]
        public string EnglishDisease { get; set; }
        [StringLength(150)]
        public string ArabicDisease { get; set; }

        // relations
        public ICollection<MedicalHistoryStatus> MedicalHistoryStatuses { get; set; }


    }
}
