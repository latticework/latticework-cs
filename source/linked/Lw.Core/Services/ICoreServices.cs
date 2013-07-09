#if !NETFX_CORE
using Lw.ComponentModel.Composition;
#else
using Lw.Composition;
#endif
using Lw.Diagnostics;

namespace Lw.Services
{
    public interface ICoreServices
    {
        IComponentContainer Components { get; }

        IExceptionManager ExceptionManager { get; }

        ILogWriter LogWriter { get; }

        ISecurityService SecurityService { get; }

#if !NETFX_CORE
        ITraceManager TraceManager { get; }
#endif
    }
}
