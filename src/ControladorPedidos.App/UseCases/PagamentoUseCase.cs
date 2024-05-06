﻿using ControladorPedidos.App.Contracts;
using ControladorPedidos.App.Entities;
using ControladorPedidos.App.Entities.Repositories;
using ControladorPedidos.App.Entities.Shared;
using ControladorPedidos.App.Entities.Exceptions;
using ControladorPedidos.App.Presenters;
using System.Net.Http.Headers;

namespace ControladorPedidos.App.UseCases;

public class PagamentoUseCase(IPedidoRepository pedidoRepository, ILogger<PagamentoUseCase> logger, 
    IPagamentoRepository pagamentoRepository, IConfiguration configuration, HttpClient httpClient) : IPagamentoUseCase
{
    public async Task EfetuarMercadoPagoQRCodeAsync(Guid pedidoId, string? token)
    {
        logger.LogInformation("Efetuando pagamento do pedido {PedidoId}", pedidoId);
        try
        {
            Pedido? pedido = await ObterPedidoPorIdAsync(pedidoId, token);

            if (pedido is null)
            {
                logger.LogError("Pedido {PedidoId} não encontrado", pedidoId);
                throw new Exception($"Pedido {pedidoId} não encontrado");
            }

            if (pedido.Status != Status.Criado)
            {
                logger.LogError("Pedido {PedidoId} não pode ser pago", pedidoId);
                throw new Exception($"Pedido {pedidoId} não pode ser pago");
            }

            string? pagamentoUrl = configuration.GetValue<string>("PagamentoUrl");
            PagamentoDto pagamentoDto = new(pedidoId, pedido.ValorTotal, pedido.ClienteId);
            await httpClient.PostAsJsonAsync(pagamentoUrl, pagamentoDto);

            logger.LogInformation("Pagamento do pedido {PedidoId} solicitado com sucesso", pedidoId);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao efetuar pagamento do pedido {PedidoId}", pedidoId);
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

    public async Task<Guid?> ConcluirPagamento(Guid pedidoId, bool aprovado, string? motivo, string? token)
    {
        logger.LogInformation("Concluindo pagamento do pedido {PedidoId}", pedidoId);
        try
        {
            if (!aprovado)
            {
                logger.LogWarning("Pagamento do pedido {PedidoId} não foi aprovado, motivo: {motivo}", pedidoId, motivo);
                return null;
            }
            else
            {
                var pedido = await ObterPedidoPorIdAsync(pedidoId, token) ?? throw new NotFoundException("Pedido não encontrado.");
                Pagamento pagamento = pedido.GerarPagamento(MetodoPagamento.MercadoPagoQRCode);
                await pagamentoRepository.Add(pagamento);
                await pedidoRepository.UpdateStatus(pedido);
                logger.LogInformation("Pagamento do pedido {PedidoId} concluído com sucesso", pedidoId);
                return pagamento.Id;
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao concluir pagamento do pedido {PedidoId}", pedidoId);
            throw;
        }
    }

    public async Task<Pedido?> ObterPedidoPorIdAsync(Guid pedidoId, string? token)
    {
        logger.LogWarning("Consultando serviço externo de pedido");

        // Configure o cabeçalho de autorização com o token Bearer
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        string? pedidoUrl = configuration.GetValue<string>("PedidoUrl");

        try
        {
            Pedido[]? pedidos = await httpClient.GetFromJsonAsync<Pedido[]>(pedidoUrl);
            IEnumerable<Pedido> pedido = pedidos.Where(x => x.Id == pedidoId);

            return pedido.Any() ? pedido.First() : null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao consultar o serviço de pedido", pedidoId);
            throw;
        }
    }
}
