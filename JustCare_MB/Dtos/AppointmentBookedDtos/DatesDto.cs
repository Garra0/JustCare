using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustCare_MB.Dtos.AppointmentBookedDtos
{
    public class DatesDto
    {
        public int Id { get; set; }
        public IEnumerable<AppointmentsDates> DateInformation { get; set; }
        public string CategoryName { get; set; }

    }
}
