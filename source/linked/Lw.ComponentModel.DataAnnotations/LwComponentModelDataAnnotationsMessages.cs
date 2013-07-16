using System;
using System.Collections.Generic;
using System.Globalization;
using Lw.ApplicationMessages;
using Lw.Collections.Generic;
using Lw.Resources;

namespace Lw.ComponentModel.DataAnnotations
{
    /// <summary>
    ///     Provides messages for the Lw.ComponentModel.DataAnnotations assembly.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     Application messages are usually created by defining an AMDL file in a project and adding the 
    ///     (ProjectName)Messages.CS.tt and (ProjectName)Messages.RESX.tt T4 templates to the project. Project 
    ///     developers us the <c>CreateMessage</c> methods to create instances of <see cref="ApplicationMessage"/> that 
    ///     correspond to the message needed. The developer passes the desired <see cref="ApplicationMessage"/> code 
    ///     defined as a static field and passes the appropriate arguments in the specified order with the specified 
    ///     types, as defined in the AMDL file. These messages can be logged directly by the 
    ///     <see cref="Lw.Diagnostics.ILogWriter"/> using the priority information within the 
    ///     <see cref="ApplicationMessage"/>.
    ///     <para></para>
    ///     Every <see cref="ApplicationMessage"/> seven properties:
    ///     <list type="table">
    ///         <listheader>
    ///             <term>Property</term>
    ///             <description>Comments</description>
    ///         </listheader>
    ///         <item>
    ///             <term>Schema</term>
    ///             <description>
    ///                 The layout identifier for the messages code. Only codes that begin with the Schema Code
    ///                 can be interpreted as an Application Message. Value is 0x00001.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>Severity</term>
    ///             <description>The message severity.</description>
    ///         </item>
    ///         <item>
    ///             <term>Priority</term>
    ///             <description>
    ///                 The message prioirty, used for logging thresholds. A value of Default (0) indicates that 
    ///                 the priority is derived from the severity.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>Authority</term>
    ///             <description>Code indicating the responsible party for defining Domain and Library.</description>
    ///         </item>
    ///         <item>
    ///             <term>Domain</term>
    ///             <description>
    ///                 For system code, represents the area of application functionality that the libraries pertain. 
    ///                 For business or application code, represents the registerd Business Domain of the library or
    ///                 application.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>Library</term>
    ///             <description>A C# project or interrelated group of projects.</description>
    ///         </item>
    ///         <item>
    ///             <term>Base Code</term>
    ///             <description>The message base code. Unique within an individual library and severity.</description>
    ///         </item>
    ///     </list>
    ///     </para><para>
    ///     Layout of the message code is described below.
    ///     <code>
    ///          /-------------- Schema:              Constant. Only these are Application Messages.
    ///          |                                    00001
    ///          |/------------- Severity Code:      Message severity.
    ///          ||                                  0 - F
    ///          ||                                          0 - A Inverse of custom severities 5 - 16. 0 = 16, etc.
    ///          ||                                          B Verbose. Used for logging purposes.
    ///          ||                                          C Information
    ///          ||                                          D Warning. 
    ///          ||                                          E Error. Application or system error.
    ///          ||                                          F Critical. Application or system failure or data 
    ///          ||                                            corruption. Application or system must be stopped.
    ///          ||/------------ Priority Code:      Message priority. Used for logging thresholds.
    ///          |||                                 0 - F
    ///          |||                                         0 Default. Prority is inferred by Severity.
    ///          |||                                                Severity    Priority
    ///          |||                                                --------    --------
    ///          |||                                                Critical    Mandatory
    ///          |||                                                Error       High
    ///          |||                                                Warning     Medium
    ///          |||                                                Information Low
    ///          |||                                                Verbose     Very Low
    ///          |||                                         1 Mandatory. Always logged.
    ///          |||                                         2 High
    ///          |||                                         3 Medium
    ///          |||                                         4 Low
    ///          |||                                         5 Very Low
    ///          |||                                         6 - F. Numeric custom priorities.
    ///          |||
    ///          |||
    ///          |||
    ///     [    ]||
    ///     0000 1F01 0101 F001
    ///             | [][] |[ ]
    ///             |  | | |  |
    ///             |  | | |  \- Base Code:           Raw code value. Unique within Library/Severity.
    ///             |  | | |                          000 - FFF
    ///             |  | | |                          0000 - FFFF if not using local severity codes. (Rare)
    ///             |  | | |
    ///             |  | | \---- Local Severity Code: Same format as Severity Code below.
    ///             |  | |
    ///             |  | \------ Library Code:        Libraries in Domain. Identifes an application or shared library.
    ///             |  |                              00 - FF Code defined by Authority.
    ///             |  |
    ///             |  \-------- Domain Code:         Domains of Authority. Identifies a business domain or shared
    ///             |                                 or application feature shared library implements.
    ///             |                                 00 - FF Code defined by Authority
    ///             |
    ///             \----------- Authority Code:      Authority. Owner of Domain and Library codes.
    ///                                               0 - F Statically defined. Only two are defined: 
    ///                                                      0 Enterprise. For Core and System Libraries.
    ///                                                      1 Corporate.  For Business Domains and Applications.
    ///                                                      2 - F Not defined.
    ///     </code>
    ///     </para>
    /// </remarks>
    /// <seealso cref="ApplicationMessage"/>
    public class LwComponentModelDataAnnotationsMessages
    {
        #region Public Methods
        /// <summary>
        ///     Creates an application message.
        /// </summary>
        /// <param name="messageCode">
        ///     A message code defined by <see cref="LwComponentModelDataAnnotationsMessages"/>.
        /// </param>
        /// <param name="args">
        ///     The message arguements. Must match the message format string.
        /// </param>
        /// <returns>
        ///     An <see cref="ApplicationMessage"/>
        /// </returns>
        /// <seealso cref="GetFormatString(long)"/>
        public static ApplicationMessage CreateMessage(
            long messageCode, params object[] args)
        {
            return CreateMessage(null, Guid.Empty, null, messageCode, (CultureInfo)null, args);
        }

