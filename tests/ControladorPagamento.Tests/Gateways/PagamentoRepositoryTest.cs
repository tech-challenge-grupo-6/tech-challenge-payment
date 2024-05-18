
using ControladorPagamento.Entities;
using ControladorPagamento.Entities.Repositories;
using ControladorPagamento.Entities.Shared;
using ControladorPagamento.Gateways;
using ControladorPagamento.Infrastructure.DataBase;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ControladorPagamento.Tests;
public class PagamentoRepositoryTest
{
    private static DatabaseContext CreateUniqContext()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new DatabaseContext(options);
    }

    [Fact]
    public async Task AdicionarPagamento()
    {
        var pagamento = new Pagamento { Id = Guid.NewGuid(), PedidoId = Guid.NewGuid(), Valor = 50, MetodoPagamento = MetodoPagamento.MercadoPagoQRCode, Status = Status.Recebido };

        using var context = CreateUniqContext();
        var repository = new PagamentoRepository(context);

        await repository.Add(pagamento);

        context.Pagamentos.Should().Contain(pagamento);

    }

}