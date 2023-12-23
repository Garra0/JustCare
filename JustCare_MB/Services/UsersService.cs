using AutoMapper;
using JustCare_MB.Data;
using JustCare_MB.Dtos;
using JustCare_MB.Helpers;
using JustCare_MB.Models;
using JustCare_MB.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
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
            var currentUser = _context.Users.FirstOrDefault(x => x.Email.ToLower() ==
                userLogin.Email.ToLower());
            if (currentUser == null
                || currentUser.Password != userLogin.Password)
                throw new InvalidUserPasswordOrUserNotExistException("User not exist or Invalid Password");// notfound

            return currentUser;
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
                // i can get these 3 lines , the key on the left and the value on the right
                new Claim(ClaimTypes.NameIdentifier,user.Email),
                new Claim(ClaimTypes.Role,UserRole),
                new Claim("Id",user.Id.ToString())
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
            var token = await GenerateToken(user);
            return token;
        }

        public async Task Register(UserRegisterDto userRegisterDto)
        {
            //if (userRegisterDto == null)
            //    throw new EmptyFieldException("Empty field");

            if (await _context.Users.AnyAsync(u => u.Email.ToLower()
            == userRegisterDto.Email.ToLower()))
                throw new ExistsException("Email is exists");

            User user = _mapper.Map<User>(userRegisterDto);

            if (user.Email.ToLower().Contains("den.just.edu.jo"))
                user.UserTypeId = 1;
            else
                user.UserTypeId = 2;
            // remove the spaces before and after the string:
            // "  hello word  ".Trim() will be => "hello word"
            //user.Email = user.Email.Trim(); but i have RegularExpression

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(int id, UserDto userEdited)
        {
            //if (userEdited == null)
            //    throw new EmptyFieldException("Empty Field");
            if (userEdited.Id != id)
                throw new InvalidIdException("Id is null or the Ids are diffrenete");
            if (!await _context.Users.AnyAsync(x => x.Id == id))
                throw new InvalidIdException("id is not exist");
            //var user2 = await _context.Users.FirstOrDefaultAsync(e => e.Id == id);
            //if (user2 == null)
            //    throw new Exception("User not found");

            User? user = await _context.Users.FindAsync(id);
            if (user == null)
                throw new NotFoundException("User not found");


            // update need a special map, copy the userEdited to user and save the other attribuits on user
            _mapper.Map(userEdited, user);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<UsersIndexDto> GetAllUsers(string? SearchTerm = null)
        {
            UsersIndexDto usersIndexDto = new UsersIndexDto();
            if (SearchTerm != null)
                usersIndexDto.SearchTerm = SearchTerm;

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
                
                if (!await users.AnyAsync())
                    throw new NotFoundException("Users have not this key");
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



        public async Task<int> GetUserIdByToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jsonToken != null)
            {
                // Retrieve user ID from the "sub" claim
                int userId =int.Parse(jsonToken
                    .Claims.First(claim => claim.Type == "Id").Value);

                return userId;
            }
            else
            {
                return 0;
            }
        }

        public async Task<User> GetUserById(int id)
        {
            if (!await _context.Users.AnyAsync(x=>x.Id==id))
                throw new InvalidIdException("id is not exist");

            User? user = await _context.Users.FindAsync(id);
            if (user == null)
                throw new NotFoundException("User is null");
            return user;
        }

        public async Task DeleteUser(int id)
        {   
            if (!await _context.Users.AnyAsync(x => x.Id == id))
                throw new InvalidIdException("id is not exist");

            User? user = await _context.Users.FindAsync(id);
            if (user == null)
                throw new NotFoundException("User is null");

            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}

