using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustCare_MB.Models
{
    public class Category
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Database will generate the ID
        public int Id { get; set; }
        [StringLength(500)]
        public string Image { get; set; }
        [StringLength(50)]
        public string EnglishName { get; set; }
        [StringLength(50)]
        public string ArabicName { get; set; } 

        // relations
        public ICollection<Appointment> Appointments { get; set; } // = new List<Appointment>();

    }
}
