using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserApi.src.DTOs;
using UserApi.src.Models;

namespace UserApi.src.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateUser(CreateUserDto userDto);
        Task<List<User>> GetAllUsers(string? sort, string? gender);
        Task<User> UpdateUser(int id, CreateUserDto updatedUserDto);
        Task<bool> DeleteUser(int id);
        Task<User?> GetUserByRUT(string rut);
    }
}