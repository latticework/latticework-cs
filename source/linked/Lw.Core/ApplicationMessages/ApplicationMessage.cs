using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#if !NETFX_CORE
using System.Runtime.Remoting.Messaging;
#endif
using Lw.Collections.Generic;
using Lw.Resources;
using System.Diagnostics.Contracts;

namespace Lw.ApplicationMessages
{
    /// <summary>
    ///     An intra-application message. Supplies errors, warning, and information between tiers and to logging systems.
    /// </summary>
#if !NETFX_CORE
    [Serializable]
#endif
    public class ApplicationMessage
#if !NETFX_CORE
        : ILogicalThreadAffinative
#endif
    {
        #region Public Constructors
        public ApplicationMessage(long messageCode, string format, params object[] args)
            : this(null, Guid.Empty, null, messageCode, format, args)
        {
        }

        public ApplicationMessage(object obj, IEnumerable<string> memberNames, long messageCode, string format, params object[] args)
            : this(obj, Guid.Empty, memberNames, messageCode, format, args)
        {
        }

        public ApplicationMessage(Guid? objectUId, IEnumerable<string> memberNames, long messageCode, string format, params object[] args)
            : this(null, objectUId, memberNames, messageCode, format, args)
        {
        }

        public ApplicationMessage(object obj, Guid? objectUId, IEnumerable<string> memberNames, long messageCode, string format, params object[] args)
        {
            Contract.Requires(format != null);

            this.TargetObject = obj;
            this.ObjectUId = ApplicationMessage.GetTransientKey(obj, objectUId);
            this.MemberNames = new List<string>(memberNames ?? new string[] { });
            this.MessageCode = messageCode;
            this.Format = format;
            this.Arguments = new List<object>(args ?? new object[] { });

            VerifyState();
        }
        #endregion Public Constructors

        #region Public Methods
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
        #endregion Public Methods

        #region Public Properties
        public IReadOnlyList<object> Arguments { get; private set; }

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
                    (int)MessageCode.GetMaskedValue(ApplicationMessageDomainCode.Mask));
            }
        }

        public string Format { get; private set; }

        public IList<string> MemberNames { get; private set; }

        public string Message
        {
            get
            {
                return Format.DoFormat(this.Arguments);
            }
        }

        public long MessageCode { get; private set; }

        public object TargetObject { get; private set; }

        public Guid? ObjectUId { get; private set; }

        public ApplicationMessageSchema Schema
        {
            get
            {
                if (MessageCode.EqualsMasked(
                    ApplicationMessageSchemaCode.Mask, ApplicationMessageSchemaCode.Enterprise))
                {
                    return ApplicationMessageSchemaCode.Enterprise.Map<ApplicationMessageSchema>();
                }

                if (MessageCode.EqualsMasked(
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

                if (MessageCode.EqualsMasked(
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
        #endregion Public Properties

        #region Private Methods
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

        private static Guid? GetTransientKey(object obj, Guid? objectUId)
        {
            var uid = objectUId;

            if (objectUId == null && obj != null)
            {
                uid = Operations.GetTransientKey(obj);
            }

            return uid;
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

            if (MessageCode.EqualsMasked(mask, value))
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
        #endregion Private Methods
    }
}
