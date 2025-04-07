using System.Linq;
using System.Text;
using AutoInject.Attributes;
using AutoInject.SourceGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

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
                    predicate: static (node, _) => node is ClassDeclarationSyntax {AttributeLists: {Count: > 0}},
                    transform: static (syntaxContext, _) => GetServiceInfo(syntaxContext))
                .Collect();

            context.RegisterSourceOutput(result,
               static (context, services) =>
                {
                    context.AddSource("ServiceCollectionGenerator.g.cs",
                        SourceText.From($"{services.First()}", Encoding.UTF8));
                });
        }

        static ServiceInfo? GetServiceInfo(GeneratorAttributeSyntaxContext context)
        {
            // Symbol is the class that has the attribute
            var symbol = context.TargetSymbol as INamedTypeSymbol;
            var ns = symbol.ContainingNamespace.ToDisplayString();
            var name = symbol.ToDisplayString();

            var interfaceName  = symbol.Interfaces.FirstOrDefault()?.ToDisplayString();
            var baseName = symbol.BaseType?.ToDisplayString();

            var arguments = context.Attributes.First().ConstructorArguments;




            return new ServiceInfo
            {
                ServiceLifetime = arguments[0].Value!.ToString() switch
                {
                    "0" => "Singleton",
                    "1" => "Scoped",
                    "2" => "Transient",
                },
                Namespace = ns,
                ImplementationName = !arguments[1].IsNull ?  interfaceName ?? baseName : (string?) arguments[1].Value,
                ServiceName = name,
                GroupName = arguments[2].Value!.ToString(),
            };
        }
    }
}
