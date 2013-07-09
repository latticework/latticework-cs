using System.Diagnostics;

namespace Lw.Diagnostics
{
    public enum LifetimeEventType
    {
          Message = 0
        , Start =
#if !NETFX_CORE
            TraceEventType.Start
#else
            256
#endif

        , Stop =
#if !NETFX_CORE
            TraceEventType.Stop
#else
            512
#endif

        , Suspend =
#if !NETFX_CORE
            TraceEventType.Suspend
#else
            1024
#endif

        , Resume =
#if !NETFX_CORE
            TraceEventType.Resume
#else
            2048
#endif

        , Transfer =
#if !NETFX_CORE
            TraceEventType.Transfer
#else
            4096
#endif
    }
}
