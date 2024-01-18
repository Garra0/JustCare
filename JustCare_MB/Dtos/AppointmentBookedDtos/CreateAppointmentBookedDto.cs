using System.ComponentModel.DataAnnotations;

namespace JustCare_MB.Dtos.AppointmentBookedDtos
{
    public class CreateAppointmentBookedDto
    {
        public int AppointmentId { get; set; }
       // public DateTime Date { get; set; }
        //public string CategoryName { get; set; }
        //public string Image { get; set; }
        public string PatientDescription { get; set; }
        [MaxLength(5, ErrorMessage = "You can upload a maximum of 5 images.")]
        public ICollection<byte[]> Images { get; set; }
    }
}
