namespace RiseOn.AutoInject
{
    public sealed record ServiceInfo
    {
        public string? ServiceName { get; set; }
        public string? ServiceLifetime { get; set; }
        public string? ImplementationName { get; set; }
        public string? CollectionName { get; set; }
        public string? Namespace { get; set; }
    }
}