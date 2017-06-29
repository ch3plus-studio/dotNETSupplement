using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

using ch3plusStudio.dotNETSupplement.Core.Event;

namespace ch3plusStudio.dotNETSupplement.Net
{
    public class ServerConnectivityMonitor
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

        public ServerConnectivityMonitor(List<IPEndPoint> address)
        {
            _Address = address;
            _LastConnectivity = Connectivity.Undefined;
        }

        public void StartMonitor()
        {
            Task.Factory.StartNew(() =>
            {
                while (!Cancelled)
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

                    System.Threading.Thread.Sleep(1000);
                }

                Cancelled = false;
            });
        }

        public void StopMonitor()
        {
            if (!Cancelled)
            {
                Cancelled = true;
            }
        }
    }
}