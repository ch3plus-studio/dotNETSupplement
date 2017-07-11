using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.IO;
using System.Threading;
using System.Diagnostics;
using ch3plusStudio.dotNETSupplement.IO;
using System.Threading.Tasks;

namespace ch3plusStudio.dotNETSupplementUnitTest.IO
{
    [TestClass]
    public class FileWatcherTest
    {
        [TestMethod]
        public void OneOfOneFileAppeared()
        {
            var fileNames = new List<string>();
            fileNames.Add(String.Format("{0} - {1}.file", this.GetType().ToString(), "OneOfOneFileAppeared"));

            var waitHandle = new AutoResetEvent(false);

            var fileWatcher = new FilesWatcher(fileNames, TimeSpan.FromSeconds(1));

            fileWatcher.OnFileAppeared += (sender, arg) => waitHandle.Set();
            fileWatcher.Start();

            foreach (var fileName in fileNames)
            {
                var sOut = File.Create(fileName);
                sOut.Close();
            }

            Assert.AreEqual(true, waitHandle.WaitOne(TimeSpan.FromSeconds(10)));

            fileWatcher.Stop();

            foreach (var fileName in fileNames)
            {
                File.Delete(fileName);
            }
        }

        [TestMethod]
        public void OneOfOneFileDisppeared()
        {
            var fileNames = new List<string>();
            fileNames.Add(String.Format("{0} - {1}.file", this.GetType().ToString(), "OneOfOneFileDisppeared"));

            var waitHandle = new AutoResetEvent(false);

            var fileWatcher = new FilesWatcher(fileNames, TimeSpan.FromSeconds(1));

            foreach (var fileName in fileNames)
            {
                var sOut = File.Create(fileName);
                sOut.Close();
            }

            fileWatcher.OnFileDisappeared += (sender, arg) => waitHandle.Set();
            fileWatcher.Start();

            // Known issue: need to wait for a while before delete it, or else can't detect changes
            Thread.Sleep(TimeSpan.FromMilliseconds(500));

            foreach (var fileName in fileNames)
            {
                File.Delete(fileName);
            }

            Assert.AreEqual(true, waitHandle.WaitOne(TimeSpan.FromSeconds(10)));

            fileWatcher.Stop();
        }

        [TestMethod]
        public void OneOfOneFileModified()
        {
            var fileNames = new List<string>();
            fileNames.Add(String.Format("{0} - {1}.file", this.GetType().ToString(), "OneOfOneFileModified"));

            var waitHandle = new AutoResetEvent(false);

            var fileWatcher = new FilesWatcher(fileNames, TimeSpan.FromSeconds(1));

            var fileSteams = fileNames.ToDictionary(fileName => fileName, fileName => File.Create(fileName));

            fileWatcher.OnFileModified += (sender, arg) => waitHandle.Set();
            fileWatcher.Start();

            // Known issue: need to wait for a while before modify it, or else can't detect changes
            Thread.Sleep(TimeSpan.FromMilliseconds(500));

            foreach (var entry in fileSteams)
            {
                entry.Value.WriteByte(10);
                entry.Value.Close();
            }

            Assert.AreEqual(true, waitHandle.WaitOne(TimeSpan.FromSeconds(10)));

            fileWatcher.Stop();

            foreach (var fileName in fileNames)
            {
                File.Delete(fileName);
            }
        }
    }
}