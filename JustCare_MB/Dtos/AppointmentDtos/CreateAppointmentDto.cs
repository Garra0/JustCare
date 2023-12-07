using JustCare_MB.Models;
using System.ComponentModel.DataAnnotations;

namespace JustCare_MB.Dtos.AppointmentDtos
{
    public class CreateAppointmentDto
    {
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
        [Required]
        public int? DentistUserId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

    }
}
