using Apilab.Application.Commands;
using ApiLab.Domain.Entities;

namespace Apilab.Application.AppServices.Interfaces
{
    public interface IClienteService
    {
        /// <summary>
        /// Cria um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto com as informações do cliente</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Id do cliente criado ou lista de erros de validação</returns>
        Task<string> CreateAsync(ClienteCreateCommand cliente, CancellationToken cancellationToken);

        /// <summary>
        /// Busca um cliente pelo Id
        /// </summary>
        /// <param name="id">Id do cliente</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Objeto com o cliente encontrado ou null, caso não encontrado</returns>
        Task<Cliente?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Busca um cliente pelo email
        /// </summary>
        /// <param name="email">Email do cliente</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Objeto com o cliente encontrado ou null, caso não encontrado</returns>
        Task<Cliente?> GetByEmailAsync(string email, CancellationToken cancellationToken);

        /// <summary>
        /// Busca todos os clientes
        /// </summary>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Lista com os clientes encontrados</returns>
        Task<List<Cliente>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Atualiza um cliente
        /// </summary>
        /// <param name="cliente">Objeto com as informações do cliente</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Id do cliente atualizado ou lista de erros de validação</returns>
        Task<string?> UpdateAsync(ClienteUpdateCommand cliente, CancellationToken cancellationToken);

        /// <summary>
        /// Deleta um cliente
        /// </summary>
        /// <param name="id">Id do cliente</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>True em caso de sucesso e false, caso contrário</returns>
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
