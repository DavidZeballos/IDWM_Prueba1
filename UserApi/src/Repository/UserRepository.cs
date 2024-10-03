using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Bogus.DataSets;
using Microsoft.EntityFrameworkCore;
using UserApi.src.Data;
using UserApi.src.DTOs;
using UserApi.src.Interfaces;
using UserApi.src.Mappers;
using UserApi.src.Models;

namespace UserApi.src.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> CreateUser(CreateUserDto userDto)
        {
            var user = userDto.ToUser();
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<List<User>> GetAllUsers(string? sort, string? gender)
        {
            var users = _context.Users.AsQueryable();
            if(!string.IsNullOrEmpty(gender))
            {
                users = users.Where(p => p.Gender.ToLower() == gender.ToLower());
            }
            if (!string.IsNullOrEmpty(sort))
            {
                if(sort.ToLower() == "asc")
                {
                    return await users.OrderBy(p => p.Name).ToListAsync();
                }
                else if(sort.ToLower() == "desc")
                {
                    return await users.OrderByDescending(p => p.Name).ToListAsync();
                }
            }
            return await users.ToListAsync();
        }

        public async Task<bool> UpdateUser(int id, CreateUserDto updatedUserDto)
        {
            var updatedUser = updatedUserDto.ToUser();
            var existingUser = await _context.Users.FindAsync(id);
            if(existingUser != null)
            {
                existingUser.RUT = updatedUser.RUT;
                existingUser.Name = updatedUser.Name;
                existingUser.Email = updatedUser.Email;
                existingUser.Gender = updatedUser.Gender;
                existingUser.BirthDate = updatedUser.BirthDate;
                await _context.SaveChangesAsync();
                return true;
            }
            
            return false;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if(user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<User?> GetUserByRUT(string rut)
        {
            var product = await _context.Users.FirstOrDefaultAsync(x => x.RUT == rut);
            return product;
        }
    }
}