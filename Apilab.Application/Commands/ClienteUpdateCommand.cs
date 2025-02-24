using Apilab.Application.Commands.Interfaces;
using ApiLab.Domain.Entities;

namespace Apilab.Application.Commands
{
    public class ClienteUpdateCommand : IClienteCommand
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
