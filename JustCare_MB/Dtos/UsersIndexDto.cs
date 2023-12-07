using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustCare_MB.Dtos
{
    public class UsersIndexDto
    {
        public string SearchTerm { get; set; }
        public List<UserDto> Users { get; set; }
    }
}
