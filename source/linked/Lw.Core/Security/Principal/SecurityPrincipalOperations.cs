using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading;

namespace Lw.Security.Principal
{
    public static class SecurityPrincipalOperations
    {
#if !NETFX_CORE
        public static TResult DoImpersonated<T, TResult>(Func<T, TResult> func, T arg)
        {
            var identity = (WindowsIdentity)Thread.CurrentPrincipal.Identity;

            return DoImpersonated(func, arg, identity);
        }

        public static TResult DoImpersonated<T, TResult>(Func<T, TResult> func, T arg, WindowsIdentity identity)
        {
            TResult result = default(TResult);

            DoImpersonated(() =>
            {
                result = func(arg);
            }, identity);

            return result;
        }

        public static TResult DoImpersonated<TResult>(Func<TResult> func)
        {
            var identity =  Thread.CurrentPrincipal.Identity;

            var windowsIdentity = (identity as WindowsIdentity) ?? (identity as ICustomIdentity).GetValueOrDefault(
                ci => ci.WindowsIdentity);

            if (windowsIdentity == null)
            {
                throw new InvalidOperationException(
                    "The current thread identity must either be a 'WindowsIdentity' or an 'ICustomIdentity' implementation that has a valid 'WindowsIdentity' member. Yours, a '{0}', does not."
                    .DoFormat(identity.GetType().FullName));
            }

            return DoImpersonated(func, windowsIdentity);
        }

        public static TResult DoImpersonated<TResult>(Func<TResult> func, WindowsIdentity identity)
        {
            TResult result = default(TResult);

            DoImpersonated(() =>
            {
                result = func();
            }, identity);

            return result;
        }

        public static void DoImpersonated(Action a)
        {
            var identity =  (WindowsIdentity)Thread.CurrentPrincipal.Identity;
            DoImpersonated(a, identity);
        }

        public static void DoImpersonated(Action a, WindowsIdentity identity)
        {
            var ctx = identity.Impersonate();

            try
            {
                a();
            }
#if DEBUG
            catch (Exception exception)
#else
            catch (Exception)
#endif
            {
#if DEBUG
                Debug.WriteLine(exception.Message);
#endif
                throw;
            }
            finally
            {
                ctx.Undo();
            }
        }
#endif
    }
}
