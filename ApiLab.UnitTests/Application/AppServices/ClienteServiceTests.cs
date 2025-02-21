using Apilab.Application.AppServices;
using Apilab.Application.Commands;
using ApiLab.Domain.Entities;
using ApiLab.Infra.Repository.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Moq;

namespace ApiLab.UnitTests.Application.AppServices
{
    public class ClienteServiceTests
    {
        private readonly Mock<IClienteRepository> _clienteRepositoryMock;
        private readonly Mock<IValidator<ClienteCreateCommand>> _createValidatorMock;
        private readonly Mock<IValidator<ClienteUpdateCommand>> _updateValidatorMock;
        private readonly ClienteService _clienteService;

        public ClienteServiceTests()
        {
            _clienteRepositoryMock = new Mock<IClienteRepository>();
            _createValidatorMock = new Mock<IValidator<ClienteCreateCommand>>();
            _updateValidatorMock = new Mock<IValidator<ClienteUpdateCommand>>();
            _clienteService = new ClienteService(
                _clienteRepositoryMock.Object,
                _createValidatorMock.Object,
                _updateValidatorMock.Object
            );
        }

        [Fact]
        public async Task CreateAsync_WithValidData_ReturnsSuccess()
        {
            // Arrange
            var command = new ClienteCreateCommand
            {
                Nome = "Test",
                Email = "test@test.com"
            };
            var validationResult = new ValidationResult();

            Cliente? capturedCliente = null;
            _clienteRepositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<Cliente>()))
                .Callback<Cliente>(cmd =>
                {
                    var cliente = cmd;
                    capturedCliente = cliente;
                })
                .ReturnsAsync(() => capturedCliente!.Id);

            _createValidatorMock
                .Setup(x => x.ValidateAsync(It.IsAny<ClienteCreateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _clienteService.CreateAsync(command, CancellationToken.None);

            // Assert
            Assert.NotNull(capturedCliente);
            Assert.Equal(capturedCliente!.Id.ToString(), result);
            _clienteRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Cliente>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WithInvalidData_ReturnsValidationErrors()
        {
            // Arrange
            var command = new ClienteCreateCommand();
            var validationFailures = new List<ValidationFailure>
            {
                new("Property1", "Error1"),
                new("Property2", "Error2")
            };
            var validationResult = new ValidationResult(validationFailures);

            _createValidatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _clienteService.CreateAsync(command, CancellationToken.None);

            // Assert
            Assert.Equal("Error1 Error2", result);
            _clienteRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Cliente>()), Times.Never);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingId_ReturnsCliente()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedCliente = new Cliente();

            _clienteRepositoryMock
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync(expectedCliente);

            // Act
            var result = await _clienteService.GetByIdAsync(id, CancellationToken.None);

            // Assert
            Assert.Equal(expectedCliente, result);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            var id = Guid.NewGuid();

            _clienteRepositoryMock
                .Setup(x => x.GetByIdAsync(id))
                .ReturnsAsync((Cliente?)null);

            // Act
            var result = await _clienteService.GetByIdAsync(id, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByEmailAsync_ExistingEmail_ReturnsCliente()
        {
            // Arrange
            var email = "test@example.com";
            var expectedCliente = new Cliente();

            _clienteRepositoryMock
                .Setup(x => x.GetByEmailAsync(email))
                .ReturnsAsync(expectedCliente);

            // Act
            var result = await _clienteService.GetByEmailAsync(email, CancellationToken.None);

            // Assert
            Assert.Equal(expectedCliente, result);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllClientes()
        {
            // Arrange
            var expectedClientes = new List<Cliente> { new(), new() };

            _clienteRepositoryMock
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(expectedClientes);

            // Act
            var result = await _clienteService.GetAllAsync(CancellationToken.None);

            // Assert
            Assert.Equal(expectedClientes.Count, result.Count);
            Assert.Equal(expectedClientes, result);
        }

        [Fact]
        public async Task UpdateAsync_WithValidData_ReturnsSuccess()
        {
            // Arrange
            var command = new ClienteUpdateCommand
            {
                Id = Guid.NewGuid(),
                Nome = "Test",
                Email = "test@test.com"
            };
            var validationResult = new ValidationResult();

            Cliente? capturedCliente = null;
            _clienteRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<Cliente>()))
                .Callback<Cliente>(cmd =>
                {
                    var cliente = cmd;
                    capturedCliente = cliente;
                })
                .ReturnsAsync(() => capturedCliente!.Id);

            _updateValidatorMock
                .Setup(x => x.ValidateAsync(It.IsAny<ClienteUpdateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _clienteService.UpdateAsync(command, CancellationToken.None);

            // Assert
            Assert.NotNull(capturedCliente);
            Assert.Equal(capturedCliente!.Id.ToString(), result);
            _clienteRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Cliente>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithInvalidData_ReturnsValidationErrors()
        {
            // Arrange
            var command = new ClienteUpdateCommand();
            var validationFailures = new List<ValidationFailure>
            {
                new("Property1", "Error1"),
                new("Property2", "Error2")
            };
            var validationResult = new ValidationResult(validationFailures);

            _updateValidatorMock
                .Setup(x => x.ValidateAsync(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _clienteService.UpdateAsync(command, CancellationToken.None);

            // Assert
            Assert.Equal("Error1 Error2", result);
            _clienteRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Cliente>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_ExistingId_ReturnsTrue()
        {
            // Arrange
            var id = Guid.NewGuid();

            _clienteRepositoryMock
                .Setup(x => x.DeleteAsync(id))
                .ReturnsAsync(true);

            // Act
            var result = await _clienteService.DeleteAsync(id, CancellationToken.None);

            // Assert
            Assert.True(result);
            _clienteRepositoryMock.Verify(x => x.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingId_ReturnsFalse()
        {
            // Arrange
            var id = Guid.NewGuid();

            _clienteRepositoryMock
                .Setup(x => x.DeleteAsync(id))
                .ReturnsAsync(false);

            // Act
            var result = await _clienteService.DeleteAsync(id, CancellationToken.None);

            // Assert
            Assert.False(result);
            _clienteRepositoryMock.Verify(x => x.DeleteAsync(id), Times.Once);
        }
    }
}
