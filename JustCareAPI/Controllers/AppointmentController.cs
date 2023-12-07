using JustCare_MB.Dtos;
using JustCare_MB.Dtos.AppointmentDtos;
using JustCare_MB.Models;
using JustCare_MB.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JustCareAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("AppointmentAPI")]
    //("api/v{version:apiVersion}/VillaAPI")
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<IEnumerable<AppointmentDto>> GetAllAppointments()
        {
            IEnumerable<AppointmentDto> appointmentDtos = await _appointmentService.GetAllAppointments();
            return appointmentDtos;

        }

        //[HttpGet("{id:int}", Name = "GetAppointment")]
        //public async Task<User> GetAppointment(int id)
        //{
        //    User user = await _userService.GetUserById(id);
        //    return user;
        //}

        [HttpPost]
        public async Task<ActionResult> CreateAppointment(CreateAppointmentDto appointmentDto)
        {
            await _appointmentService.CreateAppointment(appointmentDto);
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteAppointment(int id)
        {
            bool isSuccess = await _appointmentService.DeleteAppointment(id);
            if (isSuccess)
            {
                return Ok(id);
            }
            return BadRequest(id);
        }

        [HttpPut("{id:int}", Name = "UpdateAppointment")]
        public async Task<ActionResult> UpdateAppointment(int id,UpdateAppointmentDto appointmentDto)
        {
            await _appointmentService.UpdateAppointment(id, appointmentDto);
                return Ok();
        }

        //Task UpdateAppointment(CreateAppointmentDto appointmentDto);
        //Task<IEnumerable<Category>> GetAllCategories();
        //Task<Category> getCategoryObject(int id);
        //Task<CreateAppointmentDto> GetAppointmentDtoToShowCreatePage(int id);
        //Task<IEnumerable<WaitingApprovalAppointmentsBooked>> AllWaitingApprovalAppointments();
        //Task AppointmentStatus(int id, string status);



    }
}
