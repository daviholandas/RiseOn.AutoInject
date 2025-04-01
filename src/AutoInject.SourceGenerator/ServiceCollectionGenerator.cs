using System.Linq;
using System.Text;
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
                    transform: static (syntaxContext, token) =>
                    {
                        var symbol = syntaxContext.TargetSymbol as INamedTypeSymbol;
                        return symbol.ToDisplayString();
                    } )
                .Collect();

            context.RegisterSourceOutput(result,
               static (context, services) =>
                {
                    context.AddSource("ServiceCollectionGenerator.g.cs",
                        SourceText.From($"{services.First()}", Encoding.UTF8));
                });
        }
    }
}
