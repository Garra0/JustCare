namespace JustCare_MB.Dtos.AppointmentDtos
{
    public class MyAppointmentsByDintistTokenDto
    {
        public int Id { get; set; }
        public string DentistDescription { get; set; }
        public DateTime Date { get; set; }
        public CategoryName categoryName { get; set; }
    }
    public class CategoryName
    {
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
    }
}
