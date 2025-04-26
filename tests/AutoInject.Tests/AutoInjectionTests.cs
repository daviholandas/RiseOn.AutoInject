using Microsoft.Extensions.DependencyInjection;
using RiseOn.AutoInject;
using System.Security.Authentication.ExtendedProtection;
using AutoInject.Tests.AutoInjected;
using AutoInject.Tests.Test;

namespace AutoInject.Tests;

public class AutoInjectionTests
{
    [Fact]
    public void Test()
    {
        var ser = new ServiceCollection();

        ser.AddAutoInjectedServices()
            .AddTestServices();

        var provider = ser.BuildServiceProvider();
        using var scope = provider.CreateScope();

        var service = provider.GetService<IServiceTest>();

        Assert.NotNull(service);
    }
}


[InjectService(ServiceLifetimeType.Transient)]
public class ServiceTest {}

[InjectService(ServiceLifetimeType.Scoped, collectionName: "Test")]
public class ServiceTest2 : IServiceTest, IDisposable
{
    public void Dispose()
    {
        throw new NotImplementedException();
    }
}

public interface IServiceTest {}