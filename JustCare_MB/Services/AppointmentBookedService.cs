using AutoMapper;
using JustCare_MB.Data;
using JustCare_MB.Dtos;
using JustCare_MB.Dtos.AppointmentBookedDtos;
using JustCare_MB.Dtos.AppointmentDtos;
using JustCare_MB.Dtos.Category;
using JustCare_MB.Helpers;
using JustCare_MB.Models;
using JustCare_MB.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace JustCare_MB.Services
{
    public class AppointmentBookedService : IAppointmentBookedService
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly JustCareContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<AppointmentBookedService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppointmentBookedService(JustCareContext context
            , IMapper mapper, ILogger<AppointmentBookedService> logger
            , IHttpContextAccessor httpContextAccessor, IHostEnvironment hostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _hostEnvironment = hostEnvironment;
        }

        // 5-
        public async Task CreateAppointmentBooked(CreateAppointmentBookedDto appointmentBookedDto)
        {
            _logger.LogInformation(
     $"Create AppointmentBooked: {JsonConvert.SerializeObject(appointmentBookedDto)}");

            if (await _context.AppointmentBookeds.AnyAsync(x => x.AppointmentId == appointmentBookedDto.AppointmentId)
            )
                throw new ExistsException("Appointment is exist -_-\"");

            if (!await _context.Appointments.AnyAsync(x => x.Id == appointmentBookedDto.AppointmentId))
                throw new ExistsException("wrong Appointment id");


            if (appointmentBookedDto == null)
                throw new EmptyFieldException("appointmentBookedDto cant be null");

            AppointmentBooked appointmentBooked = _mapper.Map<AppointmentBooked>(appointmentBookedDto);

            // token
            var patientIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("Id");
            if (patientIdClaim == null
                || !int.TryParse(patientIdClaim.Value, out int patientId))
                throw new NotFoundException("patient token invalid");

            appointmentBooked.PatientUserId = patientId;
            appointmentBooked.Status = "Appointment booked";

            // upload images... (multiy images)
            //string imageName = createCategoryDto.EnglishName + ".jpg";
            //string imagePath = _hostEnvironment.ContentRootPath
            //    + "\\Images\\Categories\\" + imageName;
            //// the next 2 lines eqaules to the above
            ////string im = Path.Combine(_hostEnvironment.ContentRootPath
            ////    + "Images" + "Categories" + imageName);
            //imagePath = imagePath.Replace("JustCareAPI", "JustCare_MB"); // Replace from "JustCareAPI" to "JustCare_MB"  

            //using (var stream = new FileStream(imagePath, FileMode.Create))
            //{
            //    await stream.WriteAsync(createCategoryDto.Image);
            //}


            await _context.AppointmentBookeds.AddAsync(appointmentBooked);
            await _context.SaveChangesAsync();
        }

        // 6-
        public async Task<IEnumerable<WaitingApprovalAppointmentsBooked>> GetAllWaitingApprovalAppointments()
        {
            _logger.LogInformation(
     $"Get All Waiting Approval Appointments (appointmentbookeds)");

            // token
            var DentistIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("Id");
            if (DentistIdClaim == null
                || !int.TryParse(DentistIdClaim.Value, out int DentistId))
                throw new NotFoundException("Dentist token invalid");

            IEnumerable<WaitingApprovalAppointmentsBooked> waitingApprovalAppointmentsBooked =
                await _context.AppointmentBookeds
                .Where(x => x.Status == "Appointment booked")
                // برجع المواعيد التابعة للدكتور مو مواعيد كل الدكاترة..
                .Where(x => x.Appointment.DentistUserId == DentistId
                && x.Appointment.Id == x.Appointment.AppointmentBooked.AppointmentId)
                //.Where(x=> appointmentIdsByDentistUserId.Contains(x.AppointmentId))
                .Select(x => new WaitingApprovalAppointmentsBooked
                {
                    AppointmentBookedId=x.Id,
                    Image = x.Image,
                    Note = x.Note,
                    Status = x.Status,
                    Date = x.Appointment.Date,
                    PatientUser = new PatientUserDtoWaitingApprovalAppointments
                    {
                        Id = x.PatientUser.Id,
                        PhoneNumber = x.PatientUser.PhoneNumber,
                        Age = x.PatientUser.Age,
                        NationalId = x.PatientUser.NationalId,
                        UserType = x.PatientUser.UserType.ArabicType,
                        Gender = x.PatientUser.Gender.ArabicType,
                    },
                    Category = new CategoryDtoWaitingApprovalAppointments
                    {
                        ArabicName = x.Appointment.Category.ArabicName
                    }
                }).OrderBy(e => e.Date).ToListAsync();

            if (waitingApprovalAppointmentsBooked == null)
                throw new NotFoundException
                    ("there are no Approval AppointmentsBooked on waiting");

            return waitingApprovalAppointmentsBooked;
        }

        // 7.Accepted-
        public async Task AppointmentBookedAccepted(int appointmentBookedId)
        {
            _logger.LogInformation(
      $"AppointmentBooked Accepted: {JsonConvert.SerializeObject(appointmentBookedId)}");

            if (!await _context.AppointmentBookeds.AnyAsync(x => x.Id == appointmentBookedId))
                throw new InvalidIdException("appointmentBooked Id is not exist");

            AppointmentBooked appointmentBooked = await _context
                .AppointmentBookeds.FirstAsync(x => x.Id == appointmentBookedId);
            if (appointmentBooked == null)
                throw new InvalidIdException("the appointmentId is wrong");

            if (appointmentBooked.Status == "Accepted")
                throw new ExistsException("This appoitnmentBooked have been accepted");

            appointmentBooked.Status = "Accepted";
            _context.AppointmentBookeds.Update(appointmentBooked);
            await _context.SaveChangesAsync();
        }

        // 7.Rejected-
        public async Task AppointmentBookedRejected(int appointmentBookedId)
        {
            _logger.LogInformation(
      $"AppointmentBooked Accepted: {JsonConvert.SerializeObject(appointmentBookedId)}");

            if (!await _context.AppointmentBookeds.AnyAsync(x => x.Id == appointmentBookedId))
                throw new InvalidIdException("Id is not exist");

            AppointmentBooked appointmentBooked = await _context
              .AppointmentBookeds.FirstAsync(x => x.Id == appointmentBookedId);
            _context.AppointmentBookeds.Remove(appointmentBooked);
            await _context.SaveChangesAsync();
        }

        // to show the use page..
        public async Task<CreateAppointmentBookedDto> CreateAppointmentBookedDto(int AppointmentId)
        {
            if (!await _context.Appointments.AnyAsync(x => x.Id == AppointmentId))
                throw new InvalidIdException("id is not exist");

            DateTime date = await _context.Appointments
                .Where(e => e.Id == AppointmentId).Select(e => e.Date)
                .FirstAsync();
            // select category name -> appointment -> categoryId -> categoryName
            int CategoryId = await _context.Appointments
                .Where(e => e.Id == AppointmentId).Select(e => e.CategoryId)
                .FirstAsync();

            if (!await _context.Categories.AnyAsync(x => x.Id == CategoryId))
                throw new InvalidIdException("id is not exist");

            string CategoryName = await _context.Categories.
                Where(e => e.Id == CategoryId).Select(e => e.ArabicName).FirstAsync();

            CreateAppointmentBookedDto dto = new CreateAppointmentBookedDto();
            dto.CategoryName = CategoryName;
            dto.AppointmentId = AppointmentId;
            dto.Date = date;
            return dto;

        }



        public Task<bool> DeleteAppointmentBooked(int appointmentBookedDtoId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAppointmentBooked(AppointmentBookedDto appointmentBookedDto)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AppointmentBookedDtos>> GetAllAppointmentsBookedByUserToken()
        {
            // token
            var DentistIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("Id");
            if (DentistIdClaim == null
                || !int.TryParse(DentistIdClaim.Value, out int DentistId))
                throw new NotFoundException("Dentist token invalid");

            IEnumerable<AppointmentBookedDtos> appointmentBookedDtos =
                await _context.AppointmentBookeds
                .Where(x => x.Status != "Accepted" && x.PatientUserId == 10)
                .Select(x => new AppointmentBookedDtos
                {
                    Id = x.Id,
                    Date = x.Appointment.Date,
                    ArabicName = x.Appointment.Category.ArabicName,
                    PatientUserId = x.PatientUserId
                })
                .OrderBy(e => e.Date)
                .ToListAsync();

            return appointmentBookedDtos;
        }


    }
}
