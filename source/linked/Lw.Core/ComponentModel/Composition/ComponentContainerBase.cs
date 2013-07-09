using System;
using System.Collections.Generic;
using System.Globalization;

#if !NETFX_CORE
namespace Lw.ComponentModel.Composition
#else
namespace Lw.Composition
#endif
{
    public abstract class ComponentContainerBase : IComponentContainer
    {
        #region Public Constructors
        public ComponentContainerBase()
            : this(null)
        {
        }

        public ComponentContainerBase(IComponentContainer parent)
        {
            parentContainer = parent;
        }
        #endregion Public Constructors

        #region Public Methods
        public virtual IEnumerable<object> GetAllInstances(Type componentType)
        {
            try
            {
                return this.DoGetAllInstances(componentType);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(this.FormatActivateAllExceptionMessage(ex, componentType), ex);
            }
        }

        public virtual bool IsRegistered(Type typeToCheck)
        {
            return IsRegistered(typeToCheck, null);
        }

        public abstract bool IsRegistered(Type typeToCheck, string nameToCheck);

        public abstract void RegisterInstance(Type registeredType, string key, object instance);

        public abstract void RegisterType(
            Type registeredType, string key, Type implementationType, ComponentLifetimePolicy policy);

        public bool TryGetInstance(Type TComponent, string key, out object component)
        {
            return DoTryGetInstance(TComponent, key, out component);
        }
        #endregion Public Methods

        #region Public Properties
        /// <summary>
        ///     Gets and sets the parent container.
        /// </summary>
        /// <value>
        ///     An <see cref="IComponentContainer"/> that is accessed when the current container cannot find a requested object or 
        ///     <see langword="null"/> if the container was created without a parent.
        /// </value>
        /// <exception cref="ArgumentNullException">
        ///     The value is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The value has already been set.
        /// </exception>
        public IComponentContainer Parent
        {
            get
            {
                return parentContainer;
            }
            set
            {
                if (value == null) { throw new ArgumentNullException(); }
                if (parentContainer != null) { throw new InvalidOperationException(); }
                parentContainer = value;
            }
        }
        #endregion Public Properties


        #region Protected Methods
        protected abstract IEnumerable<object> DoGetAllInstances(Type componentType);

        protected abstract object DoGetInstance(Type componentType, string key);

        protected virtual bool DoTryGetInstance(Type TComponent, string key, out object component)
        {
            bool succeeded = true;
            component = null;
            try
            {
                component = this.DoGetInstance(TComponent, key);
            }
            catch
            {
                succeeded = false;
            }
            return succeeded;
        }

        protected virtual string FormatActivateAllExceptionMessage(Exception actualException, Type componentType)
        {
            return string.Format(CultureInfo.CurrentUICulture, Properties.Resources.ActivateAllExceptionMessage, new object[] { componentType.Name });
        }

        protected virtual string FormatActivationExceptionMessage(Exception actualException, Type componentType, string key)
        {
            return string.Format(CultureInfo.CurrentUICulture, Properties.Resources.ActivationExceptionMessage, new object[] { componentType.Name, key });
        }
        #endregion Protected Methods


        #region Private Fields
        private IComponentContainer parentContainer;
        #endregion Private Fields

    }
}
