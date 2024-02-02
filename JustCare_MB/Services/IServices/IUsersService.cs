using JustCare_MB.Dtos;
using JustCare_MB.Dtos.User;
using JustCare_MB.Models;

namespace JustCare_MB.Services.IServices
{
    public interface IUsersService
    {
        Task<UserLoginResponseDto> Login(UserLoginRequestDto userLogin);
        Task<int> Register(UserRegisterDto userRegisterDto);
        Task CreateTokenAndSaveUserOnDb(UserRegisterDto userRegisterDto);
        Task<int> ResetPassword(string email);
        Task ConfirmResetPassword(ConfirmResetPasswordDto confirmResetPasswordDto);
        Task UpdateUser(UserDto userEdited);
        Task DeleteUser(int id);
        Task<User> GetUserById(int id); 
        Task<UsersIndexDto> GetAllUsers(string? SearchTerm = null);
        Task<int> GetUserIdByToken(string token);
    }
}
