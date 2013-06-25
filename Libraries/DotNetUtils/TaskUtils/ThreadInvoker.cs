using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetUtils.TaskUtils
{
    /// <summary>
    /// Concrete implementation of <code>IThreadInvoker</code>.
    /// </summary>
    public class ThreadInvoker : IThreadInvoker
    {
        private readonly TaskScheduler _callbackThread;

        private readonly CancellationToken _cancellationToken;

        /// <summary>
        /// Constructs a thread invoker that executes actions in the context of the specified task scheduler, passing them 
        /// </summary>
        /// <param name="callbackThread"></param>
        /// <param name="cancellationToken"></param>
        public ThreadInvoker(TaskScheduler callbackThread, CancellationToken cancellationToken)
        {
            _callbackThread = callbackThread;
            _cancellationToken = cancellationToken;
        }

        public void InvokeOnUIThreadSync(Action<CancellationToken> action)
        {
            StartTask(action, _cancellationToken).Wait();
        }

        public void InvokeOnUIThreadAsync(Action<CancellationToken> action)
        {
            StartTask(action, _cancellationToken);
        }

        private Task StartTask(Action<CancellationToken> action, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() => action(cancellationToken), _cancellationToken, TaskCreationOptions.None, _callbackThread);
        }
    }
}
