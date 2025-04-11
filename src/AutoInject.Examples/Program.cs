using AutoInject.Examples.IoCServices;
using AutoInject.Examples.Samples;
using Microsoft.Extensions.DependencyInjection;
using RiseOn.AutoInject;


var services = new ServiceCollection();

services.AddIoCServicesServices();
services.AddSamplesServices();


Console.Write(typeof(InjectServiceAttribute).FullName);

