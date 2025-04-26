namespace RiseOn.AutoInject
{
    /// <summary>
    /// Represents the different lifetimes that can be assigned to a service for dependency injection.
    /// </summary>
    public enum ServiceLifetimeType
    {
        /// <summary>
        /// Specifies that the service will be registered as a singleton within the dependency injection container.
        /// </summary>
        /// <remarks>
        /// A singleton service is created once and shared across all dependencies and requests that reference it.
        /// It exists for the lifetime of the application.
        /// </remarks>
        Singleton,

        /// <summary>
        /// Specifies that the service will be registered with a scoped lifetime within the dependency injection container.
        /// </summary>
        /// <remarks>
        /// A scoped service is created once per request or unit of work, and is shared across all dependencies within that specific scope.
        /// It is disposed of at the end of the scope.
        /// </remarks>
        Scoped,

        /// <summary>
        /// Specifies that the service will be registered with a transient lifetime within the dependency injection container.
        /// </summary>
        /// <remarks>
        /// A transient service is created each time it is requested. It is not shared across components or requests.
        /// This is useful for lightweight, stateless services or services that require a new instance for each operation.
        /// </remarks>
        Transient
    }
}