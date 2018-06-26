using System;
using System.Threading;

namespace ServiceFabric.PubSubActors
{
    public class ScheduleThread : IDisposable
    {
        private readonly Action action;
        private readonly AutoResetEvent moreWork;
        private readonly object disposeLock = new object();
        private readonly Action<Exception> onException;

        public bool Running { get; private set; }
        public bool Active { get; private set; }
        public TimeSpan Timeout { get; protected set; }

        private ScheduleThread()
        {
            Running = true;
        }
        public ScheduleThread(Action action, TimeSpan timeout):this(action, timeout, null)
        {
        }
        public ScheduleThread(Action action, TimeSpan timeout, Action<Exception> onException)
            : this()
        {
            this.action = action;

            this.onException = onException;

            Timeout = timeout;

            moreWork = new AutoResetEvent(false);

            new Thread(RunThread) { IsBackground = true }.Start();
        }

        private void RunThread()
        {
            while (Running)
            {
                Active = true;

                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    onException?.Invoke(ex);
                }
                finally
                {
                    Active = false;

                    moreWork.WaitOne(Timeout);
                }
            }

            lock (disposeLock)
            {
                moreWork?.Dispose();
            }
        }

        public void RunNow()
        {
            moreWork?.Set();
        }

        public void ChangeInterval(TimeSpan newInterval)
        {
            if (Timeout != newInterval)
                Timeout = newInterval;
        }

        public void Dispose()
        {
            lock (disposeLock)
            {
                Running = false;

                Active = false;

                RunNow();
            }
        }
    }
}
