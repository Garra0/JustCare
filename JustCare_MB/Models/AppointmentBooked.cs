using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustCare_MB.Models
{
    // after booking Appointment , the info will be saved here
    public class AppointmentBooked
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Database will generate the ID
        public int Id { get; set; }
        [StringLength(70)]
        [Required]
        public string Status { get; set; }
        [StringLength(500)]
        [Required]
        public string PatientDescription { get; set; }

        // relations:
        // Appointment 1:1
        public Appointment Appointment { get; set; }
        public int AppointmentId { get; set; }
        // user
        public int PatientUserId { get; set; }
        public User PatientUser { get; set; }

        public ICollection<UserAppointmentImage> UserAppointmentImages { get; set; }

    }
}
