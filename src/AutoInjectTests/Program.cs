using AutoInjection;
using Microsoft.Extensions.DependencyInjection;

var servicesCollection = new ServiceCollection();

servicesCollection.AddAutoInject();



public interface IServiceTest
{
    void Test();
}

public interface IServiceTest2
{
    void Test();
}

public interface IServiceTest3
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

[InjectService(ServiceLife.Transient, typeof(IServiceTest2))]
public class ServiveTest2 : IServiceTest2
{
    public void Test()
    {
        Console.WriteLine("Test2");
    }
}

[InjectService(ServiceLife.Singleton, typeof(IServiceTest3))]
public class ServiveTest3 : IServiceTest3
{
    public void Test()
    {
        Console.WriteLine("Test2");
    }
}
