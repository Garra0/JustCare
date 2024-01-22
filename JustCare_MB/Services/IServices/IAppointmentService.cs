using JustCare_MB.Dtos.AppointmentBookedDtos;
using JustCare_MB.Dtos.AppointmentDtos;
using JustCare_MB.Models;

namespace JustCare_MB.Services.IServices
{
    public interface IAppointmentService
    {
        Task CreateAppointment(CreateAppointmentDto appointmentDto);
        Task<DatesDto> GetAllAppoitnmentDatesDtoByCategoryId(int CategoryId);
        Task DeleteAppointment(int appointmentId);
        Task<UpdateAppointmentDto> GetAppointmentById(int appointmentId);
        Task UpdateAppointment(int id, UpdateAppointmentDto updateAppointmentDto);
        Task<IEnumerable<GetMyAppointments>> MyAppointmentsByDintistToken();
        Task<CreateAppointmentDto> GetAppointmentDtoToShowCreatePage(int id);
      
    }
}
