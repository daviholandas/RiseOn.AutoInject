using AutoInject.Attributes;
using System.Text.Json.Serialization;

namespace AutoInject.Examples;


public interface IServiceExample
{
    void DoSomething();
}


[AutoInject(ServiceLifetime.Singleton, typeof(IServiceExample), "Example")]
[JsonSourceGenerationOptions()]
public class ServiceExample
{
    
}