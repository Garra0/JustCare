using JustCare_MB.Dtos;
using JustCare_MB.Dtos.AppointmentDtos;
using JustCare_MB.Models;
using JustCare_MB.Services;
using JustCare_MB.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
        public async Task<IActionResult> UpdateAppointment(int id, UpdateAppointmentDto appointmentDto)
        {
            await _appointmentService.UpdateAppointment(id, appointmentDto);
            return Ok();
        }

        [HttpGet("GetAllCategorys")]
        public async Task<IActionResult> GetAllCategorys()
        {
            IEnumerable<Category> categories = await _appointmentService.GetAllCategories();
            return Ok(categories);
        }


        //Task<Category> getCategoryObject(int id);
        //Task<CreateAppointmentDto> GetAppointmentDtoToShowCreatePage(int id);
        //Task<IEnumerable<WaitingApprovalAppointmentsBooked>> AllWaitingApprovalAppointments();

        [HttpGet("MyAppointments")]
        public async Task<IActionResult> MyAppointments()
        {
            IEnumerable<AppointmentDto> AppointmentList;
            AppointmentList = await _appointmentService.GetAllAppointments();
            return Ok(AppointmentList);
        }

        public async Task<IActionResult> AcceptBooking()
        {
            return View(await _appointmentService.AllWaitingApprovalAppointments());
        }
        public async Task<IActionResult> Accepted_Rejected(int id, string status)
        {
            await _appointmentService.AppointmentStatus(id, status);
            return RedirectToAction(nameof(Index));
        }
    }
}



