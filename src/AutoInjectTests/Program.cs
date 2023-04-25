using AutoInjection;
using Microsoft.Extensions.DependencyInjection;

var servicesCollection = new ServiceCollection();

servicesCollection.AddAutoInject();


public interface IServiceTest
{
    void Test();
}

[InjectService(ServiceLife.Scoped, typeof(IServiceTest))]
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