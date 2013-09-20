using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data;
using Terminals.Forms.Controls;

namespace Tests.UserInterface
{
    // todo consider creating abstract base test lab to be able test UI on persistence types

    /// <summary>
    /// The persistence layer here doesnt play role.
    /// Created test structure is:
    /// + GroupD
    /// + GroupG
    ///   + GroupK
    ///   + GroupM
    ///   + GroupS
    /// + GroupR
    /// + Favorite
    /// </summary>
    [TestClass]
    public class TreeListLoaderTest : FilePersisted.FilePersistedTestLab
    {
        // not necessary this is only support instance for asserts
        private readonly TestTreeView treeView = new TestTreeView();

        private FavoriteTreeListLoader treeLoader;

        private int AllNodesCount
        {
            get
            {
                return this.treeView.GetNodeCount(true);
            }
        }

        /// <summary>
        /// Expecting, that during test the GroupG node is still second
        /// </summary>
        private TreeNodeCollection GroupGNodes
        {
            get { return this.RootNodes[1].Nodes; }
        }

        private TreeNodeCollection RootNodes
        {
            get { return this.treeView.Nodes; }
        }

        private IGroup groupG;

        private IGroup groupR;

        private IGroup groupM;

        [TestInitialize]
        public void CreateTestTreeStructure()
        {
            this.CreateData();

            // loaded tree is needed for all tests, if if there is a "load all test", which tests next line only
            this.treeLoader = new FavoriteTreeListLoader(this.treeView, this.Persistence);
            this.treeLoader.LoadGroups();
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
            this.Persistence.StartDelayedUpdate();
            this.AddNewGroup("GroupD");
            this.groupG = this.AddNewGroup("GroupG");
            this.AddGroupWithParent("GroupK", this.groupG);
            this.groupM = this.AddGroupWithParent("GroupM", this.groupG);
             this.AddGroupWithParent("GroupS", this.groupG);
            this.groupR = this.AddNewGroup("GroupR");
            this.AddFavorite();
            this.Persistence.SaveAndFinishDelayedUpdate();
        }

        /// <summary>
        /// If this test is succesfull, than also prereguisite checks for all other test are fullfiled
        /// </summary>
        [TestMethod]
        public void LoadingAllGroupsTest()
        {
            // not 4, because all nodes should be expanded
            Assert.AreEqual(7, this.AllNodesCount, "Not all nodes were loaded properly");
            // dummy nodes may also count to 6, so check also the childs number
            int secondNodeChilds = this.GroupGNodes.Count;
            Assert.AreEqual(3, secondNodeChilds, "Child node of GroupG were not expanded properly");
        }

        [TestMethod]
        public void CreateLeadingGroupTest()
        {
            IGroup group = this.AddNewGroup("GroupA");
            this.AssertAddedNode(group, this.RootNodes[0]);
        }

        [TestMethod]
        public void CreateGroupBySortingTest()
        {
            IGroup group = this.AddNewGroup("GroupE");
            this.AssertAddedNode(group, this.RootNodes[1]);
        }

        [TestMethod]
        public void CreateLastGroupTest()
        {
            // adding group in midle of the tree just before first favorite
            IGroup group = this.AddNewGroup("GroupZ");
            this.AssertAddedNode(group, this.RootNodes[3]);
        }

        [TestMethod]
        public void CreateSecondLevelGroupTest()
        {
            IGroup group = this.AddGroupWithParent("GroupL", this.groupG);
            Assert.AreEqual(4, this.GroupGNodes.Count, "Group wasnt added to the correct child node");
            AssertTreeNode(group, this.GroupGNodes[1]);
            this.AssertNodesCount(9, 4); // noting should change on root level
        }

        [TestMethod]
        public void RenameGroupTest()
        {
            this.groupG.Name = "GroupT";
            this.Persistence.Groups.Update(this.groupG);
            // since now property GroupGNodes points to wrong node
            AssertTreeNode(this.groupG, this.RootNodes[2]);
            this.AssertNodesCount(7, 4);
        }

        [TestMethod]
        public void MoveGroupTest()
        {
            this.groupR.Parent = this.groupG;
            this.Persistence.Groups.Update(this.groupR);
            AssertTreeNode(this.groupR, this.GroupGNodes[2]);
            // 8 - because the moved node isnt expanded in the target parent
            this.AssertNodesCount(8, 3);
        }

        [TestMethod]
        public void MoveGroupToRootTest()
        {
            this.groupM.Parent = null;
            this.Persistence.Groups.Update(this.groupM);
            AssertTreeNode(this.groupM, this.RootNodes[2]);
            // 8 - because the moved node isnt expanded in the target parent 
            this.AssertNodesCount(8);
        }

        private IGroup AddGroupWithParent(string groupName, IGroup parent)
        {
            // do the add and assign parent at once to reduce number of events
            IGroup created = this.Persistence.Factory.CreateGroup(groupName);
            created.Parent = parent;
            this.Persistence.Groups.Add(created);
            return created;
        }

        private void AssertAddedNode(IGroup groupA, TreeNode addedTreeNode)
        {
            AssertTreeNode(groupA, addedTreeNode);
            this.AssertNodesCount();
        }

        private static void AssertTreeNode(IGroup groupA, TreeNode treeNode)
        {
            var groupNode = (GroupTreeNode)treeNode;
            Assert.AreEqual(groupA.Name, groupNode.Text, "Tree node wasnt updated with corect name");
        }

        private void AssertNodesCount(int expectedAllNodes = 9, int expectedRootNodes = 5)
        {
            // 9 nodes in case of added, one on first position, second as its dummy child
            Assert.AreEqual(expectedAllNodes, this.AllNodesCount, "Not all nodes were updated properly");
            Assert.AreEqual(expectedRootNodes, this.RootNodes.Count, "Nodes were not updated on the root level");
        }

        // todo add favorite changes tests
        // add favorite to the root
        // add favorite to the nested group
        // add favorite to new group
        // remove favorite from nested group
        // rename favorite
    }
}
