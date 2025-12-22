using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Application.DTOs.AuthDTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        public string Password { get; set; }
    }
}
