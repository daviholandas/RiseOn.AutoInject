using System.Text.Json.Serialization;
using RiseOn.AutoInject;

namespace AutoInject.Examples;


public interface IServiceExample
{
    void DoSomething();
}


[InjectService(ServiceLifetimeType.Transient,collectionName:"IoCServices")]
public class ServiceExample : ServiceExampleBase, IServiceExample
{
    public override void DoSomething()
    {
        throw new NotImplementedException();
    }
}

public abstract class ServiceExampleBase
{
    public abstract void DoSomething();
}

[InjectService(ServiceLifetimeType.Singleton, collectionName:"IoCServices")]
public class ServiceExampleDerived
{
    public void DoSomething()
    {
        throw new NotImplementedException();
    }
}