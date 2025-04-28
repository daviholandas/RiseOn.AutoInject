using AutoInject.Examples;
using Microsoft.Extensions.DependencyInjection;
using RiseOn.AutoInject;


var services = new ServiceCollection();

services.UseAutoInjectExamplesServices();

var pro = services.BuildServiceProvider();

var ser = pro.GetRequiredKeyedService<IServiceExample>("TestKey");


Console.Write(ser.GetType().FullName);

