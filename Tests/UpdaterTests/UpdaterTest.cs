using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Updater;

namespace UpdaterTests
{
    [TestFixture]
    public class UpdaterTest
    {
        [Test]
        public void Test()
        {
            var updater = new UpdaterClient();
            var currentVersion = new Version(0, 8, 1, 4);
            if (updater.IsUpdateAvailable(currentVersion))
            {
                updater.DownloadUpdate();
            }
        }
    }
}
