namespace AutoInject.SourceGenerator
{
    public readonly struct ServiceInfo
    {
        public ServiceInfo(string interfaceName,
            string implementationName,
            string serviceLifetime,
            string groupName)
        {
            InterfaceName = interfaceName;
            ImplementationName = implementationName;
            ServiceLifetime = serviceLifetime;
            GroupName = groupName;
        }

        public readonly string InterfaceName;
        public readonly string ImplementationName;
        public readonly string ServiceLifetime;
        public readonly string GroupName;
    }
}