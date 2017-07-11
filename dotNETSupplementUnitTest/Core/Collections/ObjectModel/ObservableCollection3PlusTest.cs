using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ch3plusStudio.dotNETSupplement.Core.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace ch3plusStudio.dotNETSupplementUnitTest.Core.Collections.ObjectModel
{
    [TestClass]
    public class ObservableCollection3PlusTest
    {
        [TestMethod]
        public void NormalBlockOps()
        {
            var Original = new ObservableCollection3Plus<int>(Enumerable.Range(1,5));
            var Expected = new ObservableCollection3Plus<int>() { 1, 2, 5, 6 };

            Original.BlockOps((list) => {
                list.Remove(3);
                list.Remove(4);
                list.Add(6);
            });

            CollectionAssert.AreEqual(Expected, Original);
        }

        [TestMethod]
        public void NormalBlockOps_Event()
        {
            var Original = new ObservableCollection3Plus<int>(Enumerable.Range(1, 5));
            var Expected = new ObservableCollection3Plus<int>() { 1, 2, 5, 6 };

            Original.CollectionChanged += ((sender, e) => CollectionAssert.AreEqual(Expected, Original));

            Original.BlockOps((list) =>
            {
                list.Remove(3);
                list.Remove(4);
                list.Add(6);
            });
        }
    }
}
