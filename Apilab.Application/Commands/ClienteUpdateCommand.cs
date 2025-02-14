using ApiLab.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Apilab.Application.Commands
{
    [ExcludeFromCodeCoverage]
    public class ClienteUpdateCommand
    {
        public Guid Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;


        public static implicit operator Cliente(ClienteUpdateCommand command)
        {
            return new Cliente
            {
                Id = command.Id,
                Nome = command.Nome,
                Email = command.Email
            };
        }
    }
}
