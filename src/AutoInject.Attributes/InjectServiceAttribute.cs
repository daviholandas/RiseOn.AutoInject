#nullable enable
using System;
using AutoInject.Attributes;

namespace RiseOn.AutoInject
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class InjectServiceAttribute : Attribute
    {
        public InjectServiceAttribute(ServiceLifetimeType serviceLifetimeType,
            Type? implementationBy = null,
            string? collectionName = "AutoInjected") { }
    }
}