        /// <summary>
        ///     Creates an application message.
        /// </summary>
		/// <param name="obj">
        ///     The object that this message applies to.
		/// </param>
		/// <param name="memberNames">
		///    Names of the properties that pertain to the message or <see langword="null"/> if none pertain.
		/// </param>
        /// <param name="messageCode">
        ///     A message code defined by <see cref="LwComponentModelDataAnnotationsMessages"/>.
        /// </param>
        /// <param name="culture">
        ///     The <see cref="CultureInfo"/> object that represents the culture 
        ///     for which the resource is localized. Note that if the resource is not localized
        ///     for this culture, the lookup will fall back using the culture's 
        ///     <see cref="CultureInfo.Parent"/> property, stopping after 
        ///     looking in the neutral culture.  If this value is <see langword="null"/>, the 
        ///     <see cref="CultureInfo"/> is obtained using the culture's
        ///     <see cref="CultureInfo.CurrentUICulture"/> property.
        /// </param>
        /// <param name="args">
        ///     The message arguements. Must match the message format string.
        /// </param>
        /// <returns>
        ///     An <see cref="ApplicationMessage"/>
        /// </returns>
        /// <seealso cref="GetFormatString(long, CultureInfo)"/>
        public static ApplicationMessage CreateMessage(
			object obj, IEnumerable<string> memberNames, long messageCode, params object[] args)
        {
            return CreateMessage(obj, null, memberNames, messageCode, (CultureInfo)null, args);
        }

        /// <summary>
        ///     Creates an application message.
        /// </summary>
        /// <param name="objectUid">
        ///     Uniquely defines the object that this message applies to.
        /// </param>
		/// <param name="memberNames">
		///    Names of the properties that pertain to the message or <see langword="null"/> if none pertain.
		/// </param>
        /// <param name="messageCode">
        ///     A message code defined by <see cref="LwComponentModelDataAnnotationsMessages"/>.
        /// </param>
        /// <param name="args">
        ///     The message arguements. Must match the message format string.
        /// </param>
        /// <returns>
        ///     An <see cref="ApplicationMessage"/>
        /// </returns>
        /// <seealso cref="GetFormatString(long)"/>
        public static ApplicationMessage CreateMessage(
            Guid? objectUid, IEnumerable<string> memberNames, long messageCode, params object[] args)
        {
            return CreateMessage(null, objectUid, memberNames, messageCode, (CultureInfo)null, args);
        }

