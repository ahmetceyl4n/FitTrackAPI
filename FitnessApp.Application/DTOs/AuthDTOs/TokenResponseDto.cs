using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Application.DTOs.AuthDTOs
{
    public class TokenResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
