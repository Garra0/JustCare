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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace JustCare_MB.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly JustCareContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AppointmentService> _logger;
        private readonly IHostEnvironment _hostEnvironment;

        public AppointmentService(JustCareContext context
            , IMapper mapper, IHttpContextAccessor httpContextAccessor
            , ILogger<AppointmentService> logger, IHostEnvironment hostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        //2-
        public async Task CreateAppointment(CreateAppointmentDto appointmentDto)
        {
            // image size / other way to get the id / no more 3 appointments
            _logger.LogInformation(
                 $"Create Appointment: {JsonConvert.SerializeObject(appointmentDto)}");

            if (appointmentDto.Images.Count > 5)
            {
                throw new ImagesBadRequest("You can upload a maximum of 5 images.");
            }
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("Id");
            if (userIdClaim == null
                || !int.TryParse(userIdClaim.Value, out int dentistUserId))
                throw new NotFoundException("The token invalid");

            // appointment not Exists?
            if (await _context.Appointments.AnyAsync(u => u.Date == appointmentDto.Date)
                && await _context.Appointments.AnyAsync(u => u.DentistUserId == dentistUserId))
                throw new ExistsException("Appointment Exists");

            // Category is Exists?
            if (!await _context.Categories.AnyAsync(x => x.Id == appointmentDto.CategoryId))
                throw new InvalidIdException("Category id is not exist");


            // the appointment should be after 12h at least
            if (appointmentDto.Date < DateTime.Now.AddHours(12))
                throw new TimeNotValid("The appointment must be at least 12 hours away");

            foreach (var image in appointmentDto.Images)
            {
                if (image.Length > (5 * 1024 * 1024)) // Assuming 5 MB as the maximum size
                {
                    throw new ImagesBadRequest("Each image can have a maximum size of 5 MB.");
                }
            }



            Appointment appointment = _mapper.Map<Appointment>(appointmentDto);
            appointment.DentistUserId = dentistUserId;

            appointment.DentistAppointmentImages =
                new List<DentistAppointmentImage>();



            //string fileName = "\\8845806b-91ea-413b-8a93-b4fc9459c678.jpg";
            //string filePath = Path.Combine(basePath, subfolder, fileName);

            // Check if the directory exists, if not, create it
            //string directoryPath = Path.Combine(basePath, subfolder);
            // create the folders Created
            if (!Directory.Exists("C:\\Images\\DentistAppointmentImages"))
            {
                Directory.CreateDirectory("C:\\Images\\DentistAppointmentImages");
            }

            //// Now you can save the file to the specified path
            //using (var stream = new FileStream(filePath, FileMode.Create))
            //{
            //    file.CopyTo(stream);
            //}
            
            foreach (var image in appointmentDto.Images)
            {
                string imagePath = "C:\\Images\\DentistAppointmentImages\\";
                string imageName = Guid.NewGuid().ToString() + ".jpg";
                //string imagePath = _hostEnvironment.ContentRootPath
                //    + "\\Images\\DentistAppointmentImages\\" + imageName;
                //imagePath = imagePath.Replace("JustCareAPI", "JustCare_MB"); // Replace from "JustCareAPI" to "JustCare_MB"  
                // add the image name to the path
                imagePath += imageName;
                DentistAppointmentImage dentistAppointmentImage
                 = new DentistAppointmentImage
                 {
                     ImageName = imageName,
                 };
                appointment.DentistAppointmentImages.Add(dentistAppointmentImage);

                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    //await stream.WriteAsync(Encoding.UTF8.GetBytes(image));
                    await stream.WriteAsync(image);
                }

            }
            await _context.Appointments.AddAsync(appointment);
            _context.SaveChanges();

        }

        //4.1-
        public async Task<DatesDto> GetAllAppoitnmentDatesDtoByCategoryId(int CategoryId)
        {
            _logger.LogInformation(
                $"Get All Appoitnment Dates By Category Id(4.1): {JsonConvert.SerializeObject(CategoryId)}");

            if (!await _context.Categories.AnyAsync(x => x.Id == CategoryId))
                throw new InvalidIdException("id is not exist");

            if (!await _context.Appointments.AnyAsync())
                throw new NotFoundException("There are no appointments");

            DatesDto datesDto = await GetAllAvailableAppointmentByCategoryId(CategoryId);
            return datesDto;
        }

        //4.2
        private async Task<DatesDto> GetAllAvailableAppointmentByCategoryId(int CategoryId)
        {
            _logger.LogInformation(
               $"Get All Appoitnment Dates By Category Id(4.2): {JsonConvert.SerializeObject(CategoryId)}");

            // all appoitnments - appoitnmentsbooked - other categories
            IEnumerable<AppointmentDates> AppointmentDatesByCategoryId
            = await _context.Appointments.
            // dont select the appoitnments witch are on AppointmentBooked table
            Where(e => e.Id != e.AppointmentBooked.AppointmentId
            && e.CategoryId == CategoryId)
            .Select(e => new AppointmentDates
            {
                AppointmentId = e.Id,
                Date = e.Date,
                DentistAppointmentImages = e.DentistAppointmentImages,
                DentistDescription = e.DentistDescription,
            }
            )
            .OrderBy(e => e.Date)
            .ToListAsync();

            if (AppointmentDatesByCategoryId == null)
                throw new NotFoundException("There are no Appointments by this Category");

            string CategoryName = await _context.Categories
                .Where(e => e.Id == CategoryId)
                .Select(x => x.ArabicName).FirstAsync();

            if (Directory.Exists("C:\\Images\\DentistAppointmentImages"))
            {
                foreach (var AppoinmtnetDate in AppointmentDatesByCategoryId)
                {
                    AppoinmtnetDate.images = new List<byte[]>();
                    foreach (var dentistAppointment in AppoinmtnetDate.DentistAppointmentImages)
                    {
                        string ImagePath = "C:\\Images\\DentistAppointmentImages\\" 
                            + dentistAppointment.ImageName;
                        AppoinmtnetDate.images.Add(await File.ReadAllBytesAsync(ImagePath));
                    }
                }
            }

            DatesDto dto = new DatesDto();
            dto.CategoryId = CategoryId;
            dto.CategoryName = CategoryName;
            dto.appointmentDates = AppointmentDatesByCategoryId;
            return dto;
        }

        public async Task<CreateAppointmentDto> GetAppointmentDtoToShowCreatePage(int CategorId)
        {
            CreateAppointmentDto createAppointmentDto = new CreateAppointmentDto();
            string CategoryName = await _context.Categories.
                Where(u => u.Id == CategorId).Select(e => e.ArabicName).
                FirstAsync();
            //createAppointmentDto.CategoryName = CategoryName;
            createAppointmentDto.CategoryId = CategorId;
            return createAppointmentDto;
        }

        public async Task DeleteAppointment(int appointmentId)
        {
            _logger.LogInformation(
      $"Delete Appointment: {JsonConvert.SerializeObject(appointmentId)}");

            if (!await _context.Appointments.AnyAsync(x => x.Id == appointmentId))
                throw new InvalidIdException("Id is not exist on Appointment");

            Appointment appointment = await _context
              .Appointments.FirstAsync(x => x.Id == appointmentId);
            if (appointment == null)
                throw new NotFoundException("appointment is not exist on DB");

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();
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
