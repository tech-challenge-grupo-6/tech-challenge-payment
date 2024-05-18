namespace ControladorPagamento.Presenters;

public record class PagamentoWebhookDto(Guid PedidoId, bool Aprovado, string? Motivo);
