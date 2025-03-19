#nullable enable
using System;

namespace AutoInject.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class AutoInjectAttribute : Attribute
    {
        public AutoInjectAttribute(ServiceLifetime serviceLifetime,
            string? groupName = "AutoInjected")
        {
            ServiceLifetime = serviceLifetime;
            ImplementationBy = null;
            GroupName = groupName ?? "AutoInjected";
        }

        public AutoInjectAttribute(ServiceLifetime serviceLifetime,
            Type? implementationBy,
            string? groupName)
        {
            ServiceLifetime = serviceLifetime;
            ImplementationBy = implementationBy;
            GroupName = groupName ?? "AutoInjected";
        }

        public ServiceLifetime ServiceLifetime { get; }
        public Type? ImplementationBy { get; }
        public string GroupName { get; }
    }
}
