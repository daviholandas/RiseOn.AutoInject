namespace RiseOn.AutoInject
{
    [System.AttributeUsage(System.AttributeTargets.Class, Inherited = false)]
    public class InjectServiceAttribute :System. Attribute
    {
        /// <summary>
        ///  Mark a class for automatic registration in the dependency injection container with a specified service lifetime.
        /// </summary>
        /// <param name="serviceLifetimeType">The lifetime type of the service, indicating how the service will be instantiated (e.g., Singleton, Scoped, Transient).</param>
        public InjectServiceAttribute(ServiceLifetimeType serviceLifetimeType) { }

        /// <summary>
        /// Gets or sets the namespace associated with the service class to be registered in the dependency injection container.
        /// This property allows specifying the namespace of the service to provide additional context or enable namespace-based
        /// filtering when managing service registrations.
        /// </summary>
        public string? ServicesNameSpace { get; set; }

        /// <summary>
        /// Gets or sets the name of the service interface or base class that the marked class implements or inherits from.
        /// If specified, the dependency injection system will register the class as the implementation of the provided interface
        /// or base class, allowing for explicit association in the dependency graph.
        /// </summary>
        public System.Type? ImplementationOf { get; set; }

        /// <summary>
        /// Gets or sets the name of the collection used to group services marked with the attribute.
        /// This property allows categorization of services within the dependency injection container,
        /// enabling more organized management of service registrations.
        /// </summary>
        public string? CollectionName { get; set; } = "AutoInjectedServices";

        /// <summary>
        /// Gets or sets the unique key associated with the service registration.
        /// This key is used to differentiate between multiple registrations of the same service type
        /// within the dependency injection container.
        /// </summary>
        public string? Key { get; set; }
    }
}