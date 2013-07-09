using System;
using System.Collections.Generic;
using System.Globalization;
using Lw.ApplicationMessages;
using Lw.Collections.Generic;
using Lw.Resources;

namespace Lw
{
    /// <summary>
    ///     Provides messages for the Lw.Core assembly.
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
    public class LwCoreMessages
    {
        #region Public Methods
        /// <summary>
        ///     Creates an application message.
        /// </summary>
        /// <param name="messageCode">
        ///     A message code defined by <see cref="LwCoreMessages"/>.
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
            return CreateMessage(Guid.Empty, messageCode, args);
        }

        /// <summary>
        ///     Creates an application message.
        /// </summary>
        /// <param name="objectUid">
        ///     Uniquely defines the object that this message applies to.
        /// </param>
        /// <param name="messageCode">
        ///     A message code defined by <see cref="LwCoreMessages"/>.
        /// </param>
        /// <param name="args">
        ///     The message arguements. Must match the message format string.
        /// </param>
        /// <returns>
        ///     An <see cref="ApplicationMessage"/>
        /// </returns>
        /// <seealso cref="GetFormatString(long)"/>
        public static ApplicationMessage CreateMessage(
            Guid objectUid, long messageCode, params object[] args)
        {
            return CreateMessage(
                objectUid, messageCode, (CultureInfo)null, args);
        }

