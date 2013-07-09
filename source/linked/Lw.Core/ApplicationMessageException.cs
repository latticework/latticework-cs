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
#endif

namespace Lw
{
#if !NETFX_CORE
    [global::System.Serializable]
#endif
    public class ApplicationMessageException : ParameterizedException
    {
        public ApplicationMessageException()
            : base(GetError().Format, GetError().Arguments)
        {
            Initialize(null);
        }

        public ApplicationMessageException(Exception inner)
            : base(inner, GetError().Format, GetError().Arguments)
        {
            Initialize(null);
        }

        public ApplicationMessageException(ApplicationMessage message) 
            : base(message.Format, message.Arguments)
        {
            Initialize(new ApplicationMessageCollection { message });
        }

        public ApplicationMessageException(ApplicationMessage message, Exception inner)
            : base(inner, message.Format, message.Arguments)
        {
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


#if !NETFX_CORE
        protected ApplicationMessageException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            messages = info.GetValue<ApplicationMessageCollection>("messages");
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(
            SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue<ApplicationMessageCollection>("messages", messages);
        }
#endif

        public ApplicationMessage Error
        {
            get { return error; }
        }

        public IEnumerable<ApplicationMessage> Errors
        {
            get { return messages.Errors; }
        }

        public ApplicationMessageCollection Messages
        {
            get { return messages; }
        }

        private static ApplicationMessage GetError(IEnumerable<ApplicationMessage> messages)
        {
            if (messages == null)
            {
                ExceptionOperations.ThrowArgumentException("messages");
            }

            ApplicationMessage errorMessage = messages.FirstOrDefault(
                am => am.Severity == ApplicationMessageSeverity.Error);

            if (errorMessage == null)
            {
                ExceptionOperations.ThrowArgumentException("messages");
            }

            return errorMessage;
        }

        public void Throw()
        {
#if !NETFX_CORE
            ApplicationMessageCollection contextMessages = ApplicationMessage.ContextMessages;

            if ((object)contextMessages != (object)messages)
            {
                messages.AddRange(contextMessages);
            }
            ApplicationMessage.ClearContextMessages();

            error = error ?? ApplicationMessage.ContextMessages.FirstOrDefault(
                am => am.MeetsThreshold(ApplicationMessageSeverity.Error));

            if (error == null)
            {
                new InternalErrorException(
                    Properties.Resources.InternalError_CreatedExceptionWithNoErrors).Throw();
            }
#endif

            throw this;
        }

        private void Initialize(IEnumerable<ApplicationMessage> messages)
        {
            if (messages == null)
            {
                messages = new ApplicationMessageCollection { };
            }

            error = messages.FirstOrDefault(am => am.MeetsThreshold(ApplicationMessageSeverity.Error));

#if !NETFX_CORE
            error = error ?? ApplicationMessage.ContextMessages.FirstOrDefault(
                am => am.MeetsThreshold(ApplicationMessageSeverity.Error));

            // Added here so that InternalErrorException will pick them up. Reentrance will happen.
            ApplicationMessage.ContextMessages.AddRange(messages);
#endif

            if (error == null)
            {
                throw new InternalErrorException(
                    Properties.Resources.InternalError_CreatedExceptionWithNoErrors);
            }

#if !NETFX_CORE
            // TODO: ApplicationMessageException.Initialize: There is a logic error here. See also Throw
            messages = ApplicationMessage.ContextMessages;
#endif



            if (messages is ApplicationMessageCollection)
            {
                this.messages = (ApplicationMessageCollection)messages;
            }
            else
            {
                this.messages = new ApplicationMessageCollection(messages);
            }
        }

        private static ApplicationMessage GetError()
        {
            throw new NotImplementedException();
        }

        private ApplicationMessageCollection messages;
        private ApplicationMessage error = null;
    }
}
