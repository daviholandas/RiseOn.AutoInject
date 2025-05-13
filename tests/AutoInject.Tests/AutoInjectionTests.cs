using Microsoft.Extensions.DependencyInjection;
using RiseOn.AutoInject;

namespace AutoInject.Tests;

public class AutoInjectionTests
{
    private IServiceCollection _serviceCollection = new ServiceCollection();

    [Fact]
    public void ServiceCollection_Extension_ShouldCreateAnExtensionClassWithInjectedServices()
    {
        // Arrange & Act
        _serviceCollection.UseServiceTests();
        var serviceTest = _serviceCollection.FirstOrDefault(x => x.ServiceType == typeof(IServiceTest));

        // Assert
        Assert.NotNull(serviceTest);
        Assert.Equal(ServiceLifetime.Scoped, serviceTest.Lifetime);
        Assert.Equal(typeof(ServiceTest), serviceTest.ImplementationType);
    }

    [Fact]
    public void ServiceCollection_Extension_ShouldCreateAnExtensionClassWithInjectedServices_UsingKey()
    {
        // Arrange & Act
        _serviceCollection.UseNewServiceTests();
        var serviceTest = _serviceCollection.FirstOrDefault(x => x.ServiceType == typeof(INewServiceTest));

        // Assert
        Assert.NotNull(serviceTest);
        Assert.Equal(ServiceLifetime.Singleton, serviceTest.Lifetime);
        Assert.Equal(typeof(NewServiceTest), serviceTest.KeyedImplementationType);
        Assert.True(serviceTest.IsKeyedService);
        Assert.Equal("NewServiceTest", serviceTest.ServiceKey);
    }
}


public interface INewServiceTest
{
    void DoSomething();
}

[InjectService(ServiceLifetimeType.Singleton,
    CollectionName = "NewServiceTests",
    Key = "NewServiceTest")]
public class NewServiceTest : INewServiceTest
{
    public void DoSomething()
    {
        // Implementation
    }
}

[InjectService(ServiceLifetimeType.Scoped,
    ImplementationOf = typeof(IServiceTest),
    CollectionName = "ServiceTests")]
public class ServiceTest : IDisposable, IServiceTest
{
    public void Dispose()
    {
        //
    }
}

public interface IServiceTest {}