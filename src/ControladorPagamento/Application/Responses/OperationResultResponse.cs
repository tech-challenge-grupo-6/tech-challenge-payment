namespace ControladorPagamento.Application.Responses;

public class OperationResultResponse
{
    public string Status { get; set; } = string.Empty;
    public Guid PedidoId { get; set; }
    public string? Motivo { get; set; }
}
