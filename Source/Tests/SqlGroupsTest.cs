using System.Collections.Generic;
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
        private int updatedCount;
        private int deletedCount;
        private SqlTestsLab lab;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

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
            this.addedCount += args.Added.Count;
            this.updatedCount += args.Updated.Count;
            this.deletedCount = args.Removed.Count;
        }

        [TestMethod]
        public void AddGroupTest()
        {
            Group childGroup = this.CreateTestGroup("TestGroupA");
            Group parentGroup = this.CreateTestGroup("TestGroupB");
            childGroup.Parent = parentGroup; // dont use entites here, we are testing intern logic
            IGroup testParent = childGroup.Parent; // dummy test to resolve parrent

            Assert.AreEqual(testParent, parentGroup, "Parent group wasnt set properly");
            ObjectSet<Group> checkedGroups = this.lab.CheckDatabase.Groups;
            Group checkedChild = checkedGroups.FirstOrDefault(group => group.Id == childGroup.Id);
            Group checkedParent = checkedGroups.FirstOrDefault(group => group.Id == parentGroup.Id);
            Assert.IsNotNull(checkedChild, "Group wasnt added to the database");
            Assert.IsNotNull(checkedParent, "Group wasnt added to the database");
            Assert.AreEqual(1, checkedParent.ChildGroups.Count, "Group wasnt added as child");
            // only one, becuase Added event is send once for each group
            Assert.AreEqual(2, this.addedCount, "Add event wasnt received"); 
        }

        [TestMethod]
        public void LoadGroupFavoritesTest()
        {
            IGroup group = this.CreateTestGroupA();
            var favorite = this.lab.AddFavoriteToPrimaryPersistence();
            group.AddFavorite(favorite);
            List<IFavorite> favorites = group.Favorites;
            Assert.AreEqual(favorites.Count, 1, "Group favorites count doesnt match.");
            Assert.AreEqual(this.updatedCount, 1, "Group favorites count doesnt match.");
        }

        [TestMethod]
        public void DeleteGroupTest()
        {
            var testGroup = this.CreateTestGroupA();
            int storedBefore = this.lab.CheckDatabase.Groups.Count();
            this.lab.Persistence.Groups.Delete(testGroup);
            int storedAfter = this.lab.CheckDatabase.Groups.Count();

            this.AssertGroupDeleted(storedAfter, storedBefore);
        }

        [TestMethod]
        public void RebuildGroupsTest()
        {
            this.CreateTestGroupA();
            int storedBefore = this.lab.CheckDatabase.Groups.Count();
            this.lab.Persistence.Groups.Rebuild();
            int storedAfter = this.lab.CheckDatabase.Groups.Count();

            this.AssertGroupDeleted(storedAfter, storedBefore);
        }

        private Group CreateTestGroupA()
        {
            return this.CreateTestGroup("TestGroupA");
        }

        private Group CreateTestGroup(string newGroupName)
        {
            // todo it is possible to receive two times an event, that favorite was added to the cache 
            IFactory factory = this.lab.Persistence.Factory;
            Group testGroup = factory.CreateGroup(newGroupName) as Group;
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
