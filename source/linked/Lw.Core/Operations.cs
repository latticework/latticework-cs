using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace Lw
{
    /// <summary>
    ///     Specifies when the iterator delegate will be execued when <see cref="Operations.DoIterated"/> 
    ///     is executed.
    /// </summary>
    [Flags]
    public enum DoIteratorExecution
    {
        /// <summary>
        ///     Specifies the iteration will not be executed before or after the iteration set; only between.
        /// </summary>
        Between = 0,
        /// <summary>Specifies the iterator will be executed before the first iteration.</summary>
        Before = 1,
        /// <summary>Specifies the iterator will be executed after the last iteration.</summary>
        After = 2,
        /// <summary>
        ///     Specifies the iterator will be executed before the first iteration and after the last 
        ///     iteration.
        /// </summary>
        BeforeAndAfter = Before | After,
    }

    public class Operations
    {
        /// <summary>
        ///     Returns a value indicating whether any of the specified values is <see langword="null"/>.               
        /// </summary>
        /// <param name="values">
        ///     A list of <see cref="object"/> values.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> any of the specified values is <see langword="null"/>; otherwise, 
        ///     <see langword="false"/>.
        /// </returns>
        [DebuggerStepThrough]
        public static bool AnyNull(params object[] values)
        {
            ExceptionOperations.VerifyNonNull(values, () => values);
            
            if (values.Count() == 0)
            {
                ExceptionOperations.ThrowArgumentException(()=>values);
            }

            return values.Any(a => a == null);
        }

        [DebuggerStepThrough]
        public static IComparer<T> Compare<T, U>(Func<T, U> compareValueSelector)
        {
            return new DelegateComparer<T, U>(compareValueSelector);
        }

        [DebuggerStepThrough]
        public static IEqualityComparer<T> CompareEquality<T, U>(Func<T, U> compareValueSelector)
        {
            return new DelegateEqualityComparer<T, U>(compareValueSelector);
        }

        /// <summary>
        ///     Performs specified <see cref="Action"/> while condition is true. Specified iterator 
        ///     <see cref="Action"/> is performed according to specified <see cref="DoIteratorExecution"/> option.
        /// </summary>
        /// <param name="statements">
        ///     <see cref="Action"/> to repeat.
        /// </param>
        /// <param name="condition">
        ///     <see langword="true"/> if the repetitions should continue; otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="iterator">
        ///     <see cref="Action"/> that is performed on each iteration. Performed according to specified option.
        /// </param>
        /// <param name="option">
        ///     Specifies if specified iteration <see cref="Action"/> is performed Before
        /// </param>
        [DebuggerStepThrough]
        public static void DoIterated(
            Action statements,
            Func<bool> condition,
            Action iterator,
            DoIteratorExecution option)
        {
            ExceptionOperations.VerifyNonNull(statements, () => statements);
            ExceptionOperations.VerifyNonNull(condition, () => condition);
            ExceptionOperations.VerifyNonNull(iterator, () => iterator);

            if (option.Includes(DoIteratorExecution.Before))
            {
                iterator();
            }

            for (; condition(); iterator())
            {
                statements();
            }

            if (option.Includes(DoIteratorExecution.After))
            {
                iterator();
            }
        }

        [DebuggerStepThrough]
        public static void DoLockedIf(Action statements, Func<bool> condition, object lockObj)
        {
            if (condition())
            {
                lock (lockObj)
                {
                    if (condition())
                    {
                        statements();
                    }
                }
            }
        }

        /// <summary>
        ///     Executes specified statements within the specified wrapper.
        /// </summary>
        /// <param name="statements">
        ///     The <see href="Action"/> to execute.
        /// </param>
        /// <param name="wrapper">
        ///     The <see href="Action"/> executed first, which executes <paramref name="statements"/>.
        /// </param>
        /// <remarks>
        ///     <note type="implementnotes">
        ///     <paramref name="wrapper"/> should execute any setup code, execute <paramref name="statements"/>, and 
        ///     then any cleanup code. You can use closures to allow <paramref name="statements"/> and 
        ///     <paramref name="wrapper"/> to interact.
        ///     </note>
        /// </remarks>
        [DebuggerStepThrough]
        public static void DoWithin(Action statements, Action<Action> wrapper)
        {
            ExceptionOperations.VerifyNonNull(statements, () => statements);
            ExceptionOperations.VerifyNonNull(wrapper, () => wrapper);

            wrapper(statements);
        }

        /// <summary>
        ///     Ensures the specified object is initialized.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the object to initialize.
        /// </typeparam>
        /// <param name="obj">
        ///     The object to initialize.
        /// </param>
        /// <returns>
        ///     The specified object or the specified object initialized from the default constructor.
        /// </returns>
        [DebuggerStepThrough]
        public static T InitializeIfNull<T>(ref T obj)
            where T : new()
        {
            return InitializeIfNull<T>(ref obj, () => new T());
        }

        /// <summary>
        ///     Ensures the specified object is initialized.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the object to initialize.
        /// </typeparam>
        /// <param name="obj">
        ///     The object to initialize.
        /// </param>
        /// <param name="initializer">
        ///     The object initalization delegate.
        /// </param>
        /// <returns>
        ///     The specified object or the specified object initialized from the specifiec initializer.
        /// </returns>
        [DebuggerStepThrough]
        public static T InitializeIfNull<T>(ref T obj, Func<T> initializer)
        {
            if (obj == null)
            {
                obj = initializer();
            }

            return obj;
        }
    }
}
