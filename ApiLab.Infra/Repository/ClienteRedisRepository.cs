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

        public async Task<Guid> CreateAsync(Cliente cliente)
        {
            string key = $"{Constants.REDIS_CLIENTE_KEY_PREFIX}{cliente.Id}";
            string emailKey = $"{Constants.REDIS_CLIENTE_KEY_PREFIX}{Constants.REDIS_EMAIL_KEY}{cliente.Email}";
            string clienteJson = JsonSerializer.Serialize(cliente);

            var transaction = _db.CreateTransaction();
            transaction.AddCondition(Condition.KeyNotExists(key));
            transaction.AddCondition(Condition.KeyNotExists(emailKey));

            List<Task> tasks = [
                transaction.StringSetAsync(key, clienteJson),
                transaction.SetAddAsync($"{Constants.REDIS_CLIENTE_KEY_PREFIX}{Constants.REDIS_ALL_KEY}", cliente.Id.ToString()), // Adiciona o ID do cliente a um set para facilitar listagem
                transaction.StringSetAsync(emailKey, cliente.Id.ToString())]; // Cria um índice pelo email para busca

            // Aguarda a conclusão das operações na ordem definida
            if (await transaction.ExecuteAsync())
            {
                foreach (var task in tasks)
                    await task;

                return cliente.Id;
            }

            return Guid.Empty;
        }

        public async Task<Cliente?> GetByIdAsync(Guid id)
        {
            string key = $"{Constants.REDIS_CLIENTE_KEY_PREFIX}{id}";
            var clienteJson = await _db.StringGetAsync(key);

            if (clienteJson.IsNull)
                return null;

            return JsonSerializer.Deserialize<Cliente>(clienteJson.ToString());
        }

        public async Task<Cliente?> GetByEmailAsync(string email)
        {
            var clienteId = await _db.StringGetAsync($"{Constants.REDIS_CLIENTE_KEY_PREFIX}{Constants.REDIS_EMAIL_KEY}{email}");

            if (clienteId.IsNull || !Guid.TryParse(clienteId, out Guid id))
                return null;

            return await GetByIdAsync(id);
        }

        public async Task<List<Cliente>> GetAllAsync()
        {
            var result = new List<Cliente>();
            var clienteIds = await _db.SetMembersAsync($"{Constants.REDIS_CLIENTE_KEY_PREFIX}{Constants.REDIS_ALL_KEY}");

            if (clienteIds.Length == 0)
                return result;

            var keys = clienteIds.Select(id => (RedisKey)$"{Constants.REDIS_CLIENTE_KEY_PREFIX}{id}").ToArray();

            // Busca todos os clientes em uma única operação
            var clientesJson = await _db.StringGetAsync(keys);

            foreach (var json in clientesJson)
            {
                if (!json.IsNull)
                {
                    var cliente = JsonSerializer.Deserialize<Cliente>(json.ToString());
                    
                    if (cliente is not null)
                        result.Add(cliente);
                }
            }

            return result;
        }

        public async Task<Guid?> UpdateAsync(Cliente cliente)
        {
            string key = $"{Constants.REDIS_CLIENTE_KEY_PREFIX}{cliente.Id}";

            var clienteJsonAtual = await _db.StringGetAsync(key);
            if (clienteJsonAtual.IsNull)
                return null;

            var clienteAtual = JsonSerializer.Deserialize<Cliente>(clienteJsonAtual.ToString());
            string clienteJsonNovo = JsonSerializer.Serialize(cliente);

            var transaction = _db.CreateTransaction();
            transaction.AddCondition(Condition.KeyExists(key));
            
            List<Task> tasks = [];

            // Remove o índice do email antigo se mudou
            if (clienteAtual?.Email != cliente.Email)
            {
                // Remove o índice do email antigo e adiciona o índice do novo email
                tasks.Add(transaction.KeyDeleteAsync($"{Constants.REDIS_CLIENTE_KEY_PREFIX}{Constants.REDIS_EMAIL_KEY}{clienteAtual?.Email}"));
                tasks.Add(transaction.StringSetAsync($"{Constants.REDIS_CLIENTE_KEY_PREFIX}{Constants.REDIS_EMAIL_KEY}{cliente.Email}", cliente.Id.ToString()));
            }

            // Atualiza os dados do cliente
            tasks.Add(transaction.StringSetAsync(key, clienteJsonNovo));

            // Aguarda a conclusão das operações na ordem definida
            if (await transaction.ExecuteAsync())
            {
                foreach (var task in tasks)
                    await task;
                
                return cliente.Id;
            }

            return Guid.Empty;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            string key = $"{Constants.REDIS_CLIENTE_KEY_PREFIX}{id}";

            var clienteJson = await _db.StringGetAsync(key);
            if (clienteJson.IsNull)
                return false;

            var cliente = JsonSerializer.Deserialize<Cliente>(clienteJson.ToString());

            var transaction = _db.CreateTransaction();
            transaction.AddCondition(Condition.KeyExists(key));

            List<Task> tasks = [
                transaction.StringGetDeleteAsync(key),
                transaction.SetRemoveAsync($"{Constants.REDIS_CLIENTE_KEY_PREFIX}{Constants.REDIS_ALL_KEY}", id.ToString()),
                transaction.KeyDeleteAsync($"{Constants.REDIS_CLIENTE_KEY_PREFIX}{Constants.REDIS_EMAIL_KEY}{cliente?.Email}")];

            // Aguarda a conclusão das operações na ordem definida
            if (await transaction.ExecuteAsync())
            {
                foreach (var task in tasks)
                    await task;
            
                return true;
            }

            return false;
        }
    }
}
