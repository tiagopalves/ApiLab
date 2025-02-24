using Apilab.Application.AppServices.Interfaces;
using Apilab.Application.Commands;
using ApiLab.CrossCutting.Common.Constants;
using ApiLab.CrossCutting.Issuer;
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
        //TODO: Verificar a cobertura de testes com o Coverlet

        private readonly ILogManager _logManager = logManager;
        private readonly IClienteService _clienteService = clienteService;

        [HttpPost]
        [ProducesResponseType(typeof(ClienteCreateCommand), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<Results<Created<Guid>, BadRequest<ProblemDetails>>> CreateAsync([FromBody] ClienteCreateCommand cliente, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _clienteService.CreateAsync(cliente, cancellationToken);

                bool isValid = Guid.TryParse(result, out Guid id);

                if (isValid && id != Guid.Empty)
                    return TypedResults.Created($"{Constants.CLIENTES_ENDPOINT}{id}", id);
                else
                {
                    _logManager.AddWarning(Issues.ControllerWarning_2001, FriendlyMessages.ErrorTitlePayload, informationData: result);

                    return TypedResults.BadRequest(new ProblemDetails
                    {
                        Title = FriendlyMessages.ErrorTitlePayload,
                        Detail = $"{result}",
                        Type = FriendlyMessages.ProblemDetailsBadRequest
                    });
                }
            }
            catch (Exception ex)
            {
                _logManager.AddError(Issues.ControllerError_4001, $"{FriendlyMessages.ErrorEndpoint} {nameof(CreateAsync)}!", ex);

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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<Results<Ok<Cliente>, NotFound, BadRequest<ProblemDetails>>> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _clienteService.GetByIdAsync(id, cancellationToken);

                if (result is null)
                {
                    _logManager.AddWarning(Issues.ControllerWarning_2002, FriendlyMessages.ClienteNotFound);

                    return TypedResults.NotFound();
                }

                return TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                _logManager.AddError(Issues.ControllerError_4002, $"{FriendlyMessages.ErrorEndpoint} {nameof(GetAllAsync)}!", ex);

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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<Results<Ok<Cliente>, NotFound, BadRequest<ProblemDetails>>> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _clienteService.GetByEmailAsync(email, cancellationToken);

                if (result is null)
                {
                    _logManager.AddWarning(Issues.ControllerWarning_2003, FriendlyMessages.ClienteNotFound);

                    return TypedResults.NotFound();
                }

                return TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                _logManager.AddError(Issues.ControllerError_4003, $"{FriendlyMessages.ErrorEndpoint} {nameof(GetByEmailAsync)}!", ex);

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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<Results<Ok<List<Cliente>>, NoContent, BadRequest<ProblemDetails>>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _clienteService.GetAllAsync(cancellationToken);

                if (result.Count == 0)
                {
                    _logManager.AddWarning(Issues.ControllerWarning_2004, FriendlyMessages.ClientesNotFound);

                    return TypedResults.NoContent();
                }
                else
                    return TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                _logManager.AddError(Issues.ControllerError_4004, $"{FriendlyMessages.ErrorEndpoint} {nameof(GetAllAsync)}!", ex);

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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<Results<Ok<ClienteUpdateCommand>, NotFound, BadRequest<ProblemDetails>>> UpdateAsync([FromBody] ClienteUpdateCommand cliente, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _clienteService.UpdateAsync(cliente, cancellationToken);

                bool isValid = Guid.TryParse(result, out Guid id);

                if (isValid && id != Guid.Empty)
                    return TypedResults.Ok(cliente);
                else if (result is null)
                    return TypedResults.NotFound();
                else
                {
                    _logManager.AddWarning(Issues.ControllerWarning_2005, FriendlyMessages.ErrorTitlePayload, informationData: result);

                    return TypedResults.BadRequest(new ProblemDetails
                    {
                        Title = FriendlyMessages.ErrorTitlePayload,
                        Detail = $"{result}",
                        Type = FriendlyMessages.ProblemDetailsBadRequest
                    });
                }
            }
            catch (Exception ex)
            {
                _logManager.AddError(Issues.ControllerError_4005, $"{FriendlyMessages.ErrorEndpoint} {nameof(UpdateAsync)}!", ex);

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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<Results<Ok, NotFound, BadRequest<ProblemDetails>>> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _clienteService.DeleteAsync(id, cancellationToken);

                if (!result)
                {
                    _logManager.AddWarning(Issues.ControllerWarning_2006, FriendlyMessages.ClienteNotFound);

                    return TypedResults.NotFound();
                }

                return TypedResults.Ok();
            }
            catch (Exception ex)
            {
                _logManager.AddError(Issues.ControllerError_4006, $"{FriendlyMessages.ErrorEndpoint} {nameof(DeleteAsync)}!", ex);

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
