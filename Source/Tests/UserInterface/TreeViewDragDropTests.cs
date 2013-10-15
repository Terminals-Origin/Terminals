using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Terminals;
using Terminals.Data;
using Terminals.Forms;
using Terminals.Forms.Controls;
using Tests.FilePersisted;

namespace Tests.UserInterface
{
    [TestClass]
    public class TreeViewDragDropTests : FilePersistedTestLab
    {
        private IFavorite sourceFavorite;
        private FavoriteTreeNode sourceFavoriteNode;
        private IGroup sourceGroup;
        private GroupTreeNode sourceGroupNode;

        private IFavorite targetFavorite;
        private IGroup targetGroup;

        [TestInitialize]
        public void TestSetup()
        {
            this.sourceFavorite = this.AddFavorite("SourceFavorite");
            this.sourceFavoriteNode = new FavoriteTreeNode(sourceFavorite);
            this.sourceGroup = this.AddNewGroup("SourceGroup");
            this.sourceGroupNode = new GroupTreeNode(sourceGroup);
            this.targetFavorite = this.AddFavorite("TargetFavorite");
            this.targetGroup = this.AddNewGroup("TargetGroup");
        }

        [TestMethod]
        public void DragGroupOnGroupTest()
        {
            TreeViewDragDrop dragdrop = this.CreateGroupDragDrop(this.targetGroup);
            Assert.AreEqual(DragDropEffects.Move, dragdrop.Effect);
        }

        [TestMethod]
        public void DragGroupOnNothingTest()
        {
            TreeViewDragDrop dragdrop = this.CreateGroupDragDrop(null);
            Assert.AreEqual(DragDropEffects.Move, dragdrop.Effect);
        }

        [TestMethod]
        public void DragGroupOnItSelfTest()
        {
            TreeViewDragDrop dragdrop = this.CreateGroupDragDrop(this.sourceGroup);
            Assert.AreEqual(DragDropEffects.None, dragdrop.Effect);
        }

        [TestMethod]
        public void DragGroupOnFavoriteTest()
        {
            DragEventArgs args = CreateDragArguments(this.sourceGroupNode);
            var keyModifiers = new TestKeyModifiers();
            var dragdrop = new TreeViewDragDrop(this.Persistence, args, keyModifiers, null, this.targetFavorite);
            Assert.AreEqual(DragDropEffects.None, dragdrop.Effect);
        }

        [TestMethod]
        public void DragFavoriteOnFavoriteTest()
        {
            // drag over it self covers drag over all favorite nodes
            TreeViewDragDrop dragdrop = this.CreateFavoriteDragDrop(this.targetFavorite);
            Assert.AreEqual(DragDropEffects.None, dragdrop.Effect);
        }

        [TestMethod]
        public void DragFavoriteOnNothingTest()
        {
            TreeViewDragDrop dragdrop = this.CreateFavoriteDragDrop(null);
            Assert.AreEqual(DragDropEffects.Move, dragdrop.Effect);
        }

        [TestMethod]
        public void DragFavoriteOnGroupToMoveTest()
        {
            TreeViewDragDrop dragdrop = this.CreateValidFavoriteDrag(false, false);
            Assert.AreEqual(DragDropEffects.Move, dragdrop.Effect);
        }

        [TestMethod]
        public void DragFavoriteOnGroupToCopyTest()
        {
            TreeViewDragDrop dragdrop = this.CreateValidFavoriteDrag(false, true);
            Assert.AreEqual(DragDropEffects.Copy, dragdrop.Effect);
        }

        [TestMethod]
        public void DragFavoriteOnGroupToAddToGroupTest()
        {
            TreeViewDragDrop dragdrop = this.CreateValidFavoriteDrag(true, false);
            Assert.AreEqual(DragDropEffects.Link, dragdrop.Effect);
        }

        [TestMethod]
        public void DropGroupOnGroupToMakeItNestedTest()
        {
            TreeViewDragDrop dragdrop = this.CreateGroupDragDrop(this.targetGroup);
            dragdrop.Drop(null);
            Assert.IsTrue(this.sourceGroup.Parent.StoreIdEquals(this.targetGroup));
        }

