using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals.Data;
using Terminals.Forms.Controls;

namespace Tests.UserInterface
{
    /// <summary>
    /// Test groups specific loading only
    /// </summary>
    [TestClass]
    public class GroupsTreeLoadingTest : TreeListLoaderTestLab
    {
        /// <summary>
        /// If this test is succesfull, than also prereguisite checks for all other test are fullfiled
        /// </summary>
        [TestCategory("NonSql")]
        [TestMethod]
        public void LoadingAllGroupsTest()
        {
            // not 4, because all nodes should be expanded
            Assert.AreEqual(8, this.AllNodesCount, "Not all nodes were loaded properly");
            // dummy nodes may also count to 6, so check also the childs number
            int secondNodeChilds = this.GroupGNodes.Count;
            Assert.AreEqual(3, secondNodeChilds, "Child node of GroupG were not expanded properly");
        }

        [TestCategory("NonSql")]
        [TestMethod]
        public void CreateLeadingGroupTest()
        {
            IGroup group = this.AddNewGroup("GroupA");
            this.AssertAddedGroupNode(group, this.RootNodes[0]);
        }

        [TestCategory("NonSql")]
        [TestMethod]
        public void CreateGroupBySortingTest()
        {
            IGroup group = this.AddNewGroup("GroupE");
            this.AssertAddedGroupNode(group, this.RootNodes[1]);
        }

        [TestCategory("NonSql")]
        [TestMethod]
        public void CreateLastGroupTest()
        {
            // adding group in midle of the tree just before first favorite
            IGroup group = this.AddNewGroup("GroupZ");
            this.AssertAddedGroupNode(group, this.RootNodes[3]);
        }

        [TestCategory("NonSql")]
        [TestMethod]
        public void CreateSecondLevelGroupTest()
        {
            IGroup group = this.AddGroupWithParent("GroupL", this.GroupG);
            Assert.AreEqual(4, this.GroupGNodes.Count, "Group wasnt added to the correct child node");
            AssertTreeNode(group.Name, this.GroupGNodes[1]);
            this.AssertNodesCount(10, 4); // noting should change on root level
        }

        [TestCategory("NonSql")]
        [TestMethod]
        public void RenameGroupTest()
        {
            this.GroupG.Name = "GroupT";
            this.Persistence.Groups.Update(this.GroupG);
            // since now property GroupGNodes points to wrong node
            AssertTreeNode(this.GroupG.Name, this.RootNodes[2]);
            this.AssertNodesCount(8, 4);
        }

        [TestCategory("NonSql")]
        [TestMethod]
        public void MoveGroupTest()
        {
            this.GroupR.Parent = this.GroupG;
            this.Persistence.Groups.Update(this.GroupR);
            AssertTreeNode(this.GroupR.Name, this.GroupGNodes[2]);
            // 9 - because the moved node isnt expanded in the target parent
            this.AssertNodesCount(9, 3);
        }

        [TestCategory("NonSql")]
        [TestMethod]
        public void MoveGroupToRootTest()
        {
            this.GroupM.Parent = null;
            this.Persistence.Groups.Update(this.GroupM);
            AssertTreeNode(this.GroupM.Name, this.RootNodes[2]);
            // 9 - because the moved node isnt expanded in the target parent 
            this.AssertNodesCount(9, 5);
        }

        [TestCategory("NonSql")]
        [TestMethod]
        public void DontAffectNotLoadedGroupTest()
        {
            IGroup groupV = this.AddNewGroup(GROUP_V_NAME);
            this.GroupM.Parent = groupV;
            this.Persistence.Groups.Update(groupV);
            this.AssertNodesCount(10, 5);
            this.AssertNotExpandedGroup();
        }

        [TestMethod]
        public void RemoveNestedGroupMovesFavoritesToRoot()
        {
            this.Persistence.Groups.Delete(this.GroupK);
            AssertTreeNode(this.FavoriteA.Name, this.RootNodes[3]);
            this.AssertNodesCount(7, 5);
        }

        [TestCategory("NonSql")]
        [TestMethod]
        public void LoadRecursiveLoadsCompleateSubtree()
        {
            var treeView = new TestTreeView();
            var treeLoader = new FavoriteTreeListLoader(treeView, this.Persistence);
            treeLoader.LoadRootNodes(); // now we have 7 including Dummy nodes 
            var rootNodes = new TreeListNodes(treeView.Nodes);

            treeLoader.LoadGroupNodesRecursive(rootNodes);
            int allLoadedCount = treeView.GetNodeCount(true);
            // or assert: treeView.Nodes[1].Nodes[0].Nodes[0].Name = "FavoriteA"
            // Dummy are replaced, and also Favorite nodes are included
            Assert.AreEqual(8, allLoadedCount, "Loading recursive subtree should load all nodes without expanding them");

            treeView.Dispose();
        }

        private void AssertAddedGroupNode(IGroup group, TreeNode addedTreeNode)
        {
            AssertTreeNode(group.Name, addedTreeNode);
            this.AssertNodesCount(10, 5);
        }
    }
}
