using ApiLab.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Apilab.Application.Commands
{
    [ExcludeFromCodeCoverage]
    public class ClienteCreateCommand
    {
        public string Nome { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        
        public static implicit operator Cliente(ClienteCreateCommand command)
        {
            return new Cliente
            {
                Nome = command.Nome,
                Email = command.Email
            };
        }
    }
}
