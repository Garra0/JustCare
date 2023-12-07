using JustCare_MB.Dtos.AppointmentDtos;
using JustCare_MB.Models;
using JustCare_MB.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace LoadTheDb.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentService _appointmentService;
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> CategoryList;
            CategoryList = await _appointmentService.GetAllCategories();
            return View(CategoryList);
        }

        public async Task<IActionResult> Create(int id)
        {
            CreateAppointmentDto dto = await _appointmentService.GetAppointmentDtoToShowCreatePage(id);
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateAppointmentDto dto)
        {
            await _appointmentService.CreateAppointment(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MyAppointment()
        {
            IEnumerable<AppointmentDto> AppointmentList;
            AppointmentList = await _appointmentService.GetAllAppointments();
            return View(AppointmentList);
        }

        public async Task<IActionResult> Delete(int id)
        {
            bool IsSuccess = await _appointmentService.DeleteAppointment(id);
            if (ModelState.IsValid)
            {
                if (IsSuccess)
                {
                    TempData["success"] = "User deleted Successfully";
                    return RedirectToAction(nameof(MyAppointment));
                }
            }
            TempData["error"] = "Error encountered";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AcceptBooking()
        {
            return View(await _appointmentService.AllWaitingApprovalAppointments());
        }
        public async Task<IActionResult> Accepted_Rejected(int id,string status)
        {
            await _appointmentService.AppointmentStatus(id,status);
            return RedirectToAction(nameof(Index));
        }
    }
}
