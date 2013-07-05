using Lw.Linq.Expressions;
using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;

namespace Lw
{
    public static class ExceptionOperations
    {
        #region Public Methods
        /// <summary>
        ///     Creates an <see cref="ArgumentException"/> with the specified argument name.
        /// </summary>
        /// <param name="paramName">
        ///     The name of the argument that is invalid.
        /// </param>
        [DebuggerStepThrough]
        public static ArgumentException CreateArgumentException(string paramName)
        {
            Contract.Requires<ArgumentNullException>(paramName != null, "paramName");

            return new ArgumentException(defaultArgumentExceptionMessage, paramName);
        }

        /// <summary>
        ///     Creates a standard description to log if a static constructor has failed.
        /// </summary>
        /// <param name="type">
        ///     The <see cref="Type"/> of the class whose static constructor threw an exception.
        /// </param>
        /// <param name="exception">
        ///     The exception that was thrown.
        /// </param>
        /// <returns>
        ///     A <see cref="String"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static string CreateStaticConstructorExceptionMessage(Type type, Exception exception)
        {
            Contract.Requires<ArgumentNullException>(type != null, "type");
            Contract.Requires<ArgumentNullException>(exception != null, "exception");

            // TODO: ExceptionOperations.CreateStaticConstructorExceptionMessage: See if an exception formatter can be leveraged.
            return Properties.Resources.Exception_InvalidOperation_StaticConstructorExcepted.DoFormat(
                type.FullName,
                exception.GetType().FullName,
                exception.Message,
                exception.StackTrace);
        }


        // TODO: ExceptionOperations.LogStaticConstructorException -- Implement
        ///// <summary>
        /////     Logs an <see cref="ApplicationMessage"/> to <see cref="Components.LogWriter"/> that 
        /////     indicates the static constructor of the specified type has thrown the specified exception. 
        /////     Adds the <see cref="ApplicationMessage"/> to the application message context.
        ///// </summary>
        ///// <param name="type">
        /////     The <see cref="Type"/> of the class whose static constructor threw an exception.
        ///// </param>
        ///// <param name="exception">
        /////     The exception that was thrown.
        ///// </param>
        ///// <remarks>
        /////     The static constructor of a class should catch and eat all exceptions. The catch block should 
        /////     call this method to record that an exception has occured.
        ///// </remarks>
        //[DebuggerStepThrough]
        //public static void LogStaticConstructorException(Type type, Exception exception)
        //{
        //    try
        //    {
        //        // TODO: ExceptionOperations.LogStaticConstructorException: See if an exception formatter can be leveraged.
        //        Components.LogWriter.Write(IimCoreMessages.CreateContextMessage(
        //            IimCoreMessages.CriticalMessageCode_InternalError,
        //            CreateStaticConstructorExceptionMessage(type, exception)));
        //    }
        //    catch { }
        //}


        /// <summary>
        ///     Throws an <see cref="ArgumentException"/>.
        /// </summary>
        /// <typeparam name="T">
        ///     The parameter type.
        /// </typeparam>
        /// <param name="paramExpr">
        ///     The parameter expression.
        /// </param>
        /// <param name="message">
        ///     The error message that explains the reason for the exception.
        /// </param>
        [DebuggerStepThrough]
        public static void ThrowArgumentException<T>(Expression<Func<T>> paramExpr, string message)
        {
            Contract.Requires<ArgumentNullException>(paramExpr != null, "paramExpr");
            Contract.Requires<ArgumentNullException>(message != null, "message");

            string paramName = LambdaExpressionOperations.FindMemberName(paramExpr);
            throw new ArgumentException(message, paramName);
        }

        /// <summary>
        ///     Throws an <see cref="ArgumentException"/>.
        /// </summary>
        /// <typeparam name="T">
        ///     The parameter type.
        /// </typeparam>
        /// <param name="paramExpr">
        ///     The parameter expression.
        /// </param>
        [DebuggerStepThrough]
        public static void ThrowArgumentException<T>(Expression<Func<T>> paramExpr)
        {
            Contract.Requires<ArgumentNullException>(paramExpr != null, "paramExpr");

            string paramName = LambdaExpressionOperations.FindMemberName(paramExpr);
            ThrowArgumentException(paramName);
        }

