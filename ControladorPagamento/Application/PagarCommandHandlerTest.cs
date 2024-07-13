using ControladorPagamento.Application.Commands;
using ControladorPagamento.Application.Handlers;
using ControladorPagamento.Contracts;
using ControladorPagamento.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace ControladorPagamentoTest.Application;

public class PagarCommandHandlerTest
{

    public Mock<IPagamentoUseCase> _pagamentoUseCase;
    public Mock<ILogger<PagarCommandHandler>> _logger;

    public PagarCommandHandlerTest()
    {
        _logger = new Mock<ILogger<PagarCommandHandler>>();
        _pagamentoUseCase = new Mock<IPagamentoUseCase>();
    }


    [Fact]
    public async Task RetornaTrueQuandoPagoComSucesso()
    {
        // Arrange
        var _handler = new PagarCommandHandler(_pagamentoUseCase.Object, _logger.Object);
        var _command = new PagarCommand { Pedido = It.IsAny<Pedido>() };

        // Act
        _pagamentoUseCase.Setup(x => x.EfetuarMercadoPagoQRCodeAsync(It.IsAny<Pedido>()))
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(_command, CancellationToken.None);

        // Assert
        Assert.True(result);

    }
}