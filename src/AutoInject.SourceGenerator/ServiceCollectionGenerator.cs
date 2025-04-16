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
                .Collect()
                .Select((service, _ ) => service.Distinct());

            context.RegisterSourceOutput(result,
               static (context, services) =>
                {
                    foreach (var serviceGrouped in services.GroupBy(x => x!.CollectionName))
                    {
                        context.AddSource($"{serviceGrouped.Key}ServiceCollectionExtension.g.cs",
                            SourceText.From(GenerateSourceClass(
                                    serviceGrouped.Select(x => x)!),
                                Encoding.UTF8));
                    }
                });
        }

        private static ServiceInfo GetServiceInfo(GeneratorAttributeSyntaxContext context)
        {
            // Symbol is the class that has the attribute
            var symbol = (INamedTypeSymbol)context.TargetSymbol;
            var ns = symbol.ContainingNamespace.ToDisplayString();
            var name = symbol.ToDisplayString();

            var interfaceName = symbol.Interfaces.FirstOrDefault()?.ToDisplayString();
            var baseTypeName = symbol.BaseType?.ToDisplayString();
            var baseName = baseTypeName == "object" ? null : baseTypeName;

            var arguments = context.Attributes.First().ConstructorArguments;

            var serviceLifetime = arguments[0].Value!.ToString() switch
            {
                "0" => "Singleton",
                "1" => "Scoped",
                "2" => "Transient",
                _ => throw new InvalidOperationException("Invalid service lifetime value")
            };

            return new ()
            {
                ServiceLifetime = serviceLifetime,
                Namespace = ns,
                ServiceName = name,
                CollectionName = arguments[2].Value!.ToString(),
                ImplementationName = arguments[1].IsNull
                    ? interfaceName ?? baseName
                    : arguments[1].Value is ITypeSymbol typeSymbol
                        ? typeSymbol.ToDisplayString()
                        : null
            };
        }

        private static string GenerateSourceClass(IEnumerable<ServiceInfo> serviceInfos)
        {
            var sb = new StringBuilder();
            var service = serviceInfos.First();
            sb.AppendLine("using Microsoft.Extensions.DependencyInjection;\n");
            sb.AppendLine($"namespace {service.Namespace}.{service.CollectionName};\n");
            sb.AppendLine($"public static class {service.CollectionName}ServiceCollectionExtensions");
            sb.AppendLine("{");
            sb.AppendLine($"    public static IServiceCollection Add{service.CollectionName}Services(this IServiceCollection services)");
            sb.AppendLine("    {");

            foreach (var serviceInfo in serviceInfos)
            {
                sb.AppendLine(serviceInfo.ImplementationName is null
                    ? $"        services.Add{serviceInfo.ServiceLifetime}<{serviceInfo.ServiceName}>();"
                    : $"        services.Add{serviceInfo.ServiceLifetime}<{serviceInfo.ImplementationName}, {serviceInfo.ServiceName}>();");
            }
            sb.AppendLine("        return services;");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
