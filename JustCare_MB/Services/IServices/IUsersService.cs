using JustCare_MB.Dtos;
using JustCare_MB.Models;

namespace JustCare_MB.Services.IServices
{
    public interface IUsersService
    {
        Task<string> Login(UserLogin userLogin);
        Task Register(UserRegisterDto userRegisterDto);
        Task UpdateUser(int id, UserDto userEdited);
        Task DeleteUser(int id);
        Task<User> GetUserById(int id); 
        Task<UsersIndexDto> GetAllUsers(string? SearchTerm = null);
    }
}
