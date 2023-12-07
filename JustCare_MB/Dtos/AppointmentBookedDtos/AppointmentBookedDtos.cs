using JustCare_MB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustCare_MB.Dtos.AppointmentBookedDtos
{
    public class AppointmentBookedDtos
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string ArabicName { get; set; }
        // user
        public int PatientUserId { get; set; }
    }
}
