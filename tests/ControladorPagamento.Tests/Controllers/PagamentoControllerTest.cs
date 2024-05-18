using ControladorPagamento.Contracts;
using ControladorPagamento.Controllers;
using ControladorPagamento.Entities;
using ControladorPagamento.Presenters;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace ControladorPagamento.Tests;
public class PagamentoControllerTest
{
    private readonly IPagamentoUseCase _pagamentoUseCase;
    private readonly ILogger<PagamentoController> _logger;
    private readonly PagamentoController _controller;

    public PagamentoControllerTest()
    {
        _pagamentoUseCase = Substitute.For<IPagamentoUseCase>();
        _logger = Substitute.For<ILogger<PagamentoController>>();
        _controller = new PagamentoController(_pagamentoUseCase, _logger)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }

    [Fact]
    public async Task Put_EfetuarPagamento_DeveRetornar201Created()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var token = "Bearer teste";
        var controller = _controller;

        // Act
        var result = await controller.Put(pedidoId);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(201, createdAtActionResult.StatusCode);
    }

    [Fact]
    public async Task Put_ReturnsBadRequest_WhenPaymentBadRequest()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var token = "Bearer token";
        _controller.HttpContext.Request.Headers["Authorization"] = token;
        var exception = new Exception("Pagamento falhou");
        _pagamentoUseCase.EfetuarMercadoPagoQRCodeAsync(pedidoId, token)
            .Throws(exception);

        // Act
        var result = await _controller.Put(pedidoId);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();

        // Verify 
        var statusResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, statusResult.StatusCode);
    }

    [Fact]
    public async Task Get_ConsultarPagamento_DeveRetornarOk()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var controller = new PagamentoController(_pagamentoUseCase, _logger);
        _pagamentoUseCase.ConsultarPagamentoPeloPedido(pedidoId).Returns(new Pagamento());

        // Act
        var result = await controller.Get(pedidoId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task Get_ConsultarPagamento_DeveRetornarBadRequest()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var exception = new Exception("Pagamento falhou");
        _pagamentoUseCase.ConsultarPagamentoPeloPedido(pedidoId).Throws(exception);

        // Act
        var result = await _controller.Get(pedidoId);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();

        // Verify 
        var statusResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, statusResult.StatusCode);
    }

    [Fact]
    public async Task Post_ConcluirPagamento_DeveRetornar201Created()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var token = "Bearer teste";
        var pagamentoWebhookDto = new PagamentoWebhookDto(pedidoId, true, null);
        var controller = new PagamentoController(_pagamentoUseCase, _logger)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    Request = { Headers = { ["Authorization"] = token } }
                }
            }
        };
        _pagamentoUseCase.ConcluirPagamento(pedidoId, true, null, token).Returns(pedidoId);

        // Act
        var result = await controller.Post(pagamentoWebhookDto);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(201, createdAtActionResult.StatusCode);
    }

    [Fact]
    public async Task Post_ConcluirPagamento_DeveRetornarBadRequest()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var token = "Bearer token";
        var pagamentoWebhookDto = new PagamentoWebhookDto(pedidoId, true, "Erro");
        var controller = new PagamentoController(_pagamentoUseCase, _logger)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    Request = { Headers = { ["Authorization"] = token } }
                }
            }
        };
        var exception = new Exception("Pagamento falhou");

        _pagamentoUseCase.ConcluirPagamento(pedidoId, true, null, token)
            .Throws(exception);

        // Act
        var result = await controller.Post(pagamentoWebhookDto);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();

        // Verify 
        var statusResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, statusResult.StatusCode);
    }

    [Fact]
    public async Task GetStatus_ObterStatusDoPedido_DeveRetornarOk()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var controller = new PagamentoController(_pagamentoUseCase, _logger);
        _pagamentoUseCase.ObterStatusDoPedidoAsync(pedidoId).Returns(true);

        // Act
        var result = await controller.GetStatus(pedidoId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    [Fact]
    public async Task GetStatus_ObterStatusDoPedido_DeveRetornarBadRequest()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var exception = new Exception("Pagamento falhou");
        _pagamentoUseCase.ObterStatusDoPedidoAsync(pedidoId).Throws(exception);

        // Act
        var result = await _controller.GetStatus(pedidoId);

        // Assert
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();

        // Verify 
        var statusResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, statusResult.StatusCode);
    }

}

