using FitnessApp.Application.DTOs.AuthDTOs;
using FitnessApp.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Application.Common.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResult<TokenResponseDto>> LoginAsync(LoginDto loginDto);
        Task<ServiceResult<TokenResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto refreshTokenRequest);
        Task<ServiceResult<bool>> RegisterAsync(RegisterDto registerDto); 
     }
}
