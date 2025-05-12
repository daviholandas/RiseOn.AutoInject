using Microsoft.Extensions.DependencyInjection;
using RiseOn.AutoInject;

namespace AutoInject.Tests;

public class AutoInjectionTests
{
    [Fact]
    public void Test()
    {
        var ser = new ServiceCollection();

        ser.UseAutoInjectTestsServices()
            .UseTest2Services();

        var provider = ser.BuildServiceProvider();
        using var scope = provider.CreateScope();

        var service = provider.GetService<IServiceTest>();

        Assert.NotNull(service);
    }
}


[InjectService(ServiceLifetimeType.Transient)]
public class ServiceTest {}

[InjectService(ServiceLifetimeType.Scoped,
    ImplementationOf = typeof(IServiceTest),
    CollectionName = "Test2Services")]
public class ServiceTest2 : IDisposable, IServiceTest
{
    public void Dispose()
    {
        //
    }
}

public interface IServiceTest {}