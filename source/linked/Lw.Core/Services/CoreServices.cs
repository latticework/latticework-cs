
#if !NETFX_CORE
using Lw.ComponentModel.Composition;
#else
using Lw.Composition;
#endif
using Lw.Diagnostics;

namespace Lw.Services
{
#if !NETFX_CORE
    using IC = Lw.ComponentModel.Composition;
#else
    using IC = Lw.Composition;
#endif

    public class CoreServices : ICoreServices
    {
        #region Public Constructors
        public CoreServices()
        {

        }

        public CoreServices(
            IComponentContainer components,
            ILogWriter logWriter,
#if !NETFX_CORE
            ITraceManager traceManager,
#endif
            IExceptionManager exceptionManager,
            ISecurityService securityService)
        {
            this.logWriter = logWriter;
#if !NETFX_CORE
            this.traceManager = traceManager;
#endif
            this.exceptionManager = exceptionManager;
            this.securityService = securityService;
        }
        #endregion Public Constructors

        #region Public Properties
        public IComponentContainer Components
        {
            get
            {
                return Operations.InitializeIfNull(
                    ref this.components,
                    () => IC::Components.Current.GetInstance<IComponentContainer>());
            }
        }

        public IExceptionManager ExceptionManager
        {
            get
            {
                return Operations.InitializeIfNull(
                    ref this.exceptionManager,
                    () => IC::Components.Current.GetInstance<IExceptionManager>());
            }
        }

        public ILogWriter LogWriter
        {
            get
            {
                return Operations.InitializeIfNull(
                    ref this.logWriter,
                    () => IC::Components.Current.GetInstance<ILogWriter>());
            }
        }

        public ISecurityService SecurityService
        {
            get
            {
                return Operations.InitializeIfNull(
                    ref this.securityService, () =>
                    {
                        ISecurityService service;
#if NETFX_CORE
                        service = IC::Components.Current.GetInstance<ISecurityService>();
#else

                        if (!IC::Components.Current.TryGetInstance<ISecurityService>(out service))
                        {
                            service = IC::Components.Current
                                .RegisterTypeInstance<ISecurityService, CoreSecurityService>();
                        }
#endif
                        return service;
                    });
            }
        }


#if !NETFX_CORE
        public ITraceManager TraceManager
        {
            get
            {
                return Operations.InitializeIfNull(
                    ref this.traceManager,
                    () => IC::Components.Current.GetInstance<ITraceManager>());
            }
        }
#endif
        #endregion Public Properties

        #region Private Fields
        private IComponentContainer components = null;
        private ILogWriter logWriter = null;
#if !NETFX_CORE
        private ITraceManager traceManager = null;
#endif
        private IExceptionManager exceptionManager = null;
        private ISecurityService securityService = null;
        #endregion Private Fields
    }
}
