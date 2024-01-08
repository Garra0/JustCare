using AutoMapper;
using JustCare_MB.Data;
using JustCare_MB.Dtos;
using JustCare_MB.Dtos.AppointmentBookedDtos;
using JustCare_MB.Dtos.AppointmentDtos;
using JustCare_MB.Helpers;
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

        // 5-
        public async Task CreateAppointmentBooked(CreateAppointmentBookedDto appointmentBookedDto)
        {
            if (await _context.AppointmentBookeds.AnyAsync(x => x.AppointmentId == appointmentBookedDto.AppointmentId))
                throw new ExistsException("Appointment is exist -_-\"");


            if (appointmentBookedDto == null)
                throw new EmptyFieldException("appointmentBookedDto cant be null");
            AppointmentBooked appointmentBooked = _mapper.Map<AppointmentBooked>(appointmentBookedDto);
            appointmentBooked.PatientUserId = 10;

            //if (await _context.AppointmentBookeds.AnyAsync(e => e.PatientUserId == appointmentBookedDto.PatientUserId))
            //    throw new Exception("Patient aredy have appointment");

            appointmentBooked.Status = "Appointment booked";

            await _context.AppointmentBookeds.AddAsync(appointmentBooked);
            await _context.SaveChangesAsync();
        }

        // 6-
        public async Task<IEnumerable<WaitingApprovalAppointmentsBooked>> GetAllWaitingApprovalAppointments()
        {
            IEnumerable<WaitingApprovalAppointmentsBooked> waitingApprovalAppointmentsBooked =
                await _context.AppointmentBookeds
                .Where(x => x.Status != "Accepted")
                // برجع المواعيد التابعة للدكتور مو مواعيد كل الدكاترة..
                .Where(x => x.Appointment.DentistUserId == 10
                && x.Appointment.Id == x.Appointment.AppointmentBooked.AppointmentId)
                //.Where(x=> appointmentIdsByDentistUserId.Contains(x.AppointmentId))
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
        }

        // 7.Accepted-
        public async Task AppointmentBookedAccepted(int appointmentBookedId)
        {
            if (!await _context.AppointmentBookeds.AnyAsync(x => x.Id == appointmentBookedId))
                throw new InvalidIdException("Id is not exist");

            AppointmentBooked appointmentBooked = await _context
                .AppointmentBookeds.FirstAsync(x => x.Id == appointmentBookedId);
           if(appointmentBooked.Status == "Accepted")
                throw new ExistsException("This appoitnmentBooked have been accepted");
            appointmentBooked.Status = "Accepted";


            _context.AppointmentBookeds.Update(appointmentBooked);
            await _context.SaveChangesAsync();
        }
        
        // 7.Rejected-
        public async Task AppointmentBookedRejected(int AppointmentBookedId)
        {
            if (!await _context.AppointmentBookeds.AnyAsync(x => x.Id == AppointmentBookedId))
                throw new InvalidIdException("Id is not exist");

            AppointmentBooked appointmentBooked = await _context
              .AppointmentBookeds.FirstAsync(x => x.Id == AppointmentBookedId);
            _context.AppointmentBookeds.Remove(appointmentBooked);
            await _context.SaveChangesAsync();
        }


        //public async Task AppointmentStatus(int id, string status)
        //{
        //    AppointmentBooked appointmentBooked = await _context.AppointmentBookeds.FirstAsync(e => e.Id == id);

        //    if (status == "Accepted")
        //    {
        //        appointmentBooked.Status = "Accepted";
        //        _context.AppointmentBookeds.Update(appointmentBooked);
        //    }
        //    else
        //    {
        //        _context.AppointmentBookeds.Remove(appointmentBooked);
        //    }
        //    await _context.SaveChangesAsync();
        //}


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
