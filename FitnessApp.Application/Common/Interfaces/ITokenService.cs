using FitnessApp.Application.DTOs.AuthDTOs;
using FitnessApp.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Claims;
// ...
namespace FitnessApp.Application.Common.Interfaces
{
    public interface ITokenService
    {
        GeneratedTokenDto CreateToken(AppUser user);
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}

