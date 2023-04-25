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

    public static ClassDeclarationSyntax? GetClassSyntax(GeneratorSyntaxContext context)
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
            services, context.CancellationToken);

        if (servicesToGenerate.Any())
            context.AddSource("ServiceCollectionExtension.g.cs",
                SourceText.From(SourceTexts.ServiceCollectionExtensionSourceText(servicesToGenerate), Encoding.UTF8));
    }

    private static IEnumerable<ServiceInfo> GetServicesToInject(Compilation compilation,
        IEnumerable<ClassDeclarationSyntax> services,
        CancellationToken cancellationToken)
    {
        var serviceInfos = new List<ServiceInfo>();
        
        var injectServiceAttributeSymbol = compilation.GetTypeByMetadataName("AutoInjection.InjectServiceAttribute");

        if (injectServiceAttributeSymbol is null)
            return Enumerable.Empty<ServiceInfo>();

        foreach (var classDeclarationSyntax in services)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var semanticModel = compilation.GetSemanticModel(classDeclarationSyntax.SyntaxTree);
            
            if(semanticModel.GetDeclaredSymbol(classDeclarationSyntax) is not INamedTypeSymbol classSymbol)
                continue;

            var serviceName = classSymbol.ToDisplayString();
            var serviceMembers = classSymbol.GetMembers();

            var members = new List<string>(serviceMembers.Length);

            foreach (var member in serviceMembers)
            {
                if(member is IFieldSymbol field && field.ConstantValue is not null)
                    members.Add(member.Name);
            }

            serviceInfos.Add(new ServiceInfo(serviceName, 
                classSymbol.ContainingNamespace.ToDisplayString(),
                classSymbol.ToDisplayString(),
                "Transient",
                classSymbol.ToDisplayString()));
        }

        return serviceInfos;
    }
}

public record struct ServiceInfo(string Name, 
    string Namespace, 
    string FullName,
    string ServiceLife,
    string ImplementationType);