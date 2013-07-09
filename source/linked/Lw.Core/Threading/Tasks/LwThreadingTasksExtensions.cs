using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lw.Threading.Tasks
{
    public static class Extensions
    {
        #region Task Extensions
        //
        // Summary:
        //     Creates a continuation that executes asynchronously when the target System.Threading.Tasks.Task
        //     completes.
        //
        // Parameters:
        //   continuationAction:
        //     An action to run when the System.Threading.Tasks.Task completes. When run,
        //     the delegate will be passed the completed task as an argument.
        //
        //   scheduler:
        //     The System.Threading.Tasks.TaskScheduler to associate with the continuation
        //     task and to use for its execution.
        //
        // Returns:
        //     A new continuation System.Threading.Tasks.Task.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     The System.Threading.Tasks.Task has been disposed.
        //
        //   System.ArgumentNullException:
        //     The continuationAction argument is null.-or-The scheduler argument is null.
        public static Task ContinueOnUIWith(
            this Task task, Action<Task> continuationAction)
        {
            return task.ContinueWith(continuationAction, TaskScheduler.FromCurrentSynchronizationContext());
        }

        //
        // Summary:
        //     Creates a continuation that executes according to the specified System.Threading.Tasks.TaskContinuationOptions.
        //
        // Parameters:
        //   continuationAction:
        //     An action to run when the System.Threading.Tasks.Task completes. When run,
        //     the delegate will be passed the completed task as an argument.
        //
        //   cancellationToken:
        //     The System.Threading.Tasks.Task.CancellationToken that will be assigned to
        //     the new continuation task.
        //
        //   continuationOptions:
        //     Options for when the continuation is scheduled and how it behaves. This includes
        //     criteria, such as System.Threading.Tasks.TaskContinuationOptions.OnlyOnCanceled,
        //     as well as execution options, such as System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously.
        //
        //   scheduler:
        //     The System.Threading.Tasks.TaskScheduler to associate with the continuation
        //     task and to use for its execution.
        //
        // Returns:
        //     A new continuation System.Threading.Tasks.Task.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     The System.Threading.Tasks.Task has been disposed.-or-The System.Threading.CancellationTokenSource
        //     that created the token has already been disposed.
        //
        //   System.ArgumentNullException:
        //     The continuationAction argument is null.-or-The scheduler argument is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     The continuationOptions argument specifies an invalid value for System.Threading.Tasks.TaskContinuationOptions.
        public static Task ContinueOnUIWith(
            this Task task,
            Action<Task> continuationAction,
            CancellationToken cancellationToken,
            TaskContinuationOptions continuationOptions)
        {
            var ui = TaskScheduler.FromCurrentSynchronizationContext();

            return task.ContinueWith(continuationAction, cancellationToken, continuationOptions, ui);
        }


        //
        // Summary:
        //     Creates a continuation that executes asynchronously when the target System.Threading.Tasks.Task
        //     completes.
        //
        // Parameters:
        //   continuationFunction:
        //     A function to run when the System.Threading.Tasks.Task completes. When run,
        //     the delegate will be passed the completed task as an argument.
        //
        //   scheduler:
        //     The System.Threading.Tasks.TaskScheduler to associate with the continuation
        //     task and to use for its execution.
        //
        // Type parameters:
        //   TResult:
        //     The type of the result produced by the continuation.
        //
        // Returns:
        //     A new continuation System.Threading.Tasks.Task<TResult>.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     The System.Threading.Tasks.Task has been disposed.
        //
        //   System.ArgumentNullException:
        //     The continuationFunction argument is null.-or-The scheduler argument is null.
        public static Task<TResult> ContinueOnUIWith<TResult>(
            this Task task, Func<Task, TResult> continuationFunction)
        {
            return task.ContinueWith(continuationFunction, TaskScheduler.FromCurrentSynchronizationContext());
        }

        //
        // Summary:
        //     Creates a continuation that executes when the target System.Threading.Tasks.Task
        //     completes.
        //
        // Parameters:
        //   continuationFunction:
        //     A function to run when the System.Threading.Tasks.Task completes. When run,
        //     the delegate will be passed the completed task as an argument.
        //
        //   cancellationToken:
        //     The System.Threading.Tasks.Task.CancellationToken that will be assigned to
        //     the new continuation task.
        //
        //   continuationOptions:
        //     Options for when the continuation is scheduled and how it behaves. This includes
        //     criteria, such as System.Threading.Tasks.TaskContinuationOptions.OnlyOnCanceled,
        //     as well as execution options, such as System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously.
        //
        //   scheduler:
        //     The System.Threading.Tasks.TaskScheduler to associate with the continuation
        //     task and to use for its execution.
        //
        // Type parameters:
        //   TResult:
        //     The type of the result produced by the continuation.
        //
        // Returns:
        //     A new continuation System.Threading.Tasks.Task<TResult>.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     The System.Threading.Tasks.Task has been disposed.-or-The System.Threading.CancellationTokenSource
        //     that created the token has already been disposed.
        //
        //   System.ArgumentNullException:
        //     The continuationFunction argument is null.-or-The scheduler argument is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     The continuationOptions argument specifies an invalid value for System.Threading.Tasks.TaskContinuationOptions.
        public static Task<TResult> ContinueOnUIWith<TResult>(
            this Task task,
            Func<Task, TResult> continuationFunction,
            CancellationToken cancellationToken,
            TaskContinuationOptions continuationOptions)
        {
            var ui = TaskScheduler.FromCurrentSynchronizationContext();

            return task.ContinueWith(continuationFunction, cancellationToken, continuationOptions, ui);
        }
        #endregion Task Extensions

        #region Task<TResult> Extensions
        
        // Summary:
        //     Creates a continuation that executes asynchronously when the target System.Threading.Tasks.Task<TResult>
        //     completes.
        //
        // Parameters:
        //   continuationAction:
        //     An action to run when the System.Threading.Tasks.Task<TResult> completes.
        //     When run, the delegate will be passed the completed task as an argument.
        //
        // Returns:
        //     A new continuation System.Threading.Tasks.Task.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     The System.Threading.Tasks.Task<TResult> has been disposed.
        //
        //   System.ArgumentNullException:
        //     The continuationAction argument is null.
        public static Task ContinueOnUIWith<TResult>(this Task<TResult> task, Action<Task<TResult>> continuationAction)
        {
            return task.ContinueWith(continuationAction, TaskScheduler.FromCurrentSynchronizationContext());
        }

        //
        // Summary:
        //     Creates a continuation that executes according the condition specified in
        //     continuationOptions.
        //
        // Parameters:
        //   continuationAction:
        //     An action to run according the condition specified in continuationOptions.
        //     When run, the delegate will be passed the completed task as an argument.
        //
        //   cancellationToken:
        //     The System.Threading.CancellationToken that will be assigned to the new continuation
        //     task.
        //
        //   continuationOptions:
        //     Options for when the continuation is scheduled and how it behaves. This includes
        //     criteria, such as System.Threading.Tasks.TaskContinuationOptions.OnlyOnCanceled,
        //     as well as execution options, such as System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously.
        //
        //   scheduler:
        //     The System.Threading.Tasks.TaskScheduler to associate with the continuation
        //     task and to use for its execution.
        //
        // Returns:
        //     A new continuation System.Threading.Tasks.Task.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     The System.Threading.Tasks.Task<TResult> has been disposed.-or-The System.Threading.CancellationTokenSource
        //     that created cancellationToken has already been disposed.
        //
        //   System.ArgumentNullException:
        //     The continuationAction argument is null.-or-The scheduler argument is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     The continuationOptions argument specifies an invalid value for System.Threading.Tasks.TaskContinuationOptions.
        public static Task ContinueOnUIWith<TResult>(
            this Task<TResult> task,
            Action<Task<TResult>> continuationAction,
            CancellationToken cancellationToken,
            TaskContinuationOptions continuationOptions)
        {
            var ui = TaskScheduler.FromCurrentSynchronizationContext();
            return task.ContinueWith(continuationAction, cancellationToken, continuationOptions, ui);
        }


        //
        // Summary:
        //     Creates a continuation that executes according to the condition specified
        //     in continuationOptions.
        //
        // Parameters:
        //   continuationFunction:
        //     A function to run according to the specified continuationOptions. When run,
        //     the delegate will be passed the completed task as an argument.
        //
        //   cancellationToken:
        //     The System.Threading.Tasks.Task.CancellationToken that will be assigned to
        //     the new continuation task.
        //
        //   continuationOptions:
        //     Options for when the continuation is scheduled and how it behaves. This includes
        //     criteria, such as System.Threading.Tasks.TaskContinuationOptions.OnlyOnCanceled,
        //     as well as execution options, such as System.Threading.Tasks.TaskContinuationOptions.ExecuteSynchronously.
        //
        //   scheduler:
        //     The System.Threading.Tasks.TaskScheduler to associate with the continuation
        //     task and to use for its execution.
        //
        // Type parameters:
        //   TResult:
        //     The type of the result produced by the continuation.
        //
        // Returns:
        //     A new continuation System.Threading.Tasks.Task<TResult>.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     The System.Threading.Tasks.Task has been disposed.-or-The System.Threading.CancellationTokenSource
        //     that created the token has already been disposed.
        //
        //   System.ArgumentNullException:
        //     The continuationFunction argument is null.-or-The scheduler argument is null.
        //
        //   System.ArgumentOutOfRangeException:
        //     The continuationOptions argument specifies an invalid value for System.Threading.Tasks.TaskContinuationOptions.
        public static Task ContinueOnUIWith<TResult>(
            this Task<TResult> task,
            Func<Task, TResult> continuationFunction,
            CancellationToken cancellationToken,
            TaskContinuationOptions continuationOptions)
        {
            var ui = TaskScheduler.FromCurrentSynchronizationContext();

            return task.ContinueWith(
                continuationFunction, cancellationToken, continuationOptions, ui);
        }


        //
        // Summary:
        //     Creates a continuation that executes asynchronously when the target System.Threading.Tasks.Task<TResult>
        //     completes.
        //
        // Parameters:
        //   continuationFunction:
        //     A function to run when the System.Threading.Tasks.Task<TResult> completes.
        //     When run, the delegate will be passed the completed task as an argument.
        //
        //   scheduler:
        //     The System.Threading.Tasks.TaskScheduler to associate with the continuation
        //     task and to use for its execution.
        //
        // Type parameters:
        //   TNewResult:
        //     The type of the result produced by the continuation.
        //
        // Returns:
        //     A new continuation System.Threading.Tasks.Task<TResult>.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     The System.Threading.Tasks.Task<TResult> has been disposed.
        //
        //   System.ArgumentNullException:
        //     The continuationFunction argument is null.-or-The scheduler argument is null.
        public static Task<TNewResult> ContinueOnUIWith<TResult, TNewResult>(
            this Task<TResult> task, Func<Task<TResult>, TNewResult> continuationFunction)
        {
            return task.ContinueWith(continuationFunction, TaskScheduler.FromCurrentSynchronizationContext());
        }

        //
        // Summary:
        //     Creates a continuation that executes when the target System.Threading.Tasks.Task<TResult>
        //     completes.
        //
        // Parameters:
        //   continuationFunction:
        //     A function to run when the System.Threading.Tasks.Task<TResult> completes.
        //     When run, the delegate will be passed the completed task as an argument.
        //
        //   cancellationToken:
        //     The System.Threading.CancellationToken that will be assigned to the new task.
        //
        // Type parameters:
        //   TNewResult:
        //     The type of the result produced by the continuation.
        //
        // Returns:
        //     A new continuation System.Threading.Tasks.Task<TResult>.
        //
        // Exceptions:
        //   System.ObjectDisposedException:
        //     The System.Threading.Tasks.Task<TResult> has been disposed.-or-The provided
        //     System.Threading.CancellationToken has already been disposed.
        //
        //   System.ArgumentNullException:
        //     The continuationFunction argument is null.
        public static Task<TNewResult> ContinueOnUIWith<TResult, TNewResult>(
            this Task<TResult> task,
            Func<Task<TResult>, TNewResult> continuationFunction, 
            CancellationToken cancellationToken,
            TaskContinuationOptions continuationOptions)
        {
            var ui = TaskScheduler.FromCurrentSynchronizationContext();
            return task.ContinueWith(continuationFunction, cancellationToken, continuationOptions, ui);
        }

        

        // http://blogs.msdn.com/b/pfxteam/archive/2011/06/27/10179452.aspx
        public static Task<TResult> ToApm<TResult>(this Task<TResult> task, AsyncCallback callback, object state)
        {
            if (task.AsyncState == state)
            {
                if (callback != null)
                {
                    task.ContinueWith(delegate { callback(task); },
                        CancellationToken.None, TaskContinuationOptions.None, TaskScheduler.Default);
                }
                return task;
            }

            var tcs = new TaskCompletionSource<TResult>(state);
            task.ContinueWith(delegate
            {
                if (task.IsFaulted) tcs.TrySetException(task.Exception.InnerExceptions);
                else if (task.IsCanceled) tcs.TrySetCanceled();
                else tcs.TrySetResult(task.Result);

                if (callback != null) callback(tcs.Task);

            }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
            return tcs.Task;
        }
        #endregion Task<TResult> Extensions

        #region TaskFactory Extensions

        //
        // Summary:
        //     Creates a continuation System.Threading.Tasks.Task that will be started upon
        //     the completion of a set of provided Tasks.
        //
        // Parameters:
        //   tasks:
        //     The array of tasks from which to continue.
        //
        //   continuationAction:
        //     The action delegate to execute when all tasks in the tasks array have completed.
        //
        //   cancellationToken:
        //     The System.Threading.CancellationToken that will be assigned to the new continuation
        //     task.
        //
        //   continuationOptions:
        //     The System.Threading.Tasks.TaskContinuationOptions value that controls the
        //     behavior of the created continuation System.Threading.Tasks.Task.
        //
        //   scheduler:
        //     The System.Threading.Tasks.TaskScheduler that is used to schedule the created
        //     continuation System.Threading.Tasks.Task.
        //
        // Returns:
        //     The new continuation System.Threading.Tasks.Task.
        //
        // Exceptions:
        //   System.ArgumentNullException:
        //     The exception that is thrown when the tasks array is null.-or-The exception
        //     that is thrown when the continuationAction argument is null.-or-The exception
        //     that is thrown when the scheduler argument is null.
        //
        //   System.ArgumentException:
        //     The exception that is thrown when the tasks array contains a null value.-or-The
        //     exception that is thrown when the tasks array is empty.
        public static Task ContinueOnUIWhenAll(
            this TaskFactory taskFactory, Task[] tasks, Action<Task[]> continuationAction)
        {
            return taskFactory.ContinueWhenAll(
                tasks.ToArray(), 
                continuationAction, 
                CancellationToken.None, 
                TaskContinuationOptions.None, 
                TaskScheduler.FromCurrentSynchronizationContext());
        }


        public static Task StartNewOnUI(this TaskFactory taskFactory, Action action)
        {
            var ui = TaskScheduler.FromCurrentSynchronizationContext();

            return taskFactory.StartNew(action, CancellationToken.None, TaskCreationOptions.None, ui);
        }
        #endregion TaskFactory Extensions
    }
}
