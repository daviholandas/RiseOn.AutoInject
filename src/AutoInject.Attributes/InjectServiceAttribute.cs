#nullable enable
using System;
using AutoInject.Attributes;

namespace RiseOn.AutoInject
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class InjectServiceAttribute : Attribute
    {
        public InjectServiceAttribute(ServiceLifetime serviceLifetime,
            string? groupName = "AutoInjected",
            Type? implementationBy = null)
        {
            ServiceLifetime = serviceLifetime;
            GroupName = groupName ?? "AutoInjected";
            ImplementationBy = implementationBy;
        }

        public ServiceLifetime ServiceLifetime { get; }
        public Type? ImplementationBy { get; }

        /// <summary>
        /// Group name for the service or namespace
        /// </summary>
        public string GroupName { get; }
    }
}
