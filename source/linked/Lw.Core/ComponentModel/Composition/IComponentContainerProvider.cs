
#if !NETFX_CORE
namespace Lw.ComponentModel.Composition
#else
namespace Lw.Composition
#endif
{
    public interface IComponentContainerProvider
    {
        IComponentContainer GetApplicationContainer();
    }
}
