﻿using AutoMapper;
using JustCare_MB.Data;
using JustCare_MB.Dtos.AppointmentBookedDtos;
using JustCare_MB.Dtos;
using JustCare_MB.Models;
using JustCare_MB.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using JustCare_MB.Dtos.AppointmentDtos;
using JustCare_MB.Services;
using Microsoft.AspNetCore.Authorization;
using JustCare_MB.Dtos.Category;

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

        [HttpPost("AppointmentBooked Rejected")]
        public async Task<IActionResult> AppointmentBookedRejected(int appointmentBookedId)
        {
            await _appointmentBookedService.AppointmentBookedRejected(appointmentBookedId);
            return Ok();
        }





        //[HttpDelete("{id:int}")]
        //public Task<IActionResult> DeleteAppointmentBooked(int appointmentBookedDtoId)
        //{
        //bool isSuccess = await _appointmentService.DeleteAppointment(id);
        //    if (isSuccess)
        //    {
        //        return Ok(id);
        //    }
        //    return BadRequest(id);
        //}
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


//public async Task<IActionResult> Index()
//{
//    IEnumerable<CategoryDto> categories = await _appointmentBookedService.GetAllCategories();
//    return View(categories);
//}

//// show the avalibaly dates
//public async Task<IActionResult> ChooseCategory(int Categoryid)
//{
//    DatesDto datesDto = await _appointmentBookedService.GetAllDatesDtoByCategoryId(Categoryid);
//    return View(datesDto);
//}
//[HttpPost]
//public async Task<IActionResult> SelectAppointmentTime(int AppointmentId)
//{
//    // from AppointmentId i can get 
//    // 1- CategoryName  2- Date of the appointment
//    CreateAppointmentBookedDto dto = await _appointmentBookedService.CreateAppointmentBookedDtoAsync(AppointmentId);
//    return View(dto);
//}
//[HttpPost]
//public async Task<IActionResult> Create(CreateAppointmentBookedDto dto)
//{
//    // from AppointmentId i can get 
//    // 1- CategoryName  2- Date of the appointment
//    await _appointmentBookedService.CreateAppointmentBooked(dto);
//    return RedirectToAction(nameof(Index));
//}

//public async Task<IActionResult> MyAppointment()
//{
//    IEnumerable<AppointmentBookedDtos> AppointmentBookedList;
//    AppointmentBookedList = await _appointmentBookedService.GetAllAppointmentsBookedByUserToken();
//    return View(AppointmentBookedList);
//}