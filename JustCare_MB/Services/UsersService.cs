using AutoMapper;
using JustCare_MB.Data;
using JustCare_MB.Dtos;
using JustCare_MB.Dtos.User;
using JustCare_MB.Helpers;
using JustCare_MB.Models;
using JustCare_MB.Services.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace JustCare_MB.Services
{
    public class UsersService : IUsersService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JustCareContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ILogger<User> _logger;
        public UsersService(JustCareContext context
            , IMapper mapper, IConfiguration configuration, ILogger<User> logger
            , IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        private async Task UpdateLoginCount(string email)
        {
            User user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
            if(user.LoginCount>2)
            {
                user.LoginCount = 0;
                user.LoginBlock = DateTime.Now.AddMinutes(1);
            }
            else
            {
                user.LoginCount++;
            }
            // update need a special map, copy the userEdited to user and save the other attribuits on user
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        //To authenticate user
        private async Task<User> Authenticate(UserLoginRequestDto userLogin)
        {
            var currentUser = _context.Users.FirstOrDefault(x => x.Email.ToLower() ==
                userLogin.Email.ToLower());
            if (currentUser == null)
                throw new InvalidUserPasswordOrUserNotExistException("User not exist or Invalid Password");// notfound
           
            User user = await _context.Users.FirstOrDefaultAsync(x => x.Email == userLogin.Email);

            if (user.LoginBlock > DateTime.Now)
                throw new ImagesBadRequest("You cant login now, you have blocked for 1 minute");

            byte[] hash;
            using (MD5 md5 = MD5.Create())
            {
                hash = md5.ComputeHash(Encoding.UTF8.GetBytes(userLogin.Password));
            }
            string hashedPassword = BitConverter.ToString(hash).Replace("-", "").ToLower();
            if (currentUser.Password != hashedPassword)
            {
                await UpdateLoginCount(userLogin.Email);
                if (user.LoginBlock > DateTime.Now)
                    throw new ImagesBadRequest("You cant login now, you have blocked for 1 minute");
                throw new InvalidUserPasswordOrUserNotExistException("User not exist or Invalid Password");// notfound
            }

            return currentUser;
        }

        // To generate token
        private async Task GenerateTokenAndCreateUserLoginDto(User user
            , UserLoginResponseDto userLoginResponseDto)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials
                (securityKey, SecurityAlgorithms.HmacSha256);

            //string UserRole =
            //    await _context.Users.Include(x=>x.UserType)
            //    .Where(x=>x.Email==user.Email)
            //    .Select(x => x.UserType.EnglishType)
            //    .FirstOrDefaultAsync();

            // equal to the above...
            string UserRole =
                await _context.Users
                .Where(x => x.Email == user.Email)
                .Select(x => x.UserType.EnglishType)
                .FirstOrDefaultAsync();

            
            // equal to the above...
            //string UserRole =
            //    (await _context.Users
            //    .FirstOrDefaultAsync(x => x.Email == user.Email)).UserType.EnglishType;

            //int userTypeId = 
            //await _context.Users
            //.Where(x => x.Email == user.Email).Select(x => x.UserTypeId)
            //.FirstOrDefaultAsync();


            //string UserRole = await _context.UserTypes
            //    .Where(x => x.Id == userTypeId)
            //    .Select(x => x.EnglishType)
            //    .FirstOrDefaultAsync();

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

            var StringToken = new JwtSecurityTokenHandler().WriteToken(token);
            userLoginResponseDto.UserRole = UserRole;
            userLoginResponseDto.Token = StringToken; // token saved here
            userLoginResponseDto.UserName = user.FullName;
        }

        public async Task<UserLoginResponseDto> Login(UserLoginRequestDto userLogin)
        {
            UserLoginResponseDto userLoginResponseDto = new UserLoginResponseDto();

            User user = await Authenticate(userLogin);// user exist?
            await GenerateTokenAndCreateUserLoginDto(user, userLoginResponseDto);

            return userLoginResponseDto;
        }
        Random random = new Random();
        int randomNumber;
        private void Authorization(string Email)
        {
            // send code using email
            randomNumber = random.Next(1000, 10000);


            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("OnlineJustCare@gmail.com");
            mailMessage.To.Add(new MailAddress(Email));
            mailMessage.Subject = "Email verification code - التحقق من الايميل";
            mailMessage.Body =  "اهلا بك بتطبيق JustCare \n" + "يرجى ادخال رمز التحقق بالتطبيق: " + randomNumber + "\n" +
                "Welcome to JustCare application \n Please enter the verification code in the application: " + randomNumber;
            SmtpClient Smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("OnlineJustCare@gmail.com", "zsukfqxxytedelba"),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            
            Smtp.Send(mailMessage);

        }
        public async Task<int> Register(UserRegisterDto userRegisterDto)
        {
            if (await _context.Users.AnyAsync(u => u.Email.ToLower()
            == userRegisterDto.Email.ToLower()))
                throw new ExistsException("Email is exists");

            Authorization(userRegisterDto.Email);
            return randomNumber;
        }

        public async Task CreateTokenAndSaveUserOnDb(UserRegisterDto userRegisterDto)
        {
            User user = _mapper.Map<User>(userRegisterDto);

            if (user.Email.ToLower().Contains("den.just.edu.jo"))
                user.UserTypeId = 1;
            else
                user.UserTypeId = 2;
            // remove the spaces before and after the string:
            // "  hello word  ".Trim() will be => "hello word"
            //user.Email = user.Email.Trim(); but i have RegularExpression

            byte[] hash;
            using (MD5 md5 = MD5.Create())
            {
                hash = md5.ComputeHash(Encoding.UTF8.GetBytes(userRegisterDto.Password));
            }
            string hashedPassword = BitConverter.ToString(hash).Replace("-", "").ToLower();
            user.Password = hashedPassword;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        int randomResetPasswordNumber;
        private  void AuthorizationForResetPassword(string Email)
        {
            // send code using email
            randomResetPasswordNumber = random.Next(1000, 10000);


            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("OnlineJustCare@gmail.com");
            mailMessage.To.Add(new MailAddress(Email));
            mailMessage.Subject = "Reset password - تغيير كلمة المرور ";
            mailMessage.Body = "اهلا بك بتطبيق JustCare \n" + "يرجى ادخال رمز التحقق بالتطبيق لتغيير كلمة المرور: " + randomResetPasswordNumber + "\n" +
                "Welcome to JustCare application \n Please enter the verification code in the application to reset password: " + randomResetPasswordNumber;
            SmtpClient Smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("OnlineJustCare@gmail.com", "zsukfqxxytedelba"),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            Smtp.Send(mailMessage);

        }
        public async Task<int> ResetPassword(string email)
        {
            if (! await _context.Users.AnyAsync(u => u.Email.ToLower()
                        == email.ToLower()))
                throw new NotFoundException("Email is not exists");

            AuthorizationForResetPassword(email);
            return randomResetPasswordNumber;
        }
        public async Task ConfirmResetPassword(ConfirmResetPasswordDto confirmResetPasswordDto)
        {
            _logger.LogInformation(
     $"Delete user: {confirmResetPasswordDto.Email}");

            User user = await _context.Users.FirstOrDefaultAsync(x => x.Email == confirmResetPasswordDto.Email);
            if (user == null)
                throw new NotFoundException("User not found");

            byte[] hash;
            using (MD5 md5 = MD5.Create())
            {
                hash = md5.ComputeHash(Encoding.UTF8.GetBytes(confirmResetPasswordDto.Password));
            }
            string hashedPassword = BitConverter.ToString(hash).Replace("-", "").ToLower();

            user.Password = hashedPassword;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();


        }

        public async Task DeleteUser(int id)
        {
            _logger.LogInformation(
      $"Delete user: {JsonConvert.SerializeObject(id)}");

            if (!await _context.Users.AnyAsync(x => x.Id == id))
                throw new InvalidIdException("Id is not found on user DB");

            User user = await _context
              .Users.FirstAsync(x => x.Id == id);
            if (user == null)
                throw new NotFoundException("User is not exist on DB");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(UserDto userEdited)
        {
            _logger.LogInformation(
      $"Delete user: {JsonConvert.SerializeObject(userEdited)}");

            var idClaim = _httpContextAccessor.HttpContext?.User.FindFirst("Id");
            if (idClaim == null
                || !int.TryParse(idClaim.Value, out int id))
                throw new NotFoundException("User token invalid");

            if (!await _context.Users.AnyAsync(x => x.Id == id))
                throw new InvalidIdException("Id is not found on user DB");

            User user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
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
                int userId = int.Parse(jsonToken
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
            if (!await _context.Users.AnyAsync(x => x.Id == id))
                throw new InvalidIdException("id is not exist");

            User? user = await _context.Users.FindAsync(id);
            if (user == null)
                throw new NotFoundException("User is null");
            return user;
        }
    }
}

