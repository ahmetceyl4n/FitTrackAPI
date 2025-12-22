using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FitnessApp.Application.Common.Interfaces;
using FitnessApp.Application.DTOs.AuthDTOs; // DTO'lar
using FitnessApp.Domain.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FitnessApp.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)
        {
            _config = config;
            var tokenKey = _config["JwtSettings:Key"];
            if (string.IsNullOrEmpty(tokenKey)) throw new Exception("JwtSettings:Key bulunamadı!");
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        }

        public GeneratedTokenDto CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName ?? "")
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // 1. Tarihi burada hesaplıyoruz (Tek yetkili)
            var durationInMinutes = double.TryParse(_config["JwtSettings:DurationInMinutes"], out double min) ? min : 60;
            var expiry = DateTime.UtcNow.AddMinutes(durationInMinutes);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiry,
                SigningCredentials = creds,
                Issuer = _config["JwtSettings:Issuer"],
                Audience = _config["JwtSettings:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // 2. Yeni oluşturduğumuz DTO ile dönüyoruz
            return new GeneratedTokenDto
            {
                Token = tokenHandler.WriteToken(token),
                RefreshToken = GenerateRefreshToken(), // Refresh Token üretiyoruz
                Expiration = expiry
            };
        }

        // Kriptografik olarak güvenli rastgele sayı üreteci (RNG) kullanarak token oluşturur
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        // Süresi dolmuş Access Token'dan kimlik (ClaimsPrincipal) bilgisini çıkartır
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _key,
                ValidateLifetime = false // ÖNEMLİ: Süresi dolmuş olsa bile validate et diyoruz (içini okumak için)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            
            // Algoritma kontrolü (Güvenlik için önemli)
            if (securityToken is not JwtSecurityToken jwtSecurityToken || 
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}