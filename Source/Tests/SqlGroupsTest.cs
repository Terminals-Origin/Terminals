using System.Data.Objects;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data;
using Group = Terminals.Data.DB.Group;

namespace Tests
{
    /// <summary>
    ///This is a test class for database implementation of Groups
    ///</summary>
    [TestClass]
    public class SqlGroupsTest
    {
        private int addedCount;
        private int deletedCount;
        private SqlTestsLab lab;
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            this.lab = new SqlTestsLab();
            this.lab.InitializeTestLab();
            this.lab.Persistence.Dispatcher.GroupsChanged += new GroupsChangedEventHandler(this.Dispatcher_GroupsChanged);
        }

        [TestCleanup]
        public void TestClose()
        {
            this.lab.ClearTestLab();
        }

        private void Dispatcher_GroupsChanged(GroupsChangedArgs args)
        {
            addedCount += args.Added.Count;
            deletedCount = args.Removed.Count;
        }

        /// <summary>
        ///A test for Add
        ///</summary>
        [TestMethod]
        public void AddGroupTest()
        {
            IFactory factory = this.lab.Persistence.Factory;
            Group childGroup = factory.CreateGroup("TestGroupA") as Group;
            Group parentGroup = factory.CreateGroup("TestGroupB") as Group;
            this.lab.Persistence.Groups.Add(parentGroup);
            this.lab.Persistence.Groups.Add(childGroup);
            childGroup.Parent = parentGroup; // dont use entites here, we are testing intern logic

            ObjectSet<Group> checkedGroups = this.lab.CheckDatabase.Groups;
            Group checkedChild = checkedGroups.FirstOrDefault(group => group.Id == childGroup.Id);
            Group checkedParent = checkedGroups.FirstOrDefault(group => group.Id == parentGroup.Id);
            Assert.IsNotNull(checkedChild, "Group wasnt added to the database");
            Assert.IsNotNull(checkedParent, "Group wasnt added to the database");
            Assert.AreEqual(1, checkedParent.ChildGroups.Count, "Group wasnt added as child");
            // only one, becuase Added event is send once for each group
            Assert.AreEqual(2, this.addedCount, "Add event wasnt received"); 
        }

        /// <summary>
        ///A test for Delete
        ///</summary>
        [TestMethod]
        public void DeleteGroupTest()
        {
            var testGroup = this.CreateTestGroup();
            int storedBefore = this.lab.CheckDatabase.Groups.Count();
            this.lab.Persistence.Groups.Delete(testGroup);
            int storedAfter = this.lab.CheckDatabase.Groups.Count();

            this.AssertGroupDeleted(storedAfter, storedBefore);
        }

        /// <summary>
        ///A test for Rebuild
        ///</summary>
        [TestMethod]
        public void RebuildGroupsTest()
        {
            this.CreateTestGroup();
            int storedBefore = this.lab.CheckDatabase.Groups.Count();
            this.lab.Persistence.Groups.Rebuild();
            int storedAfter = this.lab.CheckDatabase.Groups.Count();

            this.AssertGroupDeleted(storedAfter, storedBefore);
        }

        private Group CreateTestGroup()
        {
            IFactory factory = this.lab.Persistence.Factory;
            Group testGroup = factory.CreateGroup("TestGroupA") as Group;
            this.lab.Persistence.Groups.Add(testGroup);
            return testGroup;
        }

        private void AssertGroupDeleted(int storedAfter, int storedBefore)
        {
            Assert.AreEqual(1, storedBefore, "group wasnt created");
            Assert.AreEqual(0, storedAfter, "group wasnt deleted");
            Assert.AreEqual(1, this.deletedCount, "Deleted event wasnt received");
        }
    }
}
