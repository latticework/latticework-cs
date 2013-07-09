


#if !NETFX_CORE
namespace Lw.ComponentModel.Composition
#else
namespace Lw.Composition
#endif
{
    public static class Components
    {
        /// <summary>
        ///     The current ambient container.
        /// </summary>
        public static IComponentContainer Current 
        {
            get
            {
                if (provider == null) { return null; }

                return provider();
            }
        }

        /// <summary>
        ///     Sets the delegate that is used to retrieve the current container.
        /// </summary>
        /// <param name="provider">
        ///     Delegate that, when called, will return the current ambient container.
        /// </param>
        public static void SetContainerProvider(ComponentContainerProvider provider)
        {
            Components.provider = provider;

            Components.Current.RegisterInstance<IComponentContainer>(Components.Current);
        }

        private static ComponentContainerProvider provider = null;
    }
}
