#nullable enable
using System;
using AutoInject.Attributes;

namespace RiseOn.AutoInject
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class InjectServiceAttribute : Attribute
    {
        public InjectServiceAttribute(ServiceLifetimeType serviceLifetimeType,
            bool getImplementationBy = true,
            Type? implementationBy = null,
            string? groupName = "AutoInjected")
        {
            ServiceLifetimeType = serviceLifetimeType;
            ImplementationBy =  implementationBy;
            GroupName = groupName ?? "AutoInjected";
        }

        public ServiceLifetimeType ServiceLifetimeType { get; }
        public Type? ImplementationBy { get; }

        /// <summary>
        /// Group name for the service or namespace
        /// </summary>
        public string GroupName { get; }
    }
}
