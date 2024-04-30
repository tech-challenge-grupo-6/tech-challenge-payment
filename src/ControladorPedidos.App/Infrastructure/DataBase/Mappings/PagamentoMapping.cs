using ControladorPedidos.App.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControladorPedidos.App.Infrastructure.DataBase.Mappings;

public class PagamentoMapping : IEntityTypeConfiguration<Pagamento>
{
    public void Configure(EntityTypeBuilder<Pagamento> builder)
    {
        builder.ToTable("Pagamento");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).IsRequired();
        builder.Property(p => p.PedidoId).IsRequired();
        builder.HasOne(p => p.Pedido).WithOne(p => p.Pagamento).HasForeignKey<Pagamento>(p => p.PedidoId);
        builder.Property(p => p.Valor).IsRequired();
        builder.Property(p => p.MetodoPagamento).IsRequired();
        builder.Property(p => p.CriadoEm).IsRequired();
        builder.Property(p => p.AtualizadoEm).IsRequired(false);
    }
}
