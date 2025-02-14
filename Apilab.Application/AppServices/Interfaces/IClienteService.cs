using Apilab.Application.Commands;
using ApiLab.Domain.Entities;

namespace Apilab.Application.AppServices.Interfaces
{
    public interface IClienteService
    {
        Task<string> CreateAsync(ClienteCreateCommand cliente);

        Task<Cliente?> GetByIdAsync(Guid id);

        Task<Cliente?> GetByEmailAsync(string email);
        
        Task<List<Cliente>> GetAllAsync();

        Task<string?> UpdateAsync(ClienteUpdateCommand cliente);

        Task<bool> DeleteAsync(Guid id);
    }
}
