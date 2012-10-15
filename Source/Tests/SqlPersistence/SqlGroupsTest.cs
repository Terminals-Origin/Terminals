using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data;
using Favorite = Terminals.Data.DB.Favorite;
using Group = Terminals.Data.DB.Group;

namespace Tests
{
    /// <summary>
    ///This is a test class for database implementation of Groups
    ///</summary>
    [TestClass]
    public class SqlGroupsTest : SqlTestsLab
    {
        private int addedCount;
        private int updatedCount;
        private int deletedCount;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void TestInitialize()
        {
            this.InitializeTestLab();
            this.PrimaryPersistence.Dispatcher.GroupsChanged += new GroupsChangedEventHandler(this.Dispatcher_GroupsChanged);
        }

        [TestCleanup]
        public void TestClose()
        {
            this.ClearTestLab();
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
            ObjectSet<Group> checkedGroups = this.CheckDatabase.Groups;
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
            Favorite favorite = this.AddFavoriteToPrimaryPersistence();
            group.AddFavorite(favorite);
            List<IFavorite> favorites = group.Favorites;
            Assert.AreEqual(1, favorites.Count, "Group favorites count doesnt match.");
            Assert.AreEqual(1, this.updatedCount, "Group update event didnt reach properly.");
        }

        [TestMethod]
        public void DeleteGroupTest()
        {
            var testGroup = this.CreateTestGroupA();
            int storedBefore = this.CheckDatabase.Groups.Count();
            this.PrimaryPersistence.Groups.Delete(testGroup);
            int storedAfter = this.CheckDatabase.Groups.Count();

            this.AssertGroupDeleted(storedAfter, storedBefore);
        }

        [TestMethod]
        public void RebuildGroupsTest()
        {
            this.CreateTestGroupA();
            int storedBefore = this.CheckDatabase.Groups.Count();
            this.PrimaryPersistence.Groups.Rebuild();
            int storedAfter = this.CheckDatabase.Groups.Count();

            this.AssertGroupDeleted(storedAfter, storedBefore);
        }

        private Group CreateTestGroupA()
        {
            return this.CreateTestGroup("TestGroupA");
        }

        private Group CreateTestGroup(string newGroupName)
        {
            IFactory factory = this.PrimaryPersistence.Factory;
            Group testGroup = factory.CreateGroup(newGroupName) as Group;
            this.PrimaryPersistence.Groups.Add(testGroup);
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
