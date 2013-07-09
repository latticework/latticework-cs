using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#if !NETFX_CORE
using System.Runtime.Remoting.Messaging;
#endif
using Lw.Collections.Generic;
using Lw.Resources;

namespace Lw.ApplicationMessages
{
#if !NETFX_CORE
    [Serializable]
#endif
    public class ApplicationMessage
#if !NETFX_CORE
        : ILogicalThreadAffinative
#endif
    {
        public ApplicationMessage(long messageCode, string format, params object[] args)
            : this(Guid.Empty, messageCode, format, args)
        {
        }

        public ApplicationMessage(Guid objectUid, long messageCode, string format, params object[] args)
        {
            if (format == null)
            {
                throw new ArgumentNullException("format");
            }

            this.objectUid = objectUid;
            this.messageCode = messageCode;
            this.format = format;
            this.arguments = args ?? new object[] { };

            VerifyState();
        }

        public static IDictionary<long, string> CreateMessageCodeResourceNameMap(Type messagesType)
        {
            ExceptionOperations.VerifyNonNull(messagesType, () => messagesType);

            return (
                    from f in messagesType.GetTypeInfo().DeclaredFields
                    where f.FieldType == typeof(long)
                        && f.GetCustomAttribute<ResourceNameAttribute>(false) != null
                    select new 
                    { 
                        Key = (long)f.GetValue(null), 
                        Value = f.GetCustomAttribute<ResourceNameAttribute>(false).ResourceName
                    }
                ).ToDictionary(item => item.Key, item => item.Value);
        }

#if !NETFX_CORE
        public static void ClearContextMessages()
        {
            CallContext.FreeNamedDataSlot(applicationMessagesKey);
        }
#endif

        public static IList<ApplicationMessageDescription> GetApplicationMessages(Assembly assembly)
        {
            var sources = assembly.GetCustomAttributes<ApplicationMessageSourceAttribute>();

            if (sources.IsNullOrEmpty())
            {
                return new List<ApplicationMessageDescription> { };
            }

            return (
                    from s in sources
                    from mi in s.Type.GetTypeInfo().DeclaredMembers
                    where mi.GetCustomAttribute<ApplicationMessageCodeAttribute>(false) != null
                        && IsValidCodeMember(mi)
                    select new ApplicationMessageDescription
                    {
                        MessageFormat = GetMessageFormat(s.Type, s.MessageFormatMethod, mi),
                        Arguments = (
                                from amaa in mi.GetCustomAttributes<ApplicationMessageArgumentAttribute>(
                                    false)
                                select new MessageArgumentDescription
                                {
                                    Description = amaa.Description,
                                    Name = amaa.Name,
                                    Type = amaa.Type,
                                })
                            .ToList(),
                    })
                .ToList();
        }

        public static ApplicationMessagePriority GetDefaultPriority(ApplicationMessageSeverity severity)
        {
            switch (severity)
            {
                case ApplicationMessageSeverity.Critical:
                case ApplicationMessageSeverity.Error:
                    return ApplicationMessagePriority.Mandatory;

                case ApplicationMessageSeverity.Custom:
                default:
                    return ApplicationMessagePriority.Default;

                case ApplicationMessageSeverity.Information:
                    return ApplicationMessagePriority.Medium;

                case ApplicationMessageSeverity.Verbose:
                    return ApplicationMessagePriority.Low;

                case ApplicationMessageSeverity.Warning:
                    return ApplicationMessagePriority.High;
            }
        }

        public bool MeetsThreshold(ApplicationMessageSeverity severity)
        {
            if (severity == ApplicationMessageSeverity.None)
            {
                throw new ArgumentOutOfRangeException("severity");
            }

            return
                Severity >= ApplicationMessageSeverity.None
                    && Severity <= severity;

        }

#if !NETFX_CORE
        public static ApplicationMessageCollection ContextMessages
        {
            get
            {
                ApplicationMessageCollection messages = 
                    (ApplicationMessageCollection)CallContext.GetData(applicationMessagesKey);

                if (messages == null)
                {
                    messages = new ApplicationMessageCollection();
                    CallContext.SetData(applicationMessagesKey, messages);
                }

                return messages;
            }
        }
#endif

        public object[] Arguments
        {
            get { return arguments; }
        }

        public ApplicationMessageAuthority Authority
        {
            get
            {
                if (!IsEnterpriseSchema)
                {
                    return ApplicationMessageAuthority.None;
                }

                ApplicationMessageAuthority result = MapIfEqualsMasked<ApplicationMessageAuthority>(
                    ApplicationMessageAuthorityCode.Mask,
                    ApplicationMessageAuthorityCode.Enterprise,
                    ApplicationMessageAuthorityCode.Corporate);

                return result.GetValueIfNotEqualOrDefault(
                    ApplicationMessageAuthority.None,
                    ApplicationMessageAuthority.Custom);
            }
        }

        public ApplicationMessagePriority DefaultPriority
        {
            get
            {
                if (!IsEnterpriseSchema)
                {
                    return 0;
                }

                return GetDefaultPriority(Severity);
            }
        }

        public int Domain
        {
            get
            {
                if (!IsEnterpriseSchema)
                {
                    return 0;
                }

                return unchecked(
                    (int)messageCode.GetMaskedValue(ApplicationMessageDomainCode.Mask));
            }
        }

        public string Format
        {
            get { return format; }
        }

        public string Message
        {
            get
            {
                return format.DoFormat(arguments);
            }
        }

        public long MessageCode
        {
            get
            {
                return messageCode;
            }
        }

        public Guid ObjectUid
        {
            get { return objectUid; }
            set { objectUid = value; }
        }

        public ApplicationMessageSchema Schema
        {
            get
            {
                if (messageCode.EqualsMasked(
                    ApplicationMessageSchemaCode.Mask, ApplicationMessageSchemaCode.Enterprise))
                {
                    return ApplicationMessageSchemaCode.Enterprise.Map<ApplicationMessageSchema>();
                }

                if (messageCode.EqualsMasked(
                    ApplicationMessageSchemaCode.Mask, ApplicationMessageSchemaCode.None))
                {
                    return ApplicationMessageSchema.None.Map<ApplicationMessageSchema>();
                }

                return ApplicationMessageSchema.Custom;
            }
        }

        public ApplicationMessagePriority Priority
        {
            get
            {
                if (!IsEnterpriseSchema)
                {
                    return 0;
                }

                ApplicationMessagePriority priority = SpecifiedPriority;

                if (priority != ApplicationMessagePriority.Default)
                {
                    return priority;
                }

                return DefaultPriority;
            }
        }

        public ApplicationMessagePriority SpecifiedPriority
        {
            get
            {
                if (!IsEnterpriseSchema)
                {
                    return ApplicationMessagePriority.Default;
                }

                if (messageCode.EqualsMasked(
                    ApplicationMessagePriorityCode.Mask,
                    ApplicationMessagePriorityCode.Default))
                {
                    return ApplicationMessagePriority.Default;
                }

                ApplicationMessagePriority result;
                result = MapIfEqualsMasked<ApplicationMessagePriority>(
                    ApplicationMessagePriorityCode.Mask,
                    ApplicationMessagePriorityCode.Mandatory,
                    ApplicationMessagePriorityCode.High,
                    ApplicationMessagePriorityCode.Medium,
                    ApplicationMessagePriorityCode.Low,
                    ApplicationMessagePriorityCode.VeryLow);

                return ApplicationMessagePriority.Custom;
            }
        }

        public ApplicationMessageSeverity Severity
        {
            get
            {
                if (!IsEnterpriseSchema)
                {
                    return ApplicationMessageSeverity.None;
                }

                ApplicationMessageSeverity result;
                result = MapIfEqualsMasked<ApplicationMessageSeverity>(
                    ApplicationMessageSeverityCode.Mask,
                    ApplicationMessageSeverityCode.Critical,
                    ApplicationMessageSeverityCode.Error,
                    ApplicationMessageSeverityCode.Warning,
                    ApplicationMessageSeverityCode.Information,
                    ApplicationMessageSeverityCode.Verbose);

                return result.GetValueIfNotEqualOrDefault(
                    ApplicationMessageSeverity.None,
                    ApplicationMessageSeverity.Custom);
            }
        }



        private static string GetMessageFormat(Type type, string methodName, MemberInfo member)
        {
            return (string)type.GetRuntimeMethod(methodName, null).Invoke(
                null, new object[] { GetMessageCode(member) });
        }

        private static long GetMessageCode(MemberInfo member)
        {
            FieldInfo fi = member as FieldInfo;
            PropertyInfo pi = member as PropertyInfo;

            // Quite compiler.
            long value = 0L;

            if (fi != null)
            {
                value = (long)fi.GetValue(null);
            }
            else if (pi != null)
            {
                value = (long)pi.GetValue(null, null);
            }

            return value;
        }

        private static bool IsValidCodeMember(MemberInfo member)
        {
            FieldInfo fi = member as FieldInfo;
            PropertyInfo pi = member as PropertyInfo;

            bool valid = true;

            if (fi != null || !fi.IsStatic || fi.FieldType != typeof(long))
            {
                valid = false;
            }
            else if (pi != null || !pi.GetMethod.IsStatic || pi.PropertyType != typeof(long))
            {
                valid = false;
            }
            else
            {
                valid = false;
            }

            //if (!valid)
            //{
            //    Components.Current.GetInstance<ILogWriter>().Write(
            //        LwCoreMessages.CreateMessage(
            //            LwCoreMessages.WarningMessageCode_InvalidMessageCodeAttributeMember,
            //            member.ReflectedType,
            //            member.Name));
            //}

            return valid;
        }

        private TDestination MapIfEqualsMasked<TDestination>(Enum mask)
            where TDestination : struct
        {
            return MapIfEqualsMasked<TDestination>(
                mask, EnumOperations.GetValues<TDestination>().Cast<Enum>().ToArray());
        }

        private TDestination MapIfEqualsMasked<TDestination>(Enum mask, params Enum[] values)
        {
            TDestination result = default(TDestination);

            foreach (Enum value in values)
            {
                result = MapIfEqualsMasked<TDestination>(mask, value);

                if (!result.Equals(default(TDestination)))
                {
                    break;
                }
            }

            return result;
        }

        private TDestination MapIfEqualsMasked<TDestination>(Enum mask, Enum value)
        {
            TDestination result = default(TDestination);

            if (messageCode.EqualsMasked(mask, value))
            {
                result = value.Map<TDestination>();
            }
            return result;
        }


        private void VerifyState()
        {
            if (Schema == ApplicationMessageSchema.None)
            {
                throw new ArgumentOutOfRangeException("messageCode");
            }

            if (Schema != ApplicationMessageSchema.Enterprise)
            {
                return;
            }


            if (Severity == ApplicationMessageSeverity.None)
            {
                throw new ArgumentOutOfRangeException("messageCode");
            }

            if (Authority == ApplicationMessageAuthority.None)
            {
                throw new ArgumentOutOfRangeException("messageCode");
            }

            if (Domain == 0)
            {
                throw new ArgumentOutOfRangeException("messageCode");
            }
        }

        private bool IsEnterpriseSchema
        {
            get
            {
                return Schema == ApplicationMessageSchema.Enterprise;
            }
        }


#if !NETFX_CORE
        private static readonly string applicationMessagesKey =
            InternalUtil.CallContextPrefix + "ApplicationMessages";
#endif

        private long messageCode;
        private string format;
        private object[] arguments;
        private Guid objectUid;
    }
}
