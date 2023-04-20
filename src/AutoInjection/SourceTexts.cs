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
}