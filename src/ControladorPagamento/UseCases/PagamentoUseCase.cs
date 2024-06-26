﻿using ControladorPagamento.Contracts;
using ControladorPagamento.Entities;
using ControladorPagamento.Entities.Repositories;
using ControladorPagamento.Entities.Shared;
using ControladorPagamento.Entities.Exceptions;
using ControladorPagamento.Presenters;
using System.Net.Http.Headers;

namespace ControladorPagamento.UseCases;

public class PagamentoUseCase(ILogger<PagamentoUseCase> logger,
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
