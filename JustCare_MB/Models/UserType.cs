using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustCare_MB.Models
{
    public class UserType
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Database will generate the ID
        public int Id { get; set; }
        [StringLength(50)]
        public string EnglishType { get; set; } // admin , paitent , dental
        [StringLength(50)] 
        public string ArabicType { get; set; }


        // Relations
        public ICollection<User> Users { get; set; }

    }
}
