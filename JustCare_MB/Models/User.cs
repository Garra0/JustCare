using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustCare_MB.Models
{
    public class User
    {
        //[PrimaryKey]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Database will generate the ID
        public int Id { get; set; }

        [StringLength(50)]
        [DisplayFormat(DataFormatString = "User Name")]
        //[RegularExpression("^[\\p{L}\\p{M}'\\.\\-]+( [\\p{L}\\p{M}'\\.\\-]+)*$")]
        public string FullName { get; set; }// first and last name
        //At least 8 characters in length.
        //At least one uppercase letter.
        //At least one lowercase letter.
        //At least one digit(0-9).
        //Allows common special characters(e.g., !, @, #, $, %, etc.).
        [StringLength(100)]
        public string Password { get; set; }
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(\+9627[7-9][0-9]{7})|^([0][7][7-9][0-9]{7})$", ErrorMessage = "Invalid Jordanian phone number.")]
        public string PhoneNumber { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Age { get; set; } //
        public int? NationalId { get; set; } // 30818134


        // Relations: .. Nivation props

        // public Review Review { get; set; }

        // UserType
        public int UserTypeId { get; set; }
        public UserType UserType { get; set; }
        // Gender
        public int GenderId { get; set; }
        public Gender Gender { get; set; }
        // MedicalHistoryStatus
        public ICollection<MedicalHistoryStatus> MedicalHistoryStatuses { get; set; }

        // Appointments      
        public ICollection<Appointment> Appointments { get; set; }//Dentist
        // AppointmentBooked
        public ICollection<AppointmentBooked> AppointmentBookeds { get; set; }//Patients




    }
}
