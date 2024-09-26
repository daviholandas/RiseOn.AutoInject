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
                    Type implementationType, string collectionName =  ""AutoInject"") 
                    => (ServiceLife, ImplementationType, CollectionName) =
                             (serviceLife, implementationType, collectionName);

                public ServiceLife ServiceLife { get; }
                public Type ImplementationType { get; }
                public string CollectionName { get; }
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
        foreach (var collection in serviceInfos.Select(x => x.CollectionName).Distinct())
        {
            sb.AppendLine($"        public static IServiceCollection {collection}(this IServiceCollection services)");
            sb.AppendLine("        {");
            foreach (var serviceInfo in serviceInfos)
            {
                var serviceToInject = serviceInfo.ServiceLife switch
                {
                    "Singleton" => $"services.AddSingleton<{serviceInfo.ImplementationType}, {serviceInfo.ServiceName}>();",
                    "Scoped" => $"services.AddScoped<{serviceInfo.ImplementationType}, {serviceInfo.ServiceName}>();",
                    "Transient" => $"services.AddTransient<{serviceInfo.ImplementationType}, {serviceInfo.ServiceName}>();",
                    _ => throw new ArgumentOutOfRangeException()
                };

                sb.AppendLine($"            {serviceToInject}");
            }
            sb.AppendLine("            return services;");
            sb.AppendLine("        }");
        }
        sb.AppendLine("    }");
        sb.AppendLine("}");
        return sb.ToString();
    }
}