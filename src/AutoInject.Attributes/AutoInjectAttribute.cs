#nullable enable
using System;

namespace AutoInject.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class AutoInjectAttribute : Attribute
    {
        public AutoInjectAttribute(Type serviceInterface,
            ServiceLifetime serviceLifetime,
            string? groupName = "AutoInject")
        {
            ServiceInterface = serviceInterface;
            ServiceLifetime = serviceLifetime;
            GroupName = groupName;
        }

        public Type ServiceInterface { get; }
        public ServiceLifetime ServiceLifetime { get; }
        public string? GroupName { get; }
    }
}
