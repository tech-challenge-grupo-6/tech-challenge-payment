using ControladorPedidos.App.Contracts;
using ControladorPedidos.App.Entities;
using ControladorPedidos.App.Entities.Repositories;
using ControladorPedidos.App.Entities.Shared;
using ControladorPedidos.App.UseCases;
using Microsoft.Extensions.Logging;
using NSubstitute;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace ControladorPedidos.Tests;
public class PagamentoUseCaseTest
{
    private readonly IPagamentoRepository _pagamentoRepository;
    private readonly PagamentoUseCase _pagamentoUseCase;
    private readonly ILogger<PagamentoUseCase> _logger;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private readonly IPagamentoUseCase _teste;

    public PagamentoUseCaseTest()
    {
        _pagamentoRepository = Substitute.For<IPagamentoRepository>();
        _logger = Substitute.For<ILogger<PagamentoUseCase>>();
        _configuration = Substitute.For<IConfiguration>();
        _httpClient = Substitute.For<HttpClient>();
        _teste = Substitute.For<IPagamentoUseCase>();

        _pagamentoUseCase = new PagamentoUseCase(_logger, _pagamentoRepository, _configuration, _httpClient);
    }

    [Theory]
    [InlineData(Status.Recebido, true)]
    [InlineData(Status.Criado, false)]
    public async Task ObterStatusDoPedidoAsync_RetornaStatus(Status status, bool retornoEsperado)
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var pagamento = new Pagamento { Status = status };
        _pagamentoRepository.GetByPedidoId(pedidoId).Returns(pagamento);

        // Act
        var result = await _pagamentoUseCase.ObterStatusDoPedidoAsync(pedidoId);

        // Assert
        Assert.Equal(retornoEsperado, result);
    }


    [Theory]
    [InlineData(false, "Motivo de teste", false)]
    public async Task ConcluirPagamento_DeveRetornarIdDoPagamentoSeAprovado(bool aprovado, string motivo, bool temId)
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var token = "Bearer teste";
        Pedido pedidoEsperado = new Pedido { Id = pedidoId, Status = Status.Criado };
        _teste.ObterPedidoPorIdAsync(pedidoId, token).Returns(pedidoEsperado);


        // Act
        var result = await _pagamentoUseCase.ConcluirPagamento(pedidoId, aprovado, motivo, token);

        // Assert
        if (temId)
        {
            Assert.NotNull(result);
        }
        else
        {
            Assert.Null(result);
        }
    }

    [Fact]
    public async Task ConsultarPagamentoPeloPedido_DeveRetornarPagamento()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        Pagamento pagamentoEsperado = new Pagamento { PedidoId = pedidoId, Status = Status.Criado };
        _pagamentoRepository.GetByPedidoId(pedidoId).Returns(pagamentoEsperado);

        // Act
        var result = await _pagamentoUseCase.ConsultarPagamentoPeloPedido(pedidoId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(pedidoId, result.PedidoId);
    }
}

