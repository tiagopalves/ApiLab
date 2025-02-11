using Apilab.Application.AppServices.Interfaces;
using ApiLab.CrossCutting.Issuer;
using ApiLab.CrossCutting.LogManager.Interfaces;
using ApiLab.Domain.Entities;
using ApiLab.Infra.Repository.Interfaces;
using FluentValidation;

namespace Apilab.Application.AppServices
{
    public class ClienteService(ILogManager logManager, IClienteRepository clienteRepository, IValidator<Cliente> clienteValidator) : IClienteService
    {
        private readonly ILogManager _logManager = logManager;
        private readonly IClienteRepository _clienteRepository = clienteRepository;
        private readonly IValidator<Cliente> _clienteValidator = clienteValidator;

        //TODO: Padronizar e alterar retorno da camada de serviço

        public async Task<string> CreateAsync(Cliente cliente)
        {
            var validationResult = await _clienteValidator.ValidateAsync(cliente);
            if (!validationResult.IsValid)
            {
                var erros = string.Join(" ", validationResult.Errors);

                _logManager.AddWarning(Issues.AppServiceError_2001, erros);

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

        public async Task<string?> UpdateAsync(Cliente cliente)
        {
            //TODO: Criar validator pro Update
            var validationResult = await _clienteValidator.ValidateAsync(cliente);
            if (!validationResult.IsValid)
            {
                var erros = string.Join(" ", validationResult.Errors);

                _logManager.AddWarning(Issues.AppServiceError_2002, erros);

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