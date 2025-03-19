namespace AutoInject.SourceGenerator
{
    public sealed record ServiceInfo
    {
        public ServiceInfo(string serviceName,
            string serviceLifetime,
            string implementationName,
            string groupName,
            string ns)
        {
            ServiceName = serviceName;
            ServiceLifetime = serviceLifetime;
            ImplementationName = implementationName;
            GroupName = groupName;
            Namespace = ns;
        }
        public readonly string ServiceName;
        public readonly string ServiceLifetime;
        public readonly string ImplementationName;
        public readonly string GroupName;
        public readonly string Namespace;
    }
}