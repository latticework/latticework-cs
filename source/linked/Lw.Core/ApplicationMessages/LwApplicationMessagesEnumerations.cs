
namespace Lw.ApplicationMessages
{
    // FFFF F000 0000 0000 Mask
    // 0000 1000 0000 0000 Enterprise
    #region public enum ApplicationMessageSchema
    [EnumMapping(typeof(ApplicationMessageSchemaCode), typeof(NameEnumMappingStrategy))]
    public enum ApplicationMessageSchema
    {
        None = 0,
        Enterprise = 1,
        Custom = int.MaxValue,
    }
    #endregion public enum ApplicationMessageSchema
    #region public enum ApplicationMessageSchemaCode : long
    [EnumMapping(typeof(ApplicationMessageSchema), typeof(NameEnumMappingStrategy))]
    public enum ApplicationMessageSchemaCode : long
    {
        None = 0,
        //                       V  vv  VV  vv  V
        Mask = unchecked((long)0xFFFFF00000000000),
        //             V  vv  VV  vv  V
        Enterprise = 0x0000100000000000,
        Custom = int.MaxValue,
    }
    #endregion public enum ApplicationMessageSchemaCode : long

    // 0000 0F00 0000 0000 Mask
    // 0000 0F00 0000 0000 Error
    // 0000 0E00 0000 0000 Warning
    // 0000 0D00 0000 0000 Information
    // 0000 0C00 0000 0000 Verbose
    #region public enum ApplicationMessageSeverity
    [EnumMapping(typeof(ApplicationMessageSeverityCode), typeof(NameEnumMappingStrategy))]
    public enum ApplicationMessageSeverity
    {
        None = 0,
        Critical = 1,
        Error = 2,
        Warning = 3,
        Information = 4,
        Verbose = 5,
        Custom = int.MaxValue,
    }
    #endregion public enum ApplicationMessageSeverity
    #region public enum ApplicationMessageSeverityCode : long
    [EnumMapping(typeof(ApplicationMessageSeverity), typeof(NameEnumMappingStrategy))]
    [EquivalentEnumValueSelector("Mask", "Critical")]
    public enum ApplicationMessageSeverityCode : long
    {
        None = 0,
        //       V  vv  VV  vv  V
        Mask = 0x00000F0000000000,
        //     0x_____F__________
        //
        //         0x_____F__________
        Critical = 0x00000F0000000000,
        //      0x_____F__________
        Error = 0x00000E0000000000,
        //        0x_____F__________
        Warning = 0x00000D0000000000,
        //            0x_____F__________
        Information = 0x00000C0000000000,
        //        0x_____F__________
        Verbose = 0x00000B0000000000,
    }
    #endregion public enum ApplicationMessageSeverityCode : long
    #region public enum ApplicationMessageLocalSeverityCode : long
    [EnumMapping(typeof(ApplicationMessageSeverity), typeof(NameEnumMappingStrategy))]
    [EquivalentEnumValueSelector("Mask", "Critical")]
    public enum ApplicationMessageLocalSeverityCode : long
    {
        None = 0,
        //       V  vv  VV  vv  V
        Mask = 0x000000000000F000,
        //     0x____________F___
        //
        //         0x____________F___
        Critical = 0x000000000000F000,
        //      0x____________F___
        Error = 0x000000000000E000,
        //        0x____________F___
        Warning = 0x000000000000D000,
        //            0x____________F___
        Information = 0x000000000000C000,
        //        0x____________F___
        Verbose = 0x000000000000B000,
    }
    #endregion public enum ApplicationMessageLocalSeverityCode : long

    // 0000 00F0 0000 0000 Mask
    // 0000 0000 0000 0000 Unspecified
    // 0000 0010 0000 0000 Mandatory
    // 0000 0020 0000 0000 High
    // 0000 0030 0000 0000 Medium
    // 0000 0040 0000 0000 Low
    // 0000 0050 0000 0000 VeryLow
    #region public enum ApplicationMessagePriority : long
    [EnumMapping(typeof(ApplicationMessagePriorityCode), typeof(NameEnumMappingStrategy))]
    public enum ApplicationMessagePriority : long
    {
        Default = 0,
        Mandatory = 1,
        High = 2,
        Medium = 3,
        Low = 4,
        VeryLow = 5,
        Custom = int.MaxValue,
    }
    #endregion public enum ApplicationMessagePriority : long
    #region public enum ApplicationMessagePriorityCode : long
    [EnumMapping(typeof(ApplicationMessagePriority), typeof(NameEnumMappingStrategy))]
    public enum ApplicationMessagePriorityCode : long
    {
        Default = 0,
        //       V  vv  VV  vv  V
        Mask = 0x000000F000000000,
        //     0x______F_________
        //
        //          0x______F_________
        Mandatory = 0x0000001000000000,
        //     0x______F_________
        High = 0x0000002000000000,
        //       0x______F_________
        Medium = 0x0000003000000000,
        //    0x______F_________
        Low = 0x0000004000000000,
        //        0x______F_________
        VeryLow = 0x0000005000000000,
    }
    #endregion public enum ApplicationMessageSecondarySeverityCode : long

    // 0000 000F 0000 0000 Mask
    // 0000 0001 0000 0000 Enterprise
    // 0000 0002 0000 0000 Corporate
    #region public enum ApplicationMessageAuthority
    [EnumMapping(typeof(ApplicationMessageAuthorityCode), typeof(NameEnumMappingStrategy))]
    public enum ApplicationMessageAuthority
    {
        None = 0,
        Enterprise = 1,
        Corporate = 2,
        Custom = int.MaxValue,
    }
    #endregion public enum ApplicationMessageAuthority
    #region public enum ApplicationMessageAuthorityCode : long
    [EnumMapping(typeof(ApplicationMessageAuthority), typeof(NameEnumMappingStrategy))]
    public enum ApplicationMessageAuthorityCode : long
    {
        None = 0,
        //       V  vv  VV  vv  V
        Mask = 0x0000000F00000000,
        //     0x_______F________,
        //
        //           0x_______F________,
        Enterprise = 0x0000000100000000,
        //          0x_______F________,
        Corporate = 0x0000000200000000,
    }
    #endregion public enum ApplicationMessageAuthorityCode : long

    // 0000 0000 FF00 0000 Mask
    #region public enum ApplicationMessageDomainCode : long
    public enum ApplicationMessageDomainCode : long
    {
        None = 0,
        //       V  vv  VV  vv  V
        Mask = 0x00000000FF000000
        //     0x________FF______,
        //
    }
    #endregion public enum ApplicationMessageDomainCode : long
}
