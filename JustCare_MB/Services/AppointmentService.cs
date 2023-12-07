using AutoMapper;
using JustCare_MB.Data;
using JustCare_MB.Dtos.AppointmentBookedDtos;
using JustCare_MB.Dtos.AppointmentDtos;
using JustCare_MB.Models;
using JustCare_MB.Services.IServices;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace JustCare_MB.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly JustCareContext _context;
        private readonly IMapper _mapper;
        public AppointmentService(JustCareContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Appointment> AppointmentDtoToappointment(CreateAppointmentDto appointmentDto)
        {
            Appointment appointment = new Appointment
            {
                Date = appointmentDto.Date,
                DentistUserId = appointmentDto.DentistUserId.Value,
                CategoryId = appointmentDto.CategoryId
            };
            return appointment;
        }

        public async Task CreateAppointment(CreateAppointmentDto appointmentDto)
        {
            appointmentDto.DentistUserId = 10;
            if (await _context.Appointments.AnyAsync(u => u.Date == appointmentDto.Date)
                && await _context.Appointments.AnyAsync(u => u.DentistUserId == appointmentDto.DentistUserId))
                throw new Exception("appointment Exists");

            //if (await _context.Users.FindAsync(appointmentDto.DentistUserId) == null)
            //    throw new Exception("DentistUserId not Exists");

            Appointment appointment = await AppointmentDtoToappointment(appointmentDto);
            await _context.Appointments.AddAsync(appointment);
            _context.SaveChanges();

        }

        public async Task<CreateAppointmentDto> GetAppointmentDtoToShowCreatePage(int id)
        {
            // fill the objects
            CreateAppointmentDto createAppointmentDto = new CreateAppointmentDto();
            string CategoryName = await _context.Categories.
                Where(u => u.Id == id).Select(e => e.ArabicName).
                FirstAsync();
            createAppointmentDto.CategoryName = CategoryName;
            createAppointmentDto.CategoryId = id;
            return createAppointmentDto;
        }
        public async Task<Category> getCategoryObject(int id)
        {
            Category category = await _context.Categories.
                FirstAsync(c => c.Id == id);
            return category;
        }

        public async Task<bool> DeleteAppointment(int id)
        {
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
                .Select(x=>new AppointmentDto
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

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            if (_context.Categories == null)
                throw new Exception("Categories table is null");
            IEnumerable<Category> categories;
            categories = await _context.Categories.ToListAsync();
            return categories;
        }

        public async Task UpdateAppointment(int id,UpdateAppointmentDto updateAppointmentDto)
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


        public async Task<IEnumerable<WaitingApprovalAppointmentsBooked>> AllWaitingApprovalAppointments()
        {
            IEnumerable<WaitingApprovalAppointmentsBooked> waitingApprovalAppointmentsBooked =
                await _context.AppointmentBookeds
                .Where(x => x.Status != "Accepted")
                .Where(e => e.PatientUserId == 10)
                .Select(x => new WaitingApprovalAppointmentsBooked
                {
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

            return waitingApprovalAppointmentsBooked;

            // this include all columns , i can use "select" to chosee columns ...
            //IEnumerable<AppointmentBooked> appointmentBooked = await _context.AppointmentBookeds
            //    .Include(e=>e.PatientUser)
            //    .Where(e=>e.PatientUserId==10)// --> from Token...
            //    .Include(x=>x.Appointment)
            //    .ThenInclude(x=>x.Category)
            //        .Include(x => x.Appointment)
            //        .ThenInclude(x=>x.DentistUser)
            //    .Where(x => x.Status != "Accepted")
            //    .ToListAsync();

            //IEnumerable<WaitingApprovalAppointmentsBooked> acceptedAppointmentsList =
            //    _mapper.Map<IEnumerable<WaitingApprovalAppointmentsBooked>>(appointmentBooked);


            //foreach (var e in appointmentBooked)
            //{
            //var appointment = await _context.Appointments.
            //    Include(e=>e.Category).
            //    Include(e=>e.DentistUser).
            //    FirstOrDefaultAsync(u => u.Id == e.AppointmentId);
            //var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == e.PatientUserId);


            //if (appointment == null)
            //    throw new Exception("appointment is null");
            //var category = await _context.Categories.FirstOrDefaultAsync(u => u.Id == appointment.CategoryId);

            //if (category == null)
            //    throw new Exception("category is null");
            //appointment.Category = category;

            //if (user == null)
            //    throw new Exception("user is null");

            //if (e.Status == "Accepted")
            //    continue;

            //    WaitingApprovalAppointments acceptedAppointment = new WaitingApprovalAppointments
            //    {
            //        Appointment = e.Appointment,
            //        PatientUser = e.PatientUser,
            //        Note = e.Note,
            //        Image = e.Image,
            //        Id = e.Id,
            //    };

            //    acceptedAppointmentsList.Add(acceptedAppointment);
            //}
        }


        public async Task AppointmentStatus(int id, string status)
        {
            if (id == 0)
                throw new Exception("id is 0");
            AppointmentBooked appointmentBooked = await _context.AppointmentBookeds.FirstAsync(e => e.Id == id);

            if (appointmentBooked == null)
                throw new Exception("appointmentBooked is null");

            //appointmentBooked.Status =  status == "Accepted" ?  "Accepted" :

            if (status == "Accepted")
            {
                appointmentBooked.Status = "Accepted";
                _context.AppointmentBookeds.Update(appointmentBooked);
            }
            else
            {
                _context.AppointmentBookeds.Remove(appointmentBooked);
            }
            await _context.SaveChangesAsync();
        }


    }

}
