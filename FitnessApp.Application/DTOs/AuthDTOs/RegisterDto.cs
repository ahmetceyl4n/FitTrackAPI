using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Application.DTOs.AuthDTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email adresi zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        [MinLength(6, ErrorMessage = "Şifre en az 6 karakter olmalıdır.")]
        public string Password { get; set; }
    }
}
