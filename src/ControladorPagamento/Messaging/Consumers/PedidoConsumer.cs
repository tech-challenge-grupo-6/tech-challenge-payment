using AutoMapper;
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

                    await _messageSender.SendMessageAsync(message, "pedido-atualizado");
                }
            }
        }
        catch (Exception)
        {
            _logger.LogError("Erro ao receber a mensagem.");
            throw;
        }

    }
}
