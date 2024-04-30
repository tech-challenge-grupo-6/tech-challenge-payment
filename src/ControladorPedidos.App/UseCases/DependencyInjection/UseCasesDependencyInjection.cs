using ControladorPedidos.App.Contracts;

namespace ControladorPedidos.App.UseCases.DependencyInjection;

public static class UseCasesDependencyInjection
{
    public static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<IPagamentoUseCase, PagamentoUseCase>();

        return services;
    }
}
