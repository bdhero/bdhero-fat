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
        public void TestIsUpdateAvailable()
        {
            var updater = new UpdaterClient();
            var latest = updater.GetLatestVersionSync();

            Assert.IsTrue(updater.IsUpdateAvailableSync(new Version(0, 0, 0, 0)),
                "Update SHOULD be available when running an old version");
            Assert.IsFalse(updater.IsUpdateAvailableSync(latest.Version),
                "Update should NOT be available when already running the latest version");
            Assert.IsFalse(updater.IsUpdateAvailableSync(new Version(99, 99, 99, 99)),
                "Update should NOT be available when running a newer version than the latest available");
        }

        [Test]
        public void TestDownloadIntegrity()
        {
            Assert.DoesNotThrow(delegate
                {
                    var updater = new UpdaterClient();
                    var latest = updater.GetLatestVersionSync();
                    Console.WriteLine("Downloading v{0}", latest.Version);
                    var path = updater.DownloadUpdateAsync().Result;
                    Console.WriteLine("Successfully downloaded update file to \"{0}\"", path);
                });
        }
    }
}
