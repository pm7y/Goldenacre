using System;
using System.Timers;

namespace Goldenacre.Core
{
    public sealed class DelayedActionTimer<T>
    {
        #region Fields

        private static readonly object _lock = new object();

        private Timer t = null;
        private bool _cancel = false;
        private T _state = default(T);
        private Action<T> _toDo = null;

        #endregion Fields

        #region Constructors

        public DelayedActionTimer()
        {
            t = new Timer();
            t.AutoReset = false;
            t.Elapsed += Elapsed;
        }

        #endregion Constructors

        #region Methods

        public void CancelAction()
        {
            lock (_lock)
            {
                _cancel = true;
                if (t != null)
                {
                    t.Stop();
                }
            }
        }

        public void StartDelayedAction(Action<T> toDo, T state, int delayMs)
        {
            lock (_lock)
            {
                _cancel = true;
                if (t != null)
                {
                    t.Stop();
                }

                _toDo = toDo;
                _state = state;
                _cancel = false;

                if (t != null)
                {
                    t.Interval = Math.Max(250, delayMs); ;
                    t.Start();
                }
            }
        }

        private void Elapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            try
            {
                lock (_lock)
                {
                    if (!_cancel)
                    {
                        _toDo.Invoke(_state);
                    }
                }
            }
            catch
            {
                //
            }

        }

        #endregion Methods
    }
}