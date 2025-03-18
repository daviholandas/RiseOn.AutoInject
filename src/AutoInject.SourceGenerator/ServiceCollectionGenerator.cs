using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading;
using AutoInject.Attributes;
using Microsoft.CodeAnalysis;


namespace AutoInject.SourceGenerator
{
    [Generator]
    public class ServiceCollectionGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var result = context.SyntaxProvider.ForAttributeWithMetadataName(typeof(Attributes.AutoInjectAttribute).FullName,
                predicate:  static (node, token) => true,
                transform: (syntaxContext, token) => GetServicesInfo(syntaxContext.SemanticModel,
                    syntaxContex, token));
                );

            context.RegisterSourceOutput(result, static (sourceProduction, source) =>
            {
                if (source != null)
                {
                    sourceProduction.AddSource("ServiceCollectionGenerator.g.cs", source.ServiceLifetime.ToString());
                }
            });
        }

        static  IEnumerable<ServiceContainer> GetServicesInfo(SemanticModel syntaxContext, SyntaxNode node,
            CancellationToken token)
        {
            var services = new List<ServiceContainer>();
            foreach (var attribute in syntaxContext.Attributes)
            {
                var serviceInterface = attribute.GetAttributeArgumentValue<Type>(nameof(Attributes.AutoInjectAttribute.ServiceInterface));
                var serviceLifetime = attribute.GetAttributeArgumentValue<ServiceLifetime>(nameof(Attributes.AutoInjectAttribute.ServiceLifetime));
                var groupName = attribute.GetAttributeArgumentValue<string>(nameof(Attributes.AutoInjectAttribute.GroupName));

                services.Add(new ServiceContainer(serviceInterface, serviceLifetime, groupName));
            }

            return services;
        }
    }
}
