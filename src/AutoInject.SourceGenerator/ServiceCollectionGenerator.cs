using AutoInject.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AutoInject.SourceGenerator
{
     public class ServiceCollectionGenerator :  IIncrementalGenerator
     {
          public void Initialize(IncrementalGeneratorInitializationContext context)
          {
               context.SyntaxProvider.ForAttributeWithMetadataName<AutoInjectAttribute>(nameof(AutoInjectAttribute),
                     static (node, _) => node is AttributeSyntax,
                     (syntaxContext, token) => null);
          }
     }
}
