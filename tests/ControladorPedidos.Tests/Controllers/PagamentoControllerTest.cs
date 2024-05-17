using ControladorPedidos.App.Contracts;
using ControladorPedidos.App.Controllers;
using ControladorPedidos.App.Entities;
using ControladorPedidos.App.Presenters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace ControladorPedidos.Tests;
public class PagamentoControllerTest
{
    private readonly IPagamentoUseCase pagamentoUseCase;

    private readonly ILogger<PagamentoController> logger;

    public PagamentoControllerTest()
    {
        pagamentoUseCase = Substitute.For<IPagamentoUseCase>();
        logger = Substitute.For<ILogger<PagamentoController>>();
    }

    [Fact]
    public async Task Put_EfetuarPagamento_DeveRetornar201Created()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var token = "Bearer teste";
        var controller = new PagamentoController(pagamentoUseCase, logger)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    Request = { Headers = { ["Authorization"] = token } }
                }
            }
        };

        // Act
        var result = await controller.Put(pedidoId);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(201, createdAtActionResult.StatusCode);
    }

    [Fact]
    public async Task Get_ConsultarPagamento_DeveRetornarOk()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var controller = new PagamentoController(pagamentoUseCase, logger);
        pagamentoUseCase.ConsultarPagamentoPeloPedido(pedidoId).Returns(new Pagamento());

        // Act
        var result = await controller.Get(pedidoId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task Post_ConcluirPagamento_DeveRetornar201Created()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var token = "Bearer teste";
        var pagamentoWebhookDto = new PagamentoWebhookDto(pedidoId, true, null);
        var controller = new PagamentoController(pagamentoUseCase, logger)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    Request = { Headers = { ["Authorization"] = token } }
                }
            }
        };
        pagamentoUseCase.ConcluirPagamento(pedidoId, true, null, token).Returns(pedidoId);

        // Act
        var result = await controller.Post(pagamentoWebhookDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(201, createdAtActionResult.StatusCode);
    }

    [Fact]
    public async Task GetStatus_ObterStatusDoPedido_DeveRetornarOk()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var controller = new PagamentoController(pagamentoUseCase, logger);
        pagamentoUseCase.ObterStatusDoPedidoAsync(pedidoId).Returns(true);

        // Act
        var result = await controller.GetStatus(pedidoId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }
}

