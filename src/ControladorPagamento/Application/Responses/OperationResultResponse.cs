namespace ControladorPagamento.Application.Responses;

public class OperationResultResponse
{
    public string Status { get; set; }
    public Guid PedidoId { get; set; }
    public string? Motivo { get; set; }
}