        /// <summary>
        ///     Creates an application message.
        /// </summary>
        /// <param name="messageCode">
        ///     A message code defined by <see cref="LwComponentModelDataAnnotationsMessages"/>.
        /// </param>
        /// <param name="culture">
        ///     The <see cref="CultureInfo"/> object that represents the culture 
        ///     for which the resource is localized. Note that if the resource is not localized
        ///     for this culture, the lookup will fall back using the culture's 
        ///     <see cref="CultureInfo.Parent"/> property, stopping after 
        ///     looking in the neutral culture.  If this value is <see langword="null"/>, the 
        ///     <see cref="CultureInfo"/> is obtained using the culture's
        ///     <see cref="CultureInfo.CurrentUICulture"/> property.
        /// </param>
        /// <param name="args">
        ///     The message arguements. Must match the message format string.
        /// </param>
        /// <returns>
        ///     An <see cref="ApplicationMessage"/>
        /// </returns>
        /// <seealso cref="GetFormatString(long, CultureInfo)"/>
        public static ApplicationMessage CreateMessage(
            long messageCode, CultureInfo culture, params object[] args)
        {
            return CreateMessage(null, null, null, messageCode, culture, args);
        }

        /// <summary>
        ///     Creates an application message.
        /// </summary>
		/// <param name="obj">
        ///     The object that this message applies to.
		/// </param>
		/// <param name="memberNames">
		///    Names of the properties that pertain to the message or <see langword="null"/> if none pertain.
		/// </param>
        /// <param name="messageCode">
        ///     A message code defined by <see cref="LwComponentModelDataAnnotationsMessages"/>.
        /// </param>
        /// <param name="culture">
        ///     The <see cref="CultureInfo"/> object that represents the culture 
        ///     for which the resource is localized. Note that if the resource is not localized
        ///     for this culture, the lookup will fall back using the culture's 
        ///     <see cref="CultureInfo.Parent"/> property, stopping after 
        ///     looking in the neutral culture.  If this value is <see langword="null"/>, the 
        ///     <see cref="CultureInfo"/> is obtained using the culture's
        ///     <see cref="CultureInfo.CurrentUICulture"/> property.
        /// </param>
        /// <param name="args">
        ///     The message arguements. Must match the message format string.
        /// </param>
        /// <returns>
        ///     An <see cref="ApplicationMessage"/>
        /// </returns>
        /// <seealso cref="GetFormatString(long, CultureInfo)"/>
        public static ApplicationMessage CreateMessage(
			object obj, 
			IEnumerable<string> memberNames,
            long messageCode, 
            CultureInfo culture, 
            params object[] args)
        {
            return CreateMessage(obj, null, memberNames, messageCode, culture, args);
        }

        /// <summary>
        ///     Creates an application message.
        /// </summary>
        /// <param name="objectUid">
        ///     Uniquely defines the object that this message applies to.
        /// </param>
		/// <param name="memberNames">
		///    Names of the properties that pertain to the message or <see langword="null"/> if none pertain.
		/// </param>
        /// <param name="messageCode">
        ///     A message code defined by <see cref="LwComponentModelDataAnnotationsMessages"/>.
        /// </param>
        /// <param name="culture">
        ///     The <see cref="CultureInfo"/> object that represents the culture 
        ///     for which the resource is localized. Note that if the resource is not localized
        ///     for this culture, the lookup will fall back using the culture's 
        ///     <see cref="CultureInfo.Parent"/> property, stopping after 
        ///     looking in the neutral culture.  If this value is <see langword="null"/>, the 
        ///     <see cref="CultureInfo"/> is obtained using the culture's
        ///     <see cref="CultureInfo.CurrentUICulture"/> property.
        /// </param>
        /// <param name="args">
        ///     The message arguements. Must match the message format string.
        /// </param>
        /// <returns>
        ///     An <see cref="ApplicationMessage"/>
        /// </returns>
        /// <seealso cref="GetFormatString(long, CultureInfo)"/>
        public static ApplicationMessage CreateMessage(
            Guid? objectUid, 
			IEnumerable<string> memberNames,
            long messageCode, 
            CultureInfo culture, 
            params object[] args)
        {
            return CreateMessage(null, objectUid, memberNames, messageCode, culture, args);
        }


