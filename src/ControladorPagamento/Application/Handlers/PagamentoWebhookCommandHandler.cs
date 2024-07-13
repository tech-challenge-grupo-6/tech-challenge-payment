using ControladorPagamento.Application.Commands;
using ControladorPagamento.Contracts;
using MediatR;

namespace ControladorPagamento.Application.Handlers;

public class PagamentoWebhookCommandHandler : IRequestHandler<PagamentoWebhookCommand, bool>
{
    private readonly IPagamentoUseCase _pagamentoUseCase;
    private readonly ILogger<PagamentoWebhookCommandHandler> _logger;
    public PagamentoWebhookCommandHandler(IPagamentoUseCase pagamentoUseCase, ILogger<PagamentoWebhookCommandHandler> logger)
    {
        _pagamentoUseCase = pagamentoUseCase;
        _logger = logger;

    }
    public async Task<bool> Handle(PagamentoWebhookCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Concluindo pagamento do pedido {PedidoId}", request.Pedido.Id);
            Guid? pagamentoId = await _pagamentoUseCase.ConcluirPagamento(request.Pedido, request.Aprovado, request.Motivo);
            
            if (pagamentoId is null) 
                return false;

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao efetuar pagamento do pedido {PedidoId}", request.Pedido.Id);
            return false; 
        }
    }  
}
