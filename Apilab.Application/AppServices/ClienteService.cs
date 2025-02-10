using Apilab.Application.AppServices.Interfaces;
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

        public async Task<bool> CreateAsync(Cliente cliente)
        {
            var validationResult = await _clienteValidator.ValidateAsync(cliente);
            if (!validationResult.IsValid)
            {
                _logManager.AddInformation(string.Join(" ", validationResult.Errors));
            }

            return await _clienteRepository.CreateAsync(cliente);
        }

        public Task<Cliente?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Cliente?> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Cliente>> GetAllAsync()
        {
            return await _clienteRepository.GetAllAsync();
        }

        public Task<bool> UpdateAsync(Cliente cliente)
        {
            throw new NotImplementedException();
        }
        
        public Task<bool> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}