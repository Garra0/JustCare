using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustCare_MB.Dtos.AppointmentBookedDtos
{
    public class DatesDto
    {
        public int CategoryId { get; set; }
        public IEnumerable<AppointmentDates> appointmentDates { get; set; }
        public string CategoryName { get; set; }
        
    }
}
