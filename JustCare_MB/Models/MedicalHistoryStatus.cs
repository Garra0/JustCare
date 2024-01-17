 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustCare_MB.Models
{
    public class MedicalHistoryStatus
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Database will generate the ID
        public int Id { get; set; }
        [Required]
        public bool Status { get; set; }

        // Relations:
        // User
        public int UserId { get; set; }
        public User User { get; set; }
        // MedicalHistory
        public int MedicalHistoryId { get; set; }
        public MedicalHistory MedicalHistory { get; set; }

    }
}
