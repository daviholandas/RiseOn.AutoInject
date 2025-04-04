﻿namespace AutoInject.SourceGenerator
{
    public sealed record ServiceInfo
    {
        public string ServiceName { get; set; }
        public string ServiceLifetime { get; set; }
        public string? ImplementationName { get; set; }
        public string GroupName { get; set; }
        public string Namespace { get; set; }
    }
}