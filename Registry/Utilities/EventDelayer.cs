using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace Registry.Utilities
{
    /// <summary>
    /// If multiple events are expected from one source, this class
    /// can be used to call
    /// </summary>
    class EventDelayer<T> : IDisposable
    {
        private Timer _timer;
        private Func<T> _f;
        private int _delayMillis;
        private Dispatcher _dispatcher;

        /// <summary>
        /// Construct new delayer with a function to call.
        /// </summary>
        /// <param name="f">The function to call after delay has been passed</param>
        /// <param name="delayMillis">Number of milliseconds to wait for the next event</param>
        public EventDelayer(Func<T> f, Dispatcher dispatcher, int delayMillis = 50)
        {
            _dispatcher = dispatcher;
            _timer = new Timer(new TimerCallback(TimerCallbackImpl));
            _f = f;
            _delayMillis = delayMillis;
        }

        /// <summary>
        /// Reset the timer.
        /// </summary>
        public bool DelayEvent()
        {
            _timer.Change(_delayMillis, Timeout.Infinite);
            return true;
        }

        private void TimerCallbackImpl(object state) {
            _dispatcher.BeginInvoke(new Action(() => _f()));
        }

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
        }
    }
}
