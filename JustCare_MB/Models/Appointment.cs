using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JustCare_MB.Models
{
    public class Appointment
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Database will generate the ID
        public int Id { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        //public bool State { get; set; }

        //Relations:

        public int DentistUserId { get; set; }
        public User DentistUser { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public AppointmentBooked AppointmentBooked { get; set; }
        //public int AppointmentBookedId { get; set; }

        // -->
        //public virtual ICollection<AppointmentBooked> AppointmentBookeds { get; set; } = new List<AppointmentBooked>();

    }
}
