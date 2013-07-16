using Lw.Diagnostics;

namespace Lw.BusinessLogic.EF
{
    public static class ExceptionPolicies
    {
        [ExceptionPolicy(
            "Translates any exceptions not specified as contract exceptions to the internal exception.")]
        public const string ExecptionContract = "ExecptionContract";
    }
}
