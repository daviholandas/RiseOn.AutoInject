using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
                if(context.SemanticModel.GetSymbolInfo(attributeListSyntax).Symbol is not IMethodSymbol methodSymbol)
                    continue;
                
                var attributeContainingSymbol = methodSymbol.ContainingType;
                var fullName = attributeContainingSymbol.ToDisplayString();
                
                if(fullName == "AutoInjection.InjectServiceAttribute")
                    return classSyntax;
            }
        }

        return null;
    }
}