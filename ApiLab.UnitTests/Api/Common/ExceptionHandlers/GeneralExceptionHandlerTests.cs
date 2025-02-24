using ApiLab.Api.Common.ExceptionHandlers;
using ApiLab.CrossCutting.Issuer;
using ApiLab.CrossCutting.LogManager.Interfaces;
using Microsoft.AspNetCore.Http;
using Moq;

namespace ApiLab.UnitTests.Api.Common.ExceptionHandlers
{
    public class GeneralExceptionHandlerTests
    {
        private readonly Mock<IProblemDetailsService> _mockProblemDetailsService;
        private readonly Mock<ILogManager> _mockLogManager;
        private readonly GeneralExceptionHandler _exceptionHandler;

        public GeneralExceptionHandlerTests()
        {
            _mockProblemDetailsService = new Mock<IProblemDetailsService>();
            _mockLogManager = new Mock<ILogManager>();
            _exceptionHandler = new GeneralExceptionHandler(_mockProblemDetailsService.Object, _mockLogManager.Object);
        }

        [Fact]
        public async Task TryHandleAsync_Should_LogError_And_ReturnTrue_When_ExceptionOccurs()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var exception = new Exception("Test Exception");
            var cancellationToken = CancellationToken.None;

            _mockProblemDetailsService
                .Setup(p => p.TryWriteAsync(It.IsAny<ProblemDetailsContext>()))
                .ReturnsAsync(true);

            // Act
            var result = await _exceptionHandler.TryHandleAsync(httpContext, exception, cancellationToken);

            // Assert
            Assert.True(result);
            _mockLogManager.Verify(l => l.AddError(Issues.GeneralError_500, It.IsAny<string>(), exception, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()), Times.Once);
            _mockProblemDetailsService.Verify(p => p.TryWriteAsync(It.IsAny<ProblemDetailsContext>()), Times.Once);
        }

        [Fact]
        public async Task TryHandleAsync_Should_ReturnFalse_When_ProblemDetailsServiceFails()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            var exception = new Exception("Test Exception");
            var cancellationToken = CancellationToken.None;

            _mockProblemDetailsService
                .Setup(p => p.TryWriteAsync(It.IsAny<ProblemDetailsContext>()))
                .ReturnsAsync(false);

            // Act
            var result = await _exceptionHandler.TryHandleAsync(httpContext, exception, cancellationToken);

            // Assert
            Assert.False(result);
            _mockLogManager.Verify(l => l.AddError(Issues.GeneralError_500, It.IsAny<string>(), exception, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<object>()), Times.Once);
            _mockProblemDetailsService.Verify(p => p.TryWriteAsync(It.IsAny<ProblemDetailsContext>()), Times.Once);
        }
    }
}
