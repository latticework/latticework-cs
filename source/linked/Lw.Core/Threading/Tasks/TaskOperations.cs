using System.Threading.Tasks;

namespace Lw.Threading.Tasks
{
    // See http://social.msdn.microsoft.com/Forums/en-US/parallelextensions/thread/7d377c01-be8b-49ca-8fdc-d65564ca0ad5#ca4abc54-1b35-458c-9bee-e5978be0c407
    public static class TaskOperations
    {
        #region Public Fields
        public static readonly Task Completed = Task.FromResult(true);
        #endregion Public Fields
    }
}
