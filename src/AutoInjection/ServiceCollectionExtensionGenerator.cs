using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace AutoInjection;

[Generator]
public class ServiceCollectionExtensionGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
       context.RegisterPostInitializationOutput(ctx 
           => ctx.AddSource("InjectServiceAttribute.g.cs",
               SourceText.From(SourceTexts.ServiceInjectionSourceText, Encoding.UTF8)));
       
       context.RegisterPostInitializationOutput(ctx 
           => ctx.AddSource("ServiceLife.g.cs",
               SourceText.From(SourceTexts.ServiceLifeEnum, Encoding.UTF8)));

       var servicesDeclarations  = context.SyntaxProvider
           .CreateSyntaxProvider(
               predicate: static (node,
                   _) => GeneratorPipeline.IsClassSyntaxNode(node),
               transform: static (ctx,
                   _) => GeneratorPipeline.GetServicesToGenerate(ctx))
           .Where(static classDeclaration => classDeclaration is not null);

       var servicesAndCompilation = context.CompilationProvider
           .Combine(servicesDeclarations.Collect());

       context.RegisterSourceOutput(servicesAndCompilation,
           static (sourceProduction,
               source) => GeneratorPipeline.Execute(source.Left, source.Right, sourceProduction));
    }
}