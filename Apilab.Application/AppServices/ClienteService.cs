using Apilab.Application.AppServices.Interfaces;
using Apilab.Application.Commands;
using ApiLab.Domain.Entities;
using ApiLab.Infra.Repository.Interfaces;
using FluentValidation;

namespace Apilab.Application.AppServices
{
    public class ClienteService(IClienteRepository clienteRepository,
        IValidator<ClienteCreateCommand> clienteCreateCommandValidator,
        IValidator<ClienteUpdateCommand> clienteUpdateCommandValidator) : IClienteService
    {
        private readonly IClienteRepository _clienteRepository = clienteRepository;
        private readonly IValidator<ClienteCreateCommand> _clienteCreateCommandValidator = clienteCreateCommandValidator;
        private readonly IValidator<ClienteUpdateCommand> _clienteUpdateCommandValidator = clienteUpdateCommandValidator;

        public async Task<string> CreateAsync(ClienteCreateCommand cliente, CancellationToken cancellationToken)
        {
            var validationResult = await _clienteCreateCommandValidator.ValidateAsync(cliente, cancellationToken);
            
            if (!validationResult.IsValid)
                return await Task.FromResult(string.Join(" ", validationResult.Errors));

            var result = await _clienteRepository.CreateAsync(cliente);

            return result.ToString();
        }

        public async Task<Cliente?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _clienteRepository.GetByIdAsync(id);
        }

        public async Task<Cliente?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _clienteRepository.GetByEmailAsync(email);
        }

        public async Task<List<Cliente>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _clienteRepository.GetAllAsync();
        }

        public async Task<string?> UpdateAsync(ClienteUpdateCommand cliente, CancellationToken cancellationToken)
        {
            var validationResult = await _clienteUpdateCommandValidator.ValidateAsync(cliente, cancellationToken);
            
            if (!validationResult.IsValid)
                return await Task.FromResult(string.Join(" ", validationResult.Errors));

            var result = await _clienteRepository.UpdateAsync(cliente);

            return result?.ToString();
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _clienteRepository.DeleteAsync(id);
        }
    }
}