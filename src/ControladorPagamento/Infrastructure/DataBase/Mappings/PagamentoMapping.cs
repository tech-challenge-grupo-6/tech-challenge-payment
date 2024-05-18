using ControladorPagamento.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControladorPagamento.Infrastructure.DataBase.Mappings;

public class PagamentoMapping : IEntityTypeConfiguration<Pagamento>
{
    public void Configure(EntityTypeBuilder<Pagamento> builder)
    {
        builder.ToTable("Pagamento");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id).IsRequired();
        builder.Property(p => p.PedidoId).IsRequired();
        builder.Property(p => p.Valor).IsRequired();
        builder.Property(p => p.MetodoPagamento).IsRequired();
        builder.Property(p => p.CriadoEm).IsRequired();
        builder.Property(p => p.AtualizadoEm).IsRequired(false);
        builder.Property(p => p.Status).IsRequired();
    }
}
