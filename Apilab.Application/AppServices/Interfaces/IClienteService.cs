using ApiLab.Domain.Entities;

namespace Apilab.Application.AppServices.Interfaces
{
    public interface IClienteService
    {
        Task<string> CreateAsync(Cliente cliente);

        Task<Cliente?> GetByIdAsync(Guid id);

        Task<Cliente?> GetByEmailAsync(string email);
        
        Task<List<Cliente>> GetAllAsync();

        Task<string?> UpdateAsync(Cliente cliente);

        Task<bool> DeleteAsync(Guid id);
    }
}
