using Lw.Services;
using System;

#if !NETFX_CORE
namespace Lw.ComponentModel.Composition
#else
namespace Lw.Composition
#endif
{
    /// <summary>
    ///     Provides the <see cref="CoreServices"/> member to subclasses. Recommended base class for all components
    ///     registered in a <see cref="IComponentContainer"/>.
    /// </summary>
    public abstract class ComponentBase
    {
        #region Protected Constructors
        /// <summary>
        ///     Initializes a new <see cref="ComponentBase"/> instance. <see cref="CoreServices"/> will be initialized 
        ///     from <see cref="Components.Current"/> on first access.
        /// </summary>
        protected ComponentBase()
            : this(null)
        {
        }

        /// <summary>
        ///     Initializes a new <see cref="ComponentBase"/> instance.
        /// </summary>
        /// <param name="coreServices">
        ///     An <see cref="ICoreServices"/> implementation.
        /// </param>
        protected ComponentBase(ICoreServices coreServices)
        {
            this.coreServices = coreServices;
        }
        #endregion Protected Constructors

        #region Protected Properties
        /// <summary>
        ///     Gets an implementation of <see cref="ICoreServices"/>. Value is set either in a constructor or from 
        ///     <see cref="Components.Current"/> on first access.
        /// </summary>
        /// <value>
        ///     An implementation of <see cref="ICoreServices"/>.
        /// </value>
        protected ICoreServices CoreServices
        {
            get
            {
                return Operations.InitializeIfNull(
                    ref this.coreServices, () => Components.Current.GetInstance<ICoreServices>());
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                if (this.coreServices != null)
                {
                    throw new InvalidOperationException();
                }

                this.coreServices = value;
            }
        }
        #endregion Protected Properties

        #region Private Fields
        private ICoreServices coreServices = null;
        #endregion Private Fields
    }
}
