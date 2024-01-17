using JustCare_MB.Dtos;
using JustCare_MB.Dtos.User;
using JustCare_MB.Models;

namespace JustCare_MB.Services.IServices
{
    public interface IUsersService
    {
        Task<UserLoginResponseDto> Login(UserLoginRequestDto userLogin);
        Task Register(UserRegisterDto userRegisterDto);
        Task UpdateUser(UserDto userEdited);
        Task DeleteUser(int id);
        Task<User> GetUserById(int id); 
        Task<UsersIndexDto> GetAllUsers(string? SearchTerm = null);
        Task<int> GetUserIdByToken(string token);
    }
}
