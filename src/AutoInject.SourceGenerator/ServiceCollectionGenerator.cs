using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
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
                    predicate: static (node, _) => node is ClassDeclarationSyntax {AttributeLists: {Count: > 0}},
                    transform: static (syntaxContext, _) => SourceGeneratorHelper.GetServiceInfo(syntaxContext))
                .Collect()
                .Select((service, _) => service.Distinct());

            context.RegisterSourceOutput(result,
                static (context, services) =>
                {
                    foreach (var serviceGrouped in services.GroupBy(x => x!.CollectionName))
                    {
                        context.AddSource($"{serviceGrouped.Key}ServiceCollectionExtension.g.cs",
                            SourceText.From(SourceGeneratorHelper.GenerateSourceClass(
                                    serviceGrouped.Select(x => x)!),
                                Encoding.UTF8));
                    }
                });
        }
    }
}
