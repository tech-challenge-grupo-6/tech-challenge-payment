using ControladorPagamento.Entities;

namespace ControladorPagamento.Presenters;

public record class PagamentoWebhookDto(Pedido Pedido, bool Aprovado, string? Motivo);
