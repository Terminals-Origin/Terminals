using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data;
using Terminals.Data.DB;
using Tests.FilePersisted;

namespace Tests.SqlPersisted
{
    /// <summary>
    ///This is a test class for database implementation of Groups
    ///</summary>
    [TestClass]
    public class GroupsTest : TestsLab
    {
        private int addedCount;
        private int updatedCount;
        private int deletedCount;

        [TestInitialize]
        public void TestInitialize()
        {
            this.InitializeTestLab();
            this.PrimaryPersistence.Dispatcher.GroupsChanged += new GroupsChangedEventHandler(this.DispatcherGroupsChanged);
        }

        [TestCleanup]
        public void TestClose()
        {
            this.ClearTestLab();
        }

        private void DispatcherGroupsChanged(GroupsChangedArgs args)
        {
            if (args.Added.Count == 1)
                this.addedCount++;
            if (args.Updated.Count == 1)
                this.updatedCount++;
            if (args.Removed.Count == 1)
                this.deletedCount++;
        }

        [TestMethod]
        public void AddGroup_AddsToDatabaseAndRepoerts()
        {
            DbGroup childGroup = this.CreateTestGroupA();
            DbSet<DbGroup> checkedGroups = this.CheckDatabase.Groups;
            DbGroup checkedChild = checkedGroups.FirstOrDefault(group => group.Id == childGroup.Id);
            Assert.IsNotNull(checkedChild, "Group wasn't added to the database");
            Assert.AreEqual(1, this.addedCount, "Add event wasn't received"); 
        }

        [TestMethod]
        public void UpdateGroup_UpdatesAllProperties()
        {
            DbGroup childGroup = this.CreateTestGroupA();
            DbGroup parentGroup = this.CreateTestGroup("TestGroupB");
            childGroup.Parent = parentGroup; // don't use entities here, we are testing intern logic
            IGroup testParent = childGroup.Parent;
            const string NEWNAME = "UpdatedName";
            childGroup.Name = NEWNAME;
            this.PrimaryPersistence.Groups.Update(childGroup);

            Assert.AreEqual(testParent, parentGroup, "Parent group wasn't set properly");
            DbSet<DbGroup> checkedGroups = this.CheckDatabase.Groups;
            DbGroup checkedChild = checkedGroups.FirstOrDefault(group => group.Id == childGroup.Id);
            DbGroup checkedParent = checkedGroups.FirstOrDefault(group => group.Id == parentGroup.Id);
            Assert.IsNotNull(checkedChild, "Group wasn't added to the database");
            Assert.AreEqual(NEWNAME, checkedChild.Name, "Group name wasnt update properly");
            Assert.IsNotNull(checkedParent, "Group wasn't added to the database");
            Assert.AreEqual(1, checkedParent.ChildGroups.Count, "Group wasn't added as child");
            // only one, because Added event is send once for each group
            Assert.AreEqual(2, this.addedCount, "Add event wasn't received");
        }

        [TestMethod]
        public void AddFavorite_CachesAddedFavorite()
        {
            IGroup group = this.CreateTestGroupA();
            DbFavorite favorite = this.AddFavoriteToPrimaryPersistence();
            group.AddFavorite(favorite);
            List<IFavorite> favorites = group.Favorites;
            Assert.AreEqual(1, favorites.Count, "Group favorites count doesn't match.");
            Assert.AreEqual(1, this.updatedCount, "Group update event didn't reach properly.");
        }

        [TestMethod]
        public void DeleteGroupReportsGroupDeleted()
        {
            var testGroup = this.CreateTestGroupA();
            int storedBefore = this.CheckDatabase.Groups.Count();
            this.PrimaryPersistence.Groups.Delete(testGroup);
            int storedAfter = this.CheckDatabase.Groups.Count();

            this.AssertGroupDeleted(storedAfter, storedBefore);
        }


        [TestMethod]
        public void RemoveGroupUpdatesNestedGroups()
        {
            var groupsLab = new GroupsTestLab(this.PrimaryPersistence);
            groupsLab.DeleteParent();
            Assert.IsNull(groupsLab.Child.Parent, "Remove parent group didnt update all childs parent relationship.");
        }

        [TestMethod]
        public void RemoveGroupReportsNestedGroupUpdate()
        {
            var groupsLab = new GroupsTestLab(this.PrimaryPersistence);
            groupsLab.DeleteParent();
            Assert.AreEqual(1, groupsLab.UpdateReported, "Remove parent group didnt send child group update.");
        }

        [TestMethod]
        public void RebuildGroups_RemovesEmptyGroups()
        {
            this.CreateTestGroupA();
            int storedBefore = this.CheckDatabase.Groups.Count();
            this.PrimaryPersistence.Groups.Rebuild();
            int storedAfter = this.CheckDatabase.Groups.Count();

            this.AssertGroupDeleted(storedAfter, storedBefore);
        }

        private DbGroup CreateTestGroupA()
        {
            return this.CreateTestGroup("TestGroupA");
        }

        private DbGroup CreateTestGroup(string newGroupName)
        {
            IFactory factory = this.PrimaryPersistence.Factory;
            DbGroup testGroup = factory.CreateGroup(newGroupName) as DbGroup;
            this.PrimaryPersistence.Groups.Add(testGroup);
            return testGroup;
        }

        private void AssertGroupDeleted(int storedAfter, int storedBefore)
        {
            Assert.AreEqual(1, storedBefore, "group wasn't created");
            Assert.AreEqual(0, storedAfter, "group wasn't deleted");
            Assert.AreEqual(1, this.deletedCount, "Deleted event wasn't received");
        }
    }
}
