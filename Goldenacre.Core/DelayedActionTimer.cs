using System;
using System.Timers;

namespace Goldenacre.Core
{
    public sealed class DelayedActionTimer<T>
    {
        #region Constructors

        public DelayedActionTimer()
        {
            _t = new Timer {AutoReset = false};
            _t.Elapsed += Elapsed;
        }

        #endregion Constructors

        #region Fields

        // ReSharper disable once StaticMemberInGenericType
        private static readonly object Lock = new object();

        private readonly Timer _t;
        private bool _cancel;
        private T _state;
        private Action<T> _toDo;

        #endregion Fields

        #region Methods

        public void CancelAction()
        {
            lock (Lock)
            {
                _cancel = true;
                if (_t != null)
                {
                    _t.Stop();
                }
            }
        }

        public void StartDelayedAction(Action<T> toDo, T state, int delayMs)
        {
            lock (Lock)
            {
                _cancel = true;
                if (_t != null)
                {
                    _t.Stop();
                }

                _toDo = toDo;
                _state = state;
                _cancel = false;

                if (_t != null)
                {
                    _t.Interval = Math.Max(250, delayMs);
                    _t.Start();
                }
            }
        }

        private void Elapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            try
            {
                lock (Lock)
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