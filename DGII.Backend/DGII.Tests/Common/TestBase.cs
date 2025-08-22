using AutoFixture;
using AutoFixture.AutoMoq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ErrorOr;

namespace DGII.Tests.Common;

public abstract class TestBase
{
    protected readonly IFixture Fixture;
    protected readonly IServiceProvider ServiceProvider;

    protected TestBase()
    {
        Fixture = new Fixture()
            .Customize(new AutoMoqCustomization());

        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();
    }

    protected virtual void ConfigureServices(IServiceCollection services)
    {
        // Configuración base de servicios para tests
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Debug);
        });
    }

    protected T GetService<T>() where T : notnull
    {
        return ServiceProvider.GetRequiredService<T>();
    }
}
