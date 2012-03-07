using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Terminals.SysInfo
{
    public partial class SystemInformation : UserControl
    {
        public SystemInformation()
        {
            InitializeComponent();
        }
        SysInfo.SystemInformationNode rootSystemNode = SysInfo.SystemInformationNode.LoadRoot();
        TreeNode rootTreeNode = new TreeNode();
        private void SystemInformation_Load(object sender, EventArgs e)
        {
            this.treeView1.Nodes.Add(rootTreeNode);
            rootTreeNode.Text = rootSystemNode.NodeDisplayValue;
            rootTreeNode.Tag = rootSystemNode;

            foreach(SysInfo.SystemInformationNode n in rootSystemNode.Nodes)
            {
                AddSysNodeToTree(n, rootTreeNode);
            }
            rootTreeNode.Expand();
        }

        private void AddSysNodeToTree(SysInfo.SystemInformationNode Node, TreeNode TreeNode)
        {
            TreeNode newTreeNode = new TreeNode();
            newTreeNode.Text = Node.NodeDisplayValue;
            newTreeNode.Tag = Node;
            TreeNode.Nodes.Add(newTreeNode);
            foreach(SysInfo.SystemInformationNode n in Node.Nodes)
            {
                AddSysNodeToTree(n, newTreeNode);
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                this.propertyGrid1.SelectedObject = null;
                SysInfo.SystemInformationNode sysNode = (e.Node.Tag as SysInfo.SystemInformationNode);
                if(sysNode != null)
                {
                    DataTable dt = (DataTable)sysNode.Execute();
                    if(dt != null)
                    {
                        if(dt.Rows.Count > 0)
                        {
                            
                            //this.dataGridView1.DataSource = dt;
                            //this.propertyGrid1.SelectedObject = Terminals.Network.WMI.PivotDataTable.ConvertToNameValue(dt, 0);
                            this.propertyGrid1.SelectedObject = Terminals.Network.WMI.PivotDataTable.CreateTypeFromDataTable(dt);
                        }
                    }
                }
            }
            if(e.Button == MouseButtons.Right)
            {

            }
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
