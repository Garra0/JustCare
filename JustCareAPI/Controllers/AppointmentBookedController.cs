using JustCare_MB.Dtos.AppointmentBookedDtos;
using JustCare_MB.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using JustCare_MB.Dtos.AppointmentDtos;
using Microsoft.AspNetCore.Authorization;

namespace JustCareAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("AppointmentBookedAPI")]
    public class AppointmentBookedController : Controller
    {
        private readonly IAppointmentBookedService _appointmentBookedService;
        public AppointmentBookedController(IAppointmentBookedService appointmentBookedService)
        {
            _appointmentBookedService = appointmentBookedService;
        }

        //[HttpGet("{id:int}", Name = "GetAllAppointmentsBooked")]
        [HttpGet("GetAllAppointmentBooked", Name = "GetAllAppointmentBooked")]
        public async Task<IActionResult> GetAllAppointmentsBooked()
        {
            IEnumerable<AppointmentBookedDtos> AppointmentBookedList;
            AppointmentBookedList = await _appointmentBookedService
                .GetAllAppointmentsBookedByUserToken();
            return Ok(AppointmentBookedList);
        }

        [HttpPost("CreateAppointmentBooked")]
        public async Task<IActionResult> CreateAppointmentBookedDto(CreateAppointmentBookedDto dto)
        {
            await _appointmentBookedService.CreateAppointmentBooked(dto);
            return Ok();
        }

        [HttpGet("GetAllWaitingApprovalAppointments")]
        public async Task<ActionResult<IEnumerable<WaitingApprovalAppointmentsBooked>>> GetAllWaitingApprovalAppointments()
        {
            IEnumerable<WaitingApprovalAppointmentsBooked>
                waitingApprovalAppointmentsBookeds
                = await _appointmentBookedService.GetAllWaitingApprovalAppointments();

            return Ok(waitingApprovalAppointmentsBookeds);
        }

        [HttpPost("AppointmentBooked Accepted")]
        public async Task<IActionResult> AppointmentBookedAccepted(int appointmentBookedId)
        {
            await _appointmentBookedService.AppointmentBookedAccepted(appointmentBookedId);
            return Ok();
        }

        [HttpDelete("AppointmentBooked Rejected")]
        public async Task<IActionResult> AppointmentBookedRejected(int appointmentBookedId)
        {
            await _appointmentBookedService.AppointmentBookedRejected(appointmentBookedId);
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Deleteappointmentbooked(int appointmentBookedId)
        {
            await _appointmentBookedService.DeleteAppointmentBooked(appointmentBookedId);
            return Ok();

        }

        //[HttpPut("{id:int}", Name = "UpdateAppointment")]
        //public Task<bool> UpdateAppointmentBooked(AppointmentBookedDto appointmentBookedDto)
        //{
        ////    await _appointmentService.UpdateAppointment(id, appointmentDto);
        ////    return Ok();
        //}

        //Task CreateAppointmentBooked(CreateAppointmentBookedDto appointmentBookedDto);
        //Task<bool> UpdateAppointmentBooked(AppointmentBookedDto appointmentBookedDto);
        //Task<bool> DeleteAppointmentBooked(int appointmentBookedDtoId);
        //Task<IEnumerable<CategoryDto>> GetAllCategories();
        //Task<DatesDto> GetAllDatesDtoByCategoryId(int id);
        //Task<CreateAppointmentBookedDto> CreateAppointmentBookedDtoAsync(int id);
        //Task<IEnumerable<AppointmentBookedDtos>> GetAllAppointmentsBookedByUserToken();



    }
}
