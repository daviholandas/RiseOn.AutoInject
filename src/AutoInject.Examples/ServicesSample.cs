using RiseOn.AutoInject;

namespace AutoInject.Examples;

public interface IServicesSample
{
    void DoSomething();
}


[InjectService(ServiceLifetimeType.Singleton, collectionName: "Samples")]
public class ServicesSample
{
    public void DoSomething()
    {
        throw new NotImplementedException();
    }
}