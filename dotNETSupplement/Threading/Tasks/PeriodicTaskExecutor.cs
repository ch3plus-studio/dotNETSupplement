using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ch3plusStudio.dotNETSupplement.Threading.Tasks
{
    public class PeriodicTaskExecutor
    {
        CancellationTokenSource TokenSource;
        CancellationToken Token;

        internal Action _Action;
        internal TimeSpan _TimeSpan;

        internal PeriodicTaskExecutor() { }

        public PeriodicTaskExecutor(Action action, TimeSpan timeSpan)
        {
            _Action = action;
            _TimeSpan = timeSpan;
        }

        public void Start()
        {
            TokenSource = new CancellationTokenSource();
            Token = TokenSource.Token;

            var timer = new System.Threading.Timer(self =>
            {
                if (!Token.IsCancellationRequested)
                {
                    this._Action();
                }

                if (!Token.IsCancellationRequested)
                {
                    ((Timer)self).Change(_TimeSpan, new TimeSpan(-1));
                }
                else
                {
                    ((Timer)self).Dispose();
                }
            });
            timer.Change(0, -1);
        }

        public void Stop()
        {
            if (!TokenSource.IsCancellationRequested)
            {
                TokenSource.Cancel();
                TokenSource.Dispose();
            }
        }
    }
}
