using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserApi.src.DTOs;
using UserApi.src.Models;

namespace UserApi.src.Mappers
{
    public static class UserMapper
    {
        public static User ToUser(this CreateUserDto userDto)
        {
            return new User
            {
                RUT = userDto.RUT,
                Name = userDto.Name,
                Email = userDto.Email,
                Gender = userDto.Gender,
                BirthDate = userDto.BirthDate
            };
        }
    }
}