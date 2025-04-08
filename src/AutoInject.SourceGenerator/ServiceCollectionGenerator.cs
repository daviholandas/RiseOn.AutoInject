using AutoInject.Attributes;
using AutoInject.SourceGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RiseOn.AutoInject
{
    [Generator]
    public class ServiceCollectionGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var result = context.SyntaxProvider
                .ForAttributeWithMetadataName(
                    "RiseOn.AutoInject.InjectServiceAttribute",
                    predicate: static (node, _) => node is ClassDeclarationSyntax { AttributeLists: { Count: > 0 } },
                    transform: static (syntaxContext, _) => GetServiceInfo(syntaxContext))
                .Collect();

            context.RegisterSourceOutput(result,
               static (context, services) =>
                {
                    context.AddSource("ServiceCollectionGenerator.g.cs",
                        SourceText.From(GenerateSourceClass(services.First()), Encoding.UTF8));
                });
        }

        static ServiceInfo? GetServiceInfo(GeneratorAttributeSyntaxContext context)
        {
            // Symbol is the class that has the attribute
            var symbol = (INamedTypeSymbol)context.TargetSymbol;
            var ns = symbol.ContainingNamespace.ToDisplayString();
            var name = symbol.ToDisplayString();

            var interfaceName = symbol.Interfaces.FirstOrDefault()?.ToDisplayString();
            var baseName = symbol.BaseType?.ToDisplayString();

            var arguments = context.Attributes.First().ConstructorArguments;

            var serviceLifetime = arguments[0].Value!.ToString() switch
            {
                "0" => "Singleton",
                "1" => "Scoped",
                "2" => "Transient",
                _ => throw new InvalidOperationException("Invalid service lifetime value")
            };

            // TODO: Validate the arguments
            var implementationName = (bool?)arguments[1].Value ?? false && arguments[2].IsNull
                ? interfaceName ?? baseName
                : arguments[2].Value!.ToString();

            return new ServiceInfo
            {
                ServiceLifetime = serviceLifetime,
                Namespace = ns,
                ServiceName = name,
                GroupName = arguments[3].Value!.ToString(),
                ImplementationName = implementationName
            };
        }

        static string GenerateSourceClass(ServiceInfo serviceInfo)
        {
            var sb = new StringBuilder();
            sb.AppendLine("using Microsoft.Extensions.DependencyInjection;");
            sb.AppendLine($"namespace {serviceInfo.Namespace}.{serviceInfo.GroupName};");
            sb.AppendLine($"public static class {serviceInfo.GroupName}ServiceCollectionExtensions");
            sb.AppendLine("{");
            sb.AppendLine($"    public static IServiceCollection Add{serviceInfo.GroupName}Services(this IServiceCollection services)");
            sb.AppendLine("    {");
            sb.AppendLine($"        services.Add{serviceInfo.ServiceLifetime}<{serviceInfo.ServiceName}, {serviceInfo.ImplementationName}>();");
            sb.AppendLine("        return services;");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
