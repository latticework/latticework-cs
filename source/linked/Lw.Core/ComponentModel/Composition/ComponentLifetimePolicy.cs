
#if !NETFX_CORE
namespace Lw.ComponentModel.Composition
#else
namespace Lw.Composition
#endif
{
    public enum ComponentLifetimePolicy
    {
        Singleton = 0,
        PerCall = 1,
    }
}
