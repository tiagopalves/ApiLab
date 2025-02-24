using Apilab.Application.AppServices.Interfaces;
using Apilab.Application.Commands;
using ApiLab.Api.Controllers;
using ApiLab.CrossCutting.Common.Constants;
using ApiLab.CrossCutting.Issuer;
using ApiLab.CrossCutting.LogManager.Interfaces;
using ApiLab.CrossCutting.Resources;
using ApiLab.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ApiLab.UnitTests.Api.Controllers
{
    public class ClientesControllerTests
    {
        private readonly Mock<ILogManager> _mockLogManager;
        private readonly Mock<IClienteService> _mockClienteService;
        private readonly ClientesController _controller;
        private readonly CancellationToken _cancellationToken;

        public ClientesControllerTests()
        {
            _mockLogManager = new Mock<ILogManager>();
            _mockClienteService = new Mock<IClienteService>();
            _controller = new ClientesController(_mockLogManager.Object, _mockClienteService.Object);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task CreateAsync_ReturnsCreated_WhenServiceReturnsValidGuid()
        {
            // Arrange
            var command = new ClienteCreateCommand();
            var expectedGuid = Guid.NewGuid().ToString();

            _mockClienteService
                .Setup(s => s.CreateAsync(command, _cancellationToken))
                .ReturnsAsync(expectedGuid);

            // Act
            var result = await _controller.CreateAsync(command, _cancellationToken);

            // Assert
            var createdResult = Assert.IsType<Created<Guid>>(result.Result);
            Assert.Equal($"{Constants.CLIENTES_ENDPOINT}{expectedGuid}", createdResult.Location);
            Assert.Equal(Guid.Parse(expectedGuid), createdResult.Value);
            _mockClienteService.Verify(s => s.CreateAsync(command, _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ReturnsBadRequest_WhenServiceReturnsInvalidResult()
        {
            // Arrange
            var command = new ClienteCreateCommand();
            var errorMessage = "Validation error";

            _mockClienteService
                .Setup(s => s.CreateAsync(command, _cancellationToken))
                .ReturnsAsync(errorMessage);

            // Act
            var result = await _controller.CreateAsync(command, _cancellationToken);

            // Assert
            var badRequestResult = Assert.IsType<BadRequest<ProblemDetails>>(result.Result);
            Assert.NotNull(badRequestResult.Value);
            Assert.Equal(FriendlyMessages.ErrorTitlePayload, badRequestResult.Value!.Title);
            Assert.Equal(errorMessage, badRequestResult.Value.Detail);
            _mockLogManager.Verify(l => l.AddWarning(
                Issues.ControllerWarning_2001,
                FriendlyMessages.ErrorTitlePayload,
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<Exception>(),
                errorMessage)
            , Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var command = new ClienteCreateCommand();
            var exception = new Exception("Test exception");

            _mockClienteService
                .Setup(s => s.CreateAsync(command, _cancellationToken))
                .ThrowsAsync(exception);

            // Act
            var result = await _controller.CreateAsync(command, _cancellationToken);

            // Assert
            var badRequestResult = Assert.IsType<BadRequest<ProblemDetails>>(result.Result);
            Assert.NotNull(badRequestResult.Value);
            Assert.Equal(FriendlyMessages.ErrorTitle, badRequestResult.Value!.Title);
            Assert.Equal($"{FriendlyMessages.ErrorEndpoint} {nameof(ClientesController.CreateAsync)}!", badRequestResult.Value.Detail);
            _mockLogManager.Verify(l => l.AddError(
                Issues.ControllerError_4001,
                $"{FriendlyMessages.ErrorEndpoint} {nameof(ClientesController.CreateAsync)}!",
                exception,
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<object>())
            , Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsOk_WhenClienteExists()
        {
            // Arrange
            var clienteId = Guid.NewGuid();
            var cliente = new Cliente();

            _mockClienteService
                .Setup(s => s.GetByIdAsync(clienteId, _cancellationToken))
                .ReturnsAsync(cliente);

            // Act
            var result = await _controller.GetByIdAsync(clienteId, _cancellationToken);

            // Assert
            var okResult = Assert.IsType<Ok<Cliente>>(result.Result);
            Assert.Equal(cliente, okResult.Value);
            _mockClienteService.Verify(s => s.GetByIdAsync(clienteId, _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNotFound_WhenClienteDoesNotExist()
        {
            // Arrange
            var clienteId = Guid.NewGuid();

            _mockClienteService
                .Setup(s => s.GetByIdAsync(clienteId, _cancellationToken))
                .ReturnsAsync(null as Cliente);

            // Act
            var result = await _controller.GetByIdAsync(clienteId, _cancellationToken);

            // Assert
            Assert.IsType<NotFound>(result.Result);
            _mockLogManager.Verify(l => l.AddWarning(
               Issues.ControllerWarning_2002,
               FriendlyMessages.ClienteNotFound,
               It.IsAny<string>(),
               It.IsAny<string>(),
               It.IsAny<Exception>(),
               It.IsAny<object>())
           , Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var clienteId = Guid.NewGuid();
            var exception = new Exception("Test exception");

            _mockClienteService
                .Setup(s => s.GetByIdAsync(clienteId, _cancellationToken))
                .ThrowsAsync(exception);

            // Act
            var result = await _controller.GetByIdAsync(clienteId, _cancellationToken);

            // Assert
            var badRequestResult = Assert.IsType<BadRequest<ProblemDetails>>(result.Result);
            Assert.NotNull(badRequestResult.Value);
            Assert.Equal(FriendlyMessages.ErrorTitle, badRequestResult.Value!.Title);
            Assert.Equal($"{FriendlyMessages.ErrorEndpoint} {nameof(ClientesController.GetByIdAsync)}!", badRequestResult.Value.Detail);
            _mockLogManager.Verify(l => l.AddError(
                Issues.ControllerError_4002,
                $"{FriendlyMessages.ErrorEndpoint} {nameof(ClientesController.GetAllAsync)}!",
                exception,
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<object>())
            , Times.Once);
        }

        [Fact]
        public async Task GetByEmailAsync_ReturnsOk_WhenClienteExists()
        {
            // Arrange
            var email = "teste@exemplo.com";
            var cliente = new Cliente();

            _mockClienteService
                .Setup(s => s.GetByEmailAsync(email, _cancellationToken))
                .ReturnsAsync(cliente);

            // Act
            var result = await _controller.GetByEmailAsync(email, _cancellationToken);

            // Assert
            var okResult = Assert.IsType<Ok<Cliente>>(result.Result);
            Assert.Equal(cliente, okResult.Value);
            _mockClienteService.Verify(s => s.GetByEmailAsync(email, _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task GetByEmailAsync_ReturnsNotFound_WhenClienteDoesNotExist()
        {
            // Arrange
            var email = "email@inexistente.com";

            _mockClienteService
                .Setup(s => s.GetByEmailAsync(email, _cancellationToken))
                .ReturnsAsync(null as Cliente);

            // Act
            var result = await _controller.GetByEmailAsync(email, _cancellationToken);

            // Assert
            Assert.IsType<NotFound>(result.Result);
            _mockLogManager.Verify(l => l.AddWarning(
               Issues.ControllerWarning_2003,
               FriendlyMessages.ClienteNotFound,
               It.IsAny<string>(),
               It.IsAny<string>(),
               It.IsAny<Exception>(),
               It.IsAny<object>())
           , Times.Once);
        }

        [Fact]
        public async Task GetByEmailAsync_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var email = "teste@exemplo.com";
            var exception = new Exception("Test exception");

            _mockClienteService
                .Setup(s => s.GetByEmailAsync(email, _cancellationToken))
                .ThrowsAsync(exception);

            // Act
            var result = await _controller.GetByEmailAsync(email, _cancellationToken);

            // Assert
            var badRequestResult = Assert.IsType<BadRequest<ProblemDetails>>(result.Result);
            Assert.NotNull(badRequestResult.Value);
            Assert.Equal(FriendlyMessages.ErrorTitle, badRequestResult.Value!.Title);
            Assert.Equal($"{FriendlyMessages.ErrorEndpoint} {nameof(ClientesController.GetByEmailAsync)}!", badRequestResult.Value.Detail);
            _mockLogManager.Verify(l => l.AddError(
                Issues.ControllerError_4003,
                $"{FriendlyMessages.ErrorEndpoint} {nameof(ClientesController.GetByEmailAsync)}!",
                exception,
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<object>())
            , Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsOk_WhenClientsExist()
        {
            // Arrange
            var clientes = new List<Cliente> { new(), new() };

            _mockClienteService
                .Setup(s => s.GetAllAsync(_cancellationToken))
                .ReturnsAsync(clientes);

            // Act
            var result = await _controller.GetAllAsync(_cancellationToken);

            // Assert
            var okResult = Assert.IsType<Ok<List<Cliente>>>(result.Result);
            Assert.Equal(clientes, okResult.Value);
            _mockClienteService.Verify(s => s.GetAllAsync(_cancellationToken), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsNoContent_WhenNoClientsExist()
        {
            // Arrange
            var emptyList = new List<Cliente>();

            _mockClienteService
                .Setup(s => s.GetAllAsync(_cancellationToken))
                .ReturnsAsync(emptyList);

            // Act
            var result = await _controller.GetAllAsync(_cancellationToken);

            // Assert
            Assert.IsType<NoContent>(result.Result);
            _mockLogManager.Verify(l => l.AddWarning(
               Issues.ControllerWarning_2004,
               FriendlyMessages.ClientesNotFound,
               It.IsAny<string>(),
               It.IsAny<string>(),
               It.IsAny<Exception>(),
               It.IsAny<object>())
           , Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var exception = new Exception("Test exception");

            _mockClienteService
                .Setup(s => s.GetAllAsync(_cancellationToken))
                .ThrowsAsync(exception);

            // Act
            var result = await _controller.GetAllAsync(_cancellationToken);

            // Assert
            var badRequestResult = Assert.IsType<BadRequest<ProblemDetails>>(result.Result);
            Assert.NotNull(badRequestResult.Value);
            Assert.Equal(FriendlyMessages.ErrorTitle, badRequestResult.Value!.Title);
            Assert.Equal($"{FriendlyMessages.ErrorEndpoint} {nameof(ClientesController.GetAllAsync)}!", badRequestResult.Value.Detail);
            _mockLogManager.Verify(l => l.AddError(
                Issues.ControllerError_4004,
                $"{FriendlyMessages.ErrorEndpoint} {nameof(ClientesController.GetAllAsync)}!",
                exception,
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<object>())
            , Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsOk_WhenUpdateIsSuccessful()
        {
            // Arrange
            var command = new ClienteUpdateCommand();
            var expectedGuid = Guid.NewGuid().ToString();

            _mockClienteService
                .Setup(s => s.UpdateAsync(command, _cancellationToken))
                .ReturnsAsync(expectedGuid);

            // Act
            var result = await _controller.UpdateAsync(command, _cancellationToken);

            // Assert
            var okResult = Assert.IsType<Ok<ClienteUpdateCommand>>(result.Result);
            Assert.Equal(command, okResult.Value);
            _mockClienteService.Verify(s => s.UpdateAsync(command, _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsNotFound_WhenClienteDoesNotExist()
        {
            // Arrange
            var command = new ClienteUpdateCommand();

            _mockClienteService
                .Setup(s => s.UpdateAsync(command, _cancellationToken))
                .ReturnsAsync(null as string);

            // Act
            var result = await _controller.UpdateAsync(command, _cancellationToken);

            // Assert
            Assert.IsType<NotFound>(result.Result);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsBadRequest_WhenValidationFails()
        {
            // Arrange
            var command = new ClienteUpdateCommand();
            var errorMessage = "Validation error";

            _mockClienteService
                .Setup(s => s.UpdateAsync(command, _cancellationToken))
                .ReturnsAsync(errorMessage);

            // Act
            var result = await _controller.UpdateAsync(command, _cancellationToken);

            // Assert
            var badRequestResult = Assert.IsType<BadRequest<ProblemDetails>>(result.Result);
            Assert.NotNull(badRequestResult.Value);
            Assert.Equal(FriendlyMessages.ErrorTitlePayload, badRequestResult.Value!.Title);
            Assert.Equal(errorMessage, badRequestResult.Value.Detail);
            _mockLogManager.Verify(l => l.AddWarning(
              Issues.ControllerWarning_2005,
              FriendlyMessages.ErrorTitlePayload,
              It.IsAny<string>(),
              It.IsAny<string>(),
              It.IsAny<Exception>(),
              It.IsAny<object>())
          , Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var command = new ClienteUpdateCommand();
            var exception = new Exception("Test exception");

            _mockClienteService
                .Setup(s => s.UpdateAsync(command, _cancellationToken))
                .ThrowsAsync(exception);

            // Act
            var result = await _controller.UpdateAsync(command, _cancellationToken);

            // Assert
            var badRequestResult = Assert.IsType<BadRequest<ProblemDetails>>(result.Result);
            Assert.NotNull(badRequestResult.Value);
            Assert.Equal(FriendlyMessages.ErrorTitle, badRequestResult.Value!.Title);
            Assert.Equal($"{FriendlyMessages.ErrorEndpoint} {nameof(ClientesController.UpdateAsync)}!", badRequestResult.Value.Detail);
            _mockLogManager.Verify(l => l.AddError(
                Issues.ControllerError_4005,
                $"{FriendlyMessages.ErrorEndpoint} {nameof(ClientesController.UpdateAsync)}!",
                exception,
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<object>())
            , Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsOk_WhenDeleteIsSuccessful()
        {
            // Arrange
            var clienteId = Guid.NewGuid();

            _mockClienteService
                .Setup(s => s.DeleteAsync(clienteId, _cancellationToken))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteAsync(clienteId, _cancellationToken);

            // Assert
            Assert.IsType<Ok>(result.Result);
            _mockClienteService.Verify(s => s.DeleteAsync(clienteId, _cancellationToken), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsNotFound_WhenClienteDoesNotExist()
        {
            // Arrange
            var clienteId = Guid.NewGuid();

            _mockClienteService
                .Setup(s => s.DeleteAsync(clienteId, _cancellationToken))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteAsync(clienteId, _cancellationToken);

            // Assert
            Assert.IsType<NotFound>(result.Result);
            _mockLogManager.Verify(l => l.AddWarning(
              Issues.ControllerWarning_2006,
              FriendlyMessages.ClienteNotFound,
              It.IsAny<string>(),
              It.IsAny<string>(),
              It.IsAny<Exception>(),
              It.IsAny<object>())
          , Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var clienteId = Guid.NewGuid();
            var exception = new Exception("Test exception");

            _mockClienteService
                .Setup(s => s.DeleteAsync(clienteId, _cancellationToken))
                .ThrowsAsync(exception);

            // Act
            var result = await _controller.DeleteAsync(clienteId, _cancellationToken);

            // Assert
            var badRequestResult = Assert.IsType<BadRequest<ProblemDetails>>(result.Result);
            Assert.NotNull(badRequestResult.Value);
            Assert.Equal(FriendlyMessages.ErrorTitle, badRequestResult.Value!.Title);
            Assert.Equal($"{FriendlyMessages.ErrorEndpoint} {nameof(ClientesController.DeleteAsync)}!", badRequestResult.Value.Detail);
            _mockLogManager.Verify(l => l.AddError(
                Issues.ControllerError_4006,
                $"{FriendlyMessages.ErrorEndpoint} {nameof(ClientesController.DeleteAsync)}!",
                exception,
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<object>())
            , Times.Once);
        }
    }
}
