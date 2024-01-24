using JustCare_MB.Dtos;
using JustCare_MB.Dtos.AppointmentBookedDtos;
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

        //2-
        [HttpPost("Create appointment"), Authorize(Roles = "Dentist")]
        public async Task<ActionResult> CreateAppointment(CreateAppointmentDto appointmentDto)
        {
            await _appointmentService.CreateAppointment(appointmentDto);
            return Ok();
        }
        //4-
        [HttpGet("{Categoryid:int}", Name = "GetAllDatesDtoByCategoryId")]
        public async Task<IActionResult> GetAllDatesDtoByCategoryId(int Categoryid)
        {
            DatesDto AppoitnmentsDate = await _appointmentService.GetAllAppoitnmentDatesDtoByCategoryId(Categoryid);
            return Ok(AppoitnmentsDate);
        }


        [HttpGet]
        public async Task<IEnumerable<GetMyAppointments>> GetAllAppointments()
        {
            IEnumerable<GetMyAppointments> appointmentDtos = await _appointmentService.MyAppointmentsByDintistToken();
            return appointmentDtos;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAppointment(int appointmentId)
        {
            await _appointmentService.DeleteAppointment(appointmentId);
            return Ok();

        }
        [HttpGet("GetAppointmentById/{Id:int}")]
        public async Task<IActionResult> GetAppointmentById(int Id)
        {
            return Ok(await _appointmentService.GetAppointmentById(Id));
        }
        [HttpPut("UpdateAppointment/{id:int}", Name = "UpdateAppointment")]
        public async Task<IActionResult> UpdateAppointment(int id, UpdateAppointmentDto appointmentDto)
        {
            await _appointmentService.UpdateAppointment(id, appointmentDto);
            return Ok();
        }
        [HttpGet("MyAppointmentsByDintistToken")]
        public async Task<IActionResult> MyAppointmentsByDintistToken()
        { 
            return Ok(await _appointmentService.MyAppointmentsByDintistToken());
        }
        [HttpGet("GetAllMyAppointmentNotBookedYet")]
        public async Task<IActionResult> GetAllMyAppointmentNotBookedYet()
        {
            return Ok(await _appointmentService.GetAllMyAppointmentNotBookedYet());
        }
        


        //Task<Category> getCategoryObject(int id);
        //Task<CreateAppointmentDto> GetAppointmentDtoToShowCreatePage(int id);



        //public async Task<IActionResult> AcceptBooking()
        //{
        //    return View(await _appointmentService.AllWaitingApprovalAppointments());
        //}
        //public async Task<IActionResult> Accepted_Rejected(int id, string status)
        //{
        //    await _appointmentService.AppointmentStatus(id, status);
        //    return RedirectToAction(nameof(Index));
        //}
    }
}



