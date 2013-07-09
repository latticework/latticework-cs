using System;
//using Lw.ApplicationMessages;

using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Lw.Diagnostics
{
    public static class Extensions
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        // ILogWriter
        //public static void Write(this ILogWriter writer, params ApplicationMessage[] messages)
        //{
        //    Write(writer, (IEnumerable<ApplicationMessage>)messages);
        //}

        //public static void Write(this ILogWriter writer, IEnumerable<ApplicationMessage> messages)
        //{
        //    writer.Write(new LogItem(messages));
        //}

        #region IExceptionManager extensions
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        // IExceptionManager
        [DebuggerStepThrough]
        public static void DoWithExceptionHandling(this IExceptionManager reference,
            string policyName, Action statements)
        {
            Operations.DoWithin(statements, s =>
            {
                try
                {
                    statements();
                }
                catch (Exception exception)
                {
                    // exception.Data.Add("PolicyName", policyName);

                    Exception exceptionToThrow;

                    bool shouldRethrow = reference.HandleException(exception, policyName, out exceptionToThrow);

                    if (shouldRethrow)
                    {
                        if (exceptionToThrow != null)
                        {
                            throw exceptionToThrow;
                        }
                        else
                        {
                            throw;
                        }

                        // This logic would be with Custom Exception Handler to avoid logging twice and 
                        // policy set as ThrowNewException in web config.
                        //if (exceptionToThrow.Data.Contains("IsHandled"))
                        //{
                        //    throw;
                        //}
                        //else
                        //{
                        //    throw exceptionToThrow;
                        //}
                    }
                }
            });
        }

        /// <summary>
        ///     
        /// </summary>
        /// <param name="exceptionManager">
        ///     The exception manager.
        /// </param>
        /// <param name="exceptionToHandle">
        ///     An <see cref="Exception"/> object.
        /// </param>
        /// <param name="policyName">
        ///     The name of the policy to handle.
        /// </param>
        /// <param name="exceptionToThrow">
        ///     The replacement <see cref="Exception"/> to throw, if any.
        /// </param>
        /// <returns>
        ///     Whether or not a rethrow is recommended. 
        /// </returns>
        /// <remarks>
        ///     If a rethrow is recommended and exceptionToThrow is <see langword="null"/>, then the original 
        ///     exception <paramref name="exceptionToHandle"/> should be rethrown; otherwise, the exception returned 
        ///     in <paramref name="exceptionToThrow"/> should be thrown. 
        /// </remarks>
        [DebuggerStepThrough]
        public static bool HandleException(
            this IExceptionManager exceptionManager, 
            Exception exceptionToHandle, 
            string policyName, 
            out Exception exceptionToThrow)
        {
            ExceptionOperations.VerifyNonNull(exceptionManager, () => exceptionManager);

            try
            {
                bool shouldRethrow = exceptionManager.HandleException(exceptionToHandle, policyName);
                exceptionToThrow = null;
                return shouldRethrow;
            }
            catch (Exception exception)
            {
                exceptionToThrow = exception;
                return true;
            }
        }

        /// <summary>
        ///     Excecutes the supplied delegate action and handles any thrown exception according
        ///     to the rules configured for policyName.
        /// </summary>
        /// <param name="exceptionManager">
        ///     The exception manager.
        /// </param>
        /// <param name="action">
        ///     The delegate to execute.
        /// </param>
        /// <param name="policyName">
        ///     The name of the policy to handle.
        /// </param>
        [DebuggerStepThrough]
        public static void Process(this IExceptionManager exceptionManager, Action action, string policyName)
        {
            ExceptionOperations.VerifyNonNull(exceptionManager, () => exceptionManager);
            ExceptionOperations.VerifyNonNull(action, () => action);
            ExceptionOperations.VerifyNonNull(policyName, () => policyName);

            try
            {
                action();
            }
            catch (Exception e)
            {
                if (exceptionManager.HandleException(e, policyName))
                {
                    throw;
                }
            }
        }


        /// <summary>
        ///     Excecutes the supplied delegate action and handles any thrown exception according
        ///     to the rules configured for policyName.
        /// </summary>
        /// <typeparam name="TResult">
        ///     The type returned by the action function.
        /// </typeparam>
        /// <param name="exceptionManager">
        ///     The exception manager.
        /// </param>
        /// <param name="action">
        ///     The delegate to execute.
        /// </param>
        /// <param name="policyName">
        ///     The name of the policy to handle.
        /// </param>
        /// <returns>
        ///     The result of the action.
        /// </returns>
        [DebuggerStepThrough]
        public static TResult Process<TResult>(
            this IExceptionManager exceptionManager, Func<TResult> action, string policyName)
        {
            return Process(exceptionManager, action, default(TResult), policyName);
        }

        /// <summary>
        ///     Excecutes the supplied delegate action and handles any thrown exception according
        ///     to the rules configured for policyName.
        /// </summary>
        /// <typeparam name="TResult">
        ///     The type returned by the action function.
        /// </typeparam>
        /// <param name="exceptionManager">
        ///     The exception manager.
        /// </param>
        /// <param name="action">
        ///     The delegate to execute.
        /// </param>
        /// <param name="defaultResult">
        ///     The value to return if an exception occurs but is handled by the exception policy.
        /// </param>
        /// <param name="policyName">
        ///     The name of the policy to handle.
        /// </param>
        /// <returns>
        ///     The result of the action.
        /// </returns>
        [DebuggerStepThrough]
        public static TResult Process<TResult>(
            this IExceptionManager exceptionManager, Func<TResult> action, TResult defaultResult, string policyName)
        {
            ExceptionOperations.VerifyNonNull(exceptionManager, () => exceptionManager);
            ExceptionOperations.VerifyNonNull(action, () => action);
            ExceptionOperations.VerifyNonNull(policyName, () => policyName);

            try
            {
                return action();
            }
            catch (Exception e)
            {
                if (exceptionManager.HandleException(e, policyName))
                {
                    throw;
                }
            }

            return defaultResult;
        }
        #endregion IExceptionManager Extensions

        #region ILogWriter Extensions
        public static void Write(this ILogWriter reference,
            object source,
            LogItem item,
            [CallerMemberName] string name = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0)
        {
            Contract.Requires<ArgumentNullException>(source != null, "source");

            reference.Write(source.GetType(), item, name, filePath, lineNumber);
        }

        #endregion ILogWriter Extensions
    }
}
