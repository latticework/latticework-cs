using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;


namespace Lw.Reflection
{
    public static class Extensions
    {
        #region EventInfo Extensions
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="handlerType"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">
        ///     <para>
        ///     <paramref name="methodName"/> was not found on <paramref name="handlerType"/>
        ///     </para><para>
        ///     -or-
        ///     </para><para>
        ///     <paramref name="methodName"/> is not <see langword="static"/>.
        ///     </para><para>
        ///     -or-
        ///     </para><para>
        ///     <paramref name="methodName"/> does not have the appropriate parameters for the referenced <see cref="EventInfo"/>.
        ///     </para>
        /// </exception>
        [DebuggerStepThrough]
        public static Delegate CreateDelegateInstance(
            this EventInfo reference, Type handlerType, string methodName)
        {
            Contract.Requires<ArgumentNullException>(handlerType != null, "handlerType");
            Contract.Requires<ArgumentNullException>(methodName != null, "methodName");

            return Extensions.CreateDelegateInstanceCore(reference, handlerType, null, methodName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="handlerObject"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">
        ///     <para>
        ///     <paramref name="methodName"/> was not found on <paramref name="handlerObject"/>
        ///     </para><para>
        ///     -or-
        ///     </para><para>
        ///     <paramref name="methodName"/> is not an instance method.
        ///     </para><para>
        ///     -or-
        ///     </para><para>
        ///     <paramref name="methodName"/> does not have the appropriate parameters for the referenced <see cref="EventInfo"/>.
        ///     </para>
        /// </exception>
        [DebuggerStepThrough]
        public static Delegate CreateDelegateInstance(
            this EventInfo reference, object handlerObject, string methodName)
        {
            Contract.Requires<ArgumentNullException>(handlerObject != null, "handlerObject");
            Contract.Requires<ArgumentNullException>(methodName != null, "methodName");

            return Extensions.CreateDelegateInstanceCore(reference, handlerObject.GetType(), handlerObject, methodName);
        }

        [DebuggerStepThrough]
        public static Type GetDelegateArgsType(this EventInfo reference)
        {
            return reference.RaiseMethod.GetParameters()[1].ParameterType; // TEventArgs always second parameter.
        }

        [DebuggerStepThrough]
        public static Type GetDelegateType(this EventInfo reference)
        {
            return reference.AddMethod.GetParameters()[0].ParameterType;
        }
        #endregion EventInfo Extensions

        #region MemberInfo Extensions
        [DebuggerStepThrough]
        public static string DeclaredFullName(this MemberInfo reference)
        {
            return reference.DeclaringType.FullName + reference.Name;
        }

#if !NETFX_CORE
        [DebuggerStepThrough]
        public static string ReflectedFullName(this MemberInfo reference)
        {
            return reference.ReflectedType.FullName + "." + reference.Name;
        }
#endif
        #endregion MemberInfo Extensions

        #region MethodBase Extensions
        [DebuggerStepThrough]
        public static PropertyInfo GetPropertyMethodProperty(this MethodBase method)
        {
            if (!method.IsSpecialName || !(method.Name.StartsWith("get_") && !method.Name.StartsWith("set_")))
            {
                return null;
            }

            string propertyName = method.Name.Substring(4);

            // TODO: Lw.Reflection.Extensions.GetPropertyMethodProperty: Do we need to specify binding flags to match method?
            // TODO: Lw.Reflection.Extensions.GetPropertyMethodProperty: Do we need to handle overloads?
            return method.DeclaringType.GetTypeInfo().GetDeclaredProperty(propertyName);
        }
        #endregion MethodBase Extensions

        #region Object Extensions
        [DebuggerStepThrough]
        public static Delegate AddEventDelegate(
            this object reference, string name, Type handlerType, string methodName)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(handlerType != null, "handlerType");
            Contract.Requires<ArgumentNullException>(methodName != null, "methodName");

            var eventInfo = Extensions.GetDeclaredEvent(reference.GetType(), reference, name);

            return AddEventDelegateCore(reference, eventInfo, handlerType, null, methodName);
        }

        public static Delegate AddEventDelegate(
            this object reference, string name, string methodName)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(methodName != null, "methodName");


            var eventInfo = Extensions.GetDeclaredEvent(reference.GetType(), reference, name);

            return AddEventDelegateCore(reference, eventInfo, reference.GetType(), reference, methodName);
        }

