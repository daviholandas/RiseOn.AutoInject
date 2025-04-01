using AutoInject.Attributes;
using System.Text.Json.Serialization;
using RiseOn.AutoInject;

namespace AutoInject.Examples;


public interface IServiceExample
{
    void DoSomething();
}


[InjectService(ServiceLifetime.Singleton, "ServicesTests")]
[JsonSourceGenerationOptions()]
public class ServiceExample
{
    
}