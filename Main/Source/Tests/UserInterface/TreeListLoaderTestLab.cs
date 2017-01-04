using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data;
using Terminals.Forms.Controls;
using Tests.Connections;

namespace Tests.UserInterface
{
    // consider creating abstract base test lab to be able test UI on persistence types
    // merge usages of Favorite.Persistence into the test lab
    // consider creating unit test to check, if both persistences produce the same events

    /// <summary>
    /// The persistence layer here doesnt play role.
    /// Created test structure is:
    /// + GroupD
    /// + GroupG
    ///   + GroupK
    ///    + FavoriteA
    ///   + GroupM
    ///   + GroupS
    /// + GroupR
    /// + FavoriteC
    /// </summary>
    public class TreeListLoaderTestLab : FilePersisted.FilePersistedTestLab
    {
        // not necessary this is only support instance for asserts
        private readonly TestTreeView treeView = new TestTreeView();

        private FavoriteTreeListLoader treeLoader;

        protected int AllNodesCount
        {
            get
            {
                return this.treeView.GetNodeCount(true);
            }
        }

        /// <summary>
        /// Expecting, that during test the GroupG node is still second
        /// </summary>
        protected TreeNodeCollection GroupGNodes
        {
            get { return this.RootNodes[1].Nodes; }
        }

        protected TreeNodeCollection RootNodes
        {
            get { return this.treeView.Nodes; }
        }

        internal IGroup GroupG { get; private set; }

        internal IGroup GroupR { get; private set; }

        internal IGroup GroupM { get; private set; }

        internal IGroup GroupK { get; private set; }

        internal IFavorite FavoriteA { get; private set; }

        internal IFavorite FavoriteC { get; private set; }

        protected const string GROUP_V_NAME = "GroupV";

        [TestInitialize]
        public void CreateTestTreeStructure()
        {
            var favoriteIcons = TestConnectionManager.CreateTestFavoriteIcons();
            this.treeView.AssignServices(this.Persistence, favoriteIcons, TestConnectionManager.Instance);
            this.CreateData();

            // loaded tree is needed for all tests, if if there is a "load all test", which tests next line only
            this.treeLoader = new FavoriteTreeListLoader(this.treeView, this.Persistence, favoriteIcons);
            this.treeLoader.LoadRootNodes();
            this.treeView.ExpandAllTreeNodes();
        }

        [TestCleanup]
        public void CleanupPersistenceAndTreeView()
        {
            this.treeLoader.UnregisterEvents();
            this.treeView.Dispose();
        }

        private void CreateData()
        {
            // proper names allow to check the ordering of created tree nodes
            this.Persistence.StartDelayedUpdate();
            this.AddNewGroup("GroupD");
            this.GroupG = this.AddNewGroup("GroupG");
            this.GroupK = this.AddGroupWithParent("GroupK", this.GroupG);
            this.FavoriteA = this.AddFavorite("FavoriteA");
            this.Persistence.Favorites.UpdateFavorite(this.FavoriteA, new List<IGroup>() { this.GroupK });
            this.GroupM = this.AddGroupWithParent("GroupM", this.GroupG);
            this.AddGroupWithParent("GroupS", this.GroupG);
            this.GroupR = this.AddNewGroup("GroupR");
            this.FavoriteC = this.AddFavorite("FavoriteC");
            this.Persistence.SaveAndFinishDelayedUpdate();
        }

        internal IGroup AddGroupWithParent(string groupName, IGroup parent)
        {
            // do the add and assign parent at once to reduce number of events
            IGroup created = this.Persistence.Factory.CreateGroup(groupName);
            created.Parent = parent;
            this.Persistence.Groups.Add(created);
            return created;
        }

        protected static void AssertTreeNode(string expectedName, TreeNode treeNode)
        {
            Assert.AreEqual(expectedName, treeNode.Text, "Tree node wasnt updated with corect name");
        }

        /// <summary>
        /// By default AllNodes = 8, RootNodes = 4.
        /// E.g. in case of added group: AllNodes = 10, one on first position, second as its dummy child.
        /// </summary>
        protected void AssertNodesCount(int expectedAllNodes, int expectedRootNodes)
        {
            Assert.AreEqual(expectedAllNodes, this.AllNodesCount, "Not all nodes were updated properly");
            Assert.AreEqual(expectedRootNodes, this.RootNodes.Count, "Nodes were not updated on the root level");
        }

        protected void AssertNotExpandedGroup()
        {
            var groupNodeV = (GroupTreeNode)this.RootNodes[3];
            Assert.IsTrue(groupNodeV.NotLoadedYet, "Not expanded group node cant be expanded after favorites update.");
        }
    }
}
