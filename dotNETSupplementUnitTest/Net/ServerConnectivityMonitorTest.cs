using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ch3plusStudio.dotNETSupplement.Net;
using System.Diagnostics;

namespace ch3plusStudio.dotNETSupplementUnitTest
{
    namespace Net
    {
        [TestClass]
        public class ServerConnectivityMonitorTest
        {
            [TestMethod]
            public void Connectable()
            {
                var mon = new ServerConnectivityMonitor(Dns.GetHostAddresses("github.com").Select(x => new IPEndPoint(x, 80)).ToList(), TimeSpan.FromMilliseconds(1000));
                var waitHandle = new AutoResetEvent(false);
                var status = ServerConnectivityMonitor.Connectivity.Undefined;

                mon.OnConnectivityChanged += (sender, evntargs) => {
                    status = evntargs.EventData;
                    waitHandle.Set();
                };

                mon.Start();
                waitHandle.WaitOne();
                mon.Stop();
                waitHandle.Dispose();

                Assert.AreEqual(status, ServerConnectivityMonitor.Connectivity.Connectable);
            }

            [TestMethod]
            public void ConnectableInLongRun()
            {
                var mon = new ServerConnectivityMonitor(Dns.GetHostAddresses("github.com").Select(x => new IPEndPoint(x, 80)).ToList(), TimeSpan.FromMilliseconds(1000));
                var status = ServerConnectivityMonitor.Connectivity.Undefined;

                mon.OnConnectivityChanged += (sender, evntargs) => status = evntargs.EventData;

                mon.Start();
                Thread.Sleep(TimeSpan.FromSeconds(20));
                mon.Stop();

                Assert.AreEqual(status, ServerConnectivityMonitor.Connectivity.Connectable); // Test result is good if no exception under long run, and get expected result
            }

            [TestMethod]
            public void Unconnectable()
            {
                var mon = new ServerConnectivityMonitor(new List<IPEndPoint> { (new IPEndPoint(IPAddress.Parse("127.0.0.1"), 65534)) }, TimeSpan.FromMilliseconds(1000));
                var waitHandle = new AutoResetEvent(false);
                var status = ServerConnectivityMonitor.Connectivity.Undefined;

                mon.OnConnectivityChanged += (sender, evntargs) =>
                {
                    status = evntargs.EventData;
                    waitHandle.Set();
                };

                mon.Start();
                waitHandle.WaitOne();
                mon.Stop();
                waitHandle.Dispose();

                Assert.AreEqual(status, ServerConnectivityMonitor.Connectivity.Unconnectable);
            }

            [TestMethod]
            public void UnconnectableByTimeout()
            {
                var mon = new ServerConnectivityMonitor(Dns.GetHostAddresses("github.com").Select(x => new IPEndPoint(x, 65534)).ToList(), TimeSpan.FromMilliseconds(1000));
                var waitHandle = new AutoResetEvent(false);
                var status = ServerConnectivityMonitor.Connectivity.Undefined;

                mon.OnConnectivityChanged += (sender, evntargs) =>
                {
                    status = evntargs.EventData;
                    waitHandle.Set();
                };

                mon.Start();
                waitHandle.WaitOne();
                mon.Stop();
                waitHandle.Dispose();

                Assert.AreEqual(status, ServerConnectivityMonitor.Connectivity.Unconnectable);
            }
        }
    }
}
