using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data;

namespace Tests.FilePersisted
{
    [TestClass]
    public class GroupsTest : FilePersistedTestLab
    {
        private const string GOUP_NAME = "TestGroup";

        private int addedReported;

        private int removedReported;

        private IGroups PersistedGroups { get { return this.Persistence.Groups; } }

        [TestMethod]
        public void AddGroup()
        {
            IGroup newGroup = this.Persistence.Factory.CreateGroup(GOUP_NAME);
            this.Persistence.Dispatcher.GroupsChanged +=new GroupsChangedEventHandler(Dispatcher_GroupsChanged);
            this.PersistedGroups.Add(newGroup);
            Assert.AreEqual(1, this.addedReported, "Added group wasnt reported");
            Assert.AreEqual(1, PersistedGroups.Count(), "Group wasnt created properly");
        }

        [TestMethod]
        public void RemoveGroup()
        {
            Tuple<IFavorite, IGroup> tuple = this.AddFavoriteWithGroup(GOUP_NAME);
            // now persistence contains one favorite and one group
            this.Persistence.Dispatcher.GroupsChanged += new GroupsChangedEventHandler(Dispatcher_GroupsChanged);
            this.Persistence.Favorites.Delete(tuple.Item1);
            // now persistence should be empty, because we dont keep empty group - subject of required change
            int favoritesCount = this.Persistence.Favorites.Count();
            // redundant check, should be also tested in favorites test
            Assert.AreEqual(0, favoritesCount, "Favorite wasnt removed properly");
            int groupsCount = this.PersistedGroups.Count();
            Assert.AreEqual(1, groupsCount, "Groups arent empty");
            Assert.AreEqual(0, this.removedReported, "Removed group wasnt reported");
        }

        [TestMethod]
        public void RebuildGroupsTest()
        {
            Tuple<IFavorite, IGroup> tuple = this.AddFavoriteWithGroup(GOUP_NAME);
            IGroup newGroup = this.Persistence.Factory.CreateGroup(GOUP_NAME + "2");
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
        }
    }
}