        public static Delegate AddEventDelegate(
            this object reference, string name, object handlerObject, string methodName)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(handlerObject != null, "handlerObject");
            Contract.Requires<ArgumentNullException>(methodName != null, "methodName");


            var eventInfo = Extensions.GetDeclaredEvent(reference.GetType(), reference, name);

            return AddEventDelegateCore(reference, eventInfo, handlerObject.GetType(), handlerObject, methodName);
        }

        [DebuggerStepThrough]
        public static Delegate AddStaticEventDelegate(
            this object reference, string name, string methodName)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(methodName != null, "methodName");

            var eventInfo = Extensions.GetDeclaredEvent(reference.GetType(), reference, name);

            return AddEventDelegateCore(reference, eventInfo, reference.GetType(), null, methodName);
        }


        [DebuggerStepThrough]
        public static T GetPropertyValue<T>(this object reference, string name)
        {
            return (T)GetPropertyValue(reference, name);
        }

        [DebuggerStepThrough]
        public static object GetPropertyValue(this object reference, string name)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");

            var propertyInfo = GetDeclaredProperty(reference.GetType(), reference, name);

            return propertyInfo.GetGetMethod().Invoke(reference, null);
        }


        [DebuggerStepThrough]
        public static object InvokeGenericMethod(
            this object reference, Type[] typeArguments, string name, params object[] args)
        {
            Contract.Requires<ArgumentNullException>(typeArguments != null, "typeArguments");
            Contract.Requires<ArgumentNullException>(name != null, "name");

            var method = GetDeclaredGenericMethod(reference.GetType(), reference, typeArguments, name);

            return method.Invoke(reference, args);
        }

        [DebuggerStepThrough]
        public static T InvokeGenericMethod<T>(
            this object reference, Type[] typeArguments, string name, params object[] args)
        {
            Contract.Requires<ArgumentNullException>(typeArguments != null, "typeArguments");
            Contract.Requires<ArgumentNullException>(name != null, "name");

            var method = GetDeclaredGenericMethod(reference.GetType(), reference, typeArguments, name);

            return (T)method.Invoke(reference, args);
        }



        [DebuggerStepThrough]
        public static object InvokeMethod(this object reference, string name, params object[] args)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");

            var method = GetDeclaredMethod(reference.GetType(), reference, name);

            return method.Invoke(null, args);
        }

        [DebuggerStepThrough]
        public static T InvokeMethod<T>(this object reference, string name, params object[] args)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");

            var method = GetDeclaredMethod(reference.GetType(), reference, name);

            return (T)method.Invoke(null, args);
        }


        [DebuggerStepThrough]
        public static void RemoveEventDelegate(
            this object reference, string name, string methodName)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(methodName != null, "methodName");

            var eventInfo = GetDeclaredEvent(reference.GetType(), reference, name);

            RemoveEventDelegateCore(
                reference, eventInfo, reference.GetType(), reference, methodName);
        }

        [DebuggerStepThrough]
        public static void RemoveEventDelegate(
            this object reference, string name, object handlerObject, string methodName)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(handlerObject != null, "handlerObject");
            Contract.Requires<ArgumentNullException>(methodName != null, "methodName");

            var eventInfo = GetDeclaredEvent(reference.GetType(), reference, name);

            RemoveEventDelegateCore(
                reference, eventInfo, handlerObject.GetType(), handlerObject, methodName);
        }

        [DebuggerStepThrough]
        public static void RemoveEventDelegate(this object reference, string name, Delegate eventDelegateInstance)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(eventDelegateInstance != null, "eventDelegateInstance");

            var eventInfo = reference.GetType().GetTypeInfo().GetDeclaredEvent(name);
            eventInfo.RemoveEventHandler(reference, eventDelegateInstance);
        }

        [DebuggerStepThrough]
        public static void RemoveStaticEventDelegate(
            this object reference, string name, string methodName)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(methodName != null, "methodName");

            var eventInfo = GetDeclaredEvent(reference.GetType(), reference, name);

            RemoveEventDelegateCore(reference, eventInfo, reference.GetType(), null, methodName);
        }

        [DebuggerStepThrough]
        public static void RemoveStaticEventDelegate(
            this object reference, string name, Type handlerType, string methodName)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(handlerType != null, "handlerType");
            Contract.Requires<ArgumentNullException>(methodName != null, "methodName");

            var eventInfo = GetDeclaredEvent(reference.GetType(), reference, name);

            RemoveEventDelegateCore(reference, eventInfo, handlerType, null, methodName);
        }


        [DebuggerStepThrough]
        public static void SetPropertyValue<T>(this object reference, string name, T value)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");

            var propertyInfo = GetDeclaredProperty(reference.GetType(), reference, name);

            propertyInfo.SetMethod.Invoke(reference, new object[] { value });
        }
        #endregion Object Extensions

        #region PropertyInfo Extensions
