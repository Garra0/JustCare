﻿using JustCare_MB.Dtos.AppointmentDtos;
using JustCare_MB.Dtos.Category;

namespace JustCare_MB.Dtos.AppointmentBookedDtos
{
    public class ActiveAppointmentBookedForDentistDto
    {
        public AppointmentDtoActiveAppointmentBookedDto appointmentDto { get; set; }
        public patientInformationActiveAppointmentBooked userDto { get; set; }
    }
    public class AppointmentDtoActiveAppointmentBookedDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public CategoryName categoryName { get; set; }
    }
    
    public class patientInformationActiveAppointmentBooked
    {
        public int patientId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
    }

}