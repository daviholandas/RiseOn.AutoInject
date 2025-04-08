using AutoInject.Attributes;
using System.Text.Json.Serialization;
using RiseOn.AutoInject;

namespace AutoInject.Examples;


public interface IServiceExample
{
    void DoSomething();
}


[InjectService(ServiceLifetimeType.Transient,implementationBy:typeof(ServiceExampleBase), groupName:"IoCServices")]
[JsonSourceGenerationOptions()]
public class ServiceExample : IServiceExample
{
    public void DoSomething()
    {
        throw new NotImplementedException();
    }
}

public abstract class ServiceExampleBase
{
    public abstract void DoSomething();
}