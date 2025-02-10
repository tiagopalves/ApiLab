using ApiLab.CrossCutting.Common.Constants;
using ApiLab.Domain.Entities;
using ApiLab.Infra.Repository.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace ApiLab.Infra.Repository
{
    public class ClienteRedisRepository(IConnectionMultiplexer redis) : IClienteRepository
    {
        private readonly IDatabase _db = redis.GetDatabase();

        public async Task<bool> CreateAsync(Cliente cliente)
        {
            if (cliente.Id == Guid.Empty)
                cliente.Id = Guid.CreateVersion7();

            string key = $"{Constants.REDIS_CLIENTE_KEY_PREFIX}{cliente.Id}";
            string clienteJson = JsonSerializer.Serialize(cliente);

            var saved = await _db.StringSetAsync(key, clienteJson);

            if (saved)
            {
                // Adiciona o ID do cliente a um set para facilitar listagem
                await _db.SetAddAsync($"{Constants.REDIS_CLIENTE_KEY_PREFIX}all", cliente.Id.ToString());

                // Cria um índice pelo email para busca
                await _db.StringSetAsync($"{Constants.REDIS_CLIENTE_KEY_PREFIX}email:{cliente.Email}", cliente.Id.ToString());
            }

            return saved;
        }

        public async Task<Cliente?> GetByIdAsync(Guid id)
        {
            string key = $"{Constants.REDIS_CLIENTE_KEY_PREFIX}{id}";
            string? clienteJson = await _db.StringGetAsync(key);

            if (string.IsNullOrEmpty(clienteJson))
                return null;

            return JsonSerializer.Deserialize<Cliente>(clienteJson);
        }

        public async Task<Cliente?> GetByEmailAsync(string email)
        {
            string? clienteId = await _db.StringGetAsync($"{Constants.REDIS_CLIENTE_KEY_PREFIX}email:{email}");

            if (string.IsNullOrEmpty(clienteId) || !Guid.TryParse(clienteId, out Guid id))
                return null;

            return await GetByIdAsync(id);
        }

        public async Task<List<Cliente>> GetAllAsync()
        {
            var clientes = new List<Cliente>();
            var clienteIds = await _db.SetMembersAsync($"{Constants.REDIS_CLIENTE_KEY_PREFIX}all");

            foreach (var idRedis in clienteIds)
            {
                if (Guid.TryParse(idRedis.ToString(), out Guid id))
                {
                    var cliente = await GetByIdAsync(id);
                    if (cliente != null)
                        clientes.Add(cliente);
                }
            }

            return clientes;
        }

        public async Task<bool> UpdateAsync(Cliente cliente)
        {
            if (cliente.Id == Guid.Empty)
                return false;

            string key = $"{Constants.REDIS_CLIENTE_KEY_PREFIX}{cliente.Id}";

            // Obtém e remove o cliente atual em uma única operação
            var clienteJsonAtual = await _db.StringGetDeleteAsync(key);

            if (clienteJsonAtual.IsNull)
                return false;

            var clienteAtual = JsonSerializer.Deserialize<Cliente>(clienteJsonAtual.ToString());
            string clienteJsonNovo = JsonSerializer.Serialize(cliente);

            var transaction = _db.CreateTransaction();

            // Remove o índice do email antigo se mudou
            if (clienteAtual?.Email != cliente.Email)
            {
                // Remove o índice do email antigo e adiciona o índice do novo email
                transaction.KeyDeleteAsync($"{Constants.REDIS_CLIENTE_KEY_PREFIX}email:{clienteAtual?.Email}");
                transaction.StringSetAsync($"{Constants.REDIS_CLIENTE_KEY_PREFIX}email:{cliente.Email}", cliente.Id.ToString());
            }

            // Atualiza os dados do cliente
            transaction.StringSetAsync(key, clienteJsonNovo);

            return await transaction.ExecuteAsync();
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            string key = $"{Constants.REDIS_CLIENTE_KEY_PREFIX}{id}";

            // Obtém e remove o cliente em uma única operação
            var clienteJson = await _db.StringGetDeleteAsync(key);

            if (clienteJson.IsNull)
                return false;

            var cliente = JsonSerializer.Deserialize<Cliente>(clienteJson.ToString());

            var transaction = _db.CreateTransaction();

            // Remove os índices 
            transaction.SetRemoveAsync($"{Constants.REDIS_CLIENTE_KEY_PREFIX}all", id.ToString());
            transaction.KeyDeleteAsync($"{Constants.REDIS_CLIENTE_KEY_PREFIX}email:{cliente?.Email}");

            return await transaction.ExecuteAsync();
        }
    }
}
