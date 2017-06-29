using System;

namespace ch3plusStudio.dotNETSupplement.Core.Event
{
    public class EventArgs<T> : EventArgs
    {
        public EventArgs(T val)
        {
            EventData = val;
        }

        public T EventData { get; private set; }
    }
}