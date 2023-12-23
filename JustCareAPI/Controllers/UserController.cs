using JustCare_MB.Dtos;
using JustCare_MB.Models;
using JustCare_MB.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JustCareAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("UserAPI")]
    public class UserController : Controller
    {
        private readonly IUsersService _userService;
        public UserController(IUsersService usersService)
        {
            _userService = usersService;
        }
        //[HttpGet("GetIdByToken")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //public async Task<ActionResult> GetIdByToken(string token)
        //{
        //    int id = await _userService.GetIdByToken(token);
        //    return Ok(id);
        //}

        // [AllowAnonymous] mean all users can see this function
        // (all users who have not been authenticated)
        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Login([FromBody] UserLogin userLogin)
        {
            string token = await _userService.Login(userLogin);
            return Ok(token);
        }


        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        {
            await _userService.Register(userRegisterDto);
            return Ok();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        // [ProducesResponseType(StatusCodes.Status401Unauthorized)] the use cant be Unauthorized right?
        // [ProducesResponseType(StatusCodes.Status403Forbidden)] the other user cant see the fun..
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers(string? SearchTerm = null)
        {
            UsersIndexDto usersIndex = await _userService.GetAllUsers(SearchTerm);
            return Ok(usersIndex.Users);
        }

        [HttpGet("{id:int}", Name = "GetUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            User user = await _userService.GetUserById(id);
            return Ok(user);
        }


        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUser(id);
            return Ok();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        public async Task<ActionResult> UpdateUser(int id, UserDto userEdited)
        {
                await _userService.UpdateUser(id, userEdited);
                return Ok();
        }

        ////For Dentist Only
        //[HttpGet]
        //[Route("Dentists")]
        //[Authorize(Roles = "Dentist")]
        //public IActionResult AdminEndPoint()
        //{
        //    UserRole currentUser = GetCurrentUser();
        //    return Ok($"Hi you are an {currentUser.Role}");
        //}
        //private UserRole GetCurrentUser()
        //{
        //    var identity = HttpContext.User.Identity as ClaimsIdentity;
        //    if (identity != null)
        //    {
        //        var userClaims = identity.Claims;
        //        return new UserRole
        //        {
        //            UserName = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
        //            Role = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value
        //        };
        //    }
        //    return null;
        //}

    }
}