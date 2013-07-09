using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
#if !NETFX_CORE
using System.Security.Permissions;
#endif

#if !NETFX_CORE
using Lw.Runtime.Serialization;
#endif

namespace Lw
{
#if !NETFX_CORE
    [global::System.Serializable]
#endif
    public class ParameterizedException : Exception
#if !NETFX_CORE
        , ISerializable
#endif
    {
        public ParameterizedException(string format, params object[] args)
            : base(FormatMessage(format, args))
        {
            Initialize(format, args);
        }

        public ParameterizedException(Exception inner, string format, params object[] args)
            : base(FormatMessage(format, args), inner)
        {
            Initialize(format, args);
        }

#if !NETFX_CORE
        protected ParameterizedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            format = info.GetValue<string>("format");
            arguments = info.GetValue<List<object>>("arguments");
        }


        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue<string>("format", format);
            info.AddValue<List<object>>("arguments", arguments);
        }
#endif


        private static string FormatMessage(string format, object[] args)
        {
            if (format == null) { throw new ArgumentNullException("message"); }

            return format.DoFormat(args);
        }

        private void Initialize(string format, object[] args)
        {
            this.format = format;
            this.arguments = args.ToList();
        }


        public IList<object> Arguments { get { return arguments; } }

        public string Format { get { return format; } }


        private string format = null;
        private List<object> arguments = null;
    }
}
