using ControladorPagamento.Entities;
using ControladorPagamento.Entities.Repositories;
using ControladorPagamento.Entities.Shared;
using ControladorPagamento.Entities.Exceptions;
using ControladorPagamento.Presenters;
using System.Net.Http.Headers;
using ControladorPagamento.Contracts;


namespace ControladorPagamento.Application.UseCases;

public class PagamentoUseCase(ILogger<PagamentoUseCase> logger,
    IPagamentoRepository pagamentoRepository, IConfiguration configuration, HttpClient httpClient) : IPagamentoUseCase
{
    public async Task EfetuarMercadoPagoQRCodeAsync(Pedido pedido)
    {
        logger.LogInformation("Efetuando pagamento do pedido {PedidoId}", pedido.Id);
        try
        {
            string? pagamentoUrl = configuration.GetValue<string>("PagamentoUrl");
            PagamentoDto pagamentoDto = new(pedido.Id, pedido.ValorTotal, pedido.ClienteId);
            await httpClient.PostAsJsonAsync(pagamentoUrl, pagamentoDto);

            logger.LogInformation("Pagamento do pedido {PedidoId} solicitado com sucesso", pedido.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao efetuar pagamento do pedido {PedidoId}", pedido.Id);
            throw;
        }
    }

    public async Task<Pagamento> ConsultarPagamentoPeloPedido(Guid pedidoId)
    {
        logger.LogInformation("Consultando status do pagamento do pedido");
        try
        {
            var result = await pagamentoRepository.GetByPedidoId(pedidoId) ?? throw new NotFoundException("Pagamento não encontrado.");
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao consultar status de pagamento do pedido {PedidoId}", pedidoId);
            throw;
        }
    }

    public async Task<Guid?> ConcluirPagamento(Pedido pedido, bool aprovado, string? motivo)
    {
        logger.LogInformation("Concluindo pagamento do pedido {PedidoId}", pedido.Id);
        try
        {
            if (!aprovado)
            {
                logger.LogWarning("Pagamento do pedido {PedidoId} não foi aprovado, motivo: {motivo}", pedido.Id, motivo);
                return null;
            }
            else
            {
                Pagamento pagamento = pedido.GerarPagamento(MetodoPagamento.MercadoPagoQRCode);

                await pagamentoRepository.Add(pagamento);
                logger.LogInformation("Pagamento do pedido {PedidoId} concluído com sucesso", pedido.Id);
                return pagamento.Id;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao concluir pagamento do pedido {PedidoId}", pedido.Id);
            throw;
        }
    }

    public async Task<Pedido?> ObterPedidoPorIdAsync(Guid pedidoId, string? token)
    {
        logger.LogWarning("Consultando serviço externo de pedido");

        // Configure o cabeçalho de autorização com o token Bearer
        if (!string.IsNullOrWhiteSpace(token))
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Replace("Bearer", ""));
        }
        string? pedidoUrl = configuration.GetValue<string>("PedidoUrl");

        try
        {
            Pedido? pedido = await httpClient.GetFromJsonAsync<Pedido>($"{pedidoUrl}{pedidoId}");

            return pedido;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao consultar o serviço de pedido", pedidoId);
            throw;
        }
    }

    public async Task<bool> ObterStatusDoPedidoAsync(Guid pedidoId)
    {
        logger.LogWarning("Consultando status do pedido {PedidoId}", pedidoId);
        try
        {
            Pagamento? statusPedido = await pagamentoRepository.GetByPedidoId(pedidoId);

            bool pagamentoAprovado = statusPedido?.Status == Status.Recebido;

            return pagamentoAprovado;
        }
        catch (Exception e)
        {
            logger.LogError(e, "Erro ao consultar o status do pedido", pedidoId);
            throw;
        }

    }
}
