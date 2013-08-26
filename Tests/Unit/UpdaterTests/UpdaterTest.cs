using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using NUnit.Framework;
using Updater;

namespace UpdaterTests
{
    [TestFixture]
    public class UpdaterTest
    {
        private UpdaterClient _client;

        [SetUp]
        public void SetUpClient()
        {
            _client = new UpdaterClient();
            _client.BeforeRequest += ClientOnBeforeRequest;
        }

        [TearDown]
        public void TearDownClient()
        {
            if (_client != null)
                _client.BeforeRequest -= ClientOnBeforeRequest;
        }

        private void ClientOnBeforeRequest(HttpWebRequest request)
        {
            var timeout = (int) TimeSpan.FromSeconds(15).TotalMilliseconds;
            request.Timeout = timeout;
            request.ReadWriteTimeout = timeout;
        }

        [Test]
        public void TestIsUpdateAvailable()
        {
            var latest = _client.GetLatestVersionSync();

            Assert.IsTrue(_client.IsUpdateAvailableSync(new Version(0, 0, 0, 0)),
                "Update SHOULD be available when running an old version");
            Assert.IsFalse(_client.IsUpdateAvailableSync(latest.Version),
                "Update should NOT be available when already running the latest version");
            Assert.IsFalse(_client.IsUpdateAvailableSync(new Version(99, 99, 99, 99)),
                "Update should NOT be available when running a newer version than the latest available");
        }

        [Test]
        public void TestDownloadIntegrity()
        {
            var latest = _client.GetLatestVersionSync();
            Console.WriteLine("Downloading v{0}", latest.Version);
            var path = _client.DownloadUpdateSync();
            Console.WriteLine("Successfully downloaded update file to \"{0}\"", path);
        }
    }
}
