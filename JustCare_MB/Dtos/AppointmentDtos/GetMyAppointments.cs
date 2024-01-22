namespace JustCare_MB.Dtos.AppointmentDtos
{
    public class GetMyAppointments
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string CategoryArabicName { get; set; }
        public string CategoryEnglishName { get; set; }
        public string DentistDescription { get; set; }
        public ICollection<MyAppointmentImageDto> Images { get; set; }

    }
    public class MyAppointmentImageDto
    {
        public byte[] ImageData { get; set; }
        public string ImageName { get; set; }

    }


}
