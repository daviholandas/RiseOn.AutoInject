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

            var re = context.Attributes.FirstOrDefault();

            //var groupName = context.Attributes[0].NamedArguments.FirstOrDefault(x => x.Key == "GroupName").Value;




            return new ServiceInfo
            {
                Namespace = ns,
                ImplementationName = interfaceName ?? baseName,
                ServiceName = name,
            };
        }
    }
}
