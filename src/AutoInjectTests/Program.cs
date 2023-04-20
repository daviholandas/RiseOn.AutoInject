using AutoInjection;
using Microsoft.Extensions.DependencyInjection;

var servicesCollection = new ServiceCollection();


public interface IServiceTest
{
    void Test();
}

[InjectService(ServiceLifetime.Scoped, typeof(IServiceTest))]
public class ServiceTest : IServiceTest
{
    public void Test()
    {
        Console.WriteLine("Test");
    }
}

enum Test
{
    Car,
    Bike
}