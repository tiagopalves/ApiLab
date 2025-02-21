using ApiLab.CrossCutting.Common.Constants;
using ApiLab.Domain.Entities;
using ApiLab.Infra.Repository;
using Moq;
using StackExchange.Redis;
using System.Text.Json;

namespace ApiLab.UnitTests.Infra.Repository
{
    public class ClienteRedisRepositoryTests
    {
        private readonly Mock<IConnectionMultiplexer> _redisMock;
        private readonly Mock<IDatabase> _dbMock;
        private readonly ClienteRedisRepository _repository;

        public ClienteRedisRepositoryTests()
        {
            _redisMock = new Mock<IConnectionMultiplexer>();
            _dbMock = new Mock<IDatabase>();
            _redisMock.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(_dbMock.Object);
            _repository = new ClienteRedisRepository(_redisMock.Object);
        }

        private static Cliente CreateTestCliente()
        {
            return new Cliente
            {
                Id = Guid.NewGuid(),
                Nome = "Test Cliente",
                Email = "test@example.com"
            };
        }

        [Fact]
        public async Task CreateAsync_WhenSuccessful_ReturnsClientId()
        {
            // Arrange
            var cliente = CreateTestCliente();
            var transaction = new Mock<ITransaction>();
            var clienteJson = JsonSerializer.Serialize(cliente);
            var key = $"{Constants.REDIS_CLIENTE_KEY_PREFIX}{cliente.Id}";
            var emailKey = $"{Constants.REDIS_CLIENTE_KEY_PREFIX}{Constants.REDIS_EMAIL_KEY}{cliente.Email}";
            var allKey = $"{Constants.REDIS_CLIENTE_KEY_PREFIX}{Constants.REDIS_ALL_KEY}";

            _dbMock.Setup(x => x.CreateTransaction(It.IsAny<object>())).Returns(transaction.Object);

            transaction.Setup(x => x.AddCondition(It.IsAny<Condition>()));

            transaction.Setup(x => x.StringSetAsync(
                It.Is<RedisKey>(k => k == key),
                It.Is<RedisValue>(v => v == clienteJson),
                null, When.Always, CommandFlags.None))
                .Returns(Task.FromResult(true));

            transaction.Setup(x => x.SetAddAsync(
                It.Is<RedisKey>(k => k == allKey),
                It.Is<RedisValue>(v => v == cliente.Id.ToString()),
                CommandFlags.None))
                .Returns(Task.FromResult(true));

            transaction.Setup(x => x.StringSetAsync(
                It.Is<RedisKey>(k => k == emailKey),
                It.Is<RedisValue>(v => v == cliente.Id.ToString()),
                null, When.Always, CommandFlags.None))
                .Returns(Task.FromResult(true));

            transaction.Setup(x => x.ExecuteAsync(CommandFlags.None)).ReturnsAsync(true);

            // Act
            var result = await _repository.CreateAsync(cliente);

            // Assert
            Assert.Equal(cliente.Id, result);
            transaction.Verify(x => x.ExecuteAsync(CommandFlags.None), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_WhenClientExists_ReturnsClient()
        {
            // Arrange
            var cliente = CreateTestCliente();
            var clienteJson = JsonSerializer.Serialize(cliente);
            var key = $"{Constants.REDIS_CLIENTE_KEY_PREFIX}{cliente.Id}";

            _dbMock.Setup(x => x.StringGetAsync(key, CommandFlags.None)).ReturnsAsync(clienteJson);

            // Act
            var result = await _repository.GetByIdAsync(cliente.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cliente.Id, result.Id);
            Assert.Equal(cliente.Email, result.Email);
        }

        [Fact]
        public async Task GetByIdAsync_WhenClientDoesNotExist_ReturnsNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var key = $"{Constants.REDIS_CLIENTE_KEY_PREFIX}{id}";

            _dbMock.Setup(x => x.StringGetAsync(key, CommandFlags.None)).ReturnsAsync(RedisValue.Null);

            // Act
            var result = await _repository.GetByIdAsync(id);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetByEmailAsync_WhenClientExists_ReturnsClient()
        {
            // Arrange
            var cliente = CreateTestCliente();
            var clienteJson = JsonSerializer.Serialize(cliente);
            var emailKey = $"{Constants.REDIS_CLIENTE_KEY_PREFIX}{Constants.REDIS_EMAIL_KEY}{cliente.Email}";
            var clienteKey = $"{Constants.REDIS_CLIENTE_KEY_PREFIX}{cliente.Id}";

            _dbMock.Setup(x => x.StringGetAsync(emailKey, CommandFlags.None)).ReturnsAsync(cliente.Id.ToString());
            _dbMock.Setup(x => x.StringGetAsync(clienteKey, CommandFlags.None)).ReturnsAsync(clienteJson);

            // Act
            var result = await _repository.GetByEmailAsync(cliente.Email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cliente.Id, result.Id);
            Assert.Equal(cliente.Email, result.Email);
        }

        [Fact]
        public async Task GetAllAsync_WhenClientsExist_ReturnsAllClients()
        {
            // Arrange
            var clientes = new List<Cliente>
            {
                CreateTestCliente(),
                CreateTestCliente()
            };

            var clienteIds = clientes.Select(c => (RedisValue)c.Id.ToString()).ToArray();
            var clientesJson = clientes.Select(c => (RedisValue)JsonSerializer.Serialize(c)).ToArray();

            _dbMock.Setup(x => x.SetMembersAsync($"{Constants.REDIS_CLIENTE_KEY_PREFIX}{Constants.REDIS_ALL_KEY}", CommandFlags.None))
                .ReturnsAsync(clienteIds);

            _dbMock.Setup(x => x.StringGetAsync(It.IsAny<RedisKey[]>(), CommandFlags.None))
                .ReturnsAsync(clientesJson);

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.Equal(clientes.Count, result.Count);
            Assert.Equal(clientes[0].Id, result[0].Id);
            Assert.Equal(clientes[1].Id, result[1].Id);
        }

        [Fact]
        public async Task UpdateAsync_WhenClientExists_AndEmailChanged_UpdatesSuccessfully()
        {
            // Arrange
            var clienteAntigo = CreateTestCliente();
            var clienteNovo = new Cliente { Id = clienteAntigo.Id, Nome = clienteAntigo.Nome, Email = "novo@example.com" };
            var transaction = new Mock<ITransaction>();

            var clienteJsonAntigo = JsonSerializer.Serialize(clienteAntigo);
            var clienteJsonNovo = JsonSerializer.Serialize(clienteNovo);
            var key = $"{Constants.REDIS_CLIENTE_KEY_PREFIX}{clienteAntigo.Id}";
            var emailKeyAntigo = $"{Constants.REDIS_CLIENTE_KEY_PREFIX}{Constants.REDIS_EMAIL_KEY}{clienteAntigo.Email}";
            var emailKeyNovo = $"{Constants.REDIS_CLIENTE_KEY_PREFIX}{Constants.REDIS_EMAIL_KEY}{clienteNovo.Email}";

            _dbMock.Setup(x => x.StringGetAsync(key, CommandFlags.None)).ReturnsAsync(clienteJsonAntigo);
            _dbMock.Setup(x => x.CreateTransaction(It.IsAny<object>())).Returns(transaction.Object);

            transaction.Setup(x => x.AddCondition(It.IsAny<Condition>()));

            transaction.Setup(x => x.KeyDeleteAsync(
                It.Is<RedisKey>(k => k == emailKeyAntigo),
                CommandFlags.None))
                .Returns(Task.FromResult(true));

            transaction.Setup(x => x.StringSetAsync(
                It.Is<RedisKey>(k => k == emailKeyNovo),
                It.Is<RedisValue>(v => v == clienteNovo.Id.ToString()),
                null, When.Always, CommandFlags.None))
                .Returns(Task.FromResult(true));

            transaction.Setup(x => x.StringSetAsync(
                It.Is<RedisKey>(k => k == key),
                It.Is<RedisValue>(v => v == clienteJsonNovo),
                null, When.Always, CommandFlags.None))
                .Returns(Task.FromResult(true));

            transaction.Setup(x => x.ExecuteAsync(CommandFlags.None)).ReturnsAsync(true);

            // Act
            var result = await _repository.UpdateAsync(clienteNovo);

            // Assert
            Assert.Equal(clienteNovo.Id, result);
            transaction.Verify(x => x.ExecuteAsync(CommandFlags.None), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenClientExists_DeletesSuccessfully()
        {
            // Arrange
            var cliente = CreateTestCliente();
            var clienteJson = JsonSerializer.Serialize(cliente);
            var transaction = new Mock<ITransaction>();
            var key = $"{Constants.REDIS_CLIENTE_KEY_PREFIX}{cliente.Id}";
            var allKey = $"{Constants.REDIS_CLIENTE_KEY_PREFIX}{Constants.REDIS_ALL_KEY}";
            var emailKey = $"{Constants.REDIS_CLIENTE_KEY_PREFIX}{Constants.REDIS_EMAIL_KEY}{cliente.Email}";

            _dbMock.Setup(x => x.StringGetAsync(key, CommandFlags.None)).ReturnsAsync(clienteJson);
            _dbMock.Setup(x => x.CreateTransaction(It.IsAny<object>())).Returns(transaction.Object);

            transaction.Setup(x => x.AddCondition(It.IsAny<Condition>()));

            transaction.Setup(x => x.StringGetDeleteAsync(
                It.Is<RedisKey>(k => k == key),
                CommandFlags.None))
                .Returns(Task.FromResult<RedisValue>(clienteJson));

            transaction.Setup(x => x.SetRemoveAsync(
                It.Is<RedisKey>(k => k == allKey),
                It.Is<RedisValue>(v => v == cliente.Id.ToString()),
                CommandFlags.None))
                .Returns(Task.FromResult(true));

            transaction.Setup(x => x.KeyDeleteAsync(
                It.Is<RedisKey>(k => k == emailKey),
                CommandFlags.None))
                .Returns(Task.FromResult(true));

            transaction.Setup(x => x.ExecuteAsync(CommandFlags.None)).ReturnsAsync(true);

            // Act
            var result = await _repository.DeleteAsync(cliente.Id);

            // Assert
            Assert.True(result);
            transaction.Verify(x => x.ExecuteAsync(CommandFlags.None), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenClientDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var id = Guid.NewGuid();
            var key = $"{Constants.REDIS_CLIENTE_KEY_PREFIX}{id}";

            _dbMock.Setup(x => x.StringGetAsync(key, CommandFlags.None))
                .ReturnsAsync(RedisValue.Null);

            // Act
            var result = await _repository.DeleteAsync(id);

            // Assert
            Assert.False(result);
        }
    }
}