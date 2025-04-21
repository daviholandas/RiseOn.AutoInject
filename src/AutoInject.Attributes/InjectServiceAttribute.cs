#nullable enable
using System;
using AutoInject.Attributes;

namespace RiseOn.AutoInject
{
    // TODO: Create documentation.
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class InjectServiceAttribute : Attribute
    {
        public InjectServiceAttribute(ServiceLifetimeType serviceLifetimeType,
            Type? implementationBy = null,
            string? collectionName = "AutoInjected") { }
    }
}
