namespace ControladorPagamento.Presenters;

public record class PagamentoDto(Guid PedidoId, double Valor, Guid? ClienteId);
