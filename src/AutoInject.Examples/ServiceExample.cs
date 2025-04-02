using AutoInject.Attributes;
using System.Text.Json.Serialization;
using RiseOn.AutoInject;

namespace AutoInject.Examples;


public interface IServiceExample
{
    void DoSomething();
}


[InjectService(ServiceLifetime.Singleton, groupName: "AutoInject")]
[JsonSourceGenerationOptions()]
public class ServiceExample : ServiceExampleBase
{
    public override  void DoSomething()
    {
        throw new NotImplementedException();
    }
}

public abstract class ServiceExampleBase
{
    public abstract void DoSomething();
}