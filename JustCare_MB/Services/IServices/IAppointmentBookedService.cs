using JustCare_MB.Dtos;
using JustCare_MB.Dtos.AppointmentBookedDtos;
using JustCare_MB.Dtos.AppointmentDtos;
using JustCare_MB.Dtos.Category;
using JustCare_MB.Models;

namespace JustCare_MB.Services.IServices
{
    public interface IAppointmentBookedService
    {
        //AppointmentBooked AppointmentBookedDtoToAppointmentBooked(AppointmentBookedDto appointmentBookedDto);
        Task CreateAppointmentBooked(CreateAppointmentBookedDto appointmentBookedDto);
        Task<IEnumerable<WaitingApprovalAppointmentsBooked>> GetAllWaitingApprovalAppointments();
        Task AppointmentBookedAccepted(int AppointmentBookedId);
        Task AppointmentBookedRejected(int AppointmentBookedId);
        Task DeleteAppointmentBooked(int appointmentBookedDtoId);


        Task<bool> UpdateAppointmentBooked(AppointmentBookedDto appointmentBookedDto);
        //Task<IEnumerable<CategoryDto>> GetAllCategories();
        Task<CreateAppointmentBookedDto> CreateAppointmentBookedDto(int id);
        Task<IEnumerable<AppointmentBookedDtos>> GetAllAppointmentsBookedByUserToken();
 }
}

