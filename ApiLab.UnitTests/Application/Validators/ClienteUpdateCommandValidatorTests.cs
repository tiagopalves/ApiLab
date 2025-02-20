using Apilab.Application.Commands;
using Apilab.Application.Validators;
using ApiLab.CrossCutting.Resources;
using ApiLab.Domain.Entities;
using FluentValidation.TestHelper;

namespace ApiLab.UnitTests.Application.Validators
{
    public class ClienteUpdateCommandValidatorTests
    {
        private readonly ClienteUpdateCommandValidator _validator;

        public ClienteUpdateCommandValidatorTests()
        {
            _validator = new ClienteUpdateCommandValidator();
        }

        [Fact]
        public void Should_Pass_When_Command_Is_Valid()
        {
            // Arrange
            var command = new ClienteUpdateCommand
            {
                Id = Guid.CreateVersion7(),
                Nome = "João Silva",
                Email = "joao@email.com"
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Fail_When_Id_Is_Empty()
        {
            // Arrange
            var command = new ClienteUpdateCommand
            {
                Id = Guid.Empty,
                Nome = "João Silva",
                Email = "joao@email.com"
            };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Id)
                  .WithErrorMessage($"{nameof(Cliente.Id)} {FriendlyMessages.ValidatorRequired}");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Should_Fail_When_Nome_Is_Empty(string? nome)
        {
            // Arrange
            var command = new ClienteUpdateCommand { Nome = nome ?? string.Empty };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Nome)
                  .WithErrorMessage($"{nameof(Cliente.Nome)} {FriendlyMessages.ValidatorRequired}");
        }

        [Theory]
        [InlineData("")]
        [InlineData("email_invalido")]
        [InlineData("email@")]
        [InlineData("@dominio.com")]
        public void Should_Fail_When_Email_Is_Invalid(string email)
        {
            // Arrange
            var command = new ClienteUpdateCommand { Email = email };

            // Act
            var result = _validator.TestValidate(command);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }
    }
}
