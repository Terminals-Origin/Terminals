using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Version = SqlScriptRunner.Versioning.Version;

namespace SqlScriptRunner.Tests
{

    public class VersionTests
    {
        [Test]
        public void EqualityTest()
        {
            Versioning.Version testVersionA = new Version() { Major = 1, Minor = 2, Build = 3, Description = "Good bye world" };
            Versioning.Version testVersionB = new Version() { Major = 1, Minor = 2, Build = 3, Description = "hello world" };
            Assert.IsTrue(testVersionA == testVersionB);
        }
        [Test]
        public void EqualityTest_WidthDate()
        {
            Versioning.Version testVersionA = new Version() { Major = 1, Minor = 2, Build = 3, Description = "Good bye world", DateTime = new DateTime(2012,12,1) };
            Versioning.Version testVersionB = new Version() { Major = 1, Minor = 2, Build = 3, Description = "hello world", DateTime = new DateTime(2012, 12, 1) };
            Assert.IsTrue(testVersionA == testVersionB);
        }
        [Test]
        public void InEqualityTest()
        {
            Versioning.Version testVersionA = new Version() { Major = 1, Minor = 2, Build = 4, Description = "Good bye world" };
            Versioning.Version testVersionB = new Version() { Major = 1, Minor = 2, Build = 3, Description = "hello world" };
            Assert.IsTrue(testVersionA != testVersionB);
        }
        [Test]
        public void InEqualityTest_WidthDate()
        {
            Versioning.Version testVersionA = new Version() { Major = 1, Minor = 2, Build = 4, Description = "Good bye world", DateTime = new DateTime(2012, 12, 1) };
            Versioning.Version testVersionB = new Version() { Major = 1, Minor = 2, Build = 3, Description = "hello world", DateTime = new DateTime(2012, 12, 1) };
            Assert.IsTrue(testVersionA != testVersionB);
        }
        [Test]
        public void GreaterMajorTest()
        {
            Versioning.Version testVersionA = new Version() { Major = 2, Minor = 2, Build = 3, Description = "Good bye world" };
            Versioning.Version testVersionB = new Version() { Major = 1, Minor = 2, Build = 3, Description = "hello world" };
            Assert.IsTrue(testVersionA > testVersionB);
        }
        [Test]
        public void LessThanMajorTest()
        {
            Versioning.Version testVersionA = new Version() { Major = 1, Minor = 2, Build = 3, Description = "Good bye world" };
            Versioning.Version testVersionB = new Version() { Major = 2, Minor = 2, Build = 3, Description = "hello world" };
            Assert.IsTrue(testVersionA < testVersionB);            
        }


        [Test]
        public void GreaterMinorTest()
        {
            Versioning.Version testVersionA = new Version() { Major = 1, Minor = 3, Build = 3, Description = "Good bye world" };
            Versioning.Version testVersionB = new Version() { Major = 1, Minor = 2, Build = 3, Description = "hello world" };
            Assert.IsTrue(testVersionA > testVersionB);
        }
        [Test]
        public void LessThanMinorTest()
        {
            Versioning.Version testVersionA = new Version() { Major = 1, Minor = 1, Build = 3, Description = "Good bye world" };
            Versioning.Version testVersionB = new Version() { Major = 1, Minor = 2, Build = 3, Description = "hello world" };
            Assert.IsTrue(testVersionA < testVersionB);
        }


        [Test]
        public void GreaterRevisionTest()
        {
            Versioning.Version testVersionA = new Version() { Major = 1, Minor = 2, Build = 4, Description = "Good bye world" };
            Versioning.Version testVersionB = new Version() { Major = 1, Minor = 2, Build = 3, Description = "hello world" };
            Assert.IsTrue(testVersionA > testVersionB);
        }
        [Test]
        public void LessThanRevisionTest()
        {
            Versioning.Version testVersionA = new Version() { Major = 1, Minor = 2, Build = 2, Description = "Good bye world" };
            Versioning.Version testVersionB = new Version() { Major = 1, Minor = 2, Build = 3, Description = "hello world" };
            Assert.IsTrue(testVersionA < testVersionB);
        }

        [Test]
        public void GreaterThanEqualTo_Greater()
        {
            Versioning.Version testVersionA = new Version() { Major = 2, Minor = 2, Build = 2, Description = "Good bye world" };
            Versioning.Version testVersionB = new Version() { Major = 1, Minor = 2, Build = 2, Description = "hello world" };
            Assert.IsTrue(testVersionA >= testVersionB);
        }


        [Test]
        public void GreaterThanEqualTo_Equal()
        {
            Versioning.Version testVersionA = new Version() { Major = 2, Minor = 2, Build = 2, Description = "Good bye world" };
            Versioning.Version testVersionB = new Version() { Major = 1, Minor = 2, Build = 2, Description = "hello world" };
            Assert.IsTrue(testVersionA >= testVersionB);
        }



