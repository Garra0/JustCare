using JustCare_MB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustCare_MB.Dtos.AppointmentDtos
{
    public class WaitingApprovalAppointmentsBooked
    {
        public int AppointmentBookedId { get; set; }
        public string Image { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public PatientUserDtoWaitingApprovalAppointments PatientUser { get; set; }
        public CategoryDtoWaitingApprovalAppointments Category { get; set; }
    }
    public class PatientUserDtoWaitingApprovalAppointments
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? Age { get; set; } //
        public int? NationalId { get; set; } // 30818134
        public string UserType { get; set; }
        public string Gender { get; set; }
    }
    public class CategoryDtoWaitingApprovalAppointments
    {
        public string ArabicName { get; set; }
    }
    

}
