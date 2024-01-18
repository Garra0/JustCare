using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JustCare_MB.Models
{
    public class UserAppointmentImage
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Database will generate the ID
        public int Id { get; set; }
        [Required]
        public string ImageName { get; set; }

        public Appointment Appointment { get; set; }
        public int? AppointmentId { get; set; }
        public AppointmentBooked AppointmentBooked { get; set; }
        public int? AppointmentBookedId { get; set; }

    }
}
