using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Timers;

namespace DotNetUtils.Concurrency
{
    public interface IProgressThread
    {
        void RegisterObserver<T>(EventHandler<T> handler);

        void EnqueueEvent<T>(T state);
    }

    public class ProgressThread : IProgressThread
    {
        private readonly ConcurrentMultiValueDictionary<Type, Delegate> _handlers =
                     new ConcurrentMultiValueDictionary<Type, Delegate>();

        private readonly ConcurrentMultiValueDictionary<Type, object> _events =
                     new ConcurrentMultiValueDictionary<Type, object>();

        private readonly Timer _timer = new Timer();

        public ProgressThread()
        {
            _timer.Elapsed += TimerOnElapsed;
        }

        public TimeSpan Interval
        {
            get { return TimeSpan.FromMilliseconds(_timer.Interval); }
            set { _timer.Interval = value.TotalMilliseconds; }
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            DequeueAllEvents();
        }

        public void RegisterObserver<T>(EventHandler<T> handler)
        {
            _handlers.Enqueue(typeof (T), handler);
        }

        public void EnqueueEvent<T>(T state)
        {
            _events.Enqueue(typeof (T), state);
        }

        private EventHandler<T>[] GetHandlers<T>()
        {
            return _handlers.GetValues(typeof (T)).OfType<EventHandler<T>>().ToArray();
        }

        private void NotifyObservers<T>(T state)
        {
            foreach (var handler in GetHandlers<T>())
            {
                handler(state);
            }
        }

        private void NotifyObservers(Type eventType, object state)
        {
            // TODO: Find a way to use statically compiled expression instead
            // of referring to a method via string.
            // http://stackoverflow.com/a/2107864/467582
            // For non-public methods, you'll need to specify binding flags too
            MethodInfo method = GetType().GetMethod("NotifyObservers").MakeGenericMethod(eventType);
            method.Invoke(this, new[] { state });
        }

        private void DequeueAllEvents()
        {
            foreach (var eventType in _events.GetKeys())
            {
                object state;
                while (_events.TryDequeue(eventType, out state))
                {
                    NotifyObservers(eventType, state);
                }
            }
        }
    }

    public delegate void EventHandler<in T>(T state);

    public delegate void ProgressThreadWorker(IProgressThread thread);
}
