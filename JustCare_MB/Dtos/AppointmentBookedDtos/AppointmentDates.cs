using JustCare_MB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustCare_MB.Dtos.AppointmentBookedDtos
{
    public class AppointmentDates
    {
        public int AppointmentId { get; set; }
        public DateTime Date { get; set; }
        public List<DentistAppointmentImage> DentistAppointmentImages { get; set; }
        public string DentistDescription { get; set; }
        public IList<byte[]> images { get; set; }
    }
    
}
