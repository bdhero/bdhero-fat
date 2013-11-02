using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DotNetUtils.Concurrency
{
    public class ConcurrentLinkedSet<T>
    {
        /// <summary>
        /// Gets or sets the maximum amount of time to wait for a read or write lock before aborting the operation.
        /// The default is 5 seconds.  To abort immediately if a read/write lock is not available,
        /// set to <c>0</c>.  To wait indefinitely for a lock, set to <c>-1</c> milliseconds (<see cref="Timeout.Infinite"/>).
        /// </summary>
        public TimeSpan MaxWait = TimeSpan.FromSeconds(5);

        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        private readonly List<T> _list = new List<T>();
        private readonly ISet<T> _set = new HashSet<T>();

        public void Add(T value)
        {
            if (!_lock.TryEnterWriteLock(MaxWait)) return;

            try
            {
                if (_set.Contains(value)) return;

                _list.Add(value);
                _set.Add(value);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public List<T> ToList()
        {
            if (!_lock.TryEnterReadLock(MaxWait)) return new List<T>();

            try
            {
                return _list.ToList();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }
}