        /// <summary>
        ///     Throws an <see cref="ArgumentException"/>.
        /// </summary>
        /// <param name="paramName">
        ///     The name of the paramter that is invalid.
        /// </param>
        [DebuggerStepThrough]
        public static void ThrowArgumentException(string paramName)
        {
            Contract.Requires<ArgumentNullException>(paramName != null, "paramName");

            throw CreateArgumentException(paramName);
        }

        /// <summary>
        ///     Throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <typeparam name="T">
        ///     The parameter type.
        /// </typeparam>
        /// <param name="paramExpr">
        ///     The parameter expression.
        /// </param>
        /// <param name="actualValue">
        ///     The value of the argument that caused this exception.
        /// </param>
        [DebuggerStepThrough]
        public static void ThrowArgumentOutOfRangeException<T>(Expression<Func<T>> paramExpr, object actualValue)
        {
            Contract.Requires<ArgumentNullException>(paramExpr != null, "paramExpr");

            string paramName = LambdaExpressionOperations.FindMemberName(paramExpr);
            ThrowArgumentOutOfRangeException(paramName, actualValue);
        }

        /// <summary>
        ///     Throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <param name="paramName">
        ///     The name of the parameter that caused this exception.
        /// </param>
        /// <param name="actualValue">
        ///     The value of the argument that caused this exception.
        /// </param>
        [DebuggerStepThrough]
        public static void ThrowArgumentOutOfRangeException(string paramName, object actualValue)
        {
            Contract.Requires<ArgumentNullException>(paramName != null, "paramName");

            throw new ArgumentOutOfRangeException(
                paramName, actualValue, defaultArgumentOutOfRangeExceptionMessage);
        }



        /// <summary>
        ///     Throws an <see cref="ArgumentOutOfRangeException"/>.
        /// </summary>
        /// <typeparam name="T">
        ///     The parameter type.
        /// </typeparam>
        /// <param name="paramExpr">s
        ///     The parameter expression.
        /// </param>
        /// <param name="message">
        ///     The error message that explains the reason for the exception.
        /// </param>
        [DebuggerStepThrough]
        public static void ThrowArgumentOutOfRangeException<T>(Expression<Func<T>> paramExpr, string message)
        {
            Contract.Requires<ArgumentNullException>(paramExpr != null, "paramExpr");
            Contract.Requires<ArgumentNullException>(message != null, "message");

            string paramName = LambdaExpressionOperations.FindMemberName(paramExpr);
            throw new ArgumentOutOfRangeException(message, paramName);
        }

        /// <summary>
        ///     Throws a <see cref="ArgumentOutOfRangeException"/> with a message formatted for the specifed
        ///     <see cref="Enum"/> value.
        /// </summary>
        /// <typeparam name="TEnum">
        ///     The <see cref="Enum"/> type of the value out of range.
        /// </typeparam>
        /// <typeparam name="U">
        ///     An expression containing the parameter reference.
        /// </typeparam>
        /// <param name="enumValue">
        ///     The out of range value.
        /// </param>
        /// <param name="paramExpr">
        ///     The argument name that was out of range.
        /// </param>
        [DebuggerStepThrough]
        public static void ThrowEnumArgumentOutRange<TEnum, U>(TEnum enumValue, Expression<Func<U>> paramExpr)
            where TEnum : struct
        {
            Contract.Requires<ArgumentNullException>(paramExpr != null, "paramExpr");

            string paramName = LambdaExpressionOperations.FindMemberName(paramExpr);

            ThrowEnumArgumentOutRange<TEnum>(enumValue, paramName);
        }

        /// <summary>
        ///     Throws a <see cref="ArgumentOutOfRangeException"/> with a message formatted for the specifed
        ///     <see cref="Enum"/> value.
        /// </summary>
        /// <typeparam name="TEnum">
        ///     The <see cref="Enum"/> type of the value out of range.
        /// </typeparam>
        /// <param name="enumValue">
        ///     The out of range value.
        /// </param>
        /// <param name="paramName">
        ///     The argument name that was out of range.
        /// </param>
        [DebuggerStepThrough]
        public static void ThrowEnumArgumentOutRange<TEnum>(TEnum enumValue, string paramName)
            where TEnum : struct
        {
            {
                Contract.Requires<ArgumentNullException>(paramName != null, "paramName");

                EnumOperations.VerifyEnumType<TEnum>("TEnum");

                throw new ArgumentOutOfRangeException(
                    paramName,
                    enumValue,
                    Properties.Resources.ExceptionOperations_CreateEnumOutOfRangeMessage_EnumOutOfRanage
                        .DoFormat(enumValue.GetType().FullName, enumValue));
            }
        }