#if !NETFX_CORE
        [DebuggerStepThrough]
        public static string ReflectedFullName(this PropertyInfo reference)
        {
            return reference.ReflectedType.FullName + "." + reference.Name;
        }
#endif
        #endregion PropertyInfo Extensions

        #region Type Extensions
        [DebuggerStepThrough]
        public static Delegate AddEventDelegate(
            this Type reference, string name, string methodName)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(methodName != null, "methodName");

            var eventInfo = GetDeclaredEvent(reference, null, name);

            return AddEventDelegateCore(null, eventInfo, reference, null, methodName);
        }

        [DebuggerStepThrough]
        public static Delegate AddEventDelegate(
            this Type reference, string name, object handlerObject, string methodName)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(handlerObject != null, "handlerObject");
            Contract.Requires<ArgumentNullException>(methodName != null, "methodName");

            var eventInfo = GetDeclaredEvent(reference, null, name);

            return AddEventDelegateCore(null, eventInfo, handlerObject.GetType(), handlerObject, methodName);
        }

        [DebuggerStepThrough]
        public static Delegate AddStaticEventDelegate(
            this Type reference, string name, string methodName)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(methodName != null, "methodName");

            var eventInfo = GetDeclaredEvent(reference, null, name);

            return AddEventDelegateCore(null, eventInfo, reference, null, methodName);
        }

        [DebuggerStepThrough]
        public static Delegate AddStaticEventDelegate(
            this Type reference, string name, Type hanlderType, string methodName)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(hanlderType != null, "hanlderType");
            Contract.Requires<ArgumentNullException>(methodName != null, "methodName");

            var eventInfo = GetDeclaredEvent(reference, null, name);

            return AddEventDelegateCore(null, eventInfo, hanlderType, null, methodName);
        }


        [DebuggerStepThrough]
        public static T GetFieldValue<T>(this Type reference, string name)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");

            var fieldInfo = GetDeclaredField(reference, null, name);

            return (T)fieldInfo.GetValue(null);
        }

        public static IEnumerable<Type> GetClosedTypesForOpenType(this Type reference, Type openType)
        {
            Contract.Requires<ArgumentNullException>(openType != null, "openType");

            if (reference.GetTypeInfo().IsGenericTypeDefinition)
            {
                ExceptionOperations.ThrowArgumentException(() => reference);
            }

            if (!openType.GetTypeInfo().IsGenericTypeDefinition)
            {
                ExceptionOperations.ThrowArgumentException(() => openType);
            }

            if (reference.GetTypeInfo().IsGenericType && reference.GetGenericTypeDefinition() == openType)
            {
                return new Type[]{reference};
            }

            if (!openType.GetTypeInfo().IsInterface)
            {
                return new Type[]{};
            }

            return reference.GetTypeInfo()
                .ImplementedInterfaces
                .Where(it => it.GetTypeInfo().IsGenericType && it.GetGenericTypeDefinition() == openType);
        }

        [DebuggerStepThrough]
        public static T GetPropertyValue<T>(this Type reference, string name)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");

            var propertyInfo = GetDeclaredProperty(reference, null, name);

            return (T)propertyInfo.GetMethod.Invoke(null, null);
        }

        // http://www.kindblad.com/2010/08/05/c-how-to-check-if-a-type-has-a-defaultparameterless-constructor
        [DebuggerStepThrough]
        public static bool HasDefaultConstructor(this Type reference)
        {
            if (reference.GetTypeInfo().IsValueType) { return true; }

            // TODO: Lw.Reflection.Extensions.HasDefaultConstructor -- Test that you can't have more than one default constructor.
            return reference.GetTypeInfo().DeclaredConstructors.FirstOrDefault(ci => !ci.GetParameters().Any()) != null;
        }


        [DebuggerStepThrough]
        public static T InvokeGenericMethod<T>(
            this Type reference, Type[] typeArguments, string name, params object[] args)
        {
            return (T)InvokeGenericMethod(reference, typeArguments, name, args);
        }

        [DebuggerStepThrough]
        public static object InvokeGenericMethod(
            this Type reference, Type[] typeArguments, string name, params object[] args)
        {
            Contract.Requires<ArgumentNullException>(typeArguments != null, "typeArguments");
            Contract.Requires<ArgumentNullException>(name != null, "name");

            var method = GetDeclaredGenericMethod(reference, null, typeArguments, name);

            return method.Invoke(null, args);
        }


        [DebuggerStepThrough]
        public static bool Implements(this Type reference, Type interfaceType2)
        {
            Contract.Requires<ArgumentNullException>(interfaceType2 != null, "interfaceType2");

            if (!interfaceType2.GetTypeInfo().IsInterface)
            {
                ExceptionOperations.ThrowArgumentException(() => interfaceType2);
            }

            return reference.GetTypeInfo().ImplementedInterfaces.FirstOrDefault(it => it == interfaceType2) != null;
        }

        [DebuggerStepThrough]
        public static bool Implements<T>(this Type reference)
        {
            return Implements(reference, typeof(T));
        }


        [DebuggerStepThrough]
        public static object InvokeMethod(this Type reference, string name, params object[] args)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");

            var method = GetDeclaredMethod(reference, null, name);

            return method.Invoke(null, args);
        }

        [DebuggerStepThrough]
        public static T InvokeMethod<T>(this Type reference, string name, params object[] args)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");

            var method = GetDeclaredMethod(reference, null, name);

            return (T)method.Invoke(null, args);
        }


        // http://www.google.com/search?q=type+implements+open+generic+interface&sourceid=ie7&rls=com.microsoft:en-us:IE-SearchBox&ie=&oe=
        // http://tmont.com/blargh/2011/3/determining-if-an-open-generic-type-isassignablefrom-a-type
        [DebuggerStepThrough]
        public static bool Is(this Type reference, Type type2)
        {
            Contract.Requires<ArgumentNullException>(type2 != null, "type2");

            return
                reference == type2 ||
                reference.GetTypeInfo().IsSubclassOf(type2) ||
                (type2.GetTypeInfo().IsInterface &&
                    (
                    (reference.Implements(type2)) ||
                    (type2.GetTypeInfo().IsGenericType && reference.GetTypeInfo().ImplementedInterfaces
                        .Any(it => it.GetTypeInfo().IsGenericType && it.GetGenericTypeDefinition() == type2))
                    ));
        }

        /// <summary>
        ///     Determines whether the specified type is non nullable value type.
        /// </summary>
        /// <param name="type">
        ///     The type being inspected.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the specified type is non-nullable; otherwise, <see langword="false"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool IsNonNullableValueType(this Type type)
        {
            ExceptionOperations.VerifyNonNull(type, () => type);

            return type.GetTypeInfo().IsValueType
                && (!type.GetTypeInfo().IsGenericType || !typeof(Nullable<>).GetTypeInfo().IsAssignableFrom(
                type.GetTypeInfo().GetGenericTypeDefinition().GetTypeInfo()));
        }


        [DebuggerStepThrough]
        public static bool IsNot(this Type reference, Type type2)
        {
            Contract.Requires<ArgumentNullException>(type2 != null, "type2");

            return
                reference != type2 &&
                !reference.GetTypeInfo().IsSubclassOf(type2) &&
                (!type2.GetTypeInfo().IsInterface ||
                    (
                    (!reference.Implements(type2)) &&
                    (!type2.GetTypeInfo().IsGenericType || !reference.GetTypeInfo().ImplementedInterfaces
                        .Any(it => it.GetTypeInfo().IsGenericType && it.GetGenericTypeDefinition() == type2))
                    ));
        }


        [DebuggerStepThrough]
        public static void RemoveEventDelegate(
            this Type reference, string name, string methodName)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(methodName != null, "methodName");

            var eventInfo = GetDeclaredEvent(reference.GetType(), reference, name);

            RemoveEventDelegateCore(reference, eventInfo, reference, null, methodName);
        }

        [DebuggerStepThrough]
        public static void RemoveStaticEventDelegate(
            this Type reference, string name, Type handlerType, string methodName)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(handlerType != null, "handlerType");
            Contract.Requires<ArgumentNullException>(methodName != null, "methodName");

            var eventInfo = GetDeclaredEvent(reference.GetType(), reference, name);

            RemoveEventDelegateCore(reference, eventInfo, handlerType, null, methodName);
        }

        [DebuggerStepThrough]
        public static void RemoveEventDelegate(
            this Type reference, string name, object handlerObject, string methodName)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(handlerObject != null, "handlerObject");
            Contract.Requires<ArgumentNullException>(methodName != null, "methodName");

            var eventInfo = GetDeclaredEvent(reference.GetType(), reference, name);

            RemoveEventDelegateCore(
                reference, eventInfo, handlerObject.GetType(), handlerObject, methodName);
        }

        [DebuggerStepThrough]
        public static void RemoveStaticEventDelegate(
            this Type reference, string name, string methodName)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");
            Contract.Requires<ArgumentNullException>(methodName != null, "methodName");

            var eventInfo = GetDeclaredEvent(reference.GetType(), reference, name);

            RemoveEventDelegateCore(reference, eventInfo, reference, null, methodName);
        }


        [DebuggerStepThrough]
        public static void SetFieldValue<T>(this Type reference, string name, T value)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");

            var fieldInfo = GetDeclaredField(reference, null, name);

            fieldInfo.SetValue(null, value);
        }

        [DebuggerStepThrough]
        public static void SetPropertyValue<T>(this Type reference, string name, T value)
        {
            Contract.Requires<ArgumentNullException>(name != null, "name");

            var propertyInfo = GetDeclaredProperty(reference, null, name);

            propertyInfo.SetMethod.Invoke(null, new object[] { value });
        }
        #endregion Type Extensions

        #region TypeInfo Extensions
