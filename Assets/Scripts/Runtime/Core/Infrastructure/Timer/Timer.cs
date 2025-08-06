using System;
using UnityEngine;

namespace Runtime.Core.Infrastructure.Timer
{
    public abstract class Timer : IDisposable
    {
        private readonly Action _onTimerStart = delegate { };

        private float _initialTime;
        
        public float CurrentTime { get; set; }
        protected bool IsRunning { get; private set; }
        
        public float Progress => Mathf.Clamp(CurrentTime / _initialTime * 100, 0, 100);

        public Action OnTimerStop = delegate { };

        protected Timer(float value)
        {
            _initialTime = value;
        }

        public void Start()
        {
            CurrentTime = _initialTime;
            if (!IsRunning)
            {
                IsRunning = true;
                TimerManager.RegisterTimer(this);
                _onTimerStart.Invoke();
            }
        }

        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                TimerManager.DeregisterTimer(this);
                OnTimerStop.Invoke();
            }
        }

        public abstract void Tick();
        public abstract bool IsFinished { get; }

        public void Resume()
        {
            IsRunning = true;
        }

        public void Pause()
        {
            IsRunning = false;
        }

        public virtual void Reset()
        {
            CurrentTime = _initialTime;
        }

        public virtual void Reset(float newTime)
        {
            _initialTime = newTime;
            Reset();
        }

        private bool _disposed;

        ~Timer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
                TimerManager.DeregisterTimer(this);

            _disposed = true;
        }
    }
}