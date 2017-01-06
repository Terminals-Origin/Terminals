using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data;

namespace Tests.UserInterface
{
    /// <summary>
    /// Test favorites specific loading only
    /// </summary>
    [TestClass]
    public class FavoritesTreeLoadingTest : TreeListLoaderTestLab
    {
        private const string BFAVORITE_NAME = "BFavorite";

        [TestMethod]
        public void AddFavoriteToRootTest()
        {
            // B prefix - tests insert before the already present favorite
            IFavorite favorite = this.AddFavorite(BFAVORITE_NAME);
            AssertTreeNode(favorite.Name, this.RootNodes[3]);
            // added favorite node results into 9 nodes only, because it is not expandable
            this.AssertNodesCount(9, 5);
        }

        [TestMethod]
        public void AddFavoriteToSecondLevelTest()
        {
            // there is no ohter way to add favorite with group directly using one command
            // this also simulates add to root and then move to nested group
            IFavorite favorite = this.AddFavorite(BFAVORITE_NAME);
            this.Persistence.Favorites.UpdateFavorite(favorite, new List<IGroup>() { this.GroupG });
            this.AssertNodesCount(9, 4);
            AssertTreeNode(favorite.Name, this.GroupGNodes[3]);
        }

        // both favorite delete test are enough to test, if all nodes related
        // to the favorite will be deleted, not only one instance.
        [TestMethod]
        public void DeleteRootFavoriteTest()
        {
            this.Persistence.Favorites.Delete(this.FavoriteC);
            this.AssertNodesCount(7, 3);
        }

        [TestMethod]
        public void DeleteNestedFavoriteTest()
        {
            this.Persistence.Favorites.Delete(this.FavoriteA);
            this.AssertNodesCount(7, 4);
            var nodeKChildsCount = this.GroupGNodes[1].Nodes.Count;
            Assert.AreEqual(0, nodeKChildsCount, "Favorite node wasnt remvoed from NodeK");
        }

        [TestMethod]
        public void MoveNestedFavoriteToRootTest()
        {
            // tests changing favorite membership only
            this.Persistence.Favorites.UpdateFavorite(this.FavoriteA, new List<IGroup>());
            AssertTreeNode(this.FavoriteA.Name, this.RootNodes[3]);
            this.AssertNodesCount(8, 5);
        }

        [TestMethod]
        public void RenameFavoriteTest()
        {
            const string NAME_D = "FavoriteD";
            this.FavoriteC.Name = NAME_D;
            this.Persistence.Favorites.Update(this.FavoriteC);
            AssertTreeNode(NAME_D, this.RootNodes[3]);
            this.AssertNodesCount(8, 4);
        }

        [TestMethod]
        public void DontAffectNotLoadedGroupTest()
        {
            // adds group tree node to the root as not expanded
            IGroup groupV = this.AddNewGroup(GROUP_V_NAME);
            // now we have 10 tree nodes
            this.Persistence.Favorites.UpdateFavorite(this.FavoriteA, new List<IGroup>() { groupV });
            // now we have only 9 nodes because the moved should appear inside the not loaded group node
            // thats why the node disapears, till the related group is expanded
            this.AssertNodesCount(9, 5);
            this.AssertNotExpandedGroup();
        }
    }
}