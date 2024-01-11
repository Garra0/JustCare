using AutoMapper;
using JustCare_MB.Data;
using JustCare_MB.Dtos.AppointmentBookedDtos;
using JustCare_MB.Dtos.AppointmentDtos;
using JustCare_MB.Dtos.Category;
using JustCare_MB.Helpers;
using JustCare_MB.Models;
using JustCare_MB.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Security.Claims;

namespace JustCare_MB.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly JustCareContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AppointmentService> _logger;

        public AppointmentService(JustCareContext context
            , IMapper mapper, IHttpContextAccessor httpContextAccessor
            , ILogger<AppointmentService> logger)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        //2-
        public async Task CreateAppointment(CreateAppointmentDto appointmentDto)
        {
            _logger.LogInformation(
                 $"Create Appointment: {JsonConvert.SerializeObject(appointmentDto)}");
            
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("Id");
            if (userIdClaim == null
                || !int.TryParse(userIdClaim.Value, out int dentistUserId))
                throw new NotFoundException("The token invalid");

            // appointment not Exists?
            if (await _context.Appointments.AnyAsync(u => u.Date == appointmentDto.Date)
                && await _context.Appointments.AnyAsync(u => u.DentistUserId == dentistUserId))
                throw new ExistsException("Appointment Exists");

            // the appointment should be after 12h at least
            if (appointmentDto.Date < DateTime.Now.AddHours(12))
                throw new TimeNotValid("The appointment must be at least 12 hours away");



            Appointment appointment = _mapper.Map<Appointment>(appointmentDto);
            appointment.DentistUserId = dentistUserId;
            await _context.Appointments.AddAsync(appointment);
            _context.SaveChanges();
        }

        //4.1-
        public async Task<DatesDto> GetAllAppoitnmentDatesDtoByCategoryId(int Categoryid)
        {
            _logger.LogInformation(
                $"Get All Appoitnment Dates By Category Id: {JsonConvert.SerializeObject(Categoryid)}");

            if (!await _context.Categories.AnyAsync(x => x.Id == Categoryid))
                throw new InvalidIdException("id is not exist");

            if (!await _context.Appointments.AnyAsync())
                throw new NotFoundException("There are no appointments");

            DatesDto datesDto = await GetAllAvailableAppointmentByCategoryId(Categoryid);
            return datesDto;
        }

        //4.2
        private async Task<DatesDto> GetAllAvailableAppointmentByCategoryId(int CategoryId)
        {
            // all appoitnments - appoitnmentsbooked - other categories
            IEnumerable<AppointmentDates> Dates
            = await _context.Appointments.
            // dont select the appoitnments witch are on AppointmentBooked table
            Where(e => e.Id != e.AppointmentBooked.AppointmentId
            && e.CategoryId == CategoryId)
            .Select(e => new AppointmentDates { AppointmentId = e.Id, Date = e.Date }).OrderBy(e => e.Date)
            .ToListAsync();

            if (Dates == null)
                throw new NotFoundException("There are no Appointments by this Category");

            string CategoryName = await _context.Categories
                .Where(e => e.Id == CategoryId)
                .Select(x => x.ArabicName).FirstAsync();

            DatesDto dto = new DatesDto();
            dto.CategoryId = CategoryId;
            dto.CategoryName = CategoryName;
            dto.appointmentDates = Dates;

            return dto;
        }



        public async Task<CreateAppointmentDto> GetAppointmentDtoToShowCreatePage(int CategorId)
        {
            CreateAppointmentDto createAppointmentDto = new CreateAppointmentDto();
            string CategoryName = await _context.Categories.
                Where(u => u.Id == CategorId).Select(e => e.ArabicName).
                FirstAsync();
            createAppointmentDto.CategoryName = CategoryName;
            createAppointmentDto.CategoryId = CategorId;
            return createAppointmentDto;
        }


        public async Task<bool> DeleteAppointment(int id)
        {
            //if (!_context.Categories.Any(x => x.Id == CategorId))
            //    throw new InvalidIdException("id is not exist");
            if (id == 0)
                return false;
            var appointment = await _context.Appointments.FirstOrDefaultAsync(x => x.Id == id);
            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<AppointmentDto>> GetAllAppointments()
        {
            IEnumerable<AppointmentDto> appointmentDto =
                await _context.Appointments
                .Select(x => new AppointmentDto
                {
                    Id = x.Id,
                    Date = x.Date,
                    DentistUserId = x.DentistUserId,
                    CategoryArabicName = x.Category.ArabicName
                })
                .OrderBy(e => e.Date)
                .ToListAsync();

            return appointmentDto;

        }

        public async Task UpdateAppointment(int id, UpdateAppointmentDto updateAppointmentDto)
        {
            if (updateAppointmentDto == null || updateAppointmentDto.Id != id)
                throw new Exception("information is null or the id is deffrinet");

            Appointment appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null)
            {
                throw new Exception("appointment not found");
            }

            _mapper.Map(updateAppointmentDto, appointment);
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }


    }

}
