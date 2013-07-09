using Lw.Diagnostics;

namespace Lw
{
    public static class LogCategories
    {
        public const string AilDescription = "Applicaiton Initialization Library code.";
        public const string AsiDescription = "Applicaiton Security Infrastructure code.";
        public const string CelDescription = "Core Enterprise Library code.";
        public const string CoreDescription = ".Net Framework Core level code.";
        public const string EclDescription = "Enterprise Class Library code.";
        public const string VplDescription = "Enterprise Class Library code.";


        // [LogCategoryDefinition(Lw.LogCategories.AilDescription)]
        public const string Ail = "AIL";  // Lw.LogCategories.Ail;

        // [LogCategoryDefinition(Lw.LogCategories.AsiDescription)]
        public const string Asi = "ASI";  // Lw.LogCategories.Asi;

        [LogCategoryDefinition(Lw.LogCategories.CelDescription)]
        public const string Cel = "CEL"; // Lw.LogCategories.Cel;

        [LogCategoryDefinition(Lw.LogCategories.CoreDescription)]
        public const string Core = "Core";  // Lw.LogCategories.Core;

        [LogCategoryDefinition(Lw.LogCategories.EclDescription)]
        public const string Ecl = "ECL";  // Lw.LogCategories.Ecl;

        // [LogCategoryDefinition(Lw.LogCategories.VplDescription)]
        public const string Vpl = "VPL";  // Lw.LogCategories.Ecl;
    }
}
