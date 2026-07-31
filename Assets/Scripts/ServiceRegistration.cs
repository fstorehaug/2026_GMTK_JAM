using Microsoft.Extensions.DependencyInjection;
using UnityEngine;

public class ServiceRegistration
{
    public static ServiceProvider RegisteredServices;

    static ServiceRegistration()
    {
        RegisteredServices = RegisterServices();
    }

    public static ServiceProvider RegisterServices()
    {
        var collection = new ServiceCollection();

        collection.AddTransient<Board>();
        collection.AddTransient<TileBag>();

        return collection.BuildServiceProvider();

    }
}
