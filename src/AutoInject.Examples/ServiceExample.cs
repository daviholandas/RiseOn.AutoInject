using System.Text.Json.Serialization;
using RiseOn.AutoInject;

namespace AutoInject.Examples;


public interface IServiceExample
{
    void DoSomething();
}


[InjectService(ServiceLifetimeType.Transient, CollectionName = "AutoInjectedServices")]
public class ServiceExample : ServiceExampleBase, IServiceExample
{
    public override void DoSomething()
    {
        //
    }
}

public abstract class ServiceExampleBase
{
    public abstract void DoSomething();
}

[InjectService(ServiceLifetimeType.Singleton, Key = "TestKey")]
public class ServiceExampleDerived :IServiceExample
{
    public void DoSomething()
    {
        //
    }
}