using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data;

namespace Tests.FilePersisted
{
    [TestClass]
    public class GroupsTest : FilePersistedTestLab
    {
        private const string GROUP_NAME = "TestGroup";

        private const string GROUP_NAME2 = GROUP_NAME + "2";

        private int addedReported;

        private int removedReported;

        private int updateReported;

        private IGroups PersistedGroups { get { return this.Persistence.Groups; } }

        [TestMethod]
        public void AddGroup()
        {
            IGroup newGroup = this.Persistence.Factory.CreateGroup(GROUP_NAME);
            this.Persistence.Dispatcher.GroupsChanged += new GroupsChangedEventHandler(Dispatcher_GroupsChanged);
            this.PersistedGroups.Add(newGroup);
            Assert.AreEqual(1, this.addedReported, "Added group wasnt reported");
            Assert.AreEqual(1, PersistedGroups.Count(), "Group wasnt created properly");
        }

        [TestMethod]
        public void RemoveFavoriteShouldNotRemoveGroups()
        {
            Tuple<IFavorite, IGroup> tuple = this.ObserveAddGroupWithFavorite();
            this.Persistence.Favorites.Delete(tuple.Item1);

            // now persistence cant be empty, because we keep empty groups
            int groupsCount = this.PersistedGroups.Count();
            Assert.AreEqual(1, groupsCount, "Groups are empty");
            Assert.AreEqual(0, this.removedReported, "Removed group was reported");
        }

        [TestMethod]
        public void RemoveGroupShouldReportTheRemove()
        {
            Tuple<IFavorite, IGroup> tuple = this.ObserveAddGroupWithFavorite();
            this.PersistedGroups.Delete(tuple.Item2);

            int groupsCount = this.PersistedGroups.Count();
            Assert.AreEqual(0, groupsCount, "Group wasnt removed properly.");
            Assert.AreEqual(1, this.removedReported, "Remove group wasnt reported.");
        }

        [TestMethod]
        public void RemoveGroupShouldNotRemoveFavorites()
        {
            Tuple<IFavorite, IGroup> tuple = this.ObserveAddGroupWithFavorite();
            this.PersistedGroups.Delete(tuple.Item2);

            int favoritesCount = this.Persistence.Favorites.Count();
            Assert.AreEqual(1, favoritesCount, "Favorite cant be removed, even its group was removed.");
        }

        private Tuple<IFavorite, IGroup> ObserveAddGroupWithFavorite()
        {
            Tuple<IFavorite, IGroup> tuple = this.AddFavoriteWithGroup(GROUP_NAME);
            // now persistence contains one favorite and one group
            this.Persistence.Dispatcher.GroupsChanged += new GroupsChangedEventHandler(this.Dispatcher_GroupsChanged);
            return tuple;
        }

        [TestMethod]
        public void RemoveGroupUpdatesNestedGroups()
        {
            var groupsLab = new GroupsTestLab(this.Persistence);
            groupsLab.DeleteParent();
            Assert.IsNull(groupsLab.Child.Parent, "Remove parent group didnt update all childs parent relationship.");
        }

        [TestMethod]
        public void RemoveGroupReportsNestedGroupUpdate()
        {
            var groupsLab = new GroupsTestLab(this.Persistence);
            groupsLab.DeleteParent();
            Assert.AreEqual(1, groupsLab.UpdateReported, "Remove parent group didnt send child group update.");
        }

        [TestMethod]
        public void RebuildGroupsTest()
        {
            Tuple<IFavorite, IGroup> tuple = this.AddFavoriteWithGroup(GROUP_NAME);
            IGroup newGroup = this.Persistence.Factory.CreateGroup(GROUP_NAME2);
            this.PersistedGroups.Add(newGroup);
            this.Persistence.Dispatcher.GroupsChanged += new GroupsChangedEventHandler(Dispatcher_GroupsChanged);
            this.PersistedGroups.Rebuild();
            Assert.AreEqual(1, this.removedReported, "Removed group wasnt reported");
            IGroup survived = this.PersistedGroups.First();
            bool expectedSurvival = survived.StoreIdEquals(tuple.Item2);
            Assert.IsTrue(expectedSurvival, "Expected group doesnt exist after groups rebuild");
        }

        private void Dispatcher_GroupsChanged(GroupsChangedArgs args)
        {
            if (args.Added.Count == 1)
                this.addedReported++;
            if (args.Removed.Count == 1)
                this.removedReported++;
            if (args.Updated.Count == 1)
                this.updateReported++;
        }

        [TestMethod]
        public void UpdateGroupTest()
        {
            IGroup group = this.AddNewGroup(GROUP_NAME);
            IGroup parent = this.AddNewGroup("Parent");
            this.Persistence.Dispatcher.GroupsChanged += new GroupsChangedEventHandler(Dispatcher_GroupsChanged);

            this.UpdateGroupInPersistence(group, parent);
            IGroup resolved = GetFromSecondaryPersistence();

            Assert.IsNotNull(resolved, "Rename wasnt successfull");
            Assert.IsTrue(resolved.Parent.StoreIdEquals(parent), "Parent wasnt applied properly");
            Assert.AreEqual(1, this.updateReported, "Updated group wasnt reported");
        }

        private void UpdateGroupInPersistence(IGroup group, IGroup parent)
        {
            group.Name = GROUP_NAME2;
            group.Parent = parent;
            this.Persistence.Groups.Update(group);
        }

        private static IGroup GetFromSecondaryPersistence()
        {
            var secondaryPersistence = CreateFilePersistence();
            return secondaryPersistence.Groups[GROUP_NAME2];
        }
    }
}
