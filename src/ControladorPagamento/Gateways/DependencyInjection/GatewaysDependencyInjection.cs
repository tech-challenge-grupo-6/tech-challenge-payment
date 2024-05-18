using ControladorPagamento.Entities.Repositories;

namespace ControladorPagamento.Gateways.DependencyInjection;

public static class GatewaysDependencyInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IPagamentoRepository, PagamentoRepository>();

        return services;
    }
}
