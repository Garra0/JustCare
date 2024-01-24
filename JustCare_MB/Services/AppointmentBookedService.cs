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
using System.Security.Claims;

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

            if (appointmentBookedDto.Images.Count > 5)
            {
                throw new ImagesBadRequest("You can upload a maximum of 5 images.");
            }
            bool flag = false;
            foreach (var image in appointmentBookedDto.Images)
            {
                if (image.Length > 0)
                    flag = true;
                if (image.Length > (5 * 1024 * 1024)) // Assuming 5 MB as the maximum size
                {
                    throw new ImagesBadRequest("Each image can have a maximum size of 5 MB.");
                }
            }

            var patientIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("Id");
            if (patientIdClaim == null
                || !int.TryParse(patientIdClaim.Value, out int patientId))
                throw new NotFoundException("patient token invalid");

            if (await _context.AppointmentBookeds
                .AnyAsync(x => x.AppointmentId == appointmentBookedDto.AppointmentId))
                throw new ExistsException("Appointment Booked is exist -_-\"");

            if (!await _context.Appointments.AnyAsync(x => x.Id == appointmentBookedDto.AppointmentId))
                throw new ExistsException("wrong Appointment id");

            AppointmentBooked appointmentBooked = _mapper.Map<AppointmentBooked>(appointmentBookedDto);

            // token

            appointmentBooked.PatientUserId = patientId;
            appointmentBooked.Status = "Appointment booked";

            appointmentBooked.UserAppointmentImages =
                new List<UserAppointmentImage>();

            if (flag)
            {
                if (!Directory.Exists("C:\\Images\\UserAppointmentImages"))
                {
                    Directory.CreateDirectory("C:\\Images\\UserAppointmentImages");
                }
                foreach (var image in appointmentBookedDto.Images)
                {
                    string imageName = Guid.NewGuid().ToString() + ".jpg";
                    UserAppointmentImage patientAppointmentImage
                     = new UserAppointmentImage
                     {
                         ImageName = imageName,
                     };
                    appointmentBooked.UserAppointmentImages.Add(patientAppointmentImage);
                    using (var stream = new FileStream(
                        "C:\\Images\\UserAppointmentImages\\"
                        + imageName, FileMode.Create))
                    {
                        await stream.WriteAsync(image);
                    }
                }
            }
            await _context.AppointmentBookeds.AddAsync(appointmentBooked);
            await _context.SaveChangesAsync();
        }

        // 6-
        public async Task<IEnumerable<WaitingApprovalAppointmentsBooked>> GetAllWaitingApprovalAppointmentsUsingDentistToken()
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
                    AppointmentBookedId = x.Id,
                    PatientDescription = x.PatientDescription,
                    Status = x.Status,
                    Images = x.UserAppointmentImages.Select(x => new PatienImageDto
                    {
                        ImageName = x.ImageName,
                    }).ToList(),
                    Date = x.Appointment.Date,
                    PatientUser = new PatientUserDtoWaitingApprovalAppointments
                    {
                        Id = x.PatientUser.Id,
                        PhoneNumber = x.PatientUser.PhoneNumber,
                        BirthDay = x.PatientUser.BirthDay,
                        NationalId = x.PatientUser.NationalId,
                        UserType = x.PatientUser.UserType.ArabicType,
                        Gender = x.PatientUser.Gender.ArabicType,
                        PatientName = x.PatientUser.FullName
                    },
                    Category = new CategoryDtoWaitingApprovalAppointments
                    {
                        ArabicName = x.Appointment.Category.ArabicName,
                        EnglishName = x.Appointment.Category.EnglishName,
                    }
                }).OrderBy(e => e.Date).ToListAsync();

            if (!waitingApprovalAppointmentsBooked.Any())
                throw new NotFoundException
                    ("there are no Approval AppointmentsBooked on waiting");

            if (Directory.Exists("C:\\Images\\UserAppointmentImages"))
            {
                foreach (var approvalAppointmentsBooked in waitingApprovalAppointmentsBooked)
                {
                    //AppoinmtnetDate.Images = new List<Image>();
                    foreach (var ImageDto in approvalAppointmentsBooked.Images)
                    {
                        string ImagePath = "C:\\Images\\UserAppointmentImages\\"
                            + ImageDto.ImageName;
                        ImageDto.ImageData = (await File.ReadAllBytesAsync(ImagePath));
                    }
                }
            }

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

            if (appointmentBooked.Status == "Active")
                throw new ExistsException("This appoitnmentBooked have been accepted");

            appointmentBooked.Status = "Active";
            _context.AppointmentBookeds.Update(appointmentBooked);
            await _context.SaveChangesAsync();
        }

        // 7.Rejected-
        public async Task AppointmentBookedRejected(int appointmentBookedId)
        {
            _logger.LogInformation(
      $"AppointmentBooked Rejected: {JsonConvert.SerializeObject(appointmentBookedId)}");

            if (!await _context.AppointmentBookeds.AnyAsync(x => x.Id == appointmentBookedId))
                throw new InvalidIdException("Id is not exist");

            AppointmentBooked appointmentBooked = await _context
              .AppointmentBookeds.Include(x => x.UserAppointmentImages)
              .FirstOrDefaultAsync(x => x.Id == appointmentBookedId);

            if (appointmentBooked == null)
                throw new NotFoundException("appointmentBooked is not exist");

            if (Directory.Exists("C:\\Images\\UserAppointmentImages\\"))
                foreach (var PatientAppointmentImages in appointmentBooked.UserAppointmentImages)
                {
                    if (File.Exists("C:\\Images\\UserAppointmentImages\\"
                            + PatientAppointmentImages.ImageName))
                        File.Delete("C:\\Images\\UserAppointmentImages\\"
                            + PatientAppointmentImages.ImageName);
                }
            _context.RemoveRange(appointmentBooked.UserAppointmentImages);
            _context.AppointmentBookeds.Remove(appointmentBooked);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAppointmentBooked(int appointmentBookedId)
        {
            _logger.LogInformation(
      $"Delete AppointmentBooked: {JsonConvert.SerializeObject(appointmentBookedId)}");

            if (!await _context.AppointmentBookeds.AnyAsync(x => x.Id == appointmentBookedId))
                throw new InvalidIdException("Id is not exist on AppointmentBooked");

            AppointmentBooked appointmentBooked = await _context
              .AppointmentBookeds.Include(x => x.UserAppointmentImages)
              .FirstOrDefaultAsync(x => x.Id == appointmentBookedId);

            if (appointmentBooked == null)
                throw new NotFoundException("appointmentBooked is not exist");

            if (Directory.Exists("C:\\Images\\UserAppointmentImages\\"))
                foreach (var PatientAppointmentImages in appointmentBooked.UserAppointmentImages)
                {
                    if (File.Exists("C:\\Images\\UserAppointmentImages\\"
                            + PatientAppointmentImages.ImageName))
                        File.Delete("C:\\Images\\UserAppointmentImages\\"
                        + PatientAppointmentImages.ImageName);
                }
            _context.RemoveRange(appointmentBooked.UserAppointmentImages);
            _context.AppointmentBookeds.Remove(appointmentBooked);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAppointmentBooked(int Id, UpdateAppointmentBookedDto updateAppointmentBookedDto)
        {
            if (updateAppointmentBookedDto == null)
                throw new EmptyFieldException("updateAppointment has Empty Field Exception");

            if (!await _context.AppointmentBookeds.AnyAsync(x => x.Id == Id))
                throw new InvalidIdException("Id is not exist on AppointmentBookeds");

            AppointmentBooked appointmentBooked = await _context.AppointmentBookeds
                .FirstOrDefaultAsync(x => x.Id == Id);
            if (appointmentBooked == null)
            {
                throw new NotFoundException("appointment not found");
            }

            _mapper.Map(updateAppointmentBookedDto, appointmentBooked);
            _context.AppointmentBookeds.Update(appointmentBooked);
            await _context.SaveChangesAsync();
        }
        // Active Appointment Booked for Dentist page
        public async Task<IEnumerable<ActiveAppointmentBookedForDentistDto>> GetAllActiveAppointmentBookedForDentist()
        {
            _logger.LogInformation(
     $"Get All Get All Active AppointmentBooked For Dentist");

            // token
            var DentistIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("Id");
            if (DentistIdClaim == null
                || !int.TryParse(DentistIdClaim.Value, out int DentistId))
                throw new NotFoundException("User token invalid");

            IEnumerable<ActiveAppointmentBookedForDentistDto> activeAppointmentBookedDto =
                await _context.AppointmentBookeds
                .Where(x => x.Status == "Active")
                .Where(x => x.Appointment.DentistUserId == DentistId
                && x.Appointment.Id == x.Appointment.AppointmentBooked.AppointmentId)
                .Select(x => new ActiveAppointmentBookedForDentistDto
                {
                    appointmentDto = new AppointmentDtoActiveAppointmentBookedDto
                    {

                        Id = x.Id,
                        Date = x.Appointment.Date,
                        categoryName = new CategoryName
                        {
                            ArabicName = x.Appointment.Category.ArabicName,
                            EnglishName = x.Appointment.Category.EnglishName,
                        }
                    },
                    userDto = new patientInformationActiveAppointmentBooked
                    {
                        patientId = x.PatientUserId,
                        FullName = x.PatientUser.FullName,
                        PhoneNumber = x.PatientUser.PhoneNumber
                    }
                }

                ).OrderBy(x => x.appointmentDto.Date).ToListAsync();

            if (!activeAppointmentBookedDto.Any())
                throw new NotFoundException
                    ("there are no active AppointmentsBooked yet");

            return activeAppointmentBookedDto;
        }


     //   // Active Appointment Booked for patient page
     //   public async Task<IEnumerable<ActiveAppointmentBookedForDentistDto>> GetAllActiveAppointmentBookedForPatient()
     //   {
     //       _logger.LogInformation(
     //$"Get All Get All Active AppointmentBooked For Dentist");

     //       // token
     //       var DentistIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("Id");
     //       if (DentistIdClaim == null
     //           || !int.TryParse(DentistIdClaim.Value, out int DentistId))
     //           throw new NotFoundException("User token invalid");

     //       IEnumerable<ActiveAppointmentBookedForDentistDto> activeAppointmentBookedDto =
     //           await _context.AppointmentBookeds
     //           .Where(x => x.Status == "Active")
     //           .Where(x => x.Appointment.DentistUserId == DentistId
     //           && x.Appointment.Id == x.Appointment.AppointmentBooked.AppointmentId)
     //           .Select(x => new ActiveAppointmentBookedForDentistDto
     //           {
     //               appointmentDto = new AppointmentDtoActiveAppointmentBookedDto
     //               {

     //                   Id = x.Id,
     //                   Date = x.Appointment.Date,
     //                   categoryName = new CategoryName
     //                   {
     //                       ArabicName = x.Appointment.Category.ArabicName,
     //                       EnglishName = x.Appointment.Category.EnglishName,
     //                   }
     //               },
     //               userDto = new DentistInformationActiveAppointmentBooked
     //               {
     //                   DentistId = x.Appointment.DentistUserId,
     //                   FullName = x.Appointment.DentistUser.FullName,
     //                   PhoneNumber = x.Appointment.DentistUser.PhoneNumber
     //               }
     //           }

     //           ).OrderBy(x => x.appointmentDto.Date).ToListAsync();

     //       if (!activeAppointmentBookedDto.Any())
     //           throw new NotFoundException
     //               ("there are no active AppointmentsBooked yet");

     //       return activeAppointmentBookedDto;
     //   }
















        // to show the use page..
        //public async Task<CreateAppointmentBookedDto> CreateAppointmentBookedDto(int AppointmentId)
        //{
        //    if (!await _context.Appointments.AnyAsync(x => x.Id == AppointmentId))
        //        throw new InvalidIdException("id is not exist");

        //    DateTime date = await _context.Appointments
        //        .Where(e => e.Id == AppointmentId).Select(e => e.Date)
        //        .FirstAsync();
        //    // select category name -> appointment -> categoryId -> categoryName
        //    int CategoryId = await _context.Appointments
        //        .Where(e => e.Id == AppointmentId).Select(e => e.CategoryId)
        //        .FirstAsync();

        //    if (!await _context.Categories.AnyAsync(x => x.Id == CategoryId))
        //        throw new InvalidIdException("id is not exist");

        //    string CategoryName = await _context.Categories.
        //        Where(e => e.Id == CategoryId).Select(e => e.ArabicName).FirstAsync();

        //    CreateAppointmentBookedDto dto = new CreateAppointmentBookedDto();
        //    dto.CategoryName = CategoryName;
        //    dto.AppointmentId = AppointmentId;
        //    dto.Date = date;
        //    return dto;

        //}







    }
}
