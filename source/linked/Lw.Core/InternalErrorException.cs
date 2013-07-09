using Lw.ApplicationMessages;
using System;
using System.Runtime.Serialization;

namespace Lw
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class InternalErrorException : ApplicationMessageException
#if !NETFX_CORE
        , ISerializable
#endif
    {
        public InternalErrorException()
            : base(CreateMessage(string.Empty)) { }

        public InternalErrorException(Exception inner)
            : base(CreateMessage(string.Empty), inner) { }

        public InternalErrorException(string description)
            : base(CreateMessage(description)) { }

        public InternalErrorException(string description, Exception inner)
            : base(CreateMessage(description), inner) { }

#if !NETFX_CORE
        protected InternalErrorException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
#endif

        private static ApplicationMessage CreateMessage(string description)
        {
            return LwCoreMessages.CreateMessage(
                LwCoreMessages.CriticalMessageCodeInternalError, description);
        }
    }
}
