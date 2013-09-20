using System.Collections.Generic;
using System.Windows.Forms;
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
            this.AssertAddedFavoriteNode(favorite, this.RootNodes[3]);
        }

        [TestMethod]
        public void AddFavoriteToSecondLevelTest()
        {
            // there is no ohter way to add favorite with group directly using one command
            // this also simulates add to root and then move to nested group
            IFavorite favorite = this.AddFavorite(BFAVORITE_NAME);
            this.Persistence.Favorites.UpdateFavorite(favorite, new List<IGroup>() { this.GroupG });
            this.AssertAddedFavoriteNode(favorite, this.GroupGNodes[3]);
        }

        // both favorite delete test are enough to test, if all nodes related
        // to the favorite will be deleted, not only one instance.
        [TestMethod]
        public void DeleteRootFavoriteTest()
        {
            this.Persistence.Favorites.Delete(this.FavoriteA);
            this.AssertNodesCount(8, 4);
        }

        [TestMethod]
        public void DeleteNestedFavoriteTest()
        {
            this.Persistence.Favorites.Delete(this.FavoriteA);
            this.AssertNodesCount(8);
            var nodeKChildsCount = this.GroupGNodes[1].Nodes.Count;
            Assert.AreEqual(0, nodeKChildsCount, "Favorite node wasnt remvoed from NodeK");
        }

        [TestMethod]
        public void MoveNestedFavoriteToRootTest()
        {
            // tests changing favorite membership only
            this.Persistence.Favorites.UpdateFavorite(this.FavoriteA, new List<IGroup>());
            this.AssertAddedFavoriteNode(this.FavoriteA, this.RootNodes[3]);
        }

        [TestMethod]
        public void RenameFavoriteTest()
        {
            const string NAME_D = "FavoriteD";
            this.FavoriteC.Name = NAME_D;
            this.Persistence.Favorites.Update(this.FavoriteC);
            AssertTreeNode(NAME_D, this.RootNodes[3]);
            this.AssertNodesCount(9, 4);
        }

        private void AssertAddedFavoriteNode(IFavorite favorite, TreeNode treeNode)
        {
            AssertTreeNode(favorite.Name, treeNode);
            // added favorite node results into 9 nodes only, because it is not expandable
            this.AssertNodesCount(9);
        }
    }
}