using Apilab.Application.AppServices.Interfaces;
using ApiLab.CrossCutting.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Apilab.Application.AppServices
{
    public class TokenService(IOptionsMonitor<AccessConfiguration> accessConfiguration) : ITokenService
    {
        private readonly IOptionsMonitor<AccessConfiguration> _accessConfiguration = accessConfiguration;

        public string GenerateToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_accessConfiguration.CurrentValue.ApiTokenSecurityKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(_accessConfiguration.CurrentValue.ApiTokenExpirationTimeInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}