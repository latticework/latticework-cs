using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lw.ApplicationMessages
{
    // 0000 0000 FF00 0000 Mask
    // 0000 0000 0100 0000 Core
    // 0000 0000 0200 0000 Services
    // 0000 0000 0300 0000 ApplicationSecurity
    // 0000 0000 0400 0000 Presentation
    // 0000 0000 0500 0000 Initialization
    #region public enum EnterpriseMessageDomain
    [EnumMapping(typeof(EnterpriseMessageDomainCode), typeof(NameEnumMappingStrategy))]
    public enum EnterpriseMessageDomain
    {
        None = 0,
        Core = 1,
        Services = 2,
        ApplicationSecurity = 3,
        Presentation = 4,
        Initialization = 5,
    }
    #endregion public enum EnterpriseMessageDomain
    #region public enum EnterpriseMessageDomainCode : long
    [EnumMapping(typeof(EnterpriseMessageDomain), typeof(NameEnumMappingStrategy))]
    public enum EnterpriseMessageDomainCode : long
    {
        None = 0,
        //       V  vv  VV  vv  V
        Mask = 0x00000000FF000000,
        //     0x________FF______,
        //
        //     0x________FF______
        Core = 0x0000000001000000,
        //         0x________FF______
        Services = 0x0000000002000000,
        //                    0x________FF______
        ApplicationSecurity = 0x0000000003000000,
        //             0x________FF______
        Presentation = 0x0000000004000000,
        //               0x________FF______
        Initialization = 0x0000000005000000,
    }
    #endregion public enum EnterpriseMessageDomainCode : long


    // 0000 0000 00FF 0000 Mask
    // 0000 0000 0001 0000 Core
    // 0000 0000 0002 0000 DataAnnotations
    // 0000 0000 0003 0000 Web
    // 0000 0000 0004 0000 EntityFramework
    #region public enum CoreMessageLibrary
    [EnumMapping(typeof(CoreMessageLibraryCode), typeof(NameEnumMappingStrategy))]
    public enum CoreMessageLibrary
    {
        None = 0,
        Core = 1,
		DataAnnotations = 2,
		Web = 3,
		EntityFramework = 4,
    }
    #endregion public enum CoreMessageLibrary
    #region public enum CoreMessageLibraryCode : long
    [EnumMapping(typeof(CoreMessageLibrary), typeof(NameEnumMappingStrategy))]
    public enum CoreMessageLibraryCode : long
    {
        None = 0,
        //       V  vv  VV  vv  V
        Mask = 0x0000000000FF0000,
        //     0x__________FF____,
        //
        //     0x__________FF____,
        Core = 0x0000000000010000,
        //
        //                0x__________FF____,
        DataAnnotations = 0x0000000000020000,
        //
        //    0x__________FF____,
        Web = 0x0000000000030000,
        //
        //                0x__________FF____,
        EntityFramework = 0x0000000000040000,
    }
    #endregion public enum CoreMessageLibraryCode : long

    // 0000 0000 00FF 0000 Mask
    // 0000 0000 0001 0000 Core
    #region public enum ServicesMessageLibrary
    [EnumMapping(typeof(ServicesMessageLibraryCode), typeof(NameEnumMappingStrategy))]
    public enum ServicesMessageLibrary
    {
        None = 0,
        Core = 1,
    }
    #endregion public enum ServicesMessageLibrary
    #region public enum ServicesMessageLibraryCode : long
    [EnumMapping(typeof(ServicesMessageLibrary), typeof(NameEnumMappingStrategy))]
    public enum ServicesMessageLibraryCode : long
    {
        None = 0,
        //       V  vv  VV  vv  V
        Mask = 0x0000000000FF0000,
        //     0x__________FF____,
        //
        //     0x__________FF____,
        Core = 0x0000000000010000,
    }
    #endregion public enum ServicesMessageLibraryCode : long

    // 0000 0000 00FF 0000 Mask
    // 0000 0000 0001 0000 Core
    #region public enum ApplicationSecurityMessageLibrary
    [EnumMapping(typeof(ApplicationSecurityMessageLibraryCode), typeof(NameEnumMappingStrategy))]
    public enum ApplicationSecurityMessageLibrary
    {
        None = 0,
        Core = 1,
    }
    #endregion public enum ApplicationSecurityMessageLibrary
    #region public enum ApplicationSecurityMessageLibraryCode : long
    [EnumMapping(typeof(ApplicationSecurityMessageLibrary), typeof(NameEnumMappingStrategy))]
    public enum ApplicationSecurityMessageLibraryCode : long
    {
        None = 0,
        //       V  vv  VV  vv  V
        Mask = 0x0000000000FF0000,
        //     0x__________FF____,
        //
        //     0x__________FF____,
        Core = 0x0000000000010000,
    }
    #endregion public enum ApplicationSecurityMessageLibraryCode : long

    // 0000 0000 00FF 0000 Mask
    // 0000 0000 0001 0000 Core
    #region public enum PresentationMessageLibrary
    [EnumMapping(typeof(PresentationMessageLibraryCode), typeof(NameEnumMappingStrategy))]
    public enum PresentationMessageLibrary
    {
        None = 0,
        Core = 1,
    }
    #endregion public enum PresentationMessageLibrary
    #region public enum PresentationMessageLibraryCode : long
    [EnumMapping(typeof(PresentationMessageLibrary), typeof(NameEnumMappingStrategy))]
    public enum PresentationMessageLibraryCode : long
    {
        None = 0,
        //       V  vv  VV  vv  V
        Mask = 0x0000000000FF0000,
        //     0x__________FF____,
        //
        //     0x__________FF____,
        Core = 0x0000000000010000,
    }
    #endregion public enum PresentationMessageLibraryCode : long

    // 0000 0000 00FF 0000 Mask
    // 0000 0000 0001 0000 Core
    #region public enum InitializationMessageLibrary
    [EnumMapping(typeof(InitializationMessageLibraryCode), typeof(NameEnumMappingStrategy))]
    public enum InitializationMessageLibrary
    {
        None = 0,
        Core = 1,
    }
    #endregion public enum InitializationMessageLibrary
    #region public enum InitializationMessageLibraryCode : long
    [EnumMapping(typeof(InitializationMessageLibrary), typeof(NameEnumMappingStrategy))]
    public enum InitializationMessageLibraryCode : long
    {
        None = 0,
        //       V  vv  VV  vv  V
        Mask = 0x0000000000FF0000,
        //     0x__________FF____,
        //
        //     0x__________FF____,
        Core = 0x0000000000010000,
    }
    #endregion public enum InitializationMessageLibraryCode : long
}