        [Test]
        public void Equality_by_smaller_build()
        {
            Versioning.Version leftSide = new Version() { Major = 1, Minor = 3, Build = 0, Description = "Good bye world" };
            Versioning.Version rightSide = new Version() { Major = 1, Minor = 2, Build = 2, Description = "hello world" };
            Assert.IsTrue(leftSide > rightSide);
        }






        [Test]
        public void ReallyLogDescription()
        {
            Versioning.Version testVersion = new Version()
                                             {
                                                 Major = 1, 
                                                 Minor = 2,
                                                 Build = 3,
                                                 Description = "Really long description Really long description Really long description Really long description Really long description Really long description"
                                             };
            var input = string.Format("{0}.{1}.{2} {3}", testVersion.Major, testVersion.Minor, testVersion.Build, testVersion.Description);
            Assert.IsTrue(ParseAndValidate(input, testVersion, null));
            
        }
        [Test]
        public void SimpleVersion_NoDescription()
        {
            Versioning.Version testVersion = new Version()
            {
                Major = 1,
                Minor = 2,
                Build = 3,
                Description = null
            };
            var input = string.Format("{0}.{1}.{2} {3}", testVersion.Major, testVersion.Minor, testVersion.Build, testVersion.Description);
            Assert.IsTrue(ParseAndValidate(input, testVersion, null));
            
        }

        [Test]
        public void SimpleVersion_LargeDigits()
        {
            Versioning.Version testVersion = new Version()
            {
                Major = 1664,
                Minor = 534646542,
                Build = 32555654,
                Description = null
            };
            var input = string.Format("{0}.{1}.{2} {3}", testVersion.Major, testVersion.Minor, testVersion.Build, testVersion.Description);
            Assert.IsTrue(ParseAndValidate(input, testVersion, null));

        }



        [Test]
        public void SimpleVersion()
        {
            var input = "1.2.3 Description";
            Versioning.IParseVersions parser = new Versioning.JustVersionParser();
            var v = parser.Parse(input);
            Assert.IsNotNull(v);
            Assert.AreEqual(v.Major, 1);
            Assert.AreEqual(v.Minor, 2);
            Assert.AreEqual(v.Build, 3);
        }

        [Test]
        public void SimpleVersion_With_No_Revision()
        {
            var input = " 1.2 Description";  //in this case, the descritpion is "3 Description" , and the Build will be 0
            Versioning.IParseVersions parser = new Versioning.JustVersionParser();
            var v = parser.Parse(input);
            Assert.IsNotNull(v);
            Assert.AreEqual(v.Major, 1);
            Assert.AreEqual(v.Minor, 2);
        }

        [Test]
        public void SimpleVersion_With_No_Minor_Or_Revision()
        {
            var input = " 1 Description";  //in this case, the descritpion is "3 Description" , and the Build will be 0
            Versioning.IParseVersions parser = new Versioning.JustVersionParser();
            var v = parser.Parse(input);
            Assert.IsNotNull(v);
            Assert.AreEqual(v.Major, 1);
        }

        [Test]
        public void VersionDate_Simple()
        {
            var input = " 1.2.3.2012.12.16 Description"; 
            Versioning.IParseVersions parser = new Versioning.VersionDateParser();
            var v = parser.Parse(input);
            Assert.IsNotNull(v);
            Assert.AreEqual(v.Major, 1);
            Assert.AreEqual(v.Minor, 2);
            Assert.AreEqual(v.Build, 3);
            System.DateTime dt = new DateTime(2012, 12, 16);
            Assert.AreEqual(dt, v.DateTime);
        }
        [Test]
        public void VersionDateParser_Only_Major()
        {
            var input = " 1 Description";
            Versioning.IParseVersions parser = new Versioning.VersionDateParser();
            var v = parser.Parse(input);
            Assert.IsNotNull(v);
            Assert.AreEqual(v.Major, 1);
            Assert.AreEqual(v.Minor, 0);
            Assert.AreEqual(v.Build, 0);
        }
        [Test]
        public void VersionDateParser_Only_Major_Minor()
        {
            var input = " 1.2 Description";
            Versioning.IParseVersions parser = new Versioning.VersionDateParser();
            var v = parser.Parse(input);
            Assert.IsNotNull(v);
            Assert.AreEqual(v.Major, 1);
            Assert.AreEqual(v.Minor, 2);
            Assert.AreEqual(v.Build, 0);
        }
        [Test]
        public void VersionDateParser_Only_Major_Minor_Revision()
        {
            var input = " 1.2.3 Description";
            Versioning.IParseVersions parser = new Versioning.VersionDateParser();
            var v = parser.Parse(input);
            Assert.IsNotNull(v);
            Assert.AreEqual(v.Major, 1);
            Assert.AreEqual(v.Minor, 2);
            Assert.AreEqual(v.Build, 3);
        }

        public bool ParseAndValidate(string version, Versioning.Version testVersion, Versioning.IParseVersions parser = null)
        {
            if (parser == null) parser = new Versioning.JustVersionParser();
            Versioning.Version v = parser.Parse(version);
            return (v.Major == testVersion.Major && v.Minor == testVersion.Minor && v.Build == testVersion.Build &&
                    v.Description == testVersion.Description);
        }
    }
}