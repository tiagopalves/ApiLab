using Apilab.Application.Commands;

namespace Apilab.Application.Validators
{
    public class ClienteCreateCommandValidator : ClienteValidatorBase<ClienteCreateCommand>
    {
        public ClienteCreateCommandValidator() : base()
        {
            //Validações específicas para o comando de criação de cliente
        }
    }
}