using RiseOn.AutoInject;

namespace AutoInject.Examples;

public interface IServicesSample
{
    void DoSomething();
}


[InjectService(ServiceLifetimeType.Singleton)]
public class ServicesSample
{
    public void DoSomething()
    {
        //
    }
}