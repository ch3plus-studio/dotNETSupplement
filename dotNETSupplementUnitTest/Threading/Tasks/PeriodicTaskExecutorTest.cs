using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;

using ch3plusStudio.dotNETSupplement.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace ch3plusStudio.dotNETSupplementUnitTest.Threading.Tasks
{
    [TestClass]
    public class PeriodicTaskExecutorTest
    {
        [TestMethod]
        public void SimpleCountingTask()
        {
            var cnt = 0;
            var pte = new PeriodicTaskExecutor(() => ++cnt, TimeSpan.FromSeconds(1));

            pte.Start();

            Thread.Sleep(TimeSpan.FromSeconds(10));

            pte.Stop();

            Assert.AreEqual(10, cnt);
        }
    }
}
