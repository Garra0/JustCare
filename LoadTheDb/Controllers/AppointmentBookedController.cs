using JustCare_MB.Dtos;
using JustCare_MB.Dtos.AppointmentBookedDtos;
using JustCare_MB.Models;
using JustCare_MB.Services;
using JustCare_MB.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace LoadTheDb.Controllers
{
    public class AppointmentBookedController : Controller
    {
        private readonly IAppointmentBookedService _appointmentBookedService;

        public AppointmentBookedController(IAppointmentBookedService appointmentBookedService)
        {
            _appointmentBookedService = appointmentBookedService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<CategoryDto> categories =await _appointmentBookedService.GetAllCategories();
            return View(categories);
        }

        // show the avalibaly dates
        public async Task<IActionResult> ChooseCategory(int Categoryid)
        {
            DatesDto datesDto = await _appointmentBookedService.GetAllDatesDtoByCategoryId(Categoryid);
            return View(datesDto);
        }
        [HttpPost]
        public async Task<IActionResult> SelectAppointmentTime(int AppointmentId)
        {
            // from AppointmentId i can get 
            // 1- CategoryName  2- Date of the appointment
            CreateAppointmentBookedDto dto = await _appointmentBookedService.CreateAppointmentBookedDtoAsync(AppointmentId);
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateAppointmentBookedDto dto)
        {
            // from AppointmentId i can get 
            // 1- CategoryName  2- Date of the appointment
            await _appointmentBookedService.CreateAppointmentBooked(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MyAppointment()
        {
            IEnumerable<AppointmentBookedDtos> AppointmentBookedList;
            AppointmentBookedList = await _appointmentBookedService.GetAllAppointmentsBookedByUserToken();
            return View(AppointmentBookedList);
        }


    }
}
