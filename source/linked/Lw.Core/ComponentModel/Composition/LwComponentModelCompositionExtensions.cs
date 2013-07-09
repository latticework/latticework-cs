using System;
using System.Collections.Generic;

#if !NETFX_CORE
namespace Lw.ComponentModel.Composition
#else
namespace Lw.Composition
#endif
{
    public static class Extensions
    {
        #region IComponentContainer Extensions
        public static IEnumerable<TComponent> GetAllInstances<TComponent>(this IComponentContainer container)
        {
            foreach (object current in container.GetAllInstances(typeof(TComponent)))
            {
                yield return (TComponent)current;
            }
        }

        /// <summary>
        ///     Returns an instance registered with the referenced container.
        /// </summary>
        /// <typeparam name="TComponent">
        ///     The type of the instance to return.
        /// </typeparam>
        /// <param name="container">
        ///     An <see cref="IComponentContainer"/>.
        /// </param>
        /// <returns>
        ///     An instance of the specifed type.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     There is no registration for <typeparamref name="TComponent"/> that has no key.
        /// </exception>
        public static TComponent GetInstance<TComponent>(this IComponentContainer container)
        {
            return (TComponent)container.GetInstance(typeof(TComponent), null);
        }

        public static TComponent GetInstance<TComponent>(this IComponentContainer container, string key)
        {
            return (TComponent)container.GetInstance(typeof(TComponent), key);
        }

        public static object GetInstance(this IComponentContainer container, Type componentType)
        {
            return GetInstance(container, componentType, null);
        }

        public static object GetInstance(this IComponentContainer container, Type componentType, string key)
        {
            object value;
            if (!container.TryGetInstance(componentType, key, out value))
            {
                throw new InvalidOperationException(
                    Properties.Resources.ERROR_Extensions_CantResolveType.DoFormat(componentType.FullName));
            }

            return value;
        }


        public static TRegisteredComponent GetRegisteredPerCallInstance<TRegisteredComponent>(
            this IComponentContainer container)
        {
            return (TRegisteredComponent)GetRegisteredPerCallInstance(container, typeof(TRegisteredComponent));
        }

        public static object GetRegisteredPerCallInstance(
            this IComponentContainer container, Type registeredComponentType)
        {
            return GetRegisteredPerCallInstance(container, null, registeredComponentType);
        }

        public static object GetRegisteredPerCallInstance(
            this IComponentContainer container, string key, Type registeredComponentType)
        {
            bool registering = !container.IsRegistered(registeredComponentType);

            if (registering)
            {
                container.RegisterType(
                    registeredComponentType, key, registeredComponentType, ComponentLifetimePolicy.PerCall);
            }

            object implementation = container.GetInstance(registeredComponentType);
            return implementation;
        }


        public static TRegisteredComponent GetRegisteredTypeInstance<TRegisteredComponent>(
            this IComponentContainer container, Action<TRegisteredComponent> initializer)
        {
            return (TRegisteredComponent)GetRegisteredTypeInstance(
                container, typeof(TRegisteredComponent), obj => { initializer((TRegisteredComponent)obj); });
        }

        public static object GetRegisteredTypeInstance(
            this IComponentContainer container, Type registeredComponentType, Action<object> initializer)
        {
            return GetRegisteredTypeInstance(container, null, registeredComponentType, initializer);
        }

        public static object GetRegisteredTypeInstance(
            this IComponentContainer container, string key, Type registeredComponentType, Action<object> initializer)
        {
            bool registering = !container.IsRegistered(registeredComponentType);

            if (registering)
            {
                container.RegisterType(
                    registeredComponentType, key, registeredComponentType, ComponentLifetimePolicy.Singleton);
            }

            object implementation = container.GetInstance(registeredComponentType);

            if (registering && initializer != null)
            {
                initializer(implementation);
            }

            return implementation;
        }


        public static bool IsRegistered<T>(this IComponentContainer container)
        {
            return container.IsRegistered(typeof(T));
        }

        public static bool IsRegistered<T>(this IComponentContainer container, string nameToCheck)
        {
            return container.IsRegistered(typeof(T), nameToCheck);
        }


        /// <summary>
        ///     Registers the specified registration type and impelmentation instance.
        /// </summary>
        /// <typeparam name="TRegisteredComponent">
        ///     The type matched during instance resolution.
        /// </typeparam>
        /// <param name="container">
        ///     The <see cref="IComponentContainer"/> for the operation.
        /// </param>
        /// <param name="instance">
        ///     The instance that will be resolved.
        /// </param>
        public static void RegisterInstance<TRegisteredComponent>(
            this IComponentContainer container, TRegisteredComponent instance)
        {
            container.RegisterInstance(typeof(TRegisteredComponent), null, instance);
        }

        /// <summary>
        ///     Registers the specified registration type and impelmentation instance.
        /// </summary>
        /// <typeparam name="TRegisteredComponent">
        ///     The type matched during instance resolution.
        /// </typeparam>
        /// <param name="container">
        ///     The <see cref="IComponentContainer"/> for the operation.
        /// </param>
        /// <param name="key">
        ///     A <see cref="string"/> selector for this registration. <see langword="null"/> if the default is to be 
        ///     matched.
        /// </param>
        /// <param name="instance">
        ///     The instance that will be resolved.
        /// </param>
        public static void RegisterInstance<TRegisteredComponent>(
            this IComponentContainer container, string key, TRegisteredComponent instance)
        {
            container.RegisterInstance(typeof(TRegisteredComponent), key, instance);
        }

        /// <summary>
        ///     Registers the specified registration type and impelmentation instance.
        /// </summary>
        /// <param name="container">
        ///     The <see cref="IComponentContainer"/> for the operation.
        /// </param>
        /// <param name="registeredType">
        ///     The <see cref="Type"/> matched during instance resolution.
        /// </param>
        /// <param name="instance">
        ///     The instance that will be resolved.
        /// </param>
        public static void RegisterInstance(this IComponentContainer container, Type registeredType, object instance)
        {
            container.RegisterInstance(registeredType, null, instance);
        }



        /// <summary>
        ///     Registeres the specified registration and implementation types.
        /// </summary>
        /// <typeparam name="TRegisteredComponent">
        ///     The type matched during instance resolution.
        /// </typeparam>
        /// <param name="container">
        ///     The <see cref="IComponentContainer"/> for the operation.
        /// </param>
        /// <remarks>
        ///     By default, the registration uses the <see cref="ComponentLifetimePolicy.PerCall"/> lifetime policy.
        /// </remarks>
        public static void RegisterType<TRegisteredComponent>(this IComponentContainer container)
        {
            RegisterType<TRegisteredComponent>(container, null);
        }

        /// <summary>
        ///     Registeres the specified registration and implementation types.
        /// </summary>
        /// <typeparam name="TRegisteredComponent">
        ///     The type matched during instance resolution.
        /// </typeparam>
        /// <param name="container">
        ///     The <see cref="IComponentContainer"/> for the operation.
        /// </param>
        /// <param name="policy">
        ///     The <see cref="ComponentLifetimePolicy"/> used to resolve instances.
        /// </param>
        public static void RegisterType<TRegisteredComponent>(
            this IComponentContainer container, ComponentLifetimePolicy policy)
        {
            RegisterType<TRegisteredComponent>(container, null, policy);
        }

        /// <summary>
        ///     Registeres the specified registration type.
        /// </summary>
        /// <typeparam name="TRegisteredComponent">
        ///     The type matched during instance resolution.
        /// </typeparam>
        /// <param name="container">
        ///     The <see cref="IComponentContainer"/> for the operation.
        /// </param>
        /// <param name="key">
        ///     A <see cref="string"/> selector for this registration. <see langword="null"/> if the default is to be 
        ///     matched.
        /// </param>
        /// <remarks>
        ///     By default, the registration uses the <see cref="ComponentLifetimePolicy.PerCall"/> lifetime policy.
        /// </remarks>
        public static void RegisterType<TRegisteredComponent>(
            this IComponentContainer container, string key)
        {
            RegisterType<TRegisteredComponent>(container, key, ComponentLifetimePolicy.PerCall);
        }

        /// <summary>
        ///     Registeres the specified registration type.
        /// </summary>
        /// <typeparam name="TRegisteredComponent">
        ///     The type matched during instance resolution.
        /// </typeparam>
        /// <param name="container">
        ///     The <see cref="IComponentContainer"/> for the operation.
        /// </param>
        /// <param name="key">
        ///     A <see cref="string"/> selector for this registration. <see langword="null"/> if the default is to be 
        ///     matched.
        /// </param>
        /// <param name="policy">
        ///     The <see cref="ComponentLifetimePolicy"/> used to resolve instances.
        /// </param>
        public static void RegisterType<TRegisteredComponent>(
            this IComponentContainer container, string key, ComponentLifetimePolicy policy)
        {
            RegisterType<TRegisteredComponent, TRegisteredComponent>(container, null, policy);
        }


        /// <summary>
        ///     Registeres the specified registration and implementation types.
        /// </summary>
        /// <typeparam name="TRegisteredComponent">
        ///     The type matched during instance resolution.
        /// </typeparam>
        /// <typeparam name="TImplementationComponent">
        ///     The type that will be activated.
        /// </typeparam>
        /// <param name="container">
        ///     The <see cref="IComponentContainer"/> for the operation.
        /// </param>
        /// <remarks>
        ///     By default, the registration uses the <see cref="ComponentLifetimePolicy.PerCall"/> lifetime policy.
        /// </remarks>
        public static void RegisterType<TRegisteredComponent, TImplementationComponent>(
            this IComponentContainer container)
            where TImplementationComponent : TRegisteredComponent
        {
            RegisterType<TRegisteredComponent, TImplementationComponent>(container, null);
        }

        /// <summary>
        ///     Registeres the specified registration and implementation types.
        /// </summary>
        /// <typeparam name="TRegisteredComponent">
        ///     The type matched during instance resolution.
        /// </typeparam>
        /// <typeparam name="TImplementationComponent">
        ///     The type that will be activated.
        /// </typeparam>
        /// <param name="container">
        ///     The <see cref="IComponentContainer"/> for the operation.
        /// </param>
        /// <param name="policy">
        ///     The <see cref="ComponentLifetimePolicy"/> used to resolve instances.
        /// </param>
        public static void RegisterType<TRegisteredComponent, TImplementationComponent>(
            this IComponentContainer container, ComponentLifetimePolicy policy)
            where TImplementationComponent : TRegisteredComponent
        {
            RegisterType<TRegisteredComponent, TImplementationComponent>(container, null, policy);
        }

        /// <summary>
        ///     Registeres the specified registration and implementation types.
        /// </summary>
        /// <typeparam name="TRegisteredComponent">
        ///     The type matched during instance resolution.
        /// </typeparam>
        /// <typeparam name="TImplementationComponent">
        ///     The type that will be activated.
        /// </typeparam>
        /// <param name="container">
        ///     The <see cref="IComponentContainer"/> for the operation.
        /// </param>
        /// <param name="key">
        ///     A <see cref="string"/> selector for this registration. <see langword="null"/> if the default is to be 
        ///     matched.
        /// </param>
        /// <remarks>
        ///     By default, the registration uses the <see cref="ComponentLifetimePolicy.PerCall"/> lifetime policy.
        /// </remarks>
        public static void RegisterType<TRegisteredComponent, TImplementationComponent>(
            this IComponentContainer container, string key)
            where TImplementationComponent : TRegisteredComponent
        {
            container.RegisterType(
                typeof(TRegisteredComponent), key, typeof(TImplementationComponent), ComponentLifetimePolicy.PerCall);
        }

        /// <summary>
        ///     Registeres the specified registration and implementation types.
        /// </summary>
        /// <typeparam name="TRegisteredComponent">
        ///     The type matched during instance resolution.
        /// </typeparam>
        /// <typeparam name="TImplementationComponent">
        ///     The type that will be activated.
        /// </typeparam>
        /// <param name="container">
        ///     The <see cref="IComponentContainer"/> for the operation.
        /// </param>
        /// <param name="key">
        ///     A <see cref="string"/> selector for this registration. <see langword="null"/> if the default is to be 
        ///     matched.
        /// </param>
        /// <param name="policy">
        ///     The <see cref="ComponentLifetimePolicy"/> used to resolve instances.
        /// </param>
        public static void RegisterType<TRegisteredComponent, TImplementationComponent>(
            this IComponentContainer container, string key, ComponentLifetimePolicy policy)
            where TImplementationComponent : TRegisteredComponent
        {
            container.RegisterType(typeof(TRegisteredComponent), null, typeof(TImplementationComponent), policy);
        }



        /// <summary>
        ///     Registeres the specified registration and implementation types.
        /// </summary>
        /// <param name="container">
        ///     The <see cref="IComponentContainer"/> for the operation.
        /// </param>
        /// <param name="registeredType">
        ///     The <see cref="Type"/> matched during instance resolution.
        /// </param>
        /// <remarks>
        ///     By default, the registration uses the <see cref="ComponentLifetimePolicy.PerCall"/> lifetime policy.
        /// </remarks>
        public static void RegisterType(this IComponentContainer container, Type registeredType)
        {
            RegisterType(container, registeredType, null, registeredType);
        }

        /// <summary>
        ///     Registeres the specified registration and implementation types.
        /// </summary>
        /// <param name="container">
        ///     The <see cref="IComponentContainer"/> for the operation.
        /// </param>
        /// <param name="registeredType">
        ///     The <see cref="Type"/> matched during instance resolution.
        /// </param>
        /// <param name="policy">
        ///     The <see cref="ComponentLifetimePolicy"/> used to resolve instances.
        /// </param>
        public static void RegisterType(
            this IComponentContainer container, Type registeredType, ComponentLifetimePolicy policy)
        {
            container.RegisterType(registeredType, null, registeredType, policy);
        }

        /// <summary>
        ///     Registeres the specified registration type.
        /// </summary>
        /// <param name="container">
        ///     The <see cref="IComponentContainer"/> for the operation.
        /// </param>
        /// <param name="registeredType">
        ///     The <see cref="Type"/> matched during instance resolution.
        /// </param>
        /// <param name="key">
        ///     A <see cref="string"/> selector for this registration. <see langword="null"/> if the default is to be 
        ///     matched.
        /// </param>
        /// <remarks>
        ///     By default, the registration uses the <see cref="ComponentLifetimePolicy.PerCall"/> lifetime policy.
        /// </remarks>
        public static void RegisterType(
            this IComponentContainer container, Type registeredType, string key)
        {
            container.RegisterType(registeredType, null, registeredType, ComponentLifetimePolicy.PerCall);
        }

        /// <summary>
        ///     Registeres the specified registration type.
        /// </summary>
        /// <param name="container">
        ///     The <see cref="IComponentContainer"/> for the operation.
        /// </param>
        /// <param name="registeredType">
        ///     The <see cref="Type"/> matched during instance resolution.
        /// </param>
        /// <param name="key">
        ///     A <see cref="string"/> selector for this registration. <see langword="null"/> if the default is to be 
        ///     matched.
        /// </param>
        /// <param name="policy">
        ///     The <see cref="ComponentLifetimePolicy"/> used to resolve instances.
        /// </param>
        public static void RegisterType(
            this IComponentContainer container, Type registeredType, string key, ComponentLifetimePolicy policy)
        {
            container.RegisterType(registeredType, null, registeredType, policy);
        }


        /// <summary>
        ///     Registeres the specified registration and implementation types.
        /// </summary>
        /// <param name="container">
        ///     The <see cref="IComponentContainer"/> for the operation.
        /// </param>
        /// <param name="registeredType">
        ///     The <see cref="Type"/> matched during instance resolution.
        /// </param>
        /// <param name="implementationType">
        ///     The <see cref="Type"/> that will be activated.
        /// </param>
        /// <remarks>
        ///     By default, the registration uses the <see cref="ComponentLifetimePolicy.PerCall"/> lifetime policy.
        /// </remarks>
        public static void RegisterType(
            this IComponentContainer container, Type registeredType, Type implementationType)
        {
            RegisterType(container, registeredType, null, implementationType);
        }

        /// <summary>
        ///     Registeres the specified registration and implementation types.
        /// </summary>
        /// <param name="container">
        ///     The <see cref="IComponentContainer"/> for the operation.
        /// </param>
        /// <param name="registeredType">
        ///     The <see cref="Type"/> matched during instance resolution.
        /// </param>
        /// <param name="implementationType">
        ///     The <see cref="Type"/> that will be activated.
        /// </param>
        /// <param name="policy">
        ///     The <see cref="ComponentLifetimePolicy"/> used to resolve instances.
        /// </param>
        public static void RegisterType(
            this IComponentContainer container, 
            Type registeredType, 
            Type implementationType, 
            ComponentLifetimePolicy policy)
        {
            container.RegisterType(registeredType, null, implementationType, policy);
        }

        /// <summary>
        ///     Registeres the specified registration and implementation types.
        /// </summary>
        /// <param name="container">
        ///     The <see cref="IComponentContainer"/> for the operation.
        /// </param>
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
        /// <remarks>
        ///     By default, the registration uses the <see cref="ComponentLifetimePolicy.PerCall"/> lifetime policy.
        /// </remarks>
        public static void RegisterType(
            this IComponentContainer container, Type registeredType, string key, Type implementationType)
        {
            container.RegisterType(registeredType, null, implementationType, ComponentLifetimePolicy.PerCall);
        }

        public static TRegisteredComponent RegisterTypeInstance<TRegisteredComponent>(
            this IComponentContainer container)
        {
            return RegisterTypeInstance<TRegisteredComponent>(container, null, null);
        }

        public static TRegisteredComponent RegisterTypeInstance<TRegisteredComponent>(
            this IComponentContainer container, string key)
        {
            return RegisterTypeInstance<TRegisteredComponent>(container, key, null);
        }

        public static TRegisteredComponent RegisterTypeInstance<TRegisteredComponent>(
            this IComponentContainer container, Action<TRegisteredComponent> initializer)
        {
            return RegisterTypeInstance<TRegisteredComponent, TRegisteredComponent>(container, initializer);
        }

        public static TRegisteredComponent RegisterTypeInstance<TRegisteredComponent>(
            this IComponentContainer container, string key, Action<TRegisteredComponent> initializer)
        {
            return RegisterTypeInstance<TRegisteredComponent, TRegisteredComponent>(container, key, initializer);
        }

        public static TImplementationComponent RegisterTypeInstance<TRegisteredComponent, TImplementationComponent>(
            this IComponentContainer container)
            where TImplementationComponent : TRegisteredComponent
        {
            return RegisterTypeInstance<TRegisteredComponent, TImplementationComponent>(container, null, null);
        }

        public static TImplementationComponent RegisterTypeInstance<TRegisteredComponent, TImplementationComponent>(
            this IComponentContainer container, string key)
            where TImplementationComponent : TRegisteredComponent
        {
            return RegisterTypeInstance<TRegisteredComponent, TImplementationComponent>(container, key, null);
        }

        public static TImplementationComponent RegisterTypeInstance<TRegisteredComponent, TImplementationComponent>(
            this IComponentContainer container, Action<TImplementationComponent> initializer)
            where TImplementationComponent : TRegisteredComponent
        {
            return RegisterTypeInstance<TRegisteredComponent, TImplementationComponent>(container, null, initializer);
        }

        public static TImplementationComponent RegisterTypeInstance<TRegisteredComponent, TImplementationComponent>(
            this IComponentContainer container, string key, Action<TImplementationComponent> initializer)
            where TImplementationComponent : TRegisteredComponent
        {
            container.RegisterType<TRegisteredComponent, TImplementationComponent>(key, ComponentLifetimePolicy.Singleton);

            var implementation = (TImplementationComponent)container.GetInstance<TRegisteredComponent>();

            if (initializer != null)
            {
                initializer(implementation);
            }

            return implementation;
        }

        public static object RegisterTypeInstance(
            this IComponentContainer container, 
            Type registeredComponentType, 
            Type implementationComponentType, 
            string key, 
            Action<object> initializer)
        {
            container.RegisterType(
                registeredComponentType, key, implementationComponentType, ComponentLifetimePolicy.Singleton);

            object implementation = container.GetInstance(registeredComponentType);

            if (initializer != null)
            {
                initializer(implementation);
            }

            return implementation;
        }



        /// <summary>
        ///     Get an instance of the given <typeparamref name="TComponent"/>.
        /// </summary>
        /// <typeparam name="TComponent">
        ///     Type of object requested.
        /// </typeparam>
        /// <param name="container">
        ///     The <see cref="IComponentContainer"/> for the operation.
        /// </param>
        /// <param name="component">
        ///     The requested component instance 
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the component was found; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryGetInstance<TComponent>(this IComponentContainer container, out TComponent component)
        {
            return TryGetInstance(container, null, out component);
        }

        /// <summary>
        ///     Get an instance of the given named <typeparamref name="TComponent"/>.
        /// </summary>
        /// <typeparam name="TComponent">
        ///     Type of object requested.
        /// </typeparam>
        /// <param name="container">
        ///     The <see cref="IComponentContainer"/> for the operation.
        /// </param>
        /// <param name="key">
        ///     Name the object was registered with.
        /// </param>
        /// <param name="component">
        ///     The requested component instance or <see langword="null"/> if the component was not found.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the component was found; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryGetInstance<TComponent>(
            this IComponentContainer container, string key, out TComponent component)
        {
            object value = default(TComponent);
            bool succeeded = container.TryGetInstance(typeof(TComponent), key, out value);

            component = (TComponent)value;

            return succeeded;
        }

        /// <summary>
        ///     Get an instance of the given component Type.
        /// </summary>
        /// <param name="container">
        ///     The <see cref="IComponentContainer"/> for the operation.
        /// </param>
        /// <param name="componentType">
        ///     Type of object requested.
        /// </param>
        /// <param name="component">
        ///     The requested component instance or <see langword="null"/> if the component was not found.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the component was found; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool TryGetInstance(this IComponentContainer container, Type componentType, out object component)
        {
            return container.TryGetInstance(componentType, null, out component);
        }
        #endregion IComponentContainer Extensions
    }
}
