using AutoInject.Attributes;
using System.Text.Json.Serialization;
using RiseOn.AutoInject;

namespace AutoInject.Examples;


public interface IServiceExample
{
    void DoSomething();
}


[InjectService(ServiceLifetimeType.Transient, collectionName:"IoCServices")]
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

[InjectService(ServiceLifetimeType.Singleton, collectionName:"IoCServices")]
public class ServiceExampleDerived : ServiceExampleBase
{
    public override void DoSomething()
    {
        throw new NotImplementedException();
    }
}