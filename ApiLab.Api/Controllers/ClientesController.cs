using Apilab.Application.AppServices.Interfaces;
using Apilab.Application.Commands;
using ApiLab.CrossCutting.Common.Constants;
using ApiLab.CrossCutting.LogManager.Interfaces;
using ApiLab.CrossCutting.Resources;
using ApiLab.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ApiLab.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ClientesController(ILogManager logManager, IClienteService clienteService) : ControllerBase
    {
        private readonly ILogManager _logManager = logManager;
        private readonly IClienteService _clienteService = clienteService;

        //TODO: Padronizar retornos
        //TODO: Centralizar tratamento de erros na ApiControllerBase

        [HttpPost]
        [ProducesResponseType(typeof(ClienteCreateCommand), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Results<Created<Guid>, BadRequest<ProblemDetails>>> CreateAsync([FromBody] ClienteCreateCommand cliente)
        {
            try
            {
                var retorno = await _clienteService.CreateAsync(cliente);

                bool isValid = Guid.TryParse(retorno, out Guid id);
                
                if (isValid && id != Guid.Empty)
                    return TypedResults.Created($"{Constants.CLIENTES_ENDPOINT}{id}", id);
                else
                    return TypedResults.BadRequest(new ProblemDetails
                    {
                        Title = FriendlyMessages.ErrorTitlePayload,
                        Detail = $"{retorno}",
                        Type = FriendlyMessages.ProblemDetailsBadRequest
                    });
            }
            catch (Exception ex)
            {
                _logManager.AddError(CrossCutting.Issuer.Issues.ControllerError_4001, $"{FriendlyMessages.ErrorEndpoint} {nameof(CreateAsync)}!", ex);

                return TypedResults.BadRequest(new ProblemDetails
                {
                    Title = FriendlyMessages.ErrorTitle,
                    Detail = $"{FriendlyMessages.ErrorEndpoint} {nameof(CreateAsync)}!",
                    Type = FriendlyMessages.ProblemDetailsBadRequest
                });
            }
        }

        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(Cliente), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Results<Ok<Cliente>, NotFound, BadRequest<ProblemDetails>>> GetByIdAsync(Guid id)
        {
            try
            {
                var retorno = await _clienteService.GetByIdAsync(id);

                if (retorno is null)
                    return TypedResults.NotFound();

                return TypedResults.Ok(retorno);
            }
            catch (Exception ex)
            {
                _logManager.AddError(CrossCutting.Issuer.Issues.ControllerError_4002, $"{FriendlyMessages.ErrorEndpoint} {nameof(GetAllAsync)}!", ex);

                return TypedResults.BadRequest(new ProblemDetails
                {
                    Title = FriendlyMessages.ErrorTitle,
                    Detail = $"{FriendlyMessages.ErrorEndpoint} {nameof(GetByIdAsync)}!",
                    Type = FriendlyMessages.ProblemDetailsBadRequest
                });
            }
        }

        [HttpGet("email/{email}")]
        [ProducesResponseType(typeof(Cliente), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Results<Ok<Cliente>, NotFound, BadRequest<ProblemDetails>>> GetByEmailAsync(string email)
        {
            try
            {
                var retorno = await _clienteService.GetByEmailAsync(email);

                if (retorno is null)
                    return TypedResults.NotFound();

                return TypedResults.Ok(retorno);
            }
            catch (Exception ex)
            {
                _logManager.AddError(CrossCutting.Issuer.Issues.ControllerError_4002, $"{FriendlyMessages.ErrorEndpoint} {nameof(GetByEmailAsync)}!", ex);

                return TypedResults.BadRequest(new ProblemDetails
                {
                    Title = FriendlyMessages.ErrorTitle,
                    Detail = $"{FriendlyMessages.ErrorEndpoint} {nameof(GetByEmailAsync)}!",
                    Type = FriendlyMessages.ProblemDetailsBadRequest
                });
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Cliente>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Results<Ok<List<Cliente>>, NoContent, BadRequest<ProblemDetails>>> GetAllAsync()
        {
            try
            {
                var retorno = await _clienteService.GetAllAsync();
                
                if (retorno.Count != 0)
                    return TypedResults.Ok(retorno);
                else
                    return TypedResults.NoContent();
            }
            catch (Exception ex)
            {
                _logManager.AddError(CrossCutting.Issuer.Issues.ControllerError_4004, $"{FriendlyMessages.ErrorEndpoint} {nameof(GetAllAsync)}!", ex);

                return TypedResults.BadRequest(new ProblemDetails
                {
                    Title = FriendlyMessages.ErrorTitle,
                    Detail = $"{FriendlyMessages.ErrorEndpoint} {nameof(GetAllAsync)}!",
                    Type = FriendlyMessages.ProblemDetailsBadRequest
                });
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(ClienteUpdateCommand), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Results<Ok<ClienteUpdateCommand>, NotFound, BadRequest<ProblemDetails>>> UpdateAsync([FromBody] ClienteUpdateCommand cliente)
        {
            try
            {
                var retorno = await _clienteService.UpdateAsync(cliente);

                if (retorno is null)
                    return TypedResults.NotFound();
                else if (retorno != string.Empty)
                    return TypedResults.BadRequest(new ProblemDetails
                    {
                        Title = FriendlyMessages.ErrorTitlePayload,
                        Detail = $"{retorno}",
                        Type = FriendlyMessages.ProblemDetailsBadRequest
                    });
                else
                    return TypedResults.Ok(cliente);
            }
            catch (Exception ex)
            {
                _logManager.AddError(CrossCutting.Issuer.Issues.ControllerError_4005, $"{FriendlyMessages.ErrorEndpoint} {nameof(UpdateAsync)}!", ex);

                return TypedResults.BadRequest(new ProblemDetails
                {
                    Title = FriendlyMessages.ErrorTitle,
                    Detail = $"{FriendlyMessages.ErrorEndpoint} {nameof(UpdateAsync)}!",
                    Type = FriendlyMessages.ProblemDetailsBadRequest
                });
            }
        }

        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<Results<Ok, NotFound, BadRequest<ProblemDetails>>> DeleteAsync(Guid id)
        {
            try
            {
                var retorno = await _clienteService.DeleteAsync(id);

                if (!retorno)
                    return TypedResults.NotFound();

                return TypedResults.Ok();
            }
            catch (Exception ex)
            {
                _logManager.AddError(CrossCutting.Issuer.Issues.ControllerError_4006, $"{FriendlyMessages.ErrorEndpoint} {nameof(DeleteAsync)}!", ex);

                return TypedResults.BadRequest(new ProblemDetails
                {
                    Title = FriendlyMessages.ErrorTitle,
                    Detail = $"{FriendlyMessages.ErrorEndpoint} {nameof(DeleteAsync)}!",
                    Type = FriendlyMessages.ProblemDetailsBadRequest
                });
            }
        }
    }
}
