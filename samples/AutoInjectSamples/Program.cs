using Microsoft.Extensions.DependencyInjection;
using RiseOn.AutoInject;

var container = new ServiceCollection();


public interface ISampleService{}

public abstract class SampleServiceAbstract { }

[InjectService(ServiceLifetimeType.Scoped)]
public class SampleService : ISampleService
{
    public void Dispose()
    {
        // TODO release managed resources here
    }
}
