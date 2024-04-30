using ControladorPedidos.App.Entities;
using ControladorPedidos.App.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ControladorPedidos.App.Infrastructure.DataBase;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
        ChangeTracker.StateChanged += UpdateBaseEntity;
        ChangeTracker.Tracked += UpdateBaseEntity;
    }

    public DbSet<Usuario> Usuarios { get; set; } = null!;
    public DbSet<Pagamento> Pagamentos { get; set; } = null!;

    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
    }

    private void UpdateBaseEntity(object? sender, EntityEntryEventArgs e)
    {
        if (e.Entry.Entity is EntityBase baseEntity)
        {
            if (e.Entry.State == EntityState.Added)
            {
                baseEntity.CriadoEm = DateTime.UtcNow;
                baseEntity.AtualizadoEm ??= null!;
            }
            else if (e.Entry.State == EntityState.Modified)
            {
                baseEntity.AtualizadoEm = DateTime.UtcNow;
            }
            else
            {
                //Não fazer nada
            }
        }
    }
}
