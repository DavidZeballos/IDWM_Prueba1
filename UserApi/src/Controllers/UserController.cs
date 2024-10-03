using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using UserApi.src.DTOs;
using UserApi.src.Interfaces;
using UserApi.src.Models;

namespace UserApi.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            if(!ModelState.IsValid || (userDto.BirthDate>=DateTime.Now))
            {
                return BadRequest("Alguna validación no fue cumplida.");
            }
            var user = await _userRepository.GetUserByRUT(userDto.RUT);
            if(user != null)
            {
                return Conflict("Ya existe un usuario con el mismo RUT.");
            }

            var newUser = await _userRepository.CreateUser(userDto);

            return Created($"Usuario creado exitosamente: /api/user/{newUser.RUT}", newUser);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers([FromQuery] string? sort, [FromQuery] string? gender)
        {
            if(!string.IsNullOrEmpty(sort))
            {
                var validCategories = new[] { "asc", "desc"};
                if(!validCategories.Contains(sort))
                {
                    return BadRequest("Filtro de orden inválido");
                }
            }

            if(!string.IsNullOrEmpty(gender))
            {
                var validCategories = new[] { "masculino", "femenino", "otro", "prefiero no decirlo"};
                if(!validCategories.Contains(gender))
                {
                    return BadRequest("Filtro por género inválido");
                }
            }

            var userList = await _userRepository.GetAllUsers(sort, gender);
            return Ok(userList);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] CreateUserDto updatedUserDto)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Alguna validación no fue cumplida.");
            }
            
            var result = await _userRepository.UpdateUser(id, updatedUserDto);
            if(result)
            {
                return Ok("Usuario actualizado exitosamente");
            }

            return NotFound("Usuario no encontrado.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userRepository.DeleteUser(id);
            if(result)
            {
                return Ok("Usuario eliminado.");
            }
            
            return NotFound("Usuario no encontrado.");
        }
    }
}