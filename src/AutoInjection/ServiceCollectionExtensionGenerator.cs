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
    }
}