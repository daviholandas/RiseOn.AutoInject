#nullable enable
using System;

namespace RiseOn.AutoInject
{
    // TODO: CREATE THE KEYED ATTRIBUTE
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class InjectServiceAttribute : Attribute
    {
        /// <summary>
        ///  Mark a class for automatic registration in the dependency injection container with a specified service lifetime.
        /// </summary>
        /// <param name="serviceLifetimeType">The lifetime type of the service, indicating how the service will be instantiated (e.g., Singleton, Scoped, Transient).</param>
        /// <param name="implementationOf">Optional. Specifies the interface or base class that the implementation class should be registered as.</param>
        /// <param name="collectionName">Optional. Specifies the collection name to which the service should belong for dependency injection registration.</param>
        public InjectServiceAttribute(ServiceLifetimeType serviceLifetimeType,
            Type? implementationOf = null,
            string? collectionName = "AutoInjected")
        {
        }
    }
}