        /// <summary>
        ///     Creates an application message.
        /// </summary>
		/// <param name="obj">
        ///     The object that this message applies to.
		/// </param>
        /// <param name="objectUid">
        ///     Uniquely defines the object that this message applies to or <see langword="null"> if taken from 
		///     <paramref name="obj"/> or cannot be specified.
        /// </param>
		/// <param name="memberNames">
		///    Names of the properties that pertain to the message or <see langword="null"/> if none pertain.
		/// </param>
        /// <param name="messageCode">
        ///     A message code defined by <see cref="LwComponentModelDataAnnotationsMessages"/>.
        /// </param>
        /// <param name="culture">
        ///     The <see cref="CultureInfo"/> object that represents the culture 
        ///     for which the resource is localized. Note that if the resource is not localized
        ///     for this culture, the lookup will fall back using the culture's 
        ///     <see cref="CultureInfo.Parent"/> property, stopping after 
        ///     looking in the neutral culture.  If this value is <see langword="null"/>, the 
        ///     <see cref="CultureInfo"/> is obtained using the culture's
        ///     <see cref="CultureInfo.CurrentUICulture"/> property.
        /// </param>
        /// <param name="args">
        ///     The message arguements. Must match the message format string.
        /// </param>
        /// <returns>
        ///     An <see cref="ApplicationMessage"/>
        /// </returns>
        /// <seealso cref="GetFormatString(long, CultureInfo)"/>
        public static ApplicationMessage CreateMessage(
			object obj,
            Guid? objectUid, 
			IEnumerable<string> memberNames,
            long messageCode, 
            CultureInfo culture, 
            params object[] args)
        {
            string format = GetFormatString(messageCode, culture);

            return new ApplicationMessage(obj, objectUid, memberNames, messageCode, format, args);
        }

        /// <summary>
        ///     Returns the format string associated with the specified message code.
        /// </summary>
        /// <param name="messageCode">
        ///     A message code defined by <see cref="LwComponentModelDataAnnotationsMessages"/>.
        /// </param>
        /// <returns>
        ///     A format string localized to <see cref="CultureInfo.CurrentUICulture"/>.
        /// </returns>
        public static string GetFormatString(long messageCode)
        {
            return GetFormatString(messageCode, null);
        }

        /// <summary>
        ///     Returns the format string associated with the specified message code.
        /// </summary>
        /// <param name="messageCode">
        ///     A message code defined by <see cref="LwComponentModelDataAnnotationsMessages"/>.
        /// </param>
        /// <param name="culture">
        ///     The <see cref="CultureInfo"/> object that represents the culture 
        ///     for which the resource is localized. Note that if the resource is not localized
        ///     for this culture, the lookup will fall back using the culture's 
        ///     <see cref="CultureInfo.Parent"/> property, stopping after 
        ///     looking in the neutral culture.  If this value is <see langword="null"/>, the 
        ///     <see cref="CultureInfo"/> is obtained using the culture's
        ///     <see cref="CultureInfo.CurrentUICulture"/> property.
        /// </param>
        /// <returns>
        ///     A format string localized to the specified culture.
        /// </returns>
        public static string GetFormatString(long messageCode, CultureInfo culture)
        {
            string resourceName = LwComponentModelDataAnnotationsMessages.messageFormatResourceNames.GetValue(
                    messageCode,
                    () => { throw new ArgumentOutOfRangeException("messageCode"); });

            return LwComponentModelDataAnnotationsMessagesResources.ResourceManager.GetString(resourceName, culture);
        }
        #endregion Public Methods

        #region Public Fields
        /// <summary>
        ///     Common prefix for all message codes defined by <see cref="LwComponentModelDataAnnotationsMessages"/>.
        /// </summary>
        /// <value>
        ///     0x100101010000
        /// </value>
        /// <remarks>
        ///     <list>
        ///         <listheader>
        ///             <term>Property</term>
        ///             <description>Value</description>
        ///         </listheader>
        ///         <item>
        ///             <term>Schema</term>
        ///             <description>Enterprise</description>
        ///         </item>
        ///         <item>
        ///             <term>Authority</term>
        ///             <description>Enterprise</description>
        ///         </item>
        ///         <item>
        ///             <term>Domain</term>
        ///             <description>Core</description>
        ///         </item>
        ///         <item>
        ///             <term>Library</term>
        ///             <description>Core</description>
        ///         </item>
        ///     </list>
        /// </remarks>
        public const long MessageCodePrefix =
            (long)ApplicationMessageSchemaCode.Enterprise |
            (long)ApplicationMessageAuthorityCode.Enterprise |
            (long)Lw.ApplicationMessages.EnterpriseMessageDomainCode.Core |
            (long)Lw.ApplicationMessages.CoreMessageLibraryCode.Core;

