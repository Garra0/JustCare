using AutoMapper;
using JustCare_MB.Data;
using JustCare_MB.Dtos;
using JustCare_MB.Models;
using JustCare_MB.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JustCare_MB.Services
{
    public class UsersService : IUsersService
    {
        private readonly JustCareContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public UsersService(JustCareContext context
            , IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        //To authenticate user
        private User Authenticate(UserLogin userLogin)
        {
            // email and password
            var currentUser = _context.Users.FirstOrDefault(x => x.Email.ToLower() ==
                userLogin.Email.ToLower() && x.Password == userLogin.Password);
            if (currentUser != null)
            {
                return currentUser;
            }
            return null;
        }

        // To generate token
        private async Task<string> GenerateToken(User user)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials
                (securityKey, SecurityAlgorithms.HmacSha256);

            int userTypeId = await _context.Users
                .Where(x => x.Email == user.Email).Select(x => x.UserTypeId)
                .FirstOrDefaultAsync();


            string UserRole = await _context.UserTypes
                .Where(x => x.Id == userTypeId).Select(x => x.EnglishType)
                .FirstAsync();

            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,user.Email),
                new Claim(ClaimTypes.Role,UserRole)
            };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: credentials);


            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public async Task<string> Login(UserLogin userLogin)
        {
            var user = Authenticate(userLogin);// user exist?
            if (user != null)
            {
               var token =await GenerateToken(user);
                return token;
            }
             
            return "user not found";
        }




        public async Task<bool> CreateUser(UserRegisterDto userRegister)
        {
            if (_context.Users
                .Any(u => u.Email.ToLower() ==
                userRegister.Email.ToLower()))
                throw new Exception("user Exists");

            if (userRegister == null)
                throw new Exception("user null");


            User user = _mapper.Map<User>(userRegister);
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }

        //public async Task<bool> Register(UserRegisterDto userRegisterDto)
        //{
        //    if (_context.Users.Any(u => u.Email.ToLower() == userRegisterDto.Email.ToLower()))
        //        throw new Exception("user Exists");

        //    ApplicationUser user = new()
        //    {
        //        FullName = userRegisterDto.FullName,
        //        Email = userRegisterDto.Email,
        //       // PhoneNumber = userRegisterDto.PhoneNumber,
        //        Age = userRegisterDto.Age,
        //        NationalId = userRegisterDto.NationalId,
        //        UserTypeId = userRegisterDto.UserTypeId,
        //        GenderId = userRegisterDto.GenderId,
        //    };

        //    var result = await _userManager.CreateAsync(user, userRegisterDto.Password);

        //    return true;
        //}

        public async Task<bool> UpdateUser(int id, UserDto userEdited)
        {
            if (userEdited == null || userEdited.Id != id)
                throw new Exception("information is null or the id is deffrinet");

            //var user2 = await _context.Users.FirstOrDefaultAsync(e => e.Id == id);
            //if (user2 == null)
            //    throw new Exception("User not found");

            User user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            // update need a special map, copy the userEdited to user and save the other attribuits on user
            _mapper.Map(userEdited, user);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UsersIndexDto> GetAllUsers(UsersIndexDto? usersIndexDto = null)
        {
            if (usersIndexDto != null && !string.IsNullOrEmpty(usersIndexDto.SearchTerm))
            {
                //IQueryable<User> users = _context.Users.AsNoTracking();
                var users = from u in _context.Users
                            select u;

                if (!String.IsNullOrEmpty(usersIndexDto.SearchTerm))
                {
                    users = users.Where(s => s.FullName.ToLower().
                    Contains(usersIndexDto.SearchTerm.ToLower())
                    || s.Id.ToString().
                    Contains(usersIndexDto.SearchTerm)
                    || s.Email.ToLower().
                    Contains(usersIndexDto.SearchTerm.ToLower()));
                }
                usersIndexDto.Users = _mapper.Map<List<UserDto>>(users);
                return usersIndexDto;
            }
            else
            {
                if (usersIndexDto == null)
                    usersIndexDto = new UsersIndexDto();
                usersIndexDto.Users = _mapper.Map<List<UserDto>>(await _context.Users.ToListAsync());
                return usersIndexDto;
            }

        }

        public async Task<User> GetUserById(int id)
        {
            if (id == 0)
                throw new Exception("id cant be 0");
            if (_context.Users == null)
                throw new Exception("Users cant be null");

            User user = await _context.Users.FirstAsync(e => e.Id == id);
            return user;
        }

        public async Task<UserDto> UserToUserDtoMapper(int id)
        {
            User user = await GetUserById(id);
            UserDto UserUpdateDto = _mapper.Map<User, UserDto>(user);
            return UserUpdateDto;
        }


        public async Task<bool> DeleteUser(int id)
        {
            if (id == 0)
                throw new Exception("id cant be 0");
            User user = await _context.Users.FirstAsync(x => x.Id == id);
            _context.Users.Remove(user);
            _context.SaveChanges();
            return true;
        }
    }
}
