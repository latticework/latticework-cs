using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
#if !NETFX_CORE
using System.Security.Permissions;
#endif
using Lw.ApplicationMessages;
using Lw.Collections.Generic;
#if !NETFX_CORE
using Lw.Runtime.Serialization;
using System.Diagnostics.Contracts;
#endif

namespace Lw
{
#if !NETFX_CORE
    [global::System.Serializable]
#endif
    public class ApplicationMessageException : ParameterizedException
    {
        #region Public Constructors
        public ApplicationMessageException(ApplicationMessage message) 
            : base(message.Format, message.Arguments)
        {
            Contract.Requires(message != null);

            Initialize(new ApplicationMessageCollection { message });
        }

        public ApplicationMessageException(ApplicationMessage message, Exception inner)
            : base(inner, message.Format, message.Arguments)
        {
            Contract.Requires(message != null);

            Initialize(new ApplicationMessageCollection { message });
        }

        public ApplicationMessageException(IEnumerable<ApplicationMessage> messages)
            : base(GetError(messages).Format, GetError(messages).Arguments)
        {
            Initialize(messages);
        }

        public ApplicationMessageException(IEnumerable<ApplicationMessage> messages, Exception inner)
            : base(inner, GetError(messages).Format, GetError(messages).Arguments)
        {
            Initialize(messages);
        }
        #endregion Public Constructors

        #region Public Properties
        public ApplicationMessage Error
        {
            get { return this.Messages.Errors.First(); }
        }

        public IEnumerable<ApplicationMessage> Errors
        {
            get { return this.Messages.Errors; }
        }

        public ApplicationMessageCollection Messages { get; private set; }
        #endregion Public Properties

        #region Public Methods
#if !NETFX_CORE
        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(
            SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue<ApplicationMessageCollection>("Messages", Messages);
        }
#endif
        #endregion Public Methods


        #region Protected Constructors
#if !NETFX_CORE
        protected ApplicationMessageException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            Messages = info.GetValue<ApplicationMessageCollection>("Messages");
        }
#endif
        #endregion Protected Constructors


        #region Private Methods
        private static ApplicationMessage GetError(IEnumerable<ApplicationMessage> messages)
        {
            ApplicationMessage errorMessage = messages.FirstOrDefault(am => am.MeetsThreshold(ApplicationMessageSeverity.Error));

            if (errorMessage == null)
            {
                ExceptionOperations.ThrowArgumentException("messages");
            }

            return errorMessage;
        }

        private void Initialize(IEnumerable<ApplicationMessage> messages)
        {
            if (messages == null)
            {
                messages = new ApplicationMessageCollection { };
            }

            var error = messages.FirstOrDefault(am => am.MeetsThreshold(ApplicationMessageSeverity.Error));

            if (error == null)
            {
                throw new InternalErrorException(
                    Properties.Resources.InternalError_CreatedExceptionWithNoErrors);
            }

            if (messages is ApplicationMessageCollection)
            {
                this.Messages = (ApplicationMessageCollection)messages;
            }
            else
            {
                this.Messages = new ApplicationMessageCollection(messages);
            }
        }
        #endregion Private Methods
    }
}