        #region Error Messages
        /// <summary>
        ///     '{0}'
        /// </summary>
        /// <value>
        ///     0x1E010101E001
        /// </value>
        /// <remarks>
        ///     <para>
        ///     Properties:
        ///     </para><para>
        ///     <list>
        ///         <listheader>
        ///             <term>Property</term>
        ///             <description>Value</description>
        ///         </listheader>
        ///         <item>
        ///             <term>Prefix</term>
        ///             <description>0x100101010000</description>
        ///         </item>
        ///         <item>
        ///             <term>Severity</term>
        ///             <description>Error</description>
        ///         </item>
        ///         <item>
        ///             <term>Priority</term>
        ///             <description>Default</description>
        ///         </item>
        ///         <item>
        ///             <term>Base Code</term>
        ///             <description>0x001</description>
        ///         </item>
        ///     </list>
        ///     </para><para>
        ///     Arguments:
        ///     </para><para>
        ///     <list>
        ///         <listheader>
        ///             <term>Argument</term>
        ///             <description>Value</description>
        ///         </listheader>
        ///         <item>
        ///             <term>0</term>
        ///             <description>
        ///                 <para>
        ///                 <c>message</c>: String
        ///                 </para><para>
        ///                 The validation error message generated by the validator.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///     </list>
        ///     </para>
        /// </remarks>
        [ApplicationMessageCode]
        [ApplicationMessageArgument(
            "message", 
            typeof(String), 
            "The validation error message generated by the validator.")]
        [ResourceName("ErrorMessageValidationError")]
        public const long ErrorMessageCodeValidationError =
            LwComponentModelDataAnnotationsMessages.MessageCodePrefix | 
            (long)ApplicationMessageSeverityCode.Error | 
            (long)ApplicationMessagePriorityCode.Default | 
            (long)ApplicationMessageLocalSeverityCode.Error |
            0x001L;
        /// <summary>
        ///     Properties are not equal. The validated property has value of '{1}'. Property '{2}' has value of '{4}'.
        /// </summary>
        /// <value>
        ///     0x1E010101E011
        /// </value>
        /// <remarks>
        ///     <para>
        ///     Properties:
        ///     </para><para>
        ///     <list>
        ///         <listheader>
        ///             <term>Property</term>
        ///             <description>Value</description>
        ///         </listheader>
        ///         <item>
        ///             <term>Prefix</term>
        ///             <description>0x100101010000</description>
        ///         </item>
        ///         <item>
        ///             <term>Severity</term>
        ///             <description>Error</description>
        ///         </item>
        ///         <item>
        ///             <term>Priority</term>
        ///             <description>Default</description>
        ///         </item>
        ///         <item>
        ///             <term>Base Code</term>
        ///             <description>0x011</description>
        ///         </item>
        ///     </list>
        ///     </para><para>
        ///     Arguments:
        ///     </para><para>
        ///     <list>
        ///         <listheader>
        ///             <term>Argument</term>
        ///             <description>Value</description>
        ///         </listheader>
        ///         <item>
        ///             <term>0</term>
        ///             <description>
        ///                 <para>
        ///                 <c>PropertyType</c>: String
        ///                 </para><para>
        ///                 The simple property type name of the property with the attribute.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>1</term>
        ///             <description>
        ///                 <para>
        ///                 <c>PropertyValue</c>: String
        ///                 </para><para>
        ///                 The value of the property with the attribute converted to a string.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>2</term>
        ///             <description>
        ///                 <para>
        ///                 <c>OtherPropertyName</c>: String
        ///                 </para><para>
        ///                 Name of the other property.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>3</term>
        ///             <description>
        ///                 <para>
        ///                 <c>OtherPropertyType</c>: String
        ///                 </para><para>
        ///                 The simple property type name of the other property.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>4</term>
        ///             <description>
        ///                 <para>
        ///                 <c>OtherPropertyValue</c>: String
        ///                 </para><para>
        ///                 The value of the other property converted to a string.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///     </list>
        ///     </para>
        /// </remarks>
        [ApplicationMessageCode]
        [ApplicationMessageArgument(
            "PropertyType", 
            typeof(String), 
            "The simple property type name of the property with the attribute.")]
        [ApplicationMessageArgument(
            "PropertyValue", 
            typeof(String), 
            "The value of the property with the attribute converted to a string.")]
        [ApplicationMessageArgument(
            "OtherPropertyName", 
            typeof(String), 
            "Name of the other property.")]
        [ApplicationMessageArgument(
            "OtherPropertyType", 
            typeof(String), 
            "The simple property type name of the other property.")]
        [ApplicationMessageArgument(
            "OtherPropertyValue", 
            typeof(String), 
            "The value of the other property converted to a string.")]
        [ResourceName("ErrorMessagePropertyCompareError")]
        public const long ErrorMessageCodePropertyCompareError =
            LwComponentModelDataAnnotationsMessages.MessageCodePrefix | 
            (long)ApplicationMessageSeverityCode.Error | 
            (long)ApplicationMessagePriorityCode.Default | 
            (long)ApplicationMessageLocalSeverityCode.Error |
            0x011L;
        /// <summary>
        ///     The member must not be any longer than '{0}'. Yours has '{1}'.
        /// </summary>
        /// <value>
        ///     0x1E010101E012
        /// </value>
        /// <remarks>
        ///     <para>
        ///     Properties:
        ///     </para><para>
        ///     <list>
        ///         <listheader>
        ///             <term>Property</term>
        ///             <description>Value</description>
        ///         </listheader>
        ///         <item>
        ///             <term>Prefix</term>
        ///             <description>0x100101010000</description>
        ///         </item>
        ///         <item>
        ///             <term>Severity</term>
        ///             <description>Error</description>
        ///         </item>
        ///         <item>
        ///             <term>Priority</term>
        ///             <description>Default</description>
        ///         </item>
        ///         <item>
        ///             <term>Base Code</term>
        ///             <description>0x012</description>
        ///         </item>
        ///     </list>
        ///     </para><para>
        ///     Arguments:
        ///     </para><para>
        ///     <list>
        ///         <listheader>
        ///             <term>Argument</term>
        ///             <description>Value</description>
        ///         </listheader>
        ///         <item>
        ///             <term>0</term>
        ///             <description>
        ///                 <para>
        ///                 <c>Length</c>: Int32
        ///                 </para><para>
        ///                 The maximum allowable length.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>1</term>
        ///             <description>
        ///                 <para>
        ///                 <c>Value</c>: Int32
        ///                 </para><para>
        ///                 The actual length.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///     </list>
        ///     </para>
        /// </remarks>
        [ApplicationMessageCode]
        [ApplicationMessageArgument(
            "Length", 
            typeof(Int32), 
            "The maximum allowable length.")]
        [ApplicationMessageArgument(
            "Value", 
            typeof(Int32), 
            "The actual length.")]
        [ResourceName("ErrorMessageMaximumLengthError")]
        public const long ErrorMessageCodeMaximumLengthError =
            LwComponentModelDataAnnotationsMessages.MessageCodePrefix | 
            (long)ApplicationMessageSeverityCode.Error | 
            (long)ApplicationMessagePriorityCode.Default | 
            (long)ApplicationMessageLocalSeverityCode.Error |
            0x012L;
        /// <summary>
        ///     The member must not be any shorter than '{0}'. Yours has '{1}'.
        /// </summary>
        /// <value>
        ///     0x1E010101E013
        /// </value>
        /// <remarks>
        ///     <para>
        ///     Properties:
        ///     </para><para>
        ///     <list>
        ///         <listheader>
        ///             <term>Property</term>
        ///             <description>Value</description>
        ///         </listheader>
        ///         <item>
        ///             <term>Prefix</term>
        ///             <description>0x100101010000</description>
        ///         </item>
        ///         <item>
        ///             <term>Severity</term>
        ///             <description>Error</description>
        ///         </item>
        ///         <item>
        ///             <term>Priority</term>
        ///             <description>Default</description>
        ///         </item>
        ///         <item>
        ///             <term>Base Code</term>
        ///             <description>0x013</description>
        ///         </item>
        ///     </list>
        ///     </para><para>
        ///     Arguments:
        ///     </para><para>
        ///     <list>
        ///         <listheader>
        ///             <term>Argument</term>
        ///             <description>Value</description>
        ///         </listheader>
        ///         <item>
        ///             <term>0</term>
        ///             <description>
        ///                 <para>
        ///                 <c>Length</c>: Int32
        ///                 </para><para>
        ///                 The minimum allowable length.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>1</term>
        ///             <description>
        ///                 <para>
        ///                 <c>Value</c>: Int32
        ///                 </para><para>
        ///                 The actual length.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///     </list>
        ///     </para>
        /// </remarks>
        [ApplicationMessageCode]
        [ApplicationMessageArgument(
            "Length", 
            typeof(Int32), 
            "The minimum allowable length.")]
        [ApplicationMessageArgument(
            "Value", 
            typeof(Int32), 
            "The actual length.")]
        [ResourceName("ErrorMessageMinimumLengthError")]
        public const long ErrorMessageCodeMinimumLengthError =
            LwComponentModelDataAnnotationsMessages.MessageCodePrefix | 
            (long)ApplicationMessageSeverityCode.Error | 
            (long)ApplicationMessagePriorityCode.Default | 
            (long)ApplicationMessageLocalSeverityCode.Error |
            0x013L;
        /// <summary>
        ///     The member's value must match pattern '{0}'. Yours is '{1}'.
        /// </summary>
        /// <value>
        ///     0x1E010101E014
        /// </value>
        /// <remarks>
        ///     <para>
        ///     Properties:
        ///     </para><para>
        ///     <list>
        ///         <listheader>
        ///             <term>Property</term>
        ///             <description>Value</description>
        ///         </listheader>
        ///         <item>
        ///             <term>Prefix</term>
        ///             <description>0x100101010000</description>
        ///         </item>
        ///         <item>
        ///             <term>Severity</term>
        ///             <description>Error</description>
        ///         </item>
        ///         <item>
        ///             <term>Priority</term>
        ///             <description>Default</description>
        ///         </item>
        ///         <item>
        ///             <term>Base Code</term>
        ///             <description>0x014</description>
        ///         </item>
        ///     </list>
        ///     </para><para>
        ///     Arguments:
        ///     </para><para>
        ///     <list>
        ///         <listheader>
        ///             <term>Argument</term>
        ///             <description>Value</description>
        ///         </listheader>
        ///         <item>
        ///             <term>0</term>
        ///             <description>
        ///                 <para>
        ///                 <c>Pattern</c>: String
        ///                 </para><para>
        ///                 Regular expression pattern the member was matched against.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>1</term>
        ///             <description>
        ///                 <para>
        ///                 <c>Value</c>: String
        ///                 </para><para>
        ///                 The member value.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///     </list>
        ///     </para>
        /// </remarks>
        [ApplicationMessageCode]
        [ApplicationMessageArgument(
            "Pattern", 
            typeof(String), 
            "Regular expression pattern the member was matched against.")]
        [ApplicationMessageArgument(
            "Value", 
            typeof(String), 
            "The member value.")]
        [ResourceName("ErrorMessageValueRangeError")]
        public const long ErrorMessageCodeValueRangeError =
            LwComponentModelDataAnnotationsMessages.MessageCodePrefix | 
            (long)ApplicationMessageSeverityCode.Error | 
            (long)ApplicationMessagePriorityCode.Default | 
            (long)ApplicationMessageLocalSeverityCode.Error |
            0x014L;
        /// <summary>
        ///     The member is required to have a value. Yours does not.
        /// </summary>
        /// <value>
        ///     0x1E010101E015
        /// </value>
        /// <remarks>
        ///     <para>
        ///     Properties:
        ///     </para><para>
        ///     <list>
        ///         <listheader>
        ///             <term>Property</term>
        ///             <description>Value</description>
        ///         </listheader>
        ///         <item>
        ///             <term>Prefix</term>
        ///             <description>0x100101010000</description>
        ///         </item>
        ///         <item>
        ///             <term>Severity</term>
        ///             <description>Error</description>
        ///         </item>
        ///         <item>
        ///             <term>Priority</term>
        ///             <description>Default</description>
        ///         </item>
        ///         <item>
        ///             <term>Base Code</term>
        ///             <description>0x015</description>
        ///         </item>
        ///     </list>
        ///     </para><para>
        ///     Arguments:
        ///     </para><para>
        ///     <list>
        ///         <listheader>
        ///             <term>Argument</term>
        ///             <description>Value</description>
        ///         </listheader>
        ///     </list>
        ///     </para>
        /// </remarks>
        [ApplicationMessageCode]
        [ResourceName("ErrorMessageValueRequiredError")]
        public const long ErrorMessageCodeValueRequiredError =
            LwComponentModelDataAnnotationsMessages.MessageCodePrefix | 
            (long)ApplicationMessageSeverityCode.Error | 
            (long)ApplicationMessagePriorityCode.Default | 
            (long)ApplicationMessageLocalSeverityCode.Error |
            0x015L;
        /// <summary>
        ///     The member's value must must have between '{0}' and '{1}' characters. Yours, '{2}' does not.
        /// </summary>
        /// <value>
        ///     0x1E010101E016
        /// </value>
        /// <remarks>
        ///     <para>
        ///     Properties:
        ///     </para><para>
        ///     <list>
        ///         <listheader>
        ///             <term>Property</term>
        ///             <description>Value</description>
        ///         </listheader>
        ///         <item>
        ///             <term>Prefix</term>
        ///             <description>0x100101010000</description>
        ///         </item>
        ///         <item>
        ///             <term>Severity</term>
        ///             <description>Error</description>
        ///         </item>
        ///         <item>
        ///             <term>Priority</term>
        ///             <description>Default</description>
        ///         </item>
        ///         <item>
        ///             <term>Base Code</term>
        ///             <description>0x016</description>
        ///         </item>
        ///     </list>
        ///     </para><para>
        ///     Arguments:
        ///     </para><para>
        ///     <list>
        ///         <listheader>
        ///             <term>Argument</term>
        ///             <description>Value</description>
        ///         </listheader>
        ///         <item>
        ///             <term>0</term>
        ///             <description>
        ///                 <para>
        ///                 <c>MinimumLength</c>: Int32
        ///                 </para><para>
        ///                 Minimum number of characters for the string member.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>1</term>
        ///             <description>
        ///                 <para>
        ///                 <c>MaximumLength</c>: Int32
        ///                 </para><para>
        ///                 Minimum number of characters for the string member.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>2</term>
        ///             <description>
        ///                 <para>
        ///                 <c>Value</c>: String
        ///                 </para><para>
        ///                 The member value.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///     </list>
        ///     </para>
        /// </remarks>
        [ApplicationMessageCode]
        [ApplicationMessageArgument(
            "MinimumLength", 
            typeof(Int32), 
            "Minimum number of characters for the string member.")]
        [ApplicationMessageArgument(
            "MaximumLength", 
            typeof(Int32), 
            "Minimum number of characters for the string member.")]
        [ApplicationMessageArgument(
            "Value", 
            typeof(String), 
            "The member value.")]
        [ResourceName("ErrorMessageMinimumStringLengthError")]
        public const long ErrorMessageCodeMinimumStringLengthError =
            LwComponentModelDataAnnotationsMessages.MessageCodePrefix | 
            (long)ApplicationMessageSeverityCode.Error | 
            (long)ApplicationMessagePriorityCode.Default | 
            (long)ApplicationMessageLocalSeverityCode.Error |
            0x016L;
        #endregion Error Messages
        #endregion Public Fields


        #region Private Constructors
        static LwComponentModelDataAnnotationsMessages()
        {
            LwComponentModelDataAnnotationsMessages.messageFormatResourceNames = ApplicationMessage.CreateMessageCodeResourceNameMap(
                typeof(LwComponentModelDataAnnotationsMessages));
        }
        #endregion Private Constructors

        #region Private Fields
        private static readonly IDictionary<long, string> messageFormatResourceNames;
        #endregion Private Fields
    }
}
