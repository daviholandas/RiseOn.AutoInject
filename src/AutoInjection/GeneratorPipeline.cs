using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace AutoInjection;

public static class GeneratorPipeline
{
    public static bool IsClassSyntaxNode(SyntaxNode node)
        => node is ClassDeclarationSyntax { AttributeLists.Count:  > 0 };

    public static ClassDeclarationSyntax? GetServicesToGenerate(GeneratorSyntaxContext context)
    {
        var classSyntax = context.Node as ClassDeclarationSyntax;

        foreach (var attributeListSyntax in classSyntax?.AttributeLists!)
        {
            foreach (var attributeSyntax in attributeListSyntax.Attributes)
            {
                if(context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol methodSymbol)
                    continue;
                
                var attributeContainingSymbol = methodSymbol.ContainingType;
                var fullName = attributeContainingSymbol.ToDisplayString();
                
                if(fullName == "AutoInjection.InjectServiceAttribute")
                    return classSyntax;
            }
        }

        return null;
    }

    public static void Execute(Compilation compilation,
        ImmutableArray<ClassDeclarationSyntax?> services,
        SourceProductionContext context)
    {
        if(services.IsDefaultOrEmpty)
            return;
        
        var distinctServices = services.Distinct();

        var servicesToGenerate = GetServicesToInject(compilation,
            distinctServices, context.CancellationToken);

        if (servicesToGenerate.Any())
            context.AddSource("ServiceCollectionExtension.g.cs",
                SourceText.From(SourceTexts.ServiceCollectionExtensionSourceText(servicesToGenerate), Encoding.UTF8));
    }

    private static IEnumerable<ServiceInfo> GetServicesToInject(Compilation compilation,
        IEnumerable<ClassDeclarationSyntax?> services,
        CancellationToken cancellationToken)
    {
        var serviceInfos = new List<ServiceInfo>();
        
        var injectServiceAttributeSymbol = compilation.GetTypeByMetadataName("AutoInjection.InjectServiceAttribute");

        if (injectServiceAttributeSymbol is null)
            return Enumerable.Empty<ServiceInfo>();

        foreach (var classDeclarationSyntax in services)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var semanticModel = compilation.GetSemanticModel(classDeclarationSyntax!.SyntaxTree);

            if(semanticModel.GetDeclaredSymbol(classDeclarationSyntax) is not INamedTypeSymbol classSymbol)
                continue;
            
            foreach (var attributeData in classSymbol.GetAttributes())
            {
                if(!SymbolEqualityComparer.Default.Equals(attributeData.AttributeClass, injectServiceAttributeSymbol))
                    continue;

                var constructorArguments = attributeData.ConstructorArguments;

                if (constructorArguments.Length != 2)
                    continue;
                
                serviceInfos.Add(new (
                    classSymbol.ToDisplayString(),
                    constructorArguments[0].Value switch
                    {
                        0 => "Singleton",
                        1 => "Scoped",
                        2 => "Transient",
                        _ => throw new ArgumentOutOfRangeException()
                    },
                    constructorArguments[1].Value switch
                    {
                        INamedTypeSymbol namedTypeSymbol => namedTypeSymbol.ToDisplayString(),
                        ITypeSymbol typeSymbol => typeSymbol.ToDisplayString(),
                        _ => throw new Exception("Invalid type")
                    }));
            }
        }

        return serviceInfos;
    }
}

public record struct ServiceInfo(string Name,
    string ServiceLife,
    string ImplementationType);