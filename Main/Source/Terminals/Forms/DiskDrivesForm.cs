using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Terminals
{
    internal partial class DiskDrivesForm : Form
    {
        private readonly NewTerminalForm parentForm;

        private bool updatingState;

        private TreeNode DevicesNode
        {
            get { return treeView1.Nodes["NodeDevices"]; }   
        }

        private TreeNode DrivesNode
        {
            get { return treeView1.Nodes["NodeDrives"]; }
        }

        public DiskDrivesForm(NewTerminalForm parentForm)
        {
            InitializeComponent();
            this.parentForm = parentForm;
            LoadDevices();
        }

        private void LoadDevices()
        {
            try
            {
                this.DevicesNode.Checked = this.parentForm.RedirectDevices;

                List<string> redirectedDrives = this.parentForm.RedirectedDrives;
                if (redirectedDrives == null)
                    redirectedDrives = new List<string>();

                foreach (DriveInfo drive in DriveInfo.GetDrives())
                {
                    this.TryAddDrive(drive, redirectedDrives);
                }

                if (redirectedDrives.Count > 0 && redirectedDrives[0].Equals("true"))
                    this.DevicesNode.Checked = true;

                treeView1.ExpandAll();
            }
            catch (Exception exc)
            {
                Logging.Log.Error("Failed to load Disk Drive devices.", exc);
            }
        }

        private void TryAddDrive(DriveInfo drive, List<string> redirectedDrives)
        {
            try
            {
                string name = drive.Name.TrimEnd("\\".ToCharArray());
                string nodeName = string.Format("{0} ({1})", name, drive.VolumeLabel);
                var tn = new TreeNode(nodeName);
                tn.Name = name;
                if (redirectedDrives.Contains(name))
                    tn.Checked = true;
                this.DrivesNode.Nodes.Add(tn);
            }
            catch (Exception e)
            {
                Logging.Log.Error("Error loading a drive into the tree", e);
            }
        }

        private void DiskDrivesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            List<string> redirectedDrives = new List<string>();
            if (this.DevicesNode.Checked)
            {
                redirectedDrives.Add("true");
            }
            else
            {
                IEnumerable<string> checkedNodes = this.DrivesNode.Nodes.Cast<TreeNode>()
                    .Where(tn => tn.Checked)
                    .Select(tn => tn.Name);
                redirectedDrives.AddRange(checkedNodes);
            }
            this.parentForm.RedirectedDrives = redirectedDrives;
            this.parentForm.RedirectDevices = this.DevicesNode.Checked;
        }

        private void TreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (updatingState)
                return;

            updatingState = true;
            if (e.Node.Nodes.Count > 0)
            {
                foreach (TreeNode childNode in e.Node.Nodes)
                    childNode.Checked = e.Node.Checked;
            }

            if (e.Node.Parent != null && !e.Node.Checked)
            {
                e.Node.Parent.Checked = e.Node.Checked;
            }
            updatingState = false;
        }
    }
}
