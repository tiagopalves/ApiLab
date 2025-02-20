using Apilab.Application.AppServices;
using ApiLab.CrossCutting.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ApiLab.UnitTests.Application.AppServices
{
    public class TokenServiceTests
    {
        private readonly IConfiguration _configuration;
        private readonly Mock<IOptionsMonitor<AccessConfiguration>> _mockAccessConfiguration;
        private readonly TokenService _tokenService;
        private readonly AccessConfiguration _defaultConfig;

        public TokenServiceTests()
        {
            // Carrega o appsettings.json do projeto de teste
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Tests.json", optional: false)
                .Build();

            _defaultConfig = new AccessConfiguration
            {
                ApiTokenSecurityKey = _configuration["AccessConfiguration:ApiTokenSecurityKey"] ?? string.Empty,
                ApiTokenExpirationTimeInMinutes = int.Parse(_configuration["AccessConfiguration:ApiTokenExpirationTimeInMinutes"] ?? "0")
            };

            _mockAccessConfiguration = new Mock<IOptionsMonitor<AccessConfiguration>>();
            _mockAccessConfiguration.Setup(x => x.CurrentValue).Returns(_defaultConfig);

            _tokenService = new TokenService(_mockAccessConfiguration.Object);
        }

        [Fact]
        public void GenerateToken_WithAppSettingsConfiguration_ShouldReturnValidToken()
        {
            // Act
            var token = _tokenService.GenerateToken();

            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token);

            var tokenHandler = new JwtSecurityTokenHandler();
            Assert.True(tokenHandler.CanReadToken(token));
        }

        [Fact]
        public void GenerateToken_WithAppSettingsConfiguration_ShouldHaveValidSignature()
        {
            // Arrange
            var token = _tokenService.GenerateToken();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_defaultConfig.ApiTokenSecurityKey);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            // Act & Assert
            var exception = Record.Exception(() =>
            {
                tokenHandler.ValidateToken(token, validationParameters, out _);
            });

            Assert.Null(exception);
        }

        [Fact]
        public void GenerateToken_ShouldReturnValidJwtToken()
        {
            // Act
            var token = _tokenService.GenerateToken();

            // Assert
            Assert.NotNull(token);
            Assert.NotEmpty(token);

            var tokenHandler = new JwtSecurityTokenHandler();
            Assert.True(tokenHandler.CanReadToken(token));
        }

        [Fact]
        public void GenerateToken_ShouldCreateTokenWithCorrectExpirationTime()
        {
            // Arrange
            var expectedExpirationMinutes = _defaultConfig.ApiTokenExpirationTimeInMinutes;
            var toleranceMinutes = 1;

            // Act
            var token = _tokenService.GenerateToken();
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

            // Assert
            var expectedExpiration = DateTime.UtcNow.AddMinutes(expectedExpirationMinutes);
            var actualExpiration = jwtToken.ValidTo;

            Assert.True(Math.Abs((expectedExpiration - actualExpiration).TotalMinutes) <= toleranceMinutes);
        }

        [Fact]
        public void GenerateToken_ShouldUseCorrectSigningAlgorithm()
        {
            // Act
            var token = _tokenService.GenerateToken();
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

            // Assert
            Assert.Equal("HS256", jwtToken.SignatureAlgorithm);
        }

        [Fact]
        public void GenerateToken_ShouldCreateValidSignature()
        {
            // Arrange
            var token = _tokenService.GenerateToken();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_defaultConfig.ApiTokenSecurityKey);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            // Act & Assert
            var exception = Record.Exception(() =>
            {
                tokenHandler.ValidateToken(token, validationParameters, out _);
            });

            Assert.Null(exception);
        }

        [Fact]
        public void GenerateToken_WithDifferentExpirationTime_ShouldRespectConfiguration()
        {
            // Arrange
            var customExpirationMinutes = 60;
            var customConfig = new AccessConfiguration
            {
                ApiTokenSecurityKey = _defaultConfig.ApiTokenSecurityKey,
                ApiTokenExpirationTimeInMinutes = customExpirationMinutes
            };

            _mockAccessConfiguration.Setup(x => x.CurrentValue).Returns(customConfig);
            var tokenService = new TokenService(_mockAccessConfiguration.Object);

            // Act
            var token = tokenService.GenerateToken();
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

            // Assert
            var expectedExpiration = DateTime.UtcNow.AddMinutes(customExpirationMinutes);
            var toleranceMinutes = 1;

            Assert.True(Math.Abs((expectedExpiration - jwtToken.ValidTo).TotalMinutes) <= toleranceMinutes);
        }
    }
}
