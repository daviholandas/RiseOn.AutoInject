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
            var serviceInfoProvider = context.SyntaxProvider
                .ForAttributeWithMetadataName(
                    "RiseOn.AutoInject.InjectServiceAttribute",
                    predicate: static (node, _) => node is ClassDeclarationSyntax { AttributeLists: { Count: > 0 } },
                    transform: static (syntaxContext, _) => SourceGeneratorHelper.GetServiceInfo(syntaxContext))
                .Where(static service => service != null) // Filter out null results early
                .Collect();

            var distinctServices = serviceInfoProvider
                .Select(static (services, _) => services.Distinct());

            context.RegisterSourceOutput(distinctServices,
                static (sourceProductionContext, services) =>
                {
                    foreach (var serviceGroup in services.GroupBy(service => service!.CollectionName))
                    {
                        var sourceCode = SourceGeneratorHelper.GenerateSourceClass(serviceGroup);
                        var sourceText = SourceText.From(sourceCode, Encoding.UTF8);

                        sourceProductionContext.AddSource($"{serviceGroup.Key}CollectionExtension.g.cs", sourceText);
                    }
                });
        }
    }
}
