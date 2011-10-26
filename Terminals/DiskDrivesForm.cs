using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace Terminals
{
    internal partial class DiskDrivesForm : Form
    {
        private NewTerminalForm _parentForm;
        private bool updatingState = false;

        public DiskDrivesForm(NewTerminalForm parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;
            LoadDevices();
        }

        private void LoadDevices()
        {
            try
            {
                treeView1.Nodes["NodeDevices"].Checked = this._parentForm.RedirectDevices;

                DriveInfo[] drives = DriveInfo.GetDrives();
                List<string> _redirectedDrives = this._parentForm.RedirectedDrives;

                foreach (DriveInfo drive in drives)
                {
                    try
                    {
                        string name = drive.Name.TrimEnd("\\".ToCharArray());
                        TreeNode tn = new TreeNode(name + " (" + drive.VolumeLabel + ")");
                        tn.Name = name;
                        if (_redirectedDrives != null && _redirectedDrives.Contains(name))
                            tn.Checked = true;
                        treeView1.Nodes["NodeDrives"].Nodes.Add(tn);
                    }
                    catch (Exception e)
                    {
                        Terminals.Logging.Log.Error("Error loading a drive into the tree", e);
                    }
                }

                if (_redirectedDrives != null && _redirectedDrives.Count > 0 && _redirectedDrives[0].Equals("true"))
                    treeView1.Nodes["NodeDrives"].Checked = true;

                treeView1.ExpandAll();
            }
            catch (Exception exc)
            {
                Logging.Log.Error("Failed to load Disk Drive devices.", exc);
                throw;
            }

        }

        private void DiskDrivesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            List<string> _redirectedDrives = new List<string>();
            if (treeView1.Nodes["NodeDrives"].Checked)
            {
                _redirectedDrives.Add("true");
            }
            else
            {
                foreach (TreeNode tn in treeView1.Nodes["NodeDrives"].Nodes)
                    if (tn.Checked)
                        _redirectedDrives.Add(tn.Name);
            }
            this._parentForm.RedirectedDrives = _redirectedDrives;
            this._parentForm.RedirectDevices = treeView1.Nodes["NodeDevices"].Checked;
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
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
