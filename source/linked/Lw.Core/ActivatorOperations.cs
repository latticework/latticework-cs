using System;
using System.Diagnostics;

namespace Lw
{
    public static class ActivatorOperations
    {
        [DebuggerStepThrough]
        public static T CreateInstance<T>(params object[] args)
        {
            return (T)Activator.CreateInstance(typeof(T), args);
        }
    }
}
