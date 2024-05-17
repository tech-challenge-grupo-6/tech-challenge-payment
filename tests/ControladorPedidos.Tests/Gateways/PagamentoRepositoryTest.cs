
using ControladorPedidos.App.Entities;
using ControladorPedidos.App.Entities.Repositories;
using ControladorPedidos.App.Entities.Shared;
using ControladorPedidos.App.Gateways;
using ControladorPedidos.App.Infrastructure.DataBase;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ControladorPedidos.Tests;
public class PagamentoRepositoryTest
{
    private static DatabaseContext CreateUniqContext()
    {
        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new DatabaseContext(options);
    }

    // [Fact]
    // public async Task AdicionarPagamento()
    // {
    //     var pagamento = new Pagamento { Id = Guid.NewGuid(), PedidoId = Guid.NewGuid(), Valor = 50, MetodoPagamento = MetodoPagamento.MercadoPagoQRCode, Status = Status.Recebido };

    //     using var context = CreateUniqContext();
    //     var repository = new PagamentoRepository(context);

    //     await repository.Add(pagamento);

    //     context.Pagamentos.Should().Contain(pagamento);

    // }

}