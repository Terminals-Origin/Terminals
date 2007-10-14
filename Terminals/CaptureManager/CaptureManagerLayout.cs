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

        private void CaptureManagerLayout_Load(object sender, EventArgs e)
        {
            LoadRoot();
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
            if(treeView1.SelectedNode != null)
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
                Capture cap = (listView1.SelectedItems[0].Tag as Capture);
                cap.Show();
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listView1.SelectedItems != null && listView1.SelectedItems.Count > 0)
            {
                Capture cap = (listView1.SelectedItems[0].Tag as Capture);
                pictureBox1.Image = cap.Image;
                pictureCommentsTextBox.Text = cap.Comments;
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
                Capture cap = (listView1.SelectedItems[0].Tag as Capture);
                cap.Delete();
                listView1.Items.Remove(listView1.SelectedItems[0]);
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
    }
}