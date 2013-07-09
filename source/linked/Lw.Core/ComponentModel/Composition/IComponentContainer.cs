using System;
using System.Collections.Generic;

#if !NETFX_CORE
namespace Lw.ComponentModel.Composition
#else
namespace Lw.Composition
#endif
{
    public interface IComponentContainer
    {
        #region Public Properites
        /// <summary>
        ///     Gets and sets the parent container.
        /// </summary>
        /// <value>
        ///     An <see cref="IComponentContainer"/> that is accessed when the current container cannot find a requested object or 
        ///     <see langword="null"/> if the container was created without a parent.
        /// </value>
        /// <exception cref="ArgumentNullException">
        ///     The value is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The value has already been set.
        /// </exception>
        IComponentContainer Parent { get; set; }
        #endregion Public Properites

        #region Public Methods
        /// <summary>
        ///     Gets all registered instances of the specified component type.
        /// </summary>
        /// <param name="componentType">
        ///     A <see cref="Type"/>.
        /// </param>
        /// <returns>
        ///     An <see cref="IEnumerable{T}"/> of all registered instances.
        /// </returns>
        /// <remarks>
        ///     All returned instances are convertable from <paramref name="componentType"/>.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        ///     The component container implementation threw an exception. See <see cref="Exception.InnerException"/> 
        ///     for details.
        /// </exception>
        IEnumerable<object> GetAllInstances(Type componentType);

        /// <summary>
        ///     Gets a value that determines whether the specified type is registered.
        /// </summary>
        /// <param name="typeToCheck">
        ///     The <see cref="Type"/> to check.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the specified type is registered with the container; otherwise, 
        ///     <see langword="false"/>.
        /// </returns>
        bool IsRegistered(Type typeToCheck);

        /// <summary>
        ///     Gets a value that determines whether the specified type is registered.
        /// </summary>
        /// <param name="typeToCheck">
        ///     The <see cref="Type"/> to check.
        /// </param>
        /// <param name="nameToCheck">
        ///     The registeration name to check.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the specified type is registered with the container; otherwise, 
        ///     <see langword="false"/>.
        /// </returns>
        bool IsRegistered(Type typeToCheck, string nameToCheck);


        /// <summary>
        ///     Registeres the specified registration and implementation types.
        /// </summary>
        /// <param name="registeredType">
        ///     The <see cref="Type"/> matched during instance resolution.
        /// </param>
        /// <param name="key">
        ///     A <see cref="string"/> selector for this registration. <see langword="null"/> if the default is to be 
        ///     matched.
        /// </param>
        /// <param name="implementationType">
        ///     The <see cref="Type"/> that will be activated.
        /// </param>
        /// <param name="policy">
        ///     The <see cref="ComponentLifetimePolicy"/> used to resolve instances.
        /// </param>
        void RegisterType(
            Type registeredType, string key, Type implementationType, ComponentLifetimePolicy policy);

        /// <summary>
        ///     Registers the specified registration type and impelmentation instance.
        /// </summary>
        /// <param name="registeredType">
        ///     The <see cref="Type"/> matched during instance resolution.
        /// </param>
        /// <param name="key">
        ///     A <see cref="string"/> selector for this registration. <see langword="null"/> if the default is to be 
        ///     matched.
        /// </param>
        /// <param name="instance">
        ///     The instance that will be resolved.
        /// </param>
        void RegisterInstance(Type registeredType, string key, object instance);

        /// <summary>
        ///     Get an instance of the given component Type.
        /// </summary>
        /// <param name="TComponent">
        ///     Type of object requested.
        /// </param>
        /// <param name="key">
        ///     Name that the object was registered with.
        /// </param>
        /// <param name="component">
        ///     The requested component instance or <see langword="null"/> if the component was not found.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the component was found; otherwise, <see langword="false"/>.
        /// </returns>
        bool TryGetInstance(Type TComponent, string key, out object component);
        #endregion Public Methods
    }
}
