﻿using System;
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
            _client.CheckForUpdate(new Version(0, 0, 0, 0));
            Assert.IsTrue(_client.IsUpdateAvailable,
                "Update SHOULD be available when running an old version");

            _client.CheckForUpdate(_client.LatestUpdate.Version);
            Assert.IsFalse(_client.IsUpdateAvailable,
                "Update should NOT be available when already running the latest version");

            _client.CheckForUpdate(new Version(99, 99, 99, 99));
            Assert.IsFalse(_client.IsUpdateAvailable,
                "Update should NOT be available when running a newer version than the latest available");
        }

        [Test]
        public void TestDownloadIntegrity()
        {
            _client.CheckForUpdate(new Version(0, 0, 0, 0));
            Console.WriteLine("Downloading v{0}", _client.LatestUpdate.Version);
            _client.DownloadUpdate();
            Console.WriteLine("Successfully downloaded update file");
        }

        // TODO: Test exception throwing
    }
}
