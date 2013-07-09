using System;

namespace Lw.Diagnostics
{
    public interface IExceptionManager
    {
        #region Public Methods
        /// <summary>
        ///     Handles the specified System.Exception object according to the rules configured
        ///     for policyName.
        /// </summary>
        /// <param name="exceptionToHandle">
        ///     An <see cref="Exception"/> object.
        /// </param>
        /// <param name="policyName">
        ///     The name of the policy to handle.
        /// </param>
        /// <returns>
        ///     A value indicating whether a rethrow is recommended.
        /// </returns>
        bool HandleException(Exception exceptionToHandle, string policyName);

        /// <summary>
        ///      Handles the specified System.Exception object according to the rules configured
        ///      for policyName.
        /// </summary>
        /// <param name="exceptionToHandle">
        ///       An <see cref="Exception"/> object.
        /// </param>
        /// <param name="policyName"></param>
        ///      The name of the policy to handle.
        /// <param name="exceptionToThrow">
        ///     The new Exception to throw, if any.
        /// </param>
        /// <returns>
        ///        A value indicating whether a rethrow is recommended.
        /// </returns>
        bool HandleException(Exception exceptionToHandle, string policyName, out Exception exceptionToThrow);
        #endregion Public Methods
    }
}
