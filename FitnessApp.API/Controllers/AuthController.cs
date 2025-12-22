using FitnessApp.Application.Common.Interfaces;
using FitnessApp.Application.DTOs.AuthDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDto>> Login([FromBody] LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);

            if (result.Succeeded)
            {
                return Ok(result.Data);
            }

            return Unauthorized(new { Message = result.ErrorMessage });
        }

        // POST: api/Auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Kayıt işlemi başarılı. Giriş yapabilirsiniz." });
            }

            // Hata listesi varsa onu dön, yoksa tek hata mesajını
            if (result.Errors != null && result.Errors.Any())
            {
                 return BadRequest(new { Messages = result.Errors });
            }

            return BadRequest(new { Message = result.ErrorMessage });
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDto>> RefreshToken([FromBody] RefreshTokenRequestDto refreshTokenRequest)
        {
            var result = await _authService.RefreshTokenAsync(refreshTokenRequest);

            if (result.Succeeded)
            {
                return Ok(result.Data);
            }

            return Unauthorized(new { Message = result.ErrorMessage });
        }

        [Authorize] // Sadece giriş yapmış kullanıcılar revoke yapabilir
        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke()
        {
            var username = User.Identity?.Name;

            if (string.IsNullOrEmpty(username)) 
                return Unauthorized();

            var result = await _authService.RevokeTokenAsync(username);

            if (result.Succeeded)
            {
                return NoContent();
            }

            return BadRequest(new { Message = result.ErrorMessage });
        }
    }
}

