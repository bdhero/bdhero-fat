﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BDHero.BDROM;
using IsanPlugin;
using NUnit.Framework;

namespace IsanPluginTests
{
    [TestFixture]
    public class IsanPluginTest
    {
        private IsanMetadataProvider _provider;

        // TODO: Test parsing

        [SetUp]
        public void SetUp()
        {
            _provider = new IsanMetadataProvider();
        }

        [Test]
        public void TestEmptyTitle()
        {
            const string emptyTitleVIsan = "00000000E0AA000000000001";

            var vIsan = VIsan.TryParse(emptyTitleVIsan);

            _provider.Populate(vIsan);

            var parent = vIsan.Parent;

            Assert.IsNull(vIsan.Title);
            Assert.IsNull(vIsan.Year);
            Assert.IsNull(vIsan.LengthMin);
            Assert.IsNotNull(parent);
            Assert.AreEqual("Lord Of War", parent.Title);
            Assert.AreEqual(2004, parent.Year);
            Assert.AreEqual(100, parent.LengthMin);
        }
    }
}