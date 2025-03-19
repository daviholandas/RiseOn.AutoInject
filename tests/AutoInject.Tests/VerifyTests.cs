namespace AutoInject.Tests;

public class VerifyTests
{
    [Fact]
    public Task ServiceCollection_CreateExtensionMethod_ShouldCreateAExtensionMethodsWIthServices()
    {
        // Arrange
        var source = @"
            using System;
            using Microsoft.Extensions.DependencyInjection;

            namespace AutoInjection
            {
                public static class ServiceCollectionExtension
                {
                    public static IServiceCollection AutoInject(this IServiceCollection services)
                    {
                        services.AddSingleton<IService, Service>();
                        services.AddScoped<IService, Service>();
                        services.AddTransient<IService, Service>();
                        return services;
                    }
                }
            }";

        // Act and Assert
        return TestHelper.VerifyGeneratedCode(source);
    }
}