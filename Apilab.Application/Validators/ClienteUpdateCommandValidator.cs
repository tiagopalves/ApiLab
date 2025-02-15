using Apilab.Application.Commands;
using ApiLab.CrossCutting.Resources;
using ApiLab.Domain.Entities;
using FluentValidation;

namespace Apilab.Application.Validators
{
    public class ClienteUpdateCommandValidator : ClienteValidatorBase<ClienteUpdateCommand>
    {
        public ClienteUpdateCommandValidator() : base()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage($"{nameof(Cliente.Id)} {FriendlyMessages.ValidatorRequired}");
        }
    }
}