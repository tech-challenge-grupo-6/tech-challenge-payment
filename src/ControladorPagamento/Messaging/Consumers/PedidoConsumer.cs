﻿using AutoMapper;
using ControladorPagamento.Application.Commands;
using ControladorPagamento.Entities;
using ControladorPagamento.Messaging.Messages;
using ControladorPagamento.Messaging.Producers;
using MassTransit;
using MediatR;

namespace ControladorPagamento.Messaging.Consumers;

public class PedidoConsumer : IConsumer<PedidoMessage>
{
	private readonly ILogger<PedidoConsumer> _logger;
	private readonly IMediator _mediator;
    private readonly IMessageSender _messageSender;
    private readonly IMapper _mapper;

    public PedidoConsumer(ILogger<PedidoConsumer> logger, IMediator mediator, IMessageSender messageSender, IMapper mapper)
    {
        _logger = logger;
		_mediator = mediator;
        _messageSender = messageSender;
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<PedidoMessage> context)
    {
		_logger.LogInformation("Recebendo mensagem.");
		try
		{
            Pedido pedido = _mapper.Map<Pedido>(context.Message);

            var result = await _mediator.Send(new PagarCommand { Pedido = pedido });

            if (result)
            {
                var resultPagamento = await _mediator.Send(new PagamentoWebhookCommand { Pedido = pedido, Aprovado = true });

                if (resultPagamento)
                {
                    _logger.LogInformation("Iniciando conclusão de pagamento.");

                    var message = new PedidoMessage()
                    {
                        Id = context.Message.Id,
                        Status = context.Message.Status,
                        ClienteId = context.Message.ClienteId,
                        ValorTotal = context.Message.ValorTotal,
                        Pago = true,
                        Produtos = context.Message.Produtos,
                    };

                    var messagePagamento = new PagamentoMessage()
                    {
                        IdPedido = context.Message.Id,
                        Status = true
                    };

                    await _messageSender.SendMessageAsync(message, "pedido-atualizado");
                    await _messageSender.SendMessageAsync(messagePagamento, "pagamento-status");
                }
                else await ErroAoProcessarPedidoAsync(context);
            }
            else await ErroAoProcessarPedidoAsync(context);
        
        }
        catch (Exception)
        {
            _logger.LogError("Erro ao receber a mensagem."); 
            throw;
        }


    }
    private async Task ErroAoProcessarPedidoAsync(ConsumeContext<PedidoMessage> context)
    {
        _logger.LogError("Enviando mensagem de erro ao processar o pagamento do pedido.");

        await _messageSender.SendMessageAsync(context.Message, "pedido-atualizado");
        await _messageSender.SendMessageAsync(new PagamentoMessage { IdPedido = context.Message.Id, Status = false }, "pagamento-status");
    }
}
