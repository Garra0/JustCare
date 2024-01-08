using AutoMapper;
using JustCare_MB.Dtos;
using JustCare_MB.Dtos.AppointmentBookedDtos;
using JustCare_MB.Dtos.AppointmentDtos;
using JustCare_MB.Dtos.Category;
using JustCare_MB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustCare_MB
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<UserRegisterDto, User>().ReverseMap();
            CreateMap<AppointmentBooked, AppointmentBookedDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CreateAppointmentBookedDto, AppointmentBooked>().ReverseMap();
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<WaitingApprovalAppointmentsBooked, AppointmentBooked>().ReverseMap();
            CreateMap<UpdateAppointmentDto, Appointment>().ReverseMap();
            CreateMap<CreateAppointmentDto, Appointment>().ReverseMap();
            CreateMap<CreateCategoryDto, Category>().ReverseMap();
        }

    }
}
