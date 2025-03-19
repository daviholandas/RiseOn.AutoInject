using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading;
using AutoInject.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;


namespace AutoInject.SourceGenerator
{
    [Generator]
    public class ServiceCollectionGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var result = context.SyntaxProvider.ForAttributeWithMetadataName(
                    typeof(AutoInjectAttribute).FullName!,
                predicate:  static (_, _) => true,
                transform: (syntaxContext, token) => GetServicesInfo(syntaxContext.SemanticModel,
                    syntaxContext.TargetNode, token))
                .Where(services => services is not null);

            var services = result.Collect(); ;

            context.RegisterImplementationSourceOutput(services,
                static (context, services) =>
                {
                    context.AddSource("ServiceCollectionGenerator.g.cs",
                        SourceText.From(GenerateServiceCollection(services), Encoding.UTF8));
                });
        }

        static  ServiceInfo? GetServicesInfo(SemanticModel syntaxContext,
            SyntaxNode node,
            CancellationToken token)
        {
            var services = new List<ServiceInfo>();

            if(node is not ClassDeclarationSyntax classDeclaration)
                throw new Exception("Can't find class declaration");

            var autoInjectAttribute = classDeclaration.AttributeLists
                .SelectMany(x => x.Attributes)
                .FirstOrDefault(x => x.Name.ToString().Equals("AutoInject"));

            if(autoInjectAttribute is null)
                return null;

            /*foreach (var argumentSyntax in autoInjectAttribute.ArgumentList!.Arguments)
            {
                var re = argumentSyntax.Expression switch
                {
                    MemberAccessExpressionSyntax memberAccess => memberAccess.Name.ToString(),
                    TypeOfExpressionSyntax typeOf => typeOf.Type.ToString(),
                    LiteralExpressionSyntax literal => literal.Token.ValueText,
                    _ => throw new Exception("Can't find type of expression")
                };
            }*/

            ServiceInfo serviceInfo = new(classDeclaration.Identifier.Text,
                autoInjectAttribute.ArgumentList!.Arguments[0].ToString(),
                autoInjectAttribute.ArgumentList!.Arguments[1].ToString(),
                autoInjectAttribute.ArgumentList?.Arguments[2].ToString() ?? "AutoInjected",
                classDeclaration.Parent?.DescendantNodes().OfType<NamespaceDeclarationSyntax>().First().Name.ToString() ?? "AutoInjected" );

            return serviceInfo;
        }

        static string GenerateServiceCollection(IEnumerable<ServiceInfo> services)
        {
            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using Microsoft.Extensions.DependencyInjection;");
            sb.AppendLine();
            sb.AppendLine($"namespace {services.First().Namespace}.{services.First().GroupName}");
            sb.AppendLine("{");
            sb.AppendLine("    public static class ServiceCollectionExtension");
            sb.AppendLine("    {");
            foreach (var collection in services.Distinct())
            {
                sb.AppendLine($"        public static IServiceCollection {collection.GroupName}(this IServiceCollection services)");
                sb.AppendLine("        {");
                var serviceToInject = collection.ServiceLifetime switch
                {
                    "Singleton" => $"services.AddSingleton<{collection.ImplementationName}, {collection.ServiceName}>();",
                    "Scoped" => $"services.AddScoped<{collection.ImplementationName}, {collection.ServiceName}>();",
                    "Transient" => $"services.AddTransient<{collection.ImplementationName}, {collection.ServiceName}>();",
                    _ => throw new ArgumentOutOfRangeException()
                };
                sb.AppendLine($"            {serviceToInject}");
                sb.AppendLine("            return services;");
                sb.AppendLine("        }");
            }
            sb.AppendLine("    }");
            sb.AppendLine("}");
            return sb.ToString();
        }
    }
}
