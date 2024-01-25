using JustCare_MB.Dtos.Category;

namespace JustCare_MB.Dtos.AppointmentBookedDtos
{
    public class AppointmentBookedDtos
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public CategoryName categoryName { get; set; }
    }
}
