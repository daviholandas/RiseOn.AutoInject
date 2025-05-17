namespace RiseOn.AutoInject
{
    /// <summary>
    /// Indicates that a class should be registered as a service in the dependency injection container with the
    /// specified lifetime and optional exclusivity.
    /// </summary>
    /// <remarks>This attribute is used to mark a class for automatic registration in the dependency injection
    /// container. It allows specifying the service's lifetime (e.g., transient, scoped, or singleton) and whether the
    /// service should be registered exclusively. Additional properties can be used to provide further details about the
    /// service registration, such as the namespace, associated interface or base class, collection grouping, or a
    /// unique key for differentiation.</remarks>
    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false)]
    public class InjectServiceAttribute : System.Attribute
    {
        /// <summary>
        /// Specifies that a service should be injected with the given lifetime and optional exclusivity.
        /// </summary>
        /// <param name="serviceLifetimeType">The lifetime of the service, such as transient, scoped, or singleton.</param>
        /// <param name="injectAlone">A value indicating whether the service should be injected exclusively.  If <see langword="true"/>, the
        /// service will be injected without other implementations of the same type.</param>
        public InjectServiceAttribute(ServiceLifetimeType serviceLifetimeType,
            bool injectAlone = false) { }

        /// <summary>
        /// Gets or sets the name of the collection used to group services marked with the attribute.
        /// This property allows categorization of services within the dependency injection container,
        /// enabling more organized management of service registrations.
        /// </summary>
        public string? CollectionName { get; set; }

        /// <summary>
        /// Gets or sets the namespace associated with the service class to be registered in the dependency injection container.
        /// This property allows specifying the namespace of the service to provide additional context or enable namespace-based
        /// filtering when managing service registrations.
        /// </summary>
        public string? CollectionNamespace { get; set; }

        /// <summary>
        /// Gets or sets the name of the service interface or base class that the marked class implements or inherits from.
        /// If specified, the dependency injection system will register the class as the implementation of the provided interface
        /// or base class, allowing for explicit association in the dependency graph.
        /// </summary>
        public System.Type? ImplementationOf { get; set; }

        /// <summary>
        /// Gets or sets the unique key associated with the service registration.
        /// This key is used to differentiate between multiple registrations of the same service type
        /// within the dependency injection container.
        /// </summary>
        public string? Key { get; set; }
    }
}