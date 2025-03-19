using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading;
using AutoInject.Attributes;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace AutoInject.SourceGenerator
{
    [Generator]
    public class ServiceCollectionGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var result = context.SyntaxProvider.ForAttributeWithMetadataName(
                    typeof(AutoInjectAttribute).FullName!,
                predicate:  static (node, token) => true,
                transform: (syntaxContext, token) => GetServicesInfo(syntaxContext.SemanticModel,
                    syntaxContext.TargetNode, token))
                .Where(services => services is not null && services.Any());
        }

        static  IEnumerable<ServiceInfo> GetServicesInfo(SemanticModel syntaxContext,
            SyntaxNode node,
            CancellationToken token)
        {
            var services = new List<ServiceInfo>();

            if(node is not ClassDeclarationSyntax classDeclaration)
                throw new Exception("Can't find class declaration");

            var attribute = classDeclaration.AttributeLists.SelectMany(list => list.Attributes)
                .First(attribute => attribute.Name.ToString() == "AutoInjectAttribute");
                   

            //foreach (var attribute in(node as ClassDeclarationSyntax).AttributeLists.SelectMany(list => list.Attributes).Select(x => x.ArgumentList))
            //{
            //    /*var serviceInterface = attribute.GetAttributeArgumentValue<Type>(nameof(Attributes.AutoInjectAttribute.ServiceInterface));
            //    var serviceLifetime = attribute.GetAttributeArgumentValue<ServiceLifetime>(nameof(Attributes.AutoInjectAttribute.ServiceLifetime));
            //    var groupName = attribute.GetAttributeArgumentValue<string>(nameof(Attributes.AutoInjectAttribute.GroupName));

            //    services.Add(new ServiceContainer(serviceInterface, serviceLifetime, groupName));*/
            //}

            return services;
        }
    }
}
