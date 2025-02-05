using ApiLab.CrossCutting.Resources;
using ApiLab.Domain.Entities;
using FluentValidation;

namespace Apilab.Application.Validators
{
    public class ClienteValidator : AbstractValidator<Cliente>
    {
        public ClienteValidator()
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