using JustCare_MB.Dtos;
using JustCare_MB.Models;

namespace JustCare_MB.Services.IServices
{
    public interface IUsersService
    {
        Task<string> Login(UserLogin userLogin);
        Task<bool> CreateUser(UserRegisterDto userRegister);
        //Task<bool> Register(UserRegisterDto userRegisterDto);
        Task<bool> UpdateUser(int id, UserDto userEdited);
        Task<bool> DeleteUser(int id);
        Task<User> GetUserById(int id);
        Task<UsersIndexDto> GetAllUsers(UsersIndexDto? usersIndexDto = null);
        Task<UserDto> UserToUserDtoMapper(int id);
    }
}
