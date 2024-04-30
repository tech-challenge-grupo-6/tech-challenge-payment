namespace ControladorPedidos.App.Presenters;

public record class PagamentoWebhookDto(Guid PedidoId, bool Aprovado, string? Motivo);