#if NETFX_CORE
        public static IEnumerable<MemberInfo> GetDeclaredMembers(this TypeInfo reference)
        {
            return 
                reference.DeclaredConstructors.Cast<MemberInfo>()
                .Concat(reference.DeclaredEvents.Cast<MemberInfo>())
                .Concat(reference.DeclaredFields.Cast<MemberInfo>())
                .Concat(reference.DeclaredMethods.Cast<MemberInfo>())
                .Concat(reference.DeclaredNestedTypes.Cast<MemberInfo>())
                .Concat(reference.DeclaredProperties.Cast<MemberInfo>());
        }
#endif
        #endregion TypeInfo Extensions

        #region Private Methods
        private static Delegate AddEventDelegateCore(
            object obj, 
            EventInfo eventInfo,
            Type handlerType, 
            object handlerObject, 
            string methodName)
        {
            var eventDelegateInstance = CreateDelegateInstanceCore(eventInfo, handlerType, handlerObject, methodName);

            eventInfo.AddEventHandler(obj, eventDelegateInstance);

            return eventDelegateInstance;
        }

        private static void RemoveEventDelegateCore(
            object obj, 
            EventInfo eventInfo, 
            Type handlerType, 
            object handlerObject, 
            string methodName)
        {
            Delegate eventDelegateInstance = CreateDelegateInstanceCore(eventInfo, handlerType, handlerObject, methodName);
            eventInfo.RemoveEventHandler(obj, eventDelegateInstance);
        }


        private static Delegate CreateDelegateInstanceCore(
            EventInfo eventInfo, 
            Type handlerType, 
            object handlerObject, 
            string methodName)
        {
            var eventHandlerMethod = Extensions.GetDeclaredMethod(handlerType, handlerObject, methodName);

            if (eventHandlerMethod == null || (eventHandlerMethod.IsStatic != (handlerObject == null)))
            {
                var messageFormat = (handlerObject == null)
                    ? Properties.Resources.Exception_ArgumentException_InstanceMethodNotFoundForType
                    : Properties.Resources.Exception_ArgumentException_StaticMethodNotFoundForType;

                ExceptionOperations.ThrowArgumentException(
                    () => methodName, messageFormat.DoFormat(handlerType, methodName));
            }

            var eventArgsType = eventInfo.GetDelegateArgsType();

            var methodParameters = eventHandlerMethod.GetParameters();
            if (methodParameters.Count() != 2
                || methodParameters[0].ParameterType != typeof(object)
                || methodParameters[1].ParameterType.IsNot(typeof(EventArgs)))
            {
                var message = Properties.Resources.Exception_ArgumentException_NotHandlerMethod.DoFormat(
                    handlerType, methodName, eventArgsType);

                ExceptionOperations.ThrowArgumentException(() => methodName, message);
            }

            if (methodParameters[1].ParameterType.IsGenericParameter)
            {
                eventHandlerMethod = eventHandlerMethod.MakeGenericMethod(eventArgsType);
            }

            return Extensions.CreateDelegateInstanceCore(eventInfo, handlerObject, eventHandlerMethod);
        }

        private static Delegate CreateDelegateInstanceCore(
            EventInfo eventInfo, object handlerObject, MethodInfo eventHandlerMethod)
        {
            var eventDelegateType = eventInfo.GetDelegateType();
            var eventArgsType = eventInfo.GetDelegateArgsType();

            Delegate eventDelegateInstance = eventHandlerMethod.CreateDelegate(eventDelegateType, handlerObject);

            return eventDelegateInstance;
        }

        private static EventInfo GetDeclaredEvent(Type type, object obj, string name)
        {
            return type.GetTypeInfo().GetDeclaredEvent(name);
        }

        private static FieldInfo GetDeclaredField(Type type, object obj, string name)
        {
            return type.GetTypeInfo().GetDeclaredField(name);
        }


        private static MethodInfo GetDeclaredGenericMethod(Type type, object obj, Type[] typeArguments, string name)
        {
            var method = type.GetTypeInfo().GetDeclaredMethod(name);
            ThrowNameArgumentExceptionIfNull(method);

            return method.MakeGenericMethod(typeArguments);
        }


        private static MethodInfo GetDeclaredMethod(Type type, object obj, string name)
        {
            return type.GetTypeInfo().GetDeclaredMethod(name);
        }


        private static PropertyInfo GetDeclaredProperty(Type type, object obj, string name)
        {
            return type.GetTypeInfo().GetDeclaredProperty(name);
        }

        private static void ThrowNameArgumentExceptionIfNull(MemberInfo memberInfo)
        {
            if ((object)memberInfo == null)
            {
                ExceptionOperations.ThrowArgumentException("name");
            }
        }
        #endregion Private Methods
    }
}
