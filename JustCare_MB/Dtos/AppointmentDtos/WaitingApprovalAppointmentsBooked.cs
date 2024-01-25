using JustCare_MB.Dtos.Category;
namespace JustCare_MB.Dtos.AppointmentDtos
{
    public class WaitingApprovalAppointmentsBooked
    {
        public int AppointmentBookedId { get; set; }
        public string PatientDescription { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public PatientUserDtoWaitingApprovalAppointments PatientUser { get; set; }
        public CategoryName Category { get; set; }
        public List<PatienImageDto> Images { get; set; }
    }
    public class PatientUserDtoWaitingApprovalAppointments
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public int Age { get; set; } //
        public int? NationalId { get; set; } // 30818134
        public string UserType { get; set; }
        public string Gender { get; set; }
        public string PatientName { get; set; }
    }
    
    public class PatienImageDto
    {
        public byte[] ImageData { get; set; }
        public string ImageName { get; set; }

    }

}
