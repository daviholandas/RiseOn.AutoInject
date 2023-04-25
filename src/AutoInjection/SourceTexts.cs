using System.Text;

namespace AutoInjection;

public static class SourceTexts
{
    public static string ServiceInjectionSourceText 
        => @"
            using System;
            
            namespace AutoInjection;

            [AttributeUsage(AttributeTargets.Class, Inherited = false)]
            public class InjectServiceAttribute : Attribute
            {
                public InjectServiceAttribute(ServiceLife serviceLife,
                    Type implementationType) {}
            }";

    public static string ServiceLifeEnum
        => @"
            namespace AutoInjection;
            public enum ServiceLife
            {
                Singleton,
                Scoped,
                Transient
            }";

    public static string ServiceCollectionExtensionSourceText(IEnumerable<ServiceInfo> serviceInfos)
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
        sb.AppendLine("            return services;");
        sb.AppendLine("        }");
        sb.AppendLine("    }");
        sb.AppendLine("}");
        
        return sb.ToString();
    }
}