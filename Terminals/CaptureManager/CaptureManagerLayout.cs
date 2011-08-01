using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Terminals.Configuration;

namespace Terminals.CaptureManager
{
    public partial class CaptureManagerLayout : UserControl
    {
        private TreeNode root = new TreeNode("Capture Root Folder");
        private ToolStripMenuItem flickrMenuItem;

        public CaptureManagerLayout()
        {
            this.InitializeComponent();
        }
        
        private void CaptureManagerLayout_Load(object sender, EventArgs e)
        {
            this.flickrMenuItem = new ToolStripMenuItem("Post selected images to Flickr");
            this.flickrMenuItem.Click += new EventHandler(this.flickrMenuItem_Click);
            this.LoadRoot();
        }

        private void SendToFlickr(object state)
        {
            try
            {
                List<ListViewItem> items = (List<ListViewItem>)state;
                if (items != null && items.Count > 0)
                {
                    foreach (ListViewItem lvi in items)
                    {
                        Capture cap = (lvi.Tag as Capture);
                        cap.PostToFlickr();
                    }
                }

                System.Windows.Forms.MessageBox.Show("All images have been uploaded to Flickr.");
            }
            catch (Exception ex)
            {
                Terminals.Logging.Log.Info("There was an error uploading your screen shots to Flickr.", ex);
                System.Windows.Forms.MessageBox.Show("There was an error uploading your screen shots to Flickr:\r\n" + ex.Message);
            }
        }

        public void flickrMenuItem_Click(object sender, EventArgs e)
        {
            List<ListViewItem> items = new List<ListViewItem>();
            foreach (ListViewItem item in this.listView1.SelectedItems)
            {
                items.Add(item);
            }

            if (items.Count > 0)
            {
                if (System.Windows.Forms.MessageBox.Show("Are you sure you want to post " + items.Count + " images to your Flickr account?", "Confirmation Required", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    System.Windows.Forms.MessageBox.Show("All items have been queued for upload to Flickr.  Once the upload has been completed you will be notified.");
                    System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.SendToFlickr), items);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("You must first select a screen capture to upload.");
            }
        }

        public void RefreshView()
        {
            if (this.treeView1.SelectedNode != null)
            {
                System.IO.DirectoryInfo folder = (this.treeView1.SelectedNode.Tag as System.IO.DirectoryInfo);
                this.LoadFolder(folder.FullName, this.treeView1.SelectedNode);
            }
        }

        private void LoadRoot()
        {
            this.root.Tag = new System.IO.DirectoryInfo(CaptureManager.CaptureRoot);

            this.treeView1.Nodes.Add(this.root);
            this.treeView1.SelectedNode = this.root;

            this.LoadFolder(CaptureManager.CaptureRoot, this.root);
            this.root.Expand();
        }

        private void LoadFolder(string Path, TreeNode Parent)
        {
            this.listView1.Items.Clear();
            Parent.Nodes.Clear();
            List<System.IO.DirectoryInfo> list = CaptureManager.LoadCaptureFolder(Path);
            foreach (System.IO.DirectoryInfo folder in list)
            {
                TreeNode child = new TreeNode(folder.Name);
                child.Tag = folder;
                Parent.Nodes.Add(child);
            }

            Captures c = CaptureManager.LoadCaptures(Path);
            foreach (Capture cap in c)
            {
                ListViewItem item = new ListViewItem();
                item.Tag = cap;
                item.Text = cap.Name;
                item.ToolTipText = cap.FilePath;
                int index = this.imageList1.Images.Add(cap.Image, Color.HotPink);
                item.ImageIndex = index;
                this.listView1.Items.Add(item);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.pictureBox1.Image = null;
            this.pictureCommentsTextBox.Text = string.Empty;
            this.saveButton.Enabled = false;
            this.deleteButton.Enabled = false;

            System.IO.DirectoryInfo dir = (e.Node.Tag as System.IO.DirectoryInfo);
            this.LoadFolder(dir.FullName, e.Node);
        }

        private void newFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.treeView1.SelectedNode != null)
            {
                System.IO.DirectoryInfo dir = (this.treeView1.SelectedNode.Tag as System.IO.DirectoryInfo);
                Terminals.InputBoxResult result = Terminals.InputBox.Show("New Folder Name");
                if (result.ReturnCode == DialogResult.OK)
                {
                    string rootFolder = dir.FullName;
                    string fullNewName = System.IO.Path.Combine(rootFolder, result.Text);
                    if (!System.IO.Directory.Exists(fullNewName))
                    {
                        System.IO.DirectoryInfo info = System.IO.Directory.CreateDirectory(fullNewName);
                        TreeNode node = new TreeNode(result.Text);
                        node.Tag = info;
                        this.treeView1.SelectedNode.Nodes.Add(node);
                        this.treeView1.SelectedNode.Expand();
                    }
                }
            }
        }

