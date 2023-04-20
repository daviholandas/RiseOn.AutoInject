using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoInjection;

public static class GeneratorPipeline
{
    public static bool IsClassSyntaxNode(SyntaxNode node)
        => node is ClassDeclarationSyntax { AttributeLists.Count:  > 0 };
}