using Microsoft.EntityFrameworkCore;

namespace ControladorPedidos.App.Infrastructure.DataBase.DependencyInjection;

public static class DataBaseDependencyInjection
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseMySQL(configuration.GetConnectionString("DefaultConnection")!);
        });
        services.AddHostedService<MigrationsHostedService>();

        return services;
    }
}
