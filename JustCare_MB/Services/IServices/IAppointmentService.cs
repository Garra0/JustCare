using JustCare_MB.Dtos.AppointmentBookedDtos;
using JustCare_MB.Dtos.AppointmentDtos;
using JustCare_MB.Models;

namespace JustCare_MB.Services.IServices
{
    public interface IAppointmentService
    {
        Task CreateAppointment(CreateAppointmentDto appointmentDto);
        Task<DatesDto> GetAllAppoitnmentDatesDtoByCategoryId(int CategoryId);
        Task<bool> DeleteAppointment(int id);
        Task UpdateAppointment(int id, UpdateAppointmentDto updateAppointmentDto);
        Task<IEnumerable<AppointmentDto>> GetAllAppointments();
        Task<CreateAppointmentDto> GetAppointmentDtoToShowCreatePage(int id);
      
    }
}
