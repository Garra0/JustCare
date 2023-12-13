﻿using JustCare_MB.Dtos;
using JustCare_MB.Models;

namespace JustCare_MB.Services.IServices
{
    public interface IUsersService
    {
        Task<string> Login(UserLogin userLogin);
        Task CreateUser(UserRegisterDto userRegister);
        //Task<bool> Register(UserRegisterDto userRegisterDto);
        Task<bool> UpdateUser(int id, UserDto userEdited);
        Task DeleteUser(int id);
        Task<User> GetUserById(int id); 
        Task<UsersIndexDto> GetAllUsers(string? SearchTerm = null);
        Task<UserDto> UserToUserDtoMapper(int id);
    }
}
