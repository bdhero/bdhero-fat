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
        private Action<CancellationToken> _succeed;
        private Action<CancellationToken, Exception> _fail;
        private Action<CancellationToken> _finally;

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
        /// <returns>Reference to this <code>TaskBuilder</code></returns>
        public TaskBuilder BeforeStart(Action<CancellationToken> beforeStart)
        {
            _beforeStart = beforeStart;
            return this;
        }

        /// <summary>
        /// Runs in background (non-UI) thread, but may execute an action on the UI thread by calling the passed thread invoker.
        /// </summary>
        /// <param name="work">Action to invoke on the background thread</param>
        /// <returns>Reference to this <code>TaskBuilder</code></returns>
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
        /// <returns>Reference to this <code>TaskBuilder</code></returns>
        public TaskBuilder Succeed(Action<CancellationToken> succeed)
        {
            _succeed = succeed;
            return this;
        }

        /// <summary>
        /// Runs the specified action in the UI thread after the main <see cref="DoWork"/> action runs
        /// and fails (due to an exception, etc.).
        /// </summary>
        /// <param name="fail">Action to invoke on the UI thread when the main DoWork action fails</param>
        /// <returns>Reference to this <code>TaskBuilder</code></returns>
        public TaskBuilder Fail(Action<CancellationToken, Exception> fail)
        {
            _fail = fail;
            return this;
        }

        /// <summary>
        /// Runs the specified action in the UI thread after the main <see cref="DoWork"/> action runs
        /// and after the Success or Failure callbacks have ran, regardless of whether the main DoWork action failed or succeeded.
        /// </summary>
        /// <param name="finally"></param>
        /// <returns>Reference to this <code>TaskBuilder</code></returns>
        public TaskBuilder Finally(Action<CancellationToken> @finally)
        {
            _finally = @finally;
            return this;
        }

        public Task Build()
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

            var stageTask = new Task(delegate
                {
                    try
                    {
                        if (InvokeBeforeStart() && InvokeDoWork())
                        {
                            InvokeSucceed();
                        }
                    }
                    finally
                    {
                        InvokeFinally();
                    }
                });

            return stageTask;
        }

        private bool InvokeBeforeStart()
        {
            if (_beforeStart == null) return true;
            return Try(() => _invoker.InvokeOnUIThreadSync(_beforeStart));
        }

        private bool InvokeDoWork()
        {
            Debug.Assert(_work != null);
            return Try(() => _work(_invoker, _cancellationToken));
        }

        private void InvokeSucceed()
        {
            if (_succeed == null) return;
            Try(() => _invoker.InvokeOnUIThreadSync(_succeed));
        }

        private void InvokeFinally()
        {
            if (_finally == null) return;
            Try(() => _invoker.InvokeOnUIThreadSync(_finally));
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
                    _fail(_cancellationToken, exception);
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