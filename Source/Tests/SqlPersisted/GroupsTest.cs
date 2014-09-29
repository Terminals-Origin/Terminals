using System;
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
        public void AddGroup_AddsToDatabase()
        {
            DbGroup childGroup = this.AddGroupAToPrimaryPersistence();
            DbSet<DbGroup> checkedGroups = this.CheckDatabase.Groups;
            DbGroup checkedChild = checkedGroups.FirstOrDefault(group => group.Id == childGroup.Id);
            Assert.IsNotNull(checkedChild, "Group wasn't added to the database");
        }

        [TestMethod]
        public void AddGroup_ReportsAddedGroup()
        {
            this.AddGroupAToPrimaryPersistence();
            Assert.AreEqual(1, this.addedCount, "Add event wasn't received");
        }

        // "Not implemented yet."
        [TestMethod]
        public void LoadGroup_LoadsParentProperty()
        {
            Tuple<DbGroup, DbGroup> created = this.AssignParentToChildGroup();
            // we ask the Secondary persistence, not Check database, to ensure loading by the presistence.
            IGroup childGroup = this.FindInSecondaryPersistence(created.Item1.Id);
            Assert.IsNotNull(childGroup.Parent, "Parent property has to be loaded on startup.");
        }

        private IGroup FindInSecondaryPersistence(int databaseId)
        {
            return this.SecondaryPersistence.Groups
                       .FirstOrDefault(group => ((DbGroup)group).Id == databaseId);
        }

        // "Not implemented yet."
        [TestMethod]
        public void AddGroup_SavesParentToDatabase()
        {
            DbGroup parentGroup = this.AddNewGroupToPrimaryPersistence("TestGroupB");
            // dont save the group before parent is assigned.
            DbGroup childGroup = this.CreateNewTestGroup("TestGroupA");
            childGroup.Parent = parentGroup; // don't use entities here, we are testing intern logic
            this.PrimaryPersistence.Groups.Add(childGroup);

            DbGroup checkedChild = this.FindCheckedGroupById(childGroup.Id);
            Assert.IsNotNull(checkedChild.Parent, "Newly added group parent wasnt saved to the database.");
        }

        [TestMethod]
        public void UpdateGroup_UpdatesParentProperty()
        {
            Tuple<DbGroup, DbGroup> created = this.AssignParentToChildGroup();

            IGroup assignedParent = created.Item2.Parent;
            Assert.AreEqual(created.Item1, assignedParent, "Parent group wasn't set properly");
        }

        [TestMethod]
        public void UpdateGroup_SavesParentPropertyToDatabase()
        {
            Tuple<DbGroup, DbGroup> created = this.AssignParentToChildGroup();
            DbGroup checkedParent = this.FindCheckedGroupById(created.Item1.Id);
            DbGroup checkedChild = this.FindCheckedGroupById(created.Item2.Id);

            Assert.IsNotNull(checkedChild, "Child group wasn't added to the database");
            Assert.IsNotNull(checkedParent, "Parent group wasn't added to the database");
            Assert.AreEqual(1, checkedParent.ChildGroups.Count, "Group wasn't added as child");
        }

        [TestMethod]
        public void UpdateGroup_ReportsGroupUpdated()
        {
            this.AssignParentToChildGroup();
            // only one, because Added event is send once for each group
            Assert.AreEqual(1, this.updatedCount, "Update event wasn't received");
        }

        /// <summary>
        /// Returns the newly created groups in Primary persistence. Item1 = Parent, Item2 = Child.
        /// </summary>
        private Tuple<DbGroup, DbGroup> AssignParentToChildGroup()
        {
            DbGroup childGroup = this.AddGroupAToPrimaryPersistence();
            DbGroup parentGroup = this.AddNewGroupToPrimaryPersistence("TestGroupB");
            childGroup.Parent = parentGroup; // don't use entities here, we are testing intern logic
            this.PrimaryPersistence.Groups.Update(childGroup);
            return new Tuple<DbGroup, DbGroup>(parentGroup, childGroup);
        }

        [TestMethod]
        public void UpdateGroup_UpdatesName()
        {
            DbGroup childGroup = this.AddGroupAToPrimaryPersistence();
            const string NEWNAME = "UpdatedName";
            childGroup.Name = NEWNAME;
            this.PrimaryPersistence.Groups.Update(childGroup);

            DbGroup checkedChild = this.FindCheckedGroupById(childGroup.Id);
            Assert.IsNotNull(checkedChild, "Group wasn't added to the database");
            Assert.AreEqual(NEWNAME, checkedChild.Name, "Group name wasnt update properly");
        }

        private DbGroup FindCheckedGroupById(int groupId)
        {
            DbSet<DbGroup> checkedGroups = this.CheckDatabase.Groups;
            return checkedGroups.FirstOrDefault(group => group.Id == groupId);
        }

        [TestMethod]
        public void AddFavorite_CachesAddedFavorite()
        {
            IGroup group = this.AddGroupAToPrimaryPersistence();
            DbFavorite favorite = this.AddFavoriteToPrimaryPersistence();
            group.AddFavorite(favorite);
            List<IFavorite> favorites = group.Favorites;
            Assert.AreEqual(1, favorites.Count, "Group favorites count doesn't match.");
            Assert.AreEqual(1, this.updatedCount, "Group update event didn't reach properly.");
        }

        [TestMethod]
        public void DeleteGroupReportsGroupDeleted()
        {
            var testGroup = this.AddGroupAToPrimaryPersistence();
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
            this.AddGroupAToPrimaryPersistence();
            int storedBefore = this.CheckDatabase.Groups.Count();
            this.PrimaryPersistence.Groups.Rebuild();
            int storedAfter = this.CheckDatabase.Groups.Count();

            this.AssertGroupDeleted(storedAfter, storedBefore);
        }

        private DbGroup AddGroupAToPrimaryPersistence()
        {
            return this.AddNewGroupToPrimaryPersistence("TestGroupA");
        }

        private DbGroup AddNewGroupToPrimaryPersistence(string newGroupName)
        {
            DbGroup testGroup = this.CreateNewTestGroup(newGroupName);
            this.PrimaryPersistence.Groups.Add(testGroup);
            return testGroup;
        }

        private DbGroup CreateNewTestGroup(string newGroupName)
        {
            IFactory factory = this.PrimaryPersistence.Factory;
            return factory.CreateGroup(newGroupName) as DbGroup;
        }

        private void AssertGroupDeleted(int storedAfter, int storedBefore)
        {
            Assert.AreEqual(1, storedBefore, "group wasn't created");
            Assert.AreEqual(0, storedAfter, "group wasn't deleted");
            Assert.AreEqual(1, this.deletedCount, "Deleted event wasn't received");
        }
    }
}
