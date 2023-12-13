using JustCare_MB.Dtos;
using JustCare_MB.Dtos.AppointmentBookedDtos;
using JustCare_MB.Models;

namespace JustCare_MB.Services.IServices
{
    public interface IAppointmentBookedService
    {
        //AppointmentBooked AppointmentBookedDtoToAppointmentBooked(AppointmentBookedDto appointmentBookedDto);
        Task CreateAppointmentBooked(CreateAppointmentBookedDto appointmentBookedDto);
        Task<bool> UpdateAppointmentBooked(AppointmentBookedDto appointmentBookedDto);
        Task<bool> DeleteAppointmentBooked(int appointmentBookedDtoId);
        Task<IEnumerable<CategoryDto>> GetAllCategories();
        Task<DatesDto> GetAllDatesDtoByCategoryId(int id);
        Task<CreateAppointmentBookedDto> CreateAppointmentBookedDtoAsync(int id);
        Task<IEnumerable<AppointmentBookedDtos>> GetAllAppointmentsBookedByUserToken();
 }
}

