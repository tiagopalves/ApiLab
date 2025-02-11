using ApiLab.Domain.Entities;

namespace ApiLab.Infra.Repository.Interfaces
{
    public interface IClienteRepository
    {
        Task<Guid> CreateAsync(Cliente cliente);
        
        Task<Cliente?> GetByIdAsync(Guid id);

        Task<Cliente?> GetByEmailAsync(string email);

        Task<List<Cliente>> GetAllAsync();
     
        Task<bool> UpdateAsync(Cliente cliente);
        
        Task<bool> DeleteAsync(Guid id);
    }
}
