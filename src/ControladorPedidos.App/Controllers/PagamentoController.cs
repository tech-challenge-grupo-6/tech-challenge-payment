using ControladorPedidos.App.Contracts;
using ControladorPedidos.App.Entities.Exceptions;
using ControladorPedidos.App.Presenters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControladorPedidos.App.Controllers;

[ApiController]
[Route("[controller]")]
public class PagamentoController(IPagamentoUseCase pagamentoUseCase, ILogger<PagamentoController> logger) : ControllerBase
{
    /// <summary>
    /// Realiza pagamento do pedido
    /// </summary>
    /// <param name="pedidoId">Id do pedido</param>
    /// <response code="201">Pagamento do pedido realizado com sucesso.</response>
    /// <response code="400">Erro ao fazer a Request.</response>
    [Authorize]
    [HttpPut("pagar/{pedidoId}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Put(Guid pedidoId)
    {
        try
        {
            logger.LogInformation("Efetuando pagamento do pedido {PedidoId}", pedidoId);
            await pagamentoUseCase.EfetuarMercadoPagoQRCodeAsync(pedidoId);
            return CreatedAtAction(nameof(Put), new { message = "Pedido de pagamento efetuado com sucesso" });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao efetuar pagamento do pedido {PedidoId}", pedidoId);
            return BadRequest($"Erro ao efetuar pagamento do pedido {pedidoId}");
        }
    }

    /// <summary>
    /// Verifica o status do pagamento do pedido
    /// </summary>
    /// <param name="pedidoId">Id do pedido</param>
    /// <response code="200">Retorna se pagamento foi aprovado ou não.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="404">Não encontrado.</response>
    /// <response code="500">Erro interno.</response>
    [Authorize]
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
    [Authorize]
    [HttpPost("webhook")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Post([FromBody] PagamentoWebhookDto pagamentoWebhookDto)
    {
        var (pedidoId, aprovado, motivo) = pagamentoWebhookDto;
        try
        {

            logger.LogInformation("Concluindo pagamento do pedido {PedidoId}", pedidoId);
            Guid? pagamentoId = await pagamentoUseCase.ConcluirPagamento(pedidoId, aprovado, motivo);
            if (pagamentoId is null)
            {
                return BadRequest($"Pagamento do pedido {pedidoId} não foi aprovado. Motivo: {motivo}");
            }
            return CreatedAtAction(nameof(Post), new { id = pagamentoId });
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao efetuar pagamento do pedido {PedidoId}", pedidoId);
            return BadRequest($"Erro ao efetuar pagamento do pedido {pedidoId}");
        }
    }
}