        /// <summary>
        ///     Creates an application message.
        /// </summary>
        /// <param name="messageCode">
        ///     A message code defined by <see cref="LwCoreMessages"/>.
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
            return CreateMessage(Guid.Empty, messageCode, culture, args);
        }

        /// <summary>
        ///     Creates an application message.
        /// </summary>
        /// <param name="objectUid">
        ///     Uniquely defines the object that this message applies to.
        /// </param>
        /// <param name="messageCode">
        ///     A message code defined by <see cref="LwCoreMessages"/>.
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
            Guid objectUid, 
            long messageCode, 
            CultureInfo culture, 
            params object[] args)
        {
            string format = GetFormatString(messageCode, culture);

            return new ApplicationMessage(objectUid, messageCode, format, args);
        }

        /// <summary>
        ///     Returns the format string associated with the specified message code.
        /// </summary>
        /// <param name="messageCode">
        ///     A message code defined by <see cref="LwCoreMessages"/>.
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
        ///     A message code defined by <see cref="LwCoreMessages"/>.
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
            string resourceName = LwCoreMessages.messageFormatResourceNames.GetValue(
                    messageCode,
                    () => { throw new ArgumentOutOfRangeException("messageCode"); });

            return LwCoreMessagesResources.ResourceManager.GetString(resourceName, culture);
        }
        #endregion Public Methods

        #region Public Fields
        /// <summary>
        ///     Common prefix for all message codes defined by <see cref="LwCoreMessages"/>.
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

        #region Critical Messages
        /// <summary>
        ///     An internal application error has occured. The application cannot continue. Description: '{0}'.
        /// </summary>
        /// <value>
        ///     0x1F010101F001
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
        ///             <description>Critical</description>
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
        ///                 <c>description</c>: String
        ///                 </para><para>
        ///                 The conditions under which the internal error occured.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///     </list>
        ///     </para>
        /// </remarks>
        [ApplicationMessageCode]
        [ApplicationMessageArgument(
            "description", 
            typeof(String), 
            "The conditions under which the internal error occured.")]
        [ResourceName("CriticalMessageInternalError")]
        public const long CriticalMessageCodeInternalError =
            LwCoreMessages.MessageCodePrefix | 
            (long)ApplicationMessageSeverityCode.Critical | 
            (long)ApplicationMessagePriorityCode.Default | 
            (long)ApplicationMessageLocalSeverityCode.Critical |
            0x001L;
        #endregion Critical Messages

        #region Error Messages
        /// <summary>
        ///     The request is in error. Description: '{0}'.
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
        ///                 <c>description</c>: String
        ///                 </para><para>
        ///                 Explanation of error.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///     </list>
        ///     </para>
        /// </remarks>
        [ApplicationMessageCode]
        [ApplicationMessageArgument(
            "description", 
            typeof(String), 
            "Explanation of error.")]
        [ResourceName("ErrorMessageRequestError")]
        public const long ErrorMessageCodeRequestError =
            LwCoreMessages.MessageCodePrefix | 
            (long)ApplicationMessageSeverityCode.Error | 
            (long)ApplicationMessagePriorityCode.Default | 
            (long)ApplicationMessageLocalSeverityCode.Error |
            0x011L;
        /// <summary>
        ///     The request at offset '{0}' is null.
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
        ///                 <c>requestIndex</c>: Int32
        ///                 </para><para>
        ///                 The zero-offset index of the request in the request list.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///     </list>
        ///     </para>
        /// </remarks>
        [ApplicationMessageCode]
        [ApplicationMessageArgument(
            "requestIndex", 
            typeof(Int32), 
            "The zero-offset index of the request in the request list.")]
        [ResourceName("ErrorMessageRequestNullError")]
        public const long ErrorMessageCodeRequestNullError =
            LwCoreMessages.MessageCodePrefix | 
            (long)ApplicationMessageSeverityCode.Error | 
            (long)ApplicationMessagePriorityCode.Default | 
            (long)ApplicationMessageLocalSeverityCode.Error |
            0x012L;
        /// <summary>
        ///     The request at offset '{0}' has an invalid property '{1}'. Message: '{2}'.
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
        ///                 <c>requestIndex</c>: Int32
        ///                 </para><para>
        ///                 The zero-offset index of the request in the request list.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>1</term>
        ///             <description>
        ///                 <para>
        ///                 <c>propertyName</c>: String
        ///                 </para><para>
        ///                 The name of the request property with the error.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>2</term>
        ///             <description>
        ///                 <para>
        ///                 <c>errorMessage</c>: String
        ///                 </para><para>
        ///                 A message describing the error.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///     </list>
        ///     </para>
        /// </remarks>
        [ApplicationMessageCode]
        [ApplicationMessageArgument(
            "requestIndex", 
            typeof(Int32), 
            "The zero-offset index of the request in the request list.")]
        [ApplicationMessageArgument(
            "propertyName", 
            typeof(String), 
            "The name of the request property with the error.")]
        [ApplicationMessageArgument(
            "errorMessage", 
            typeof(String), 
            "A message describing the error.")]
        [ResourceName("ErrorMessageRequestParameterError")]
        public const long ErrorMessageCodeRequestParameterError =
            LwCoreMessages.MessageCodePrefix | 
            (long)ApplicationMessageSeverityCode.Error | 
            (long)ApplicationMessagePriorityCode.Default | 
            (long)ApplicationMessageLocalSeverityCode.Error |
            0x013L;
        /// <summary>
        ///     The request at offset '{0}' has a null property '{1}'.
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
        ///                 <c>requestIndex</c>: Int32
        ///                 </para><para>
        ///                 The zero-offset index of the request in the request list.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>1</term>
        ///             <description>
        ///                 <para>
        ///                 <c>propertyName</c>: String
        ///                 </para><para>
        ///                 The name of the request property with the error.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///     </list>
        ///     </para>
        /// </remarks>
        [ApplicationMessageCode]
        [ApplicationMessageArgument(
            "requestIndex", 
            typeof(Int32), 
            "The zero-offset index of the request in the request list.")]
        [ApplicationMessageArgument(
            "propertyName", 
            typeof(String), 
            "The name of the request property with the error.")]
        [ResourceName("ErrorMessageRequestParameterNullError")]
        public const long ErrorMessageCodeRequestParameterNullError =
            LwCoreMessages.MessageCodePrefix | 
            (long)ApplicationMessageSeverityCode.Error | 
            (long)ApplicationMessagePriorityCode.Default | 
            (long)ApplicationMessageLocalSeverityCode.Error |
            0x014L;
        /// <summary>
        ///     The request at offset '{0}' has a property '{1}' whose value is out the range of valid values.
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
        ///         <item>
        ///             <term>0</term>
        ///             <description>
        ///                 <para>
        ///                 <c>requestIndex</c>: Int32
        ///                 </para><para>
        ///                 The zero-offset index of the request in the request list.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>1</term>
        ///             <description>
        ///                 <para>
        ///                 <c>propertyName</c>: String
        ///                 </para><para>
        ///                 The name of the request property with the error.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///     </list>
        ///     </para>
        /// </remarks>
        [ApplicationMessageCode]
        [ApplicationMessageArgument(
            "requestIndex", 
            typeof(Int32), 
            "The zero-offset index of the request in the request list.")]
        [ApplicationMessageArgument(
            "propertyName", 
            typeof(String), 
            "The name of the request property with the error.")]
        [ResourceName("ErrorMessageRequestParameterOutOfRangeError")]
        public const long ErrorMessageCodeRequestParameterOutOfRangeError =
            LwCoreMessages.MessageCodePrefix | 
            (long)ApplicationMessageSeverityCode.Error | 
            (long)ApplicationMessagePriorityCode.Default | 
            (long)ApplicationMessageLocalSeverityCode.Error |
            0x015L;
        #endregion Error Messages

        #region Warning Messages
        /// <summary>
        ///     An ApplicationMessageCodeAttribute was placed on type member incorrectly. The member must be an Int64 field or property. Member '{1}' of Type '{0}'.
        /// </summary>
        /// <value>
        ///     0x1D010101D001
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
        ///             <description>Warning</description>
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
        ///                 <c>type</c>: String
        ///                 </para><para>
        ///                 Type that has the invalid attributet.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <term>1</term>
        ///             <description>
        ///                 <para>
        ///                 <c>name</c>: String
        ///                 </para><para>
        ///                 Name of member with the invalid attribute.
        ///                 </para>
        ///             </description>
        ///         </item>
        ///     </list>
        ///     </para>
        /// </remarks>
        [ApplicationMessageCode]
        [ApplicationMessageArgument(
            "type", 
            typeof(String), 
            "Type that has the invalid attributet.")]
        [ApplicationMessageArgument(
            "name", 
            typeof(String), 
            "Name of member with the invalid attribute.")]
        [ResourceName("WarningMessageInvalidMessageCodeAttributeMember")]
        public const long WarningMessageCodeInvalidMessageCodeAttributeMember =
            LwCoreMessages.MessageCodePrefix | 
            (long)ApplicationMessageSeverityCode.Warning | 
            (long)ApplicationMessagePriorityCode.Default | 
            (long)ApplicationMessageLocalSeverityCode.Warning |
            0x001L;
        #endregion Warning Messages
        #endregion Public Fields


        #region Private Constructors
        static LwCoreMessages()
        {
            LwCoreMessages.messageFormatResourceNames = ApplicationMessage.CreateMessageCodeResourceNameMap(
                typeof(LwCoreMessages));
        }
        #endregion Private Constructors

        #region Private Fields
        private static readonly IDictionary<long, string> messageFormatResourceNames;
        #endregion Private Fields
    }
}
