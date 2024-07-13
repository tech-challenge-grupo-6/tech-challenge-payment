using ControladorPagamento.Application.Commands;
using ControladorPagamento.Contracts;
using MediatR;

namespace ControladorPagamento.Application.Handlers;

public class PagarCommandHandler : IRequestHandler<PagarCommand, bool>
{
    private readonly IPagamentoUseCase _pagamentoUseCase;
    private readonly ILogger<PagarCommandHandler> _logger;

    public PagarCommandHandler(IPagamentoUseCase pagamentoUseCase, ILogger<PagarCommandHandler> logger)
    {
        _pagamentoUseCase = pagamentoUseCase;
        _logger = logger;
    }
    public async Task<bool> Handle(PagarCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Efetuando pagamento do pedido {PedidoId}", request.Pedido);
            await _pagamentoUseCase.EfetuarMercadoPagoQRCodeAsync(request.Pedido);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao efetuar pagamento do pedido {PedidoId}", request.Pedido.Id);
            return false;
        }
    }
}
