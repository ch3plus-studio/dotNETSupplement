using ch3plusStudio.dotNETSupplement.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace ch3plusStudio.dotNETSupplementUnitTest.IO
{
    [TestClass]
    public class FileChangesInfoSnapshotTest
    {
        [TestMethod]
        public void FileModified()
        {
            var fileName = String.Format("{0} - {1}.file", this.GetType().ToString(), "FileModified");

            var sOut = File.Create(fileName);

            var prevInfo = new FileChangesInfoSnapshot(fileName);

            sOut.WriteByte((byte)100);
            sOut.Close();

            Assert.AreNotEqual(prevInfo, new FileChangesInfoSnapshot(fileName));

            File.Delete(fileName);
        }

        [TestMethod]
        public void FileNotModified()
        {
            var fileName = String.Format("{0} - {1}.file", this.GetType().ToString(), "FileNotModified");

            var sOut = File.Create(fileName);
            sOut.Close();

            var prevInfo = new FileChangesInfoSnapshot(fileName);

            Assert.AreEqual(prevInfo, new FileChangesInfoSnapshot(fileName));

            File.Delete(fileName);
        }

        [TestMethod]
        public void FileExist()
        {
            var fileName = String.Format("{0} - {1}.file", this.GetType().ToString(), "FileExist");

            var sOut = File.Create(fileName);
            sOut.Close();

            Assert.AreEqual(true,(new FileChangesInfoSnapshot(fileName)).Exists);

            File.Delete(fileName);
        }

        [TestMethod]
        public void FileNotExist()
        {
            var fileName = String.Format("{0} - {1}.file", this.GetType().ToString(), "FileNotExist");

            Assert.AreEqual(false, (new FileChangesInfoSnapshot(fileName)).Exists);
        }
    }
}
