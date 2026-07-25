using Microsoft.Extensions.DependencyInjection;
using UnityEngine;

public static class ServiceRegistration
{
    public static ServiceProvider ServiceProvider = RegisterServices();

    public static ServiceProvider RegisterServices()
    {

        var services = new ServiceCollection();

        services
            .AddTransient<ShootService>()
            .AddSingleton<MatchMaking>()
            .AddSingleton<ConnectionService>()
            .AddSingleton<RegisterPlayerWithDb>();

        return services.BuildServiceProvider();
    }

}
