using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

using Lw;
using Lw.ApplicationMessages;

namespace Lw.Build.T4.ApplicationMessages
{
    public class AmdlTemplateDataBuilder
    {
        #region Public Constructors
        public AmdlTemplateDataBuilder(string templateFileName)
        {
            this.templateFileName = templateFileName;
        }
        #endregion Public Constructors

        #region Public Methods
        public FileData GetFileData(string inputExtension, string outputExtension)
        {
            var filePath = Path.GetDirectoryName(templateFileName);
            var dottedInputExtension = "." + inputExtension;
            var dottedOutputExtension = "." + outputExtension;
            var dottedCapsOutputExtension = dottedOutputExtension.ToUpperInvariant();

            var baseInputFilename = Path
                .GetFileNameWithoutExtension(templateFileName)
                .Replace(dottedCapsOutputExtension, "") + dottedInputExtension;

            var inputFileName = Path.Combine(filePath, baseInputFilename);


            var baseOutputFileName = Path
                .GetFileNameWithoutExtension(templateFileName)
                .Replace(dottedCapsOutputExtension, "");

            if (outputExtension.EqualsOrdinal("resx"))
            {
                baseOutputFileName += "Resources";
            }

            baseOutputFileName += dottedOutputExtension;


            var outputFileName = Path.Combine(filePath, baseOutputFileName);

            return new FileData()
            {
                InputFileName = inputFileName,
                OutputFileName = outputFileName,
            };
        }

