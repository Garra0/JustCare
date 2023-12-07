using JustCare_MB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustCare_MB.Dtos.AppointmentDtos
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int DentistUserId { get; set; }
        public string CategoryArabicName { get; set; }
    }
}
