using Microsoft.Extensions.DependencyInjection;
using UnityEngine;

public static class ServiceRegistration
{
    public static IServiceScope ServiceProvider = RegisterServices().CreateScope();

    public static ServiceProvider RegisterServices()
    {

        var services = new ServiceCollection();

        services
            .AddTransient<ShootService>()
            .AddSingleton<MatchMaking>()
            .AddSingleton<ConnectionService>()
            .AddSingleton<RegisterPlayerWithDb>();

        
        services
            .AddSingleton<ScopeService>()
            .AddScoped<CountDownData>();

        return services.BuildServiceProvider();
    }

}
