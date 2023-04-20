namespace AutoInjection;

public static class SourceTexts
{
    public static string ServiceInjectionSourceText 
        => @"
            using System;
            using Microsoft.Extensions.DependencyInjection;
            
            namespace AutoInjection;

            [AttributeUsage(AttributeTargets.Class, Inherited = false)]
            public class InjectServiceAttribute : Attribute
            {
                public InjectServiceAttribute(ServiceLifetime serviceLifetime,
                    Type implementationType) {}
            }";

    public static string ServiceCollectionExtensionSourceText()
    {
        /*context.RegisterImplementationSourceOutput(context.CompilationProvider, (context, compilation) =>
        {
            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using Microsoft.Extensions.DependencyInjection;");
            sb.AppendLine();
            sb.AppendLine("namespace AutoInjection");
            sb.AppendLine("{");
            sb.AppendLine("    public static class ServiceCollectionExtension");
            sb.AppendLine("    {");
            sb.AppendLine("        public static IServiceCollection AddAutoInject(this IServiceCollection services)");
            sb.AppendLine("        {");
            foreach (var type in compilation.Assembly.GlobalNamespace.GetNamespaceTypes())
            {
                if (type.GetAttributes().Any(x => x.AttributeClass?.Name == "InjectServiceAttribute"))
                {
                    sb.AppendLine($"            services.AddTransient<{type.Name}>();");
                }
            }
            sb.AppendLine("            return services;");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            context.AddSource("ServiceCollectionExtension", SourceText.From(sb.ToString(), Encoding.UTF8));
        });*/

        return string.Empty;
    }
}