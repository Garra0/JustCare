using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustCare_MB.Dtos.AppointmentBookedDtos
{
    public class CreateAppointmentBookedDto
    {
        public int AppointmentId { get; set; }
        public DateTime Date { get; set; }
        public string CategoryName { get; set; }
        public string Image { get; set; }
        public string Note { get; set; } 
    }
}
