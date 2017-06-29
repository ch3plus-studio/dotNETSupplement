using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ch3plusStudio.dotNETSupplement.Net;

namespace dotNETSupplementUnitTest
{
    namespace Net
    {
        [TestClass]
        public class ServerConnectivityMonitorTest
        {
            [TestMethod]
            public void Connectable()
            {
                var mon = new ServerConnectivityMonitor(Dns.GetHostAddresses("github.com").Select(x => new IPEndPoint(x, 80)).ToList());
                var waitHandle = new AutoResetEvent(false);
                var status = ServerConnectivityMonitor.Connectivity.Undefined;

                mon.OnConnectivityChanged += (sender, evntargs) => {
                    status = evntargs.EventData;
                    waitHandle.Set();
                };

                mon.StartMonitor();
                waitHandle.WaitOne();
                mon.StopMonitor();

                Assert.AreEqual(status, ServerConnectivityMonitor.Connectivity.Connectable);
            }

            [TestMethod]
            public void Unconnectable()
            {
                var mon = new ServerConnectivityMonitor(new List<IPEndPoint>{(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 65534))});
                var waitHandle = new AutoResetEvent(false);
                var status = ServerConnectivityMonitor.Connectivity.Undefined;

                mon.OnConnectivityChanged += (sender, evntargs) =>
                {
                    status = evntargs.EventData;
                    waitHandle.Set();
                };

                mon.StartMonitor();
                waitHandle.WaitOne();
                mon.StopMonitor();

                Assert.AreEqual(status, ServerConnectivityMonitor.Connectivity.Unconnectable);
            }

            [TestMethod]
            public void UnconnectableByTimeout()
            {
                var mon = new ServerConnectivityMonitor(Dns.GetHostAddresses("github.com").Select(x => new IPEndPoint(x, 65534)).ToList());
                var waitHandle = new AutoResetEvent(false);
                var status = ServerConnectivityMonitor.Connectivity.Undefined;

                mon.OnConnectivityChanged += (sender, evntargs) =>
                {
                    status = evntargs.EventData;
                    waitHandle.Set();
                };

                mon.StartMonitor();
                waitHandle.WaitOne();
                mon.StopMonitor();

                Assert.AreEqual(status, ServerConnectivityMonitor.Connectivity.Unconnectable);
            }
        }
    }
}
