using AutoMapper;
using JustCare_MB.Data;
using JustCare_MB.Dtos;
using JustCare_MB.Dtos.AppointmentBookedDtos;
using JustCare_MB.Models;
using JustCare_MB.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace JustCare_MB.Services
{
    public class AppointmentBookedService : IAppointmentBookedService
    {
        private readonly JustCareContext _context;
        private readonly IMapper _mapper;
        public AppointmentBookedService(JustCareContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategories()
        {
            if (_context.Categories == null)
                throw new Exception("Categories table is empty");
            IEnumerable<Category> categories = await _context.Categories.ToListAsync();
            IEnumerable<CategoryDto> categoryDtos = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return categoryDtos;
        }
        private async Task<DatesDto> GetAllAvailableAppointmentByCategoryId(int Categoryid)
        {
            if (_context.Categories == null)
                throw new Exception("Categories is null");
            Category catName = await _context.Categories.FirstAsync(e => e.Id == Categoryid);
            if (catName == null)
                throw new Exception("There are no Category by this Category");
            //1-
            //IEnumerable<DateTime> Dates = await _context.Appointments
            //    .Where(e => e.CategoryId == Categoryid).Select(e => e.Date).ToListAsync();

            //2-
            // iagbefvlagfvlaihbsfvj ghva VWDSJHGVBA JLKHFB aklhbalsbhsf lkhbf    ok...
            //IEnumerable<AppointmentsDates> Dates = await _context.Appointments
            //    .Where(e => e.CategoryId == Categoryid)
            //      .Select(e => new AppointmentsDates { AppointmentId = e.Id, Date = e.Date })
            //         .ToListAsync();

            //3-
            // when the appointment is booked its wont shown on the appointments page
            // and the appointments will shown 

            // if the appointment on the appointmentBooked then i dont need to booked it again!
            var appointmentBooked_AppointmentIds = await _context.AppointmentBookeds.
                Select(e => e.AppointmentId).ToListAsync();

            IEnumerable<AppointmentsDates> Dates
            = await _context.Appointments.
            Where(e => !appointmentBooked_AppointmentIds.Contains(e.Id) && e.CategoryId == Categoryid)
            .Select(e => new AppointmentsDates { AppointmentId = e.Id, Date = e.Date }).OrderBy(e => e.Date)
            .ToListAsync();


            if (Dates == null)
                throw new Exception("There are no Appointments by this Category");

            DatesDto dto = new DatesDto();
            dto.Id = Categoryid;
            dto.CategoryName = catName.ArabicName;
            dto.DateInformation = Dates;
            return dto;
        }
        public async Task<DatesDto> GetAllDatesDtoByCategoryId(int Categoryid)
        {
            if (Categoryid == 0)
                throw new Exception("id cant be 0");
            if (_context.Appointments == null)
                throw new Exception("There are no appointments");
            DatesDto datesDto = await GetAllAvailableAppointmentByCategoryId(Categoryid);
            return datesDto;
        }

        public async Task<CreateAppointmentBookedDto> CreateAppointmentBookedDtoAsync(int AppointmentId)
        {
            // create CreateAppointmentBookedDto obj
            if (AppointmentId == 0)
                throw new Exception("id cant be 0");
            if (_context.Appointments == null)
                throw new Exception("There are no appointments");

            DateTime date = await _context.Appointments
                .Where(e => e.Id == AppointmentId).Select(e => e.Date)
                .FirstOrDefaultAsync();
            // select category name -> appointment -> categoryId -> categoryName
            int CategoryId = await _context.Appointments
                .Where(e => e.Id == AppointmentId).Select(e => e.CategoryId)
                .FirstOrDefaultAsync();
            if (CategoryId == 0)
                throw new Exception("CategoryId cant be 0");
            if (_context.Categories == null)
                throw new Exception("There are no Categories");

            string CategoryName = await _context.Categories.
                Where(e => e.Id == CategoryId).Select(e => e.ArabicName).FirstAsync();

            CreateAppointmentBookedDto dto = new CreateAppointmentBookedDto();
            dto.CategoryName = CategoryName;
            dto.AppointmentId = AppointmentId;
            dto.Date = date;
            return dto;

        }
        //public AppointmentBooked AppointmentBookedDtoToAppointmentBooked(AppointmentBookedDto appointmentBookedDto)
        //{
        //    AppointmentBooked appointmentBooked = new AppointmentBooked
        //    {
        //        PatientUserId = appointmentBookedDto.PatientUserId,
        //        AppointmentId = appointmentBookedDto.AppointmentId,
        //        Note = appointmentBookedDto.Note,
        //        Image = appointmentBookedDto.Image,
        //        Status = appointmentBookedDto.Status
        //    };
        //    return appointmentBooked;
        //}

        private AppointmentBooked convert(CreateAppointmentBookedDto dto)
        {
            AppointmentBooked appointmentBooked = new AppointmentBooked
            {
                Image = dto.Image,
                Note = dto.Note,
                AppointmentId = dto.AppointmentId,
                PatientUserId = 10
            };
            return appointmentBooked;
        }

        public async Task CreateAppointmentBooked(CreateAppointmentBookedDto appointmentBookedDto)
        {
            if (appointmentBookedDto == null)
                throw new Exception("appointmentBookedDto cant be 0");
            AppointmentBooked appointmentBooked = convert(appointmentBookedDto);

            if (appointmentBooked.Status == "Appointment booked")
                throw new Exception("appointment is aredy booked");
            //if (await _context.AppointmentBookeds.AnyAsync(e => e.PatientUserId == appointmentBookedDto.PatientUserId))
            //    throw new Exception("Patient aredy have appointment");

            appointmentBooked.Status = "Appointment booked";
            //AppointmentBooked appointmentBooked = AppointmentBookedDtoToAppointmentBooked(appointmentBookedDto);

            await _context.AppointmentBookeds.AddAsync(appointmentBooked);
            await _context.SaveChangesAsync();

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

            IEnumerable<AppointmentBookedDtos> appointmentBookedDtos =
                await _context.AppointmentBookeds
                .Where(x => x.Status!="Accepted" && x.PatientUserId==10)
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
