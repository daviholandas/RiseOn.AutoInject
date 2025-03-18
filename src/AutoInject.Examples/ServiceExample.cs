using AutoInject.Attributes;

namespace AutoInject.Examples;


public interface IServiceExample
{
    void DoSomething();
}


[AutoInject(typeof(IServiceExample), ServiceLifetime.Singleton)]
public class ServiceExample
{
    
}