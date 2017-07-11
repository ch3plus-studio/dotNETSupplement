using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using ch3plusStudio.dotNETSupplement.Core.Event;
using ch3plusStudio.dotNETSupplement.Threading.Tasks;

namespace ch3plusStudio.dotNETSupplement.Net
{
    public class ServerConnectivityMonitor : PeriodicTaskExecutor
    {
        public enum Connectivity { Connectable, Unconnectable, Undefined }

        private volatile Connectivity _LastConnectivity;
        public Connectivity LastConnectivity
        {
            get { return _LastConnectivity; }
            private set
            {
                if (value == _LastConnectivity)
                {
                    return;
                }

                _LastConnectivity = value;

                if (OnConnectivityChanged != null)
                {
                    OnConnectivityChanged.Invoke(this, new EventArgs<Connectivity>(value));
                }
            }
        }

        private volatile bool Cancelled;
            
        private List<IPEndPoint> _Address;

        public event EventHandler<EventArgs<Connectivity>> OnConnectivityChanged;

        private ServerConnectivityMonitor(Action action, TimeSpan timeSpan) : base(action, timeSpan) { }

        public ServerConnectivityMonitor(List<IPEndPoint> address, TimeSpan timeSpan)
        {
            _Address = address;
            _LastConnectivity = Connectivity.Undefined;

            _Action = () =>
            {
                var s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                s.ReceiveTimeout = 5000;

                foreach (var ipEndPt in _Address)
                {
                    try
                    {
                        s.Connect(ipEndPt);
                        LastConnectivity = Connectivity.Connectable;
                        s.Disconnect(true);
                        break;
                    }
                    catch (Exception e)
                    {
                        LastConnectivity = Connectivity.Unconnectable;
                    }
                }
            };

            _TimeSpan = timeSpan;
        }

        [System.Obsolete("StartMonitor() is deprecated, please use Start() instead.")]
        public void StartMonitor()
        {
            base.Start();
        }

        [System.Obsolete("StopMonitor() is deprecated, please use Stop() instead.")]
        public void StopMonitor()
        {
            base.Stop();
        }
    }
}