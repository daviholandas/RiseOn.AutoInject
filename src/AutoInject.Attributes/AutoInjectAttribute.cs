#nullable enable

using System;

namespace AutoInject.Attributes
{
    public class AutoInjectAttribute : Attribute
    {
        public AutoInjectAttribute(Type serviceInterface, ServiceLifetime serviceLifetime, string groupName)
        {
            ServiceInterface = serviceInterface;
            ServiceLifetime = serviceLifetime;
            GroupName = groupName;
        }

        public Type ServiceInterface { get; }
        public ServiceLifetime ServiceLifetime { get; }
        public string GroupName { get; }
    }
}
