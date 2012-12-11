using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SqlScriptRunner.Versioning;
using Version = SqlScriptRunner.Versioning.Version;

namespace SqlScriptRunner.Tests
{
    [TestFixture]
    public class ReleaseTests
    {

        [Test]
        public void Version1_0_to_1_1()
        {
            Versioning.Release release = new Release();
            release.Description = "Initial public release";
            release.FromVersion = new Version(){ Major = 1, Minor = 0, Build = 0 };
            release.ToVersion = new Version() { Major = 1, Minor = 1, Build = 0 };
            SortedList<string, string> scripts = release.LoadScripts("VersionScripts", "*.sql", true, Mother.ResolveEnvironmentDirectory());

            Assert.IsNotNull(scripts);
            Assert.AreEqual(scripts.Count, 5);

        }
        [Test]
        public void Version1_0_to_Max()
        {
            Versioning.Release release = new Release();
            release.Description = "Initial public release";
            release.FromVersion = new Version() { Major = 1, Minor = 0, Build = 0 };
            release.ToVersion = Version.Max;
            SortedList<string, string> scripts = release.LoadScripts("VersionScripts", "*.sql", true, Mother.ResolveEnvironmentDirectory());

            Assert.IsNotNull(scripts);
            Assert.AreEqual(scripts.Count, 14);

        }
    }
}
