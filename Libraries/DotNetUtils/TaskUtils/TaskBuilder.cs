using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetUtils.TaskUtils
{
    public class TaskBuilder
    {
        private TaskScheduler _callbackThread;
        private CancellationToken _cancellationToken;
        private IThreadInvoker _invoker;

        private Action<CancellationToken> _beforeStart;
        private Action<IThreadInvoker, CancellationToken> _work;
        private Action _succeed;
        private Action<Exception> _fail;
        private Action _finally;

        public TaskBuilder OnThread(TaskScheduler callbackThread)
        {
            _callbackThread = callbackThread;
            return this;
        }

        public TaskBuilder OnCurrentThread()
        {
            // Get the calling thread's context
            _callbackThread = (SynchronizationContext.Current != null
                                  ? TaskScheduler.FromCurrentSynchronizationContext()
                                  : TaskScheduler.Default);
            return this;
        }

        public TaskBuilder CancelWith(CancellationToken cancellationToken)
        {
            _cancellationToken = cancellationToken;
            return this;
        }

        /// <summary>
        /// Runs the specified action in the UI thread before invoking the main <see cref="DoWork"/> action.
        /// </summary>
        /// <param name="beforeStart"></param>
        /// <returns>Reference to this <c>TaskBuilder</c></returns>
        public TaskBuilder BeforeStart(Action<CancellationToken> beforeStart)
        {
            _beforeStart = beforeStart;
            return this;
        }

        /// <summary>
        /// Runs in background (non-UI) thread, but may execute an action on the UI thread by calling the passed thread invoker.
        /// </summary>
        /// <param name="work">Action to invoke on the background thread</param>
        /// <returns>Reference to this <c>TaskBuilder</c></returns>
        public TaskBuilder DoWork(Action<IThreadInvoker, CancellationToken> work)
        {
            _work = work;
            return this;
        }

        /// <summary>
        /// Runs the specified action in the UI thread after the main <see cref="DoWork"/> action runs
        /// and completes successfully.
        /// </summary>
        /// <param name="succeed">Action to invoke on the UI thread when the main DoWork action completes successfully</param>
        /// <returns>Reference to this <c>TaskBuilder</c></returns>
        public TaskBuilder Succeed(Action succeed)
        {
            _succeed = succeed;
            return this;
        }

        /// <summary>
        /// Runs the specified action in the UI thread after the main <see cref="DoWork"/> action runs
        /// and fails (due to an exception, etc.).
        /// </summary>
        /// <param name="fail">Action to invoke on the UI thread when the main DoWork action fails</param>
        /// <returns>Reference to this <c>TaskBuilder</c></returns>
        public TaskBuilder Fail(Action<Exception> fail)
        {
            _fail = fail;
            return this;
        }

        /// <summary>
        /// Runs the specified action in the UI thread after the main <see cref="DoWork"/> action runs
        /// and after the Success or Failure callbacks have ran, regardless of whether the main DoWork action failed or succeeded.
        /// </summary>
        /// <param name="finally"></param>
        /// <returns>Reference to this <c>TaskBuilder</c></returns>
        public TaskBuilder Finally(Action @finally)
        {
            _finally = @finally;
            return this;
        }

        public Task<bool> Build()
        {
            var scheduler = _callbackThread;
            var token = _cancellationToken;

            if (scheduler == null)
            {
                throw new InvalidOperationException("Missing TaskScheduler");
            }

            if (token == null)
            {
                throw new InvalidOperationException("Missing CancellationToken");
            }

            _invoker = new ThreadInvoker(scheduler, token);

            var task = new Task<bool>(delegate
                {
                    try
                    {
                        if (InvokeBeforeStart() && InvokeDoWork())
                        {
                            InvokeSucceed();
                            return true;
                        }
                    }
                    finally
                    {
                        // TODO: Log uncaught exceptions?
                        // TODO: Where to handle uncaught exceptions?
                        InvokeFinally();
                    }
                    return false;
                }, token, TaskCreationOptions.None);

            return task;
        }

        private bool InvokeBeforeStart()
        {
            if (_beforeStart == null) return true;
            return Try(() => _invoker.InvokeOnUIThreadSync(_beforeStart));
        }

        private bool InvokeDoWork()
        {
            if (_work == null) return true;
            return Try(() => _work(_invoker, _cancellationToken));
        }

        private void InvokeSucceed()
        {
            if (_succeed == null) return;
            _invoker.InvokeOnUIThreadSync(token => _succeed());
        }

        private void InvokeFail(Exception exception)
        {
            if (_fail == null) return;
            _invoker.InvokeOnUIThreadSync(token => _fail(exception));
        }

        private void InvokeFinally()
        {
            if (_finally == null) return;
            _invoker.InvokeOnUIThreadSync(token => _finally());
        }

        private bool Try(Action action)
        {
            if (_fail != null)
            {
                try
                {
                    action();
                }
                catch (Exception exception)
                {
                    InvokeFail(exception);
                    return false;
                }
            }
            else
            {
                action();
            }
            return true;
        }
    }
}