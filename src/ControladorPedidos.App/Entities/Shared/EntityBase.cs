namespace ControladorPedidos.App.Entities.Shared;

public abstract class EntityBase
{
    public Guid Id { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime? AtualizadoEm { get; set; }
}
