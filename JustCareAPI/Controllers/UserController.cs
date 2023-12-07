using JustCare_MB.Dtos;
using JustCare_MB.Models;
using JustCare_MB.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JustCareAPI.Controllers
{
    [ApiController]
    [Route("UserAPI")]
    public class UserController : Controller
    {
        private readonly IUsersService _userService;
        public UserController(IUsersService usersService)
        {
            _userService = usersService;
        }

        // [AllowAnonymous] mean all users can see this function
        // (all users who have not been authenticated)
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] UserLogin userLogin)
        {
            string token = await _userService.Login(userLogin);
            if (token != null)
            {
                return Ok(token);
            }
            return NotFound("user not found");
        }



        //[HttpPost("Register")]
        //public async Task<IActionResult> Register([FromBody] UserRegisterDto userRegisterDto)
        //{
        //    bool isSuccess = await _userService.Register(userRegisterDto);
        //    if (isSuccess == false)
        //        return BadRequest();
        //    return Ok();
        //}

        [HttpPost]
        public async Task<ActionResult> CreateUser(UserRegisterDto userRegisterDto)
        {
            bool isSuccess = await _userService.CreateUser(userRegisterDto);
            if (isSuccess)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IEnumerable<UserDto>> GetAllUser(string? SearchTerm = null)
        {

            UsersIndexDto usersIndexDto = new UsersIndexDto();
            if (SearchTerm != null)
                usersIndexDto.SearchTerm = SearchTerm;

            UsersIndexDto usersIndex = await _userService.GetAllUsers(usersIndexDto);
            return usersIndex.Users;

        }

        [HttpGet("{id:int}", Name = "GetUser")]
        public async Task<User> GetUser(int id)
        {
            User user = await _userService.GetUserById(id);
            return user;

        }


        

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            bool isSuccess = await _userService.DeleteUser(id);
            if (isSuccess)
            {
                return Ok(id);
            }
            return BadRequest(id);
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        public async Task<ActionResult> UpdateUser(int id, UserDto userEdited)
        {
            bool isSuccess = await _userService.UpdateUser(id, userEdited);
            if (isSuccess)
            {
                return Ok(userEdited);
            }
            return BadRequest(userEdited);

        }

        //For Dentist Only
        [HttpGet]
        [Route("Dentists")]
        [Authorize(Roles = "Dentist")]
        public IActionResult AdminEndPoint()
        {
            UserRole currentUser = GetCurrentUser();
            return Ok($"Hi you are an {currentUser.Role}");
        }
        private UserRole GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new UserRole
                {
                    UserName = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                    Role = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value
                };
            }
            return null;
        }

    }
}