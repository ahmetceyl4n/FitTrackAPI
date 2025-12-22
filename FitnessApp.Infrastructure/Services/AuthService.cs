using FitnessApp.Application.Common.Interfaces;
using FitnessApp.Application.DTOs.AuthDTOs;
using FitnessApp.Application.Common.Models;
using FitnessApp.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration; // Eklendi
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;



namespace FitnessApp.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration; // Eklendi

        public AuthService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ITokenService tokenService,
            IConfiguration configuration) // Eklendi
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _configuration = configuration; // Eklendi
        }

        public async Task<ServiceResult<TokenResponseDto>> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null) return ServiceResult<TokenResponseDto>.Failure("Kullanıcı veya şifre hatalı.");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded) return ServiceResult<TokenResponseDto>.Failure("Kullanıcı veya şifre hatalı.");

            // 1. TokenService bize hem token'ı hem tarihi içeren DTO veriyor
            var generatedToken = _tokenService.CreateToken(user);

            // [NEW] Refresh Token'ı DB'ye kaydet
            user.RefreshToken = generatedToken.RefreshToken;
            
            // Access Token süresini config'den al, yoksa varsayılan 60 dk
            double accessTokenDuration = double.TryParse(_configuration["JwtSettings:DurationInMinutes"], out double val) ? val : 60;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(accessTokenDuration + 30); // Access Token + 30 dk
            
            await _userManager.UpdateAsync(user);

            // 2. Biz de bunu Client'a döneceğimiz asıl DTO'ya çeviriyoruz
            var tokenResponse = new TokenResponseDto
            {
                AccessToken = generatedToken.Token,
                RefreshToken = generatedToken.RefreshToken,
                Expiration = generatedToken.Expiration, // Direkt oradan gelen tarihi kullanıyoruz
                UserId = user.Id.ToString(),
                UserName = user.UserName
            };

            return ServiceResult<TokenResponseDto>.Success(tokenResponse);
        }

        public async Task<ServiceResult<TokenResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto tokenRequest)
        {
            try
            {
                var principal = _tokenService.GetPrincipalFromExpiredToken(tokenRequest.AccessToken);
                if (principal == null) 
                    return ServiceResult<TokenResponseDto>.Failure("Invalid access token or refresh token");

                var userId = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                if (userId == null)
                    return ServiceResult<TokenResponseDto>.Failure("Invalid token claims");

                var user = await _userManager.FindByIdAsync(userId);

                if (user == null || user.RefreshToken != tokenRequest.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                {
                    return ServiceResult<TokenResponseDto>.Failure("Invalid access token or refresh token");
                }

                // Yeni token üret ve kaydet
                var newGeneratedToken = _tokenService.CreateToken(user);
                user.RefreshToken = newGeneratedToken.RefreshToken;

                // Access Token süresini config'den al, yoksa varsayılan 60 dk
                double accessTokenDuration = double.TryParse(_configuration["JwtSettings:DurationInMinutes"], out double duration) ? duration : 60;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(accessTokenDuration + 30); // Access Token + 30 dk
                
                await _userManager.UpdateAsync(user);

                return ServiceResult<TokenResponseDto>.Success(new TokenResponseDto
                {
                    AccessToken = newGeneratedToken.Token,
                    RefreshToken = newGeneratedToken.RefreshToken,
                    Expiration = newGeneratedToken.Expiration,
                    UserId = user.Id.ToString(),
                    UserName = user.UserName
                });
            }
            catch (Exception)
            {
                return ServiceResult<TokenResponseDto>.Failure("Invalid access token or refresh token");
            }
        }

        public async Task<ServiceResult<bool>> RegisterAsync(RegisterDto registerDto)
        {
            // Burası aynı kalıyor
            var userExists = await _userManager.FindByEmailAsync(registerDto.Email);
            if (userExists != null) return ServiceResult<bool>.Failure("Bu email zaten kullanımda.");

            var newUser = new AppUser
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                FullName = registerDto.FullName
            };

            var result = await _userManager.CreateAsync(newUser, registerDto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return ServiceResult<bool>.Failure(errors);
            }

            return ServiceResult<bool>.Success(true);
        }
    }
}