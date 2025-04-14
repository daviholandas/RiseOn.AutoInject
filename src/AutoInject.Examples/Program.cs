using AutoInject.Examples.IoCServices;
using Microsoft.Extensions.DependencyInjection;
using RiseOn.AutoInject;


var services = new ServiceCollection();

services.AddIoCServicesServices();


Console.Write(typeof(InjectServiceAttribute).FullName);

