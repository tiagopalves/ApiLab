using Apilab.Application.AppServices.Interfaces;
using ApiLab.Api.Controllers;
using ApiLab.CrossCutting.Issuer;
using ApiLab.CrossCutting.LogManager.Interfaces;
using ApiLab.CrossCutting.Resources;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ApiLab.UnitTests.Api.Controllers
{
    public class TokensControllerTests
    {
        private readonly Mock<ILogManager> _mockLogManager;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly TokensController _controller;

        public TokensControllerTests()
        {
            _mockLogManager = new Mock<ILogManager>();
            _mockTokenService = new Mock<ITokenService>();
            _controller = new TokensController(_mockLogManager.Object, _mockTokenService.Object);
        }

        [Fact]
        public void GetToken_ReturnsOk_WhenTokenIsGenerated()
        {
            // Arrange
            var expectedToken = "valid-token";
            _mockTokenService.Setup(service => service.GenerateToken()).Returns(expectedToken);

            // Act
            var result = _controller.GetToken();

            // Assert
            var okResult = Assert.IsType<Ok<string>>(result.Result);
            Assert.Equal(expectedToken, okResult.Value);
        }

        [Fact]
        public void GetToken_ReturnsBadRequest_WhenTokenIsNullOrEmpty()
        {
            // Arrange
            _mockTokenService.Setup(service => service.GenerateToken()).Returns(string.Empty);

            // Act
            var result = _controller.GetToken();

            // Assert
            var badRequestResult = Assert.IsType<BadRequest<ProblemDetails>>(result.Result);
            Assert.NotNull(badRequestResult.Value);
            Assert.Equal(FriendlyMessages.ErrorTitle, badRequestResult.Value!.Title);
            Assert.Equal(FriendlyMessages.TokenGenerationError, badRequestResult.Value.Detail);
            Assert.Equal(FriendlyMessages.ProblemDetailsBadRequest, badRequestResult.Value.Type);
        }

        [Fact]
        public void GetToken_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var exception = new Exception("Unexpected error");
            _mockTokenService.Setup(service => service.GenerateToken()).Throws(exception);

            // Act
            var result = _controller.GetToken();

            // Assert
            var badRequestResult = Assert.IsType<BadRequest<ProblemDetails>>(result.Result);
            Assert.NotNull(badRequestResult.Value);
            Assert.Equal(FriendlyMessages.ErrorTitle, badRequestResult.Value!.Title);
            Assert.Contains(nameof(_controller.GetToken), badRequestResult.Value.Detail);
            Assert.Equal(FriendlyMessages.ProblemDetailsBadRequest, badRequestResult.Value.Type);

            // Verifica se o log foi chamado
            _mockLogManager.Verify(log => log.AddError(
                Issues.ControllerError_4004,
                It.Is<string>(msg => msg.Contains(nameof(_controller.GetToken))),
                exception,
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<object>())
            , Times.Once);
        }
    }
}
