using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace UserApi.src.DTOs
{
    public class CreateUserDto
    {
        [Required]
        public string RUT { get; set; } = string.Empty;
        [StringLength(100, MinimumLength = 3)]
        public string Name { get; set; } = string.Empty;
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [RegularExpression("(?i)masculino|femenino|otro|prefiero no decirlo")]
        public string Gender { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
    }
}