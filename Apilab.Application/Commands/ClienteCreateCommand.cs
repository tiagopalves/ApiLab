using Apilab.Application.Commands.Interfaces;
using ApiLab.Domain.Entities;

namespace Apilab.Application.Commands
{
    public class ClienteCreateCommand : IClienteCommand
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