        public TGeneratorData ProcessAmdl<TGeneratorData>(
            string amdlFileName, Func<MessageSourceType, TGeneratorData> generatorFunction)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MessageSourceType));

            MessageSourceType source = null;
            using (var amdlFile = File.OpenText(amdlFileName))
            {
                source = (MessageSourceType)serializer.Deserialize(amdlFile);
            }

            return generatorFunction(source);
        }

        public MessageSourceData GenerateData(MessageSourceType source, Type domainEnumType)
        {
            var domainAssembly = domainEnumType.Assembly;

            long messageCodePrefixValue =
                (long)ApplicationMessageSchemaCode.Enterprise |
                (long)Enum.Parse(typeof(ApplicationMessageAuthorityCode), source.Authority) |
                GetCodeValue(domainAssembly, source.Domain) |
                GetCodeValue(domainAssembly, source.Library);

            string messageCodePrefix = "0x" + messageCodePrefixValue.ToString("X");


            return new MessageSourceData()
            {
                AuthorityName = source.Authority,
                ClassName = source.Name,
                Description = source.Description,
                DomainCodeType = GetCodeTypeName(domainAssembly, source.Domain),
                DomainCodeValue = GetCodeValueName(domainAssembly, source.Domain),
                LibraryCodeType = GetCodeTypeName(domainAssembly, source.Library),
                LibraryCodeValue = GetCodeValueName(domainAssembly, source.Library),
                MessageCodePrefix = messageCodePrefix,
                MessageCodePrefixValue = messageCodePrefixValue,
                Messages = this.GetOrderedMessages(source).Select(m =>
                {
                    return ToCSMessageData(source, messageCodePrefixValue, m);
                }).ToList(),
                NamespaceName = source.Namespace,
                UseLocalSeverityCodes = source.UseLocalSeverityCodes,
                Xml = source,
            };
        }

        private IEnumerable<MessageType>
            GetOrderedMessages(MessageSourceType source)
        {
            return source.Messages
                .OrderByDescending(
                    m => GetValue<ApplicationMessageSeverity>(m.Severity), SeverityComparer.Default)
                .ThenByDescending(
                    m => GetValue<ApplicationMessagePriority>(m.Priority), PriorityComparer.Default);
        }
        #endregion Public Methods

        #region Private Types
        private class SeverityComparer : IComparer<ApplicationMessageSeverity>
        {
            private SeverityComparer() { }

            public int Compare(ApplicationMessageSeverity x, ApplicationMessageSeverity y)
            {
                long xValue = 16L - (long)x;
                long yValue = 16L - (long)y;

                return xValue.CompareTo(yValue);
            }

            public static readonly SeverityComparer Default = new SeverityComparer();
        }

        private class PriorityComparer : IComparer<ApplicationMessagePriority>
        {
            private PriorityComparer() { }

            public int Compare(ApplicationMessagePriority x, ApplicationMessagePriority y)
            {
                long xValue = 16L - (long)x;
                long yValue = 16L - (long)y;

                return xValue.CompareTo(yValue);
            }

            public static readonly PriorityComparer Default = new PriorityComparer();
        }
        #endregion Private Types

        #region Private Fields
        private readonly string templateFileName;
        #endregion Private Fields

        #region Private Methods
        private static string GetBaseCodeComment(byte[] baseCode, bool useLocalSeverityCodes)
        {
            long baseCodeValue = GetBaseCodeValue(baseCode);

            if (useLocalSeverityCodes && baseCodeValue > 0xFFF)
            {
                var messageFormat = "The base message code '{0}' is greater than the maximum allowed value when UseLocalSeverityCodes is chosen. The maximum value is '0xFFF'.";
                throw new FormatException(string.Format(messageFormat, "0x" + baseCode));
            }

            string formatString = (useLocalSeverityCodes) ? "X3" : "X4";

            return string.Concat("0x", baseCodeValue.ToString(formatString));
        }

        public static string GetBaseCode(byte[] baseCode, bool useLocalSeverityCodes)
        {
            return GetBaseCodeComment(baseCode, useLocalSeverityCodes) + "L";
        }

        private static long GetBaseCodeValue(byte[] baseCode)
        {
            return (long)baseCode[0] * 0x0100L + (long)baseCode[1];
        }

        private static string GetCodeTypeName(Assembly assembly, string enumFieldName)
        {
            string typeName;
            string name;

            ParseEnumFieldName(enumFieldName, out typeName, out name);

            Type type = assembly.GetType(typeName);

            EnumMappingAttribute mapping = type.GetTypeInfo().GetCustomAttribute<EnumMappingAttribute>(
                false);

            return mapping.MappedType.FullName;
        }

        private static void ParseEnumFieldName(string enumFieldName, out string typeName, out string name)
        {
            typeName = null;
            name = null;

            var separaterIndex = enumFieldName.LastIndexOf('.');
            typeName = enumFieldName.Substring(0, separaterIndex);
            name = enumFieldName.Substring(separaterIndex + 1);
        }

        private static string GetCodeValueName(Assembly assembly, string enumFieldName)
        {
            string typeName;
            string name;

            ParseEnumFieldName(enumFieldName, out typeName, out name);

            Type type = assembly.GetType(typeName);

            EnumMappingAttribute mapping = type.GetTypeInfo().GetCustomAttribute<EnumMappingAttribute>(
                false);

            object value = Enum.Parse(type, name);

            var nameValue = (Enum)mapping.Map((Enum)value);

            return Enum.GetName(nameValue.GetType(), nameValue);
        }

        private static long GetCodeValue(Assembly assembly, string enumFieldName)
        {
            string typeName;
            string name;

            ParseEnumFieldName(enumFieldName, out typeName, out name);

            Type type = assembly.GetType(typeName);

            if (type == null)
            {
                string message = string.Format(
                    "Type with name '{0}' cannot be found in assembly '{1}'.",
                    typeName,
                    assembly.CodeBase);

                throw new InvalidOperationException(message);
            }

            EnumMappingAttribute mapping = type.GetTypeInfo().GetCustomAttribute<EnumMappingAttribute>(
                false);

            object value = Enum.Parse(type, name);

            return (long)mapping.Map((Enum)value);
        }

        private string GetLocalSeverity(ApplicationMessageSeverity severity)
        {
            if (severity > ApplicationMessageSeverity.Verbose)
            {
                return string.Concat("0x", (GetLocalSeverityCode(severity)).ToString("X"), "L");
            }

            return "(long){0}.{1}".DoFormat(typeof(ApplicationMessageLocalSeverityCode).Name, severity.GetName());
        }

        private long GetLocalSeverityCode(ApplicationMessageSeverity severity)
        {
            return 0x1000L * (16L - (long)severity);
        }

        private string GetMessageCodeValue(long prefix,
            ApplicationMessageSeverity severity,
            ApplicationMessagePriority priority,
            byte[] baseCode,
            bool useLocalSeverityCodes)
        {
            long messageCode =
                prefix |
                GetSeverityCode(severity) |
                GetPriorityCodeValue(priority, severity) |
                GetBaseCodeValue(baseCode);

            if (useLocalSeverityCodes)
            {
                messageCode |= GetLocalSeverityCode(severity);
            }

            return "0x" + messageCode.ToString("X");
        }

        private string GetPriority(
            ApplicationMessagePriority priority, ApplicationMessageSeverity severity)
        {
            if (priority > ApplicationMessagePriority.VeryLow)
            {
                return string.Concat(
                    "0x",
                    (GetPriorityCode(priority)).ToString("X12"),
                    "L");
            }

            string prefix = "(long)" + typeof(ApplicationMessagePriorityCode).Name;

            if (priority == ApplicationMessagePriority.Default)
            {
                if (severity > ApplicationMessageSeverity.Verbose)
                {
                    var message = "You must specify a message priority. The specified message severity, '{0}' is a custom severity.";
                    throw new FormatException(string.Format(message, ((long)severity).ToString()));
                }

                //var code = this.Map<ApplicationMessagePriorityCode>(this.GetDefaultPriority(severity));
                //return string.Concat(prefix, ".", Enum.GetName(code.GetType(), code));
            }

            return string.Concat(prefix, ".", Enum.GetName(priority.GetType(), priority));
        }

        private long GetPriorityCode(ApplicationMessagePriority priority)
        {
            return (long)priority;
        }

        private long GetPriorityCodeValue(
            ApplicationMessagePriority priority, ApplicationMessageSeverity severity)
        {
            if (priority > ApplicationMessagePriority.VeryLow)
            {
                return GetPriorityCode(priority);
            }

            if (priority == ApplicationMessagePriority.Default)
            {
                if (severity > ApplicationMessageSeverity.Verbose)
                {
                    var message = "You must specify a message priority. The specified message severity, '{0}' is a custom severity.";
                    throw new FormatException(string.Format(message, ((long)severity).ToString()));
                }

                //return GetPriorityCode(this.GetDefaultPriority(severity));
            }

            return GetPriorityCode(priority);
        }

        private string GetSeverity(ApplicationMessageSeverity severity)
        {
            if (severity > ApplicationMessageSeverity.Verbose)
            {
                return string.Concat(
                    "0x",
                    (GetSeverityCode(severity)).ToString("X12"),
                    "L");
            }

            return string.Format("(long){0}.{1}",
                typeof(ApplicationMessageSeverityCode).Name,
                Enum.GetName(severity.GetType(), severity));
        }

        //private TDestination Map<TDestination>(Enum reference)
        //{
        //    var mappers = reference.GetType().GetTypeInfo().GetCustomAttributes<EnumMappingAttribute>(false);

        //    EnumMappingAttribute mapper = mappers
        //        .SingleOrDefault(ema => ema.MappedType == typeof(TDestination));

        //    return mapper.Map<TDestination>(reference);
        //}


        private MessageData ToCSMessageData(MessageSourceType source, long messageCodePrefixValue, MessageType message)
        {
            ApplicationMessageSeverity severityValue = GetValue<ApplicationMessageSeverity>(message.Severity);
            ApplicationMessagePriority priorityValue = GetValue<ApplicationMessagePriority>(message.Priority);

            string messageCodeValue = GetMessageCodeValue(
                messageCodePrefixValue,
                severityValue,
                priorityValue,
                message.BaseCode,
                source.UseLocalSeverityCodes);

            return new MessageData()
            {
                Arguments = message.Arguments.OrderBy(a => a.Index).Select(a =>
                {
                    return new MessageArgumentData()
                    {
                        ArgumentType = a.Type,
                        Description = a.Description,
                        Index = a.Index,
                        Name = a.Name,
                        Xml = a,
                    };
                }).ToList(),
                BaseCode = GetBaseCode(message.BaseCode, source.UseLocalSeverityCodes),
                BaseCodeComment = GetBaseCodeComment(message.BaseCode, source.UseLocalSeverityCodes),
                FormatString = message.Format,
                LocalSeverityExpression = GetLocalSeverity(severityValue),
                MessageCodeValue = messageCodeValue,
                Name = message.Name,
                PriorityExpression = GetPriority(priorityValue, severityValue),
                PriorityName = message.Priority,
                SeverityExpression = GetSeverity(severityValue),
                SeverityName = message.Severity,
                SeverityPrefix = GetSeverityPrefix(severityValue),
                Xml = message,
            };
        }

        private long GetSeverityCode(ApplicationMessageSeverity severity)
        {
            return 0x10000000000L * (16L - (long)severity);
        }

        private string GetSeverityPrefix(ApplicationMessageSeverity severity)
        {
            if (severity <= ApplicationMessageSeverity.Verbose)
            {
                return Enum.GetName(severity.GetType(), severity);
            }

            return ApplicationMessageSeverity.Custom + ((long)severity).ToString("D2");
        }

        private T GetValue<T>(string name)
        {
            long numericValue;
            bool isCustom = long.TryParse(name, out numericValue);

            if (isCustom)
            {
                return (T)(object)numericValue;
            }

            return (T)Enum.Parse(typeof(T), name);
        }
        #endregion Private Methods
    }
}