        [TestMethod]
        public void DropGroupToMoveToRootTest()
        {
            this.sourceGroup.Parent = this.targetGroup;
            this.Persistence.Groups.Update(this.sourceGroup);
            TreeViewDragDrop dragdrop = this.CreateGroupDragDrop(null);
            dragdrop.Drop(null);
            Assert.IsNull(this.sourceGroup.Parent);
        }

        [TestMethod]
        public void DropFavoriteOnGroupToMoveTest()
        {
            var dragdrop = this.CreateValidFavoriteDrop(DragDropEffects.Move);
            dragdrop.Drop(null);
            bool hasTargetGroup = this.sourceFavorite.Groups.Any(candidate => candidate.StoreIdEquals(this.targetGroup));
            Assert.IsTrue(hasTargetGroup);
        }

        [TestMethod]
        public void DropFavoriteToMoveToRootTest()
        {
            this.Persistence.Favorites.UpdateFavorite(this.sourceFavorite, new List<IGroup>() { this.sourceGroup });
            var args = CreateDragArguments(this.sourceFavoriteNode);
            args.Effect = DragDropEffects.Move;
            var dragdrop = new TreeViewDragDrop(this.Persistence, args, new TestKeyModifiers(), null, null);
            dragdrop.Drop(null);
            Assert.IsFalse(this.sourceFavorite.Groups.Any(), "Favorite cant be listed in any group");
        }

        [TestMethod]
        public void DropFavoriteOnGroupToCopyTest()
        {
            TreeViewDragDrop dragdrop = this.CreateValidFavoriteDrop(DragDropEffects.Copy);
            Func<InputBoxResult> copyFunction = () => new InputBoxResult() {Text = "Copy", ReturnCode = DialogResult.OK};
            dragdrop.CopyCommnad = new CopyFavoriteUI(this.Persistence, copyFunction);
            dragdrop.Drop(null);
            this.AssertTargetGroupAfterFavoriteDrop(true);
        }

        [TestMethod]
        public void DropFavoriteOnGroupToAddToGroupTest()
        {
            var dragdrop = this.CreateValidFavoriteDrop(DragDropEffects.Link);
            dragdrop.Drop(null);
            this.AssertTargetGroupAfterFavoriteDrop(false);
        }

        private void AssertTargetGroupAfterFavoriteDrop(bool expectedCopy)
        {
            List<IFavorite> targetFavorites = this.targetGroup.Favorites;
            Assert.AreEqual(1, targetFavorites.Count, "The target group should contain only the created copy");
            Assert.AreEqual(expectedCopy, !this.sourceFavorite.StoreIdEquals(targetFavorites[0]));
        }

        private TreeViewDragDrop CreateGroupDragDrop(IGroup targetGroup)
        {
            var args = CreateDragArguments(this.sourceGroupNode);
            var keyModifiers = new TestKeyModifiers();
            return new TreeViewDragDrop(this.Persistence, args, keyModifiers, targetGroup, null);
        }

        private TreeViewDragDrop CreateFavoriteDragDrop(IFavorite target)
        {
            var args = CreateDragArguments(this.sourceFavoriteNode);
            var keyModifiers = new TestKeyModifiers();
            return new TreeViewDragDrop(this.Persistence, args, keyModifiers, null, target);
        }

        private TreeViewDragDrop CreateValidFavoriteDrag(bool withShift, bool withControl)
        {
            var args = CreateDragArguments(this.sourceFavoriteNode);
            var keyModifiers = new TestKeyModifiers() { WithControl = withControl, WithShift = withShift };
            return new TreeViewDragDrop(this.Persistence, args, keyModifiers, this.targetGroup, null);
        }

        private TreeViewDragDrop CreateValidFavoriteDrop(DragDropEffects effect)
        {
            var args = CreateDragArguments(this.sourceFavoriteNode);
            args.Effect = effect; // for drop the effect isnt obtained from current state, but from the arguments
            var keyModifiers = new TestKeyModifiers();
            return new TreeViewDragDrop(this.Persistence, args, keyModifiers, this.targetGroup, null);
        }

        private static DragEventArgs CreateDragArguments(object source)
        {
            var data = new DataObject(source);
            return new DragEventArgs(data, 0, 0, 0, DragDropEffects.All, DragDropEffects.None);
        }
    }
}
