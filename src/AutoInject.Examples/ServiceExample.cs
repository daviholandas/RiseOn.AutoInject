using AutoInject.Attributes;
using System.Text.Json.Serialization;
using RiseOn.AutoInject;

namespace AutoInject.Examples;


public interface IServiceExample
{
    void DoSomething();
}


[InjectService(ServiceLifetimeType.Transient, groupName:"IoCServices")]
[JsonSourceGenerationOptions()]
public class ServiceExample
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