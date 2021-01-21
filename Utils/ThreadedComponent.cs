using System;
using System.Threading;

namespace LightCheat.Utils
{
    public abstract class ThreadedComponent :
        IDisposable
    {
        #region // Storage

        // Custom thread name
        protected virtual string ThreadName => nameof(ThreadedComponent);

        // Timeout for thread to finish
        protected virtual TimeSpan ThreadTimeout { get; set; } = new TimeSpan(0, 0, 0, 3);

        // Thread frame sleep
        protected virtual TimeSpan ThreadFrameSleep { get; set; } = new TimeSpan(0, 0, 0, 0, 1);

        // Thread for this component
        private Thread Thread { get; set; }

        #endregion

        #region // Contructor

        protected ThreadedComponent()
        {
            Thread = new Thread(ThreadStart)
            {
                Name = ThreadName, // Virtual member call in constructor
            };
        }

        public virtual void Dispose()
        {
            Thread.Interrupt();
            if (!Thread.Join(ThreadTimeout))
            {
                Thread.Abort();
            }
            Thread = default;
        }

        #endregion

        #region // Routines

        // Launch thread for execute frames
        public void Start()
        {
            Thread.Start();
        }

        // Thread method
        private void ThreadStart()
        {
            try
            {
                while (true)
                {
                    FrameAction();
                    Thread.Sleep(ThreadFrameSleep);
                }
            }
            catch (ThreadInterruptedException)
            {
                // Left blank intentionally
            }
        }

        // Frame to loop inside a thread
        protected abstract void FrameAction();

        #endregion
    }
}