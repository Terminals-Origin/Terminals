using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace Terminals.CaptureManager
{
    public partial class CaptureManagerLayout : UserControl
    {
        public CaptureManagerLayout()
        {
            InitializeComponent();
        }
        TreeNode root = new TreeNode("Root");

        ToolStripMenuItem flickrMenuItem;
        private void CaptureManagerLayout_Load(object sender, EventArgs e)
        {
            flickrMenuItem = new ToolStripMenuItem("Post selected images to Flickr");
            flickrMenuItem.Click += new EventHandler(flickrMenuItem_Click);
            LoadRoot();
        }

        private void SendToFlickr(object state) {
            try {
                List<ListViewItem> items = (List<ListViewItem>)state;
                if(items != null && items.Count > 0) {
                    foreach(ListViewItem lvi in items) {
                        Capture cap = (lvi.Tag as Capture);
                        cap.PostToFlickr();
                    }
                }
                System.Windows.Forms.MessageBox.Show("All images have been uploaded to Flickr.");
            } catch(Exception exc) {
                System.Windows.Forms.MessageBox.Show("There was an error uploading your screen shots to Flickr:\r\n" + exc.Message);
                Terminals.Logging.Log.Error("There was an error uploading your screen shots to Flickr.", exc);
            }
        }
        void flickrMenuItem_Click(object sender, EventArgs e) {
            List<ListViewItem> items = new List<ListViewItem>();
            foreach(ListViewItem item in listView1.SelectedItems) {
                items.Add(item);
            }
            if(items.Count > 0) {
                if(System.Windows.Forms.MessageBox.Show("Are you sure you want to post " + items.Count + " images to your Flickr account?", "Confirmation Required", MessageBoxButtons.OKCancel) == DialogResult.OK) {
                    System.Windows.Forms.MessageBox.Show("All items have been queued for upload to Flickr.  Once the upload has been completed you will be notified.");
                    System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(SendToFlickr), items);
                }
            } else {
                System.Windows.Forms.MessageBox.Show("You must first select a screen capture to upload.");
            }
        }
        public void RefreshView()
        {
            if(this.treeView1.SelectedNode != null)
            {
                System.IO.DirectoryInfo folder = (this.treeView1.SelectedNode.Tag as System.IO.DirectoryInfo);
                LoadFolder(folder.FullName, this.treeView1.SelectedNode);
            }
        }
        private void LoadRoot()
        {
            root.Tag = new System.IO.DirectoryInfo(CaptureManager.CaptureRoot);
            this.treeView1.Nodes.Add(root);
            
            LoadFolder(CaptureManager.CaptureRoot, root);
            root.Expand();
        }
        private void LoadFolder(string Path, TreeNode Parent)
        {
            listView1.Items.Clear();
            Parent.Nodes.Clear();
            List<System.IO.DirectoryInfo> list = CaptureManager.LoadCaptureFolder(Path);
            foreach(System.IO.DirectoryInfo folder in list)
            {
                TreeNode child = new TreeNode(folder.Name);
                child.Tag = folder;
                Parent.Nodes.Add(child);
            }
            Captures c = CaptureManager.LoadCaptures(Path);
            foreach(Capture cap in c)
            {

                ListViewItem item = new ListViewItem();
                item.Tag = cap;
                item.Text = cap.Name;
                item.ToolTipText = cap.FilePath;
                int index = this.imageList1.Images.Add(cap.Image, Color.HotPink);
                item.ImageIndex = index;
                listView1.Items.Add(item);
            }
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            pictureBox1.Image = null;
            pictureCommentsTextBox.Text = "";
            saveButton.Enabled = false;
            deleteButton.Enabled = false;

            System.IO.DirectoryInfo dir = (e.Node.Tag as System.IO.DirectoryInfo);
            LoadFolder(dir.FullName, e.Node);
        }


        private void newFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(treeView1.SelectedNode != null)
            {
                System.IO.DirectoryInfo dir = (treeView1.SelectedNode.Tag as System.IO.DirectoryInfo);
                Terminals.InputBoxResult result = Terminals.InputBox.Show("New Folder Name");
                if(result.ReturnCode == DialogResult.OK)
                {
                    string rootFolder = dir.FullName;
                    string fullNewName = System.IO.Path.Combine(rootFolder, result.Text);
                    if(!System.IO.Directory.Exists(fullNewName))
                    {
                        System.IO.DirectoryInfo info = System.IO.Directory.CreateDirectory(fullNewName);
                        TreeNode node = new TreeNode(result.Text);
                        node.Tag = info;
                        treeView1.SelectedNode.Nodes.Add(node);
                        treeView1.SelectedNode.Expand();
                    }
                }
            }
        }

        private void deleteFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(treeView1.SelectedNode != null && treeView1.SelectedNode!=this.root)
            {
                System.IO.DirectoryInfo dir = (treeView1.SelectedNode.Tag as System.IO.DirectoryInfo);
                if(System.IO.Directory.Exists(dir.FullName))
                {
                    System.IO.FileInfo[] files = dir.GetFiles();
                    System.IO.DirectoryInfo[] dirs = dir.GetDirectories();
                    System.Text.StringBuilder sb = new StringBuilder();
                    sb.Append("Are you sure you want to delete this folder?");
                    sb.Append("\r\n");
                    sb.Append(treeView1.SelectedNode.Text);
                    sb.Append("\r\n");
                    if(files.Length > 0)
                    {
                        sb.Append("The directory contains ");
                        sb.Append(files.Length);
                        sb.Append(" files.\r\n");
                    }
                    if(dirs.Length > 0)
                    {
                        sb.Append("The directory contains ");
                        sb.Append(dirs.Length);
                        sb.Append(" directories.\r\n");
                    }
                    DialogResult result = System.Windows.Forms.MessageBox.Show(sb.ToString(), "Confirmation", MessageBoxButtons.OKCancel);
                    if(result == DialogResult.OK)
                    {
                        string rootFolder = dir.FullName;
                        System.IO.Directory.Delete(rootFolder, true);
                        treeView1.SelectedNode.Remove();
                    }
                }
                else
                {
                    treeView1.SelectedNode.Remove();
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

            if(e.Data.GetDataPresent("System.Windows.Forms.ListViewItem", false))
            {
                Point pt = ((TreeView)sender).PointToClient(new Point(e.X, e.Y));
                TreeNode DestinationNode = ((TreeView)sender).GetNodeAt(pt);
                NewNode = (System.Windows.Forms.ListViewItem)e.Data.GetData("System.Windows.Forms.ListViewItem");
                Terminals.CaptureManager.Capture c = (NewNode.Tag as Terminals.CaptureManager.Capture);
                System.IO.DirectoryInfo destInfo = (DestinationNode.Tag as System.IO.DirectoryInfo);
                string dest = System.IO.Path.Combine(destInfo.FullName, System.IO.Path.GetFileName(c.FilePath));
                c.Move(dest);
                treeView1_AfterSelect(null, new TreeViewEventArgs(this.treeView1.SelectedNode));
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(listView1.SelectedItems != null && listView1.SelectedItems.Count > 0)
            {
                foreach(ListViewItem lvi in listView1.SelectedItems)
                {
                    Capture cap = (lvi.Tag as Capture);
                    cap.Show();
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            pictureCommentsTextBox.Text = "";
            saveButton.Enabled = false;
            deleteButton.Enabled = false;
            if(listView1.SelectedItems != null && listView1.SelectedItems.Count > 0)
            {
                Capture cap = (listView1.SelectedItems[0].Tag as Capture);
                pictureBox1.Image = cap.Image;
                pictureCommentsTextBox.Text = cap.Comments;
                saveButton.Enabled = true;
                deleteButton.Enabled = true;
            }
        }


        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right && listView1.SelectedItems != null && listView1.SelectedItems.Count > 0)
            {
                listView1.ContextMenuStrip.Show();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItems != null && listView1.SelectedItems.Count > 0)
            {
                foreach(ListViewItem lvi in listView1.SelectedItems)
                {
                    Capture cap = (lvi.Tag as Capture);
                    cap.Delete();
                    listView1.Items.Remove(lvi);
                }
            }
        }

        private void copyImageToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItems != null && listView1.SelectedItems.Count > 0)
            {
                Capture cap = (listView1.SelectedItems[0].Tag as Capture);
                Clipboard.SetImage(cap.Image);
            }
        }

        private void copyImagePathToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItems != null && listView1.SelectedItems.Count > 0)
            {
                Capture cap = (listView1.SelectedItems[0].Tag as Capture);
                Clipboard.SetText(cap.FilePath);
            }

        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            deleteToolStripMenuItem_Click(null, null);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItems != null && listView1.SelectedItems.Count > 0)
            {
                Capture cap = (listView1.SelectedItems[0].Tag as Capture);
                cap.Comments = this.pictureCommentsTextBox.Text;
                cap.Save();
            }
        }

        private void View_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(ViewMode.Text)
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
            RefreshView();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            this.imageList1.ImageSize = new Size(trackBar1.Value, trackBar1.Value);
            this.listView1.View = View.LargeIcon;
            RefreshView();

        }

        private void thumbsContextMenu_Opening(object sender, CancelEventArgs e) {
            if(Settings.FlickrToken != "" && (listView1.SelectedItems != null && listView1.SelectedItems.Count > 0)) {
                thumbsContextMenu.Items.Add(flickrMenuItem);
            }
        }
    }
}