        private void deleteFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.treeView1.SelectedNode != null && this.treeView1.SelectedNode != this.root)
            {
                System.IO.DirectoryInfo dir = (this.treeView1.SelectedNode.Tag as System.IO.DirectoryInfo);
                if (System.IO.Directory.Exists(dir.FullName))
                {
                    System.IO.FileInfo[] files = dir.GetFiles();
                    System.IO.DirectoryInfo[] dirs = dir.GetDirectories();
                    string msg = string.Format("{0}\r\n\r\n", Program.Resources.GetString("ConfirmDeleteSingleFolder"));
                    if (files.Length > 0)
                    {
                        msg += string.Format("The folder \"{0}\" contains {1} files.", this.treeView1.SelectedNode.Text, files.Length);
                    }

                    if (dirs.Length > 0)
                    {
                        msg += string.Format("The folder \"{0}\" contains {1} directories.", this.treeView1.SelectedNode.Text, dirs.Length);
                    }

                    DialogResult result = System.Windows.Forms.MessageBox.Show(msg, Program.Resources.GetString("ConfirmCaptionDeleteSingleFolder"), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.OK)
                    {
                        string rootFolder = dir.FullName;
                        System.IO.Directory.Delete(rootFolder, true);
                        this.treeView1.SelectedNode.Remove();
                    }
                }
                else
                {
                    this.treeView1.SelectedNode.Remove();
                }
            }
        }

        private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void treeView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void listView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.None;
        }

        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            System.Windows.Forms.ListViewItem NewNode;

            if (e.Data.GetDataPresent("System.Windows.Forms.ListViewItem", false))
            {
                Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
                TreeNode DestinationNode = ((TreeView)sender).GetNodeAt(pt);
                NewNode = (System.Windows.Forms.ListViewItem)e.Data.GetData("System.Windows.Forms.ListViewItem");
                Terminals.CaptureManager.Capture c = (NewNode.Tag as Terminals.CaptureManager.Capture);

                if (DestinationNode != null)
                {
                    System.IO.DirectoryInfo destInfo = (DestinationNode.Tag as System.IO.DirectoryInfo);
                    string dest = System.IO.Path.Combine(destInfo.FullName, System.IO.Path.GetFileName(c.FilePath));
                    c.Move(dest);

                    this.treeView1_AfterSelect(null, new TreeViewEventArgs(this.treeView1.SelectedNode));
                }
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.listView1.SelectedItems != null && this.listView1.SelectedItems.Count > 0)
            {
                foreach (ListViewItem lvi in this.listView1.SelectedItems)
                {
                    Capture cap = (lvi.Tag as Capture);
                    cap.Show();
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.pictureBox1.Image = null;
            this.pictureCommentsTextBox.Text = string.Empty;
            this.saveButton.Enabled = false;
            this.deleteButton.Enabled = false;
            if (this.listView1.SelectedItems != null && this.listView1.SelectedItems.Count > 0)
            {
                Capture cap = (this.listView1.SelectedItems[0].Tag as Capture);
                this.pictureBox1.Image = cap.Image;
                this.pictureCommentsTextBox.Text = cap.Comments;
                this.saveButton.Enabled = true;
                this.deleteButton.Enabled = true;
            }
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && this.listView1.SelectedItems != null && this.listView1.SelectedItems.Count > 0)
            {
                this.listView1.ContextMenuStrip.Show();
            }
        }

        /// <summary>
        /// Delete selected listview item on delete key press.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                this.deleteFileToolStripMenuItem_Click(null, null);
            }
        }

        private void treeView1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && this.treeView1.SelectedNode != null)
            {
                this.treeView1.SelectedNode = this.treeView1.HitTest(e.Location).Node;
            }
        }

        private void treeView1_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {
            Point pos = this.treeView1.PointToClient(new Point(e.X, e.Y));
            TreeViewHitTestInfo hit = this.treeView1.HitTest(pos);

            if (hit.Node != null)
            {
                hit.Node.Expand();
                this.treeView1.SelectedNode = hit.Node;
                e.Effect = DragDropEffects.Move;
            }
        }

        private void treeView1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            this.deleteFolderToolStripMenuItem_Click(null, null);
        }

        private void deleteFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems != null && this.listView1.SelectedItems.Count > 0)
            {
                int cnt = this.listView1.SelectedItems.Count;
                string msg = string.Empty;
                string cpt = string.Empty;

                if (this.listView1.SelectedItems.Count == 1)
                {
                    msg = Program.Resources.GetString("ConfirmDeleteSingleFile");
                    cpt = Program.Resources.GetString("ConfirmCaptionDeleteSingleItem");
                }
                else
                {
                    msg = string.Format(Program.Resources.GetString("ConfirmDeleteMultipleFiles"), cnt);
                    cpt = Program.Resources.GetString("ConfirmCaptionDeleteMultipleItems");
                }

                if (MessageBox.Show(msg, cpt, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                {
                    foreach (ListViewItem lvi in this.listView1.SelectedItems)
                    {
                        Capture cap = (lvi.Tag as Capture);
                        cap.Delete();
                        this.listView1.Items.Remove(lvi);
                    }
                }
            }
        }

        private void copyImageToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems != null && this.listView1.SelectedItems.Count > 0)
            {
                Capture cap = (this.listView1.SelectedItems[0].Tag as Capture);
                Clipboard.SetImage(cap.Image);
            }
        }

        private void copyImagePathToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems != null && this.listView1.SelectedItems.Count > 0)
            {
                Capture cap = (this.listView1.SelectedItems[0].Tag as Capture);
                Clipboard.SetText(cap.FilePath);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            this.deleteFileToolStripMenuItem_Click(null, null);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (this.listView1.SelectedItems != null && this.listView1.SelectedItems.Count > 0)
            {
                Capture cap = (this.listView1.SelectedItems[0].Tag as Capture);
                cap.Comments = this.pictureCommentsTextBox.Text;
                cap.Save();
            }
        }

        private void View_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (this.ViewMode.Text)
            {
                case "Large Icons":
                    this.imageList1.ImageSize = new Size(150, 150);
                    this.listView1.View = View.LargeIcon;
                    break;

                case "Small Icons":
                    this.imageList1.ImageSize = new Size(50, 50);
                    this.listView1.View = View.LargeIcon;
                    break;

                case "List":
                    this.imageList1.ImageSize = new Size(150, 150);
                    this.listView1.View = View.List;
                    break;

                case "Tile":
                    this.imageList1.ImageSize = new Size(150, 150);
                    this.listView1.View = View.Tile;
                    break;

                default:
                    break;
            }

            this.RefreshView();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.imageList1.ImageSize = new Size(this.trackBar1.Value, this.trackBar1.Value);
            this.listView1.View = View.LargeIcon;
            this.RefreshView();
        }

        private void thumbsContextMenu_Opening(object sender, CancelEventArgs e)
        {
            if (Settings.FlickrToken != string.Empty && (this.listView1.SelectedItems != null && this.listView1.SelectedItems.Count > 0))
            {
                this.thumbsContextMenu.Items.Add(this.flickrMenuItem);
            }
        }
    }
}