using Apilab.Application.Commands;
using ApiLab.CrossCutting.Resources;
using ApiLab.Domain.Entities;
using FluentValidation;

namespace Apilab.Application.Validators
{
    public class ClienteUpdateValidator : AbstractValidator<ClienteUpdateCommand>
    {
        public ClienteUpdateValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage($"{nameof(Cliente.Id)} {FriendlyMessages.ValidatorRequired}");
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage($"{nameof(Cliente.Nome)} {FriendlyMessages.ValidatorRequired}");
            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage($"{nameof(Cliente.Email)} {FriendlyMessages.ValidatorInvalid}");
        }
    }
}