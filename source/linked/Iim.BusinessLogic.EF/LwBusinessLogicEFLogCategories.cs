using Lw.Diagnostics;

namespace Lw.BusinessLogic.EF
{
    public static class LogCategories
    {
        [LogCategoryDefinition(Lw.LogCategories.CoreDescription)]
        public const string Core = Lw.LogCategories.Core;

        [LogCategoryDefinition(Lw.LogCategories.EclDescription)]
        public const string Ecl = Lw.LogCategories.Ecl;

        [LogCategoryDefinition(Lw.LogCategories.CelDescription)]
        public const string Cel = Lw.LogCategories.Cel;
    }
}
