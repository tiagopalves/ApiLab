using Apilab.Application.AppServices.Interfaces;
using Apilab.Application.Commands;
using ApiLab.CrossCutting.Issuer;
using ApiLab.CrossCutting.LogManager.Interfaces;
using ApiLab.Domain.Entities;
using ApiLab.Infra.Repository.Interfaces;
using FluentValidation;

namespace Apilab.Application.AppServices
{
    public class ClienteService(ILogManager logManager, 
        IClienteRepository clienteRepository, 
        IValidator<ClienteCreateCommand> clienteCreateCommandValidator, 
        IValidator<ClienteUpdateCommand> clienteUpdateCommandValidator) : IClienteService
    {
        private readonly ILogManager _logManager = logManager;
        private readonly IClienteRepository _clienteRepository = clienteRepository;
        private readonly IValidator<ClienteCreateCommand> _clienteCreateCommandValidator = clienteCreateCommandValidator;
        private readonly IValidator<ClienteUpdateCommand> _clienteUpdateCommandValidator = clienteUpdateCommandValidator;

        //TODO: Padronizar e alterar retorno da camada de serviço, avaliar uso de nova classe ServiceResult
        //TODO: Criar testes unitários

        public async Task<string> CreateAsync(ClienteCreateCommand cliente)
        {
            var validationResult = await _clienteCreateCommandValidator.ValidateAsync(cliente);
            if (!validationResult.IsValid)
            {
                var erros = string.Join(" ", validationResult.Errors);

                _logManager.AddWarning(Issues.AppServiceWarning_2001, erros);

                return await Task.FromResult(erros);
            }

            var retorno = await _clienteRepository.CreateAsync(cliente);

            return retorno.ToString();
        }

        public async Task<Cliente?> GetByIdAsync(Guid id)
        {
            return await _clienteRepository.GetByIdAsync(id);
        }

        public async Task<Cliente?> GetByEmailAsync(string email)
        {
            return await _clienteRepository.GetByEmailAsync(email);
        }

        public async Task<List<Cliente>> GetAllAsync()
        {
            return await _clienteRepository.GetAllAsync();
        }

        public async Task<string?> UpdateAsync(ClienteUpdateCommand cliente)
        {
            var validationResult = await _clienteUpdateCommandValidator.ValidateAsync(cliente);
            if (!validationResult.IsValid)
            {
                var erros = string.Join(" ", validationResult.Errors);

                _logManager.AddWarning(Issues.AppServiceWarning_2002, erros);

                return await Task.FromResult(erros);
            }

            var isUpdated = await _clienteRepository.UpdateAsync(cliente);
            if (isUpdated)
                return string.Empty;
            else
                return null;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _clienteRepository.DeleteAsync(id);
        }
    }
}