using Apilab.Application.Commands;
using ApiLab.CrossCutting.Resources;
using ApiLab.Domain.Entities;
using FluentValidation;

namespace Apilab.Application.Validators
{
    public class ClienteCreateValidator : AbstractValidator<ClienteCreateCommand>
    {
        public ClienteCreateValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage($"{nameof(Cliente.Nome)} {FriendlyMessages.ValidatorRequired}");
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage($"{nameof(Cliente.Email)} {FriendlyMessages.ValidatorInvalid}");
        }
    }
}