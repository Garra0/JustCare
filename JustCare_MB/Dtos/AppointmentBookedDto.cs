using JustCare_MB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustCare_MB.Dtos
{
    // after booking Appointment , the info will be saved here
    public class AppointmentBookedDto
    {
        [StringLength(500)]
        public string Image { get; set; }
        [StringLength(500)]
        public string Note { get; set; }
        [StringLength(70)]
        public string Status { get; set; }
        public int AppointmentId { get; set; }
        public int PatientUserId { get; set; }
    }
}
