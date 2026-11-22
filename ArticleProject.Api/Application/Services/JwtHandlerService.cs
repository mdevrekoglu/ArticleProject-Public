using ArticleProject.Domain.Aggregates;
using ArticleProject.Domain.Common.Interfaces.Service;
using ArticleProject.Model.Models.CommonModels.DataTransferModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ArticleProject.Api.Application.Services
{
    public class JwtHandlerService : IBaseSingletonService
    {
        private readonly IConfigurationSection _jwtSettings;

        public JwtHandlerService(IConfiguration configuration)
        {
            _jwtSettings = configuration.GetSection("JWTSettings");
        }

        public TokenTransferModel CreateToken(User user, IList<string> roles, bool createRefreshToken)
        {
            var signingCredentials = GetSigningCredentials();
            var claims = GetClaims(user, roles);
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            return new TokenTransferModel
            {
                Token = new JwtSecurityTokenHandler().WriteToken(tokenOptions),
                RefreshToken = createRefreshToken ? CreateRefreshToken() : string.Empty
            };
        }

        private string CreateRefreshToken()
        {
            int len = 128;

            using (var rng = new RNGCryptoServiceProvider())
            {
                var byteArray = new byte[len];
                rng.GetBytes(byteArray);
                return Convert.ToBase64String(byteArray).Substring(0, len);
            }
        }

        public bool ValidateRefreshToken(string refreshToken, string? refreshTokenDB, DateTime? refreshTokenDBExpiryDate)
        {
            if (!string.IsNullOrEmpty(refreshToken) && !string.IsNullOrEmpty(refreshTokenDB) && refreshTokenDBExpiryDate.HasValue)
            {
                return string.Equals(refreshToken, refreshTokenDB) && DateTime.UtcNow < refreshTokenDBExpiryDate;
            }
            return false;
        }

        public ClaimsPrincipal GetPrincipalsFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings["securityKey"]!);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = false, // Allow expired tokens
                ValidIssuer = _jwtSettings["validIssuer"],
                ValidAudience = _jwtSettings["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

                if (securityToken is JwtSecurityToken jwtToken &&
                    jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return principal;
                }
                else
                {
                    throw new SecurityTokenException("Invalid token");
                }
            }
            catch (Exception)
            {
                throw new SecurityTokenException("Invalid token");
            }
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettings["securityKey"]!);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private List<Claim> GetClaims(User user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FirstName!),
                new Claim(ClaimTypes.Surname, user.LastName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.NameIdentifier, user.Id!)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
                issuer: _jwtSettings["validIssuer"],
                audience: _jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings["expiryInMinutes"])),
                signingCredentials: signingCredentials
            );

            return tokenOptions;
        }
    }
}