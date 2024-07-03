using ControladorPagamento.Application.Commands;
using ControladorPagamento.Contracts;
using ControladorPagamento.Entities.Exceptions;
using ControladorPagamento.Presenters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControladorPagamento.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class PagamentoController(IPagamentoUseCase pagamentoUseCase, ILogger<PagamentoController> logger, IMediator _mediator) : ControllerBase
{

    private bool statusDoPedido = false;

    /// <summary>
    /// Realiza pagamento do pedido
    /// </summary>
    /// <param name="pedidoId">Id do pedido</param>
    /// <response code="201">Pagamento do pedido realizado com sucesso.</response>
    /// <response code="400">Erro ao fazer a Request.</response>
    [HttpPut("pagar/{pedidoId}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Put(Guid pedidoId)
    { 
        //var result = await _mediator.Send(new PagarCommand { Pedido = pedido });

        //if (result.Status == "201")
        //{
        //    CreatedAtAction(nameof(Put), $"Efetuando pagamento do pedido { result.PedidoId }");
        //}
        //return BadRequest($"Erro ao efetuar pagamento do pedido { result.PedidoId }");
        throw new NotImplementedException();
    }

    /// <summary>
    /// Verifica o status do pagamento do pedido
    /// </summary>
    /// <param name="pedidoId">Id do pedido</param>
    /// <response code="200">Retorna se pagamento foi aprovado ou não.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="404">Não encontrado.</response>
    /// <response code="500">Erro interno.</response>
    [HttpGet("check/{pedidoId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get(Guid pedidoId)
    {
        try
        {
            logger.LogInformation("Consultando status pagamento do pedido {PedidoId}", pedidoId);
            var pagamento = await pagamentoUseCase.ConsultarPagamentoPeloPedido(pedidoId);
            var successResponse = new { status = "Aprovado" };
            return Ok(successResponse);
        }
        catch (NotFoundException e)
        {
            logger.LogError(e.Message);
            var errorResponse = new { status = "Não Aprovado" };
            return NotFound(errorResponse);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao tentar consultar o status de pagamento do pedido {PedidoId}", pedidoId);
            return BadRequest($"Erro ao tentar consultar o status de pagamento do pedido {pedidoId}");
        }
    }

    /// <summary>
    /// Realiza conclusão do pagamento do pedido
    /// </summary>
    /// <param name="pagamentoWebhookDto">Dados do pagamento</param>
    /// <response code="201">Pagamento do pedido realizado com sucesso.</response>
    /// <response code="400">Erro ao fazer a Request.</response>
    [HttpPost("webhook")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] PagamentoWebhookDto pagamentoWebhookDto)
    {
        //var result = await _mediator.Send(new PagamentoWebhookCommand 
        //{ PedidoId = pagamentoWebhookDto.PedidoId, Aprovado = pagamentoWebhookDto.Aprovado, Motivo = pagamentoWebhookDto.Motivo });
        
        //if (result.Status == "200")
        //{
        //    return Ok(result.PedidoId);
        //}
        //return BadRequest(result.PedidoId);
        throw new NotImplementedException();
    }

    /// <summary>
    /// Verifica o status do pagamento do pedido
    /// </summary>
    /// <param name="pedidoId">Id do pedido</param>
    /// <response code="200">Retorna se pagamento foi aprovado ou não.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="404">Não encontrado.</response>
    /// <response code="500">Erro interno.</response>
    [HttpGet("status/{pedidoId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetStatus(Guid pedidoId)
    {
        try
        {
            logger.LogInformation("Consultando status do pedido {PedidoId}", pedidoId);
            statusDoPedido = await pagamentoUseCase.ObterStatusDoPedidoAsync(pedidoId);
            var successResponse = new { id = pedidoId, status = statusDoPedido };
            return Ok(successResponse);
        }
        catch (NotFoundException e)
        {
            logger.LogError(e.Message);
            var errorResponse = new { id = pedidoId, status = statusDoPedido };
            return NotFound(errorResponse);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao tentar consultar o status de pagamento do pedido {PedidoId}", pedidoId);
            return BadRequest($"Erro ao tentar consultar o status de pagamento do pedido {pedidoId}");
        }
    }
}