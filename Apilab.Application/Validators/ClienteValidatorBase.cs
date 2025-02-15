using Apilab.Application.Commands.Interfaces;
using ApiLab.CrossCutting.Resources;
using ApiLab.Domain.Entities;
using FluentValidation;

namespace Apilab.Application.Validators
{
    public abstract class ClienteValidatorBase<T> : AbstractValidator<T> where T : IClienteCommand
    {
        protected ClienteValidatorBase()
        {
            RuleFor(x => x.Nome)
                 .NotEmpty().WithMessage($"{nameof(Cliente.Nome)} {FriendlyMessages.ValidatorRequired}");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage($"{nameof(Cliente.Email)} {FriendlyMessages.ValidatorRequired}")
                .EmailAddress().WithMessage($"{nameof(Cliente.Email)} {FriendlyMessages.ValidatorInvalid}");
        }
    }
}