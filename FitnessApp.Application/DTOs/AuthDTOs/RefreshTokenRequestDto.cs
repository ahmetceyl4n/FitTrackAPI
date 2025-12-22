using System.ComponentModel.DataAnnotations;

namespace FitnessApp.Application.DTOs.AuthDTOs
{
    public class RefreshTokenRequestDto
    {
        [Required]
        public string AccessToken { get; set; }

        [Required]
        public string RefreshToken { get; set; }
    }
}
