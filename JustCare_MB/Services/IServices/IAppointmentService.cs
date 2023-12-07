using JustCare_MB.Dtos.AppointmentDtos;
using JustCare_MB.Models;

namespace JustCare_MB.Services.IServices
{
    public interface IAppointmentService
    {
        Task CreateAppointment(CreateAppointmentDto appointmentDto);
        Task<bool> DeleteAppointment(int id);
        Task UpdateAppointment(int id, UpdateAppointmentDto updateAppointmentDto);
        Task<IEnumerable<AppointmentDto>> GetAllAppointments();
        Task<IEnumerable<Category>> GetAllCategories();
        Task<Category> getCategoryObject(int id);
        Task<CreateAppointmentDto> GetAppointmentDtoToShowCreatePage(int id);
        Task<IEnumerable<WaitingApprovalAppointmentsBooked>> AllWaitingApprovalAppointments();
        Task AppointmentStatus(int id,string status);
    }
}
