using ApiLab.Domain.Entities;

namespace Apilab.Application.AppServices.Interfaces
{
    public interface IClienteService
    {
        Task<bool> CreateAsync(Cliente cliente);

        Task<Cliente?> GetByIdAsync(Guid id);

        Task<Cliente?> GetByEmailAsync(string email);
        
        Task<List<Cliente>> GetAllAsync();

        Task<bool> UpdateAsync(Cliente cliente);

        Task<bool> DeleteAsync(Guid id);
    }
}