        /// <summary>
        ///     Throws a standard <see cref="InvalidOperationException"/> when the instance is in an 
        ///     invalid state.
        /// </summary>
        /// <param name="obj">
        ///     The type or the instance's type that is in an invalid state.
        /// </param>
        [DebuggerStepThrough]
        public static void ThrowInvalidOperationException<T>(T obj)
        {
            Contract.Requires<ArgumentNullException>(obj != null, "obj");

            ThrowInvalidOperationException(obj.GetType());
        }

        /// <summary>
        ///     Throws a standard <see cref="InvalidOperationException"/> when the type is in an 
        ///     invalid state.
        /// </summary>
        /// <typeparam name="T">
        ///     The type or the instance's type that is in an invalid state.
        /// </typeparam>
        [DebuggerStepThrough]
        public static void ThrowInvalidOperationException<T>()
        {
            ThrowInvalidOperationException(typeof(T));
        }

        /// <summary>
        ///     Throws a standard <see cref="InvalidOperationException"/> when the type or instance is in an 
        ///     invalid state.
        /// </summary>
        /// <param name="type">
        ///     The type or the instance's type that is in an invalid state.
        /// </param>
        [DebuggerStepThrough]
        public static void ThrowInvalidOperationException(Type type)
        {
            Contract.Requires<ArgumentNullException>(type != null, "type");

            throw new InvalidOperationException(
                Properties.Resources.Exception_InvalidOperation_NotInitialized.DoFormat(type.Name));
        }

        /// <summary>
        ///     Throws a standard <see cref="InvalidOperationException"/> when the type or instance is in an 
        ///     invalid state.
        /// </summary>
        /// <typeparam name="T">
        ///     The type or the type of the instance's that is in an invalid state.
        /// </typeparam>
        [DebuggerStepThrough]
        public static void ThrowStateIsInvalid<T>()
        {
            ThrowInvalidOperationException(typeof(T));
        }

        /// <summary>
        ///     Throws an exception if the string argument is null or empty.
        /// </summary>
        /// <param name="param">
        ///     The parameter value.
        /// </param>
        /// <param name="paramExpr">
        ///     The parameter value as a LINQ expression.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     The argument is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     The argument is the empty string.
        /// </exception>
        [DebuggerStepThrough]
        public static void VerifyNonEmpty(string param, Expression<Func<string>> paramExpr)
        {
            Contract.Requires<ArgumentNullException>(param != null, "param");
            Contract.Requires<ArgumentNullException>(paramExpr != null, "paramExpr");

            VerifyNonNull(param, paramExpr);

            if (param.Length == 0)
            {
                ThrowArgumentOutOfRangeException(paramExpr, string.Empty);
            }
        }

        /// <summary>
        ///     Throws a standard <see cref="ArgumentNullException"/> if an argument is 
        ///     <see langword="null"/>.
        /// </summary>
        /// <typeparam name="T">
        ///     The parameter type.
        /// </typeparam>
        /// <param name="param">
        ///     The parameter value.
        /// </param>
        /// <param name="paramExpr">
        ///     The parameter value as a LINQ expression.
        /// </param>
        [DebuggerStepThrough]
        public static void VerifyNonNull<T>(T param, Expression<Func<T>> paramExpr)
        {
            Contract.Requires<ArgumentNullException>(param != null, "param");
            Contract.Requires<ArgumentNullException>(paramExpr != null, "paramExpr");

            if (typeof(T).GetTypeInfo().IsValueType) { return; }

            if ((object)param == null)
            {
                string name = LambdaExpressionOperations.FindMemberName(paramExpr);

                throw new ArgumentNullException(name);
            }

        }
        #endregion Public Methods

        #region Private Constructors
        // Exceptions created to get messages only. They are not thrown.
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations")]
        static ExceptionOperations()
        {
            ExceptionOperations.defaultArgumentExceptionMessage = new ArgumentException().Message;
            ExceptionOperations.defaultArgumentOutOfRangeExceptionMessage = new ArgumentOutOfRangeException().Message;
        }
        #endregion Private Constructors

        #region Private Fields
        private static string defaultArgumentExceptionMessage;
        private static string defaultArgumentOutOfRangeExceptionMessage;
        #endregion Private Fields
    }
}
