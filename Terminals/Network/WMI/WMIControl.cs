using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace WMITestClient
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class WMIControl : System.Windows.Forms.UserControl
    {
        System.Collections.ArrayList history = new System.Collections.ArrayList();
        private System.Windows.Forms.Button QueryButton;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button StopButton;
        private System.Windows.Forms.ComboBox QueryTextBox;
        private System.Windows.Forms.MainMenu mainMenu1;
        private System.Windows.Forms.MenuItem menuItem1;
        private System.Windows.Forms.MenuItem menuItem2;
        private System.Windows.Forms.MenuItem LoginMenuItem;
        private System.Windows.Forms.MenuItem ExitmenuItem;
        private System.Windows.Forms.MenuItem SavemenuItem;
        private System.Windows.Forms.MenuItem LoadmenuItem;
        private System.Windows.Forms.MenuItem ClearmenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.MenuItem BasicTreemenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage QueryTabPage;
        private System.Windows.Forms.TabPage ClassesTabPage;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.TreeView treeView2;
        private Button ConnectButton;
        private SplitContainer splitContainer1;
        private Panel panel1;
        private Label ConnectionLabel;
        private IContainer components;


        public WMIControl()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            Form1_Resize(null, null);
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("To load-> Double Click");
            this.QueryButton = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.StopButton = new System.Windows.Forms.Button();
            this.QueryTextBox = new System.Windows.Forms.ComboBox();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.LoginMenuItem = new System.Windows.Forms.MenuItem();
            this.menuItem2 = new System.Windows.Forms.MenuItem();
            this.SavemenuItem = new System.Windows.Forms.MenuItem();
            this.LoadmenuItem = new System.Windows.Forms.MenuItem();
            this.ClearmenuItem = new System.Windows.Forms.MenuItem();
            this.BasicTreemenuItem = new System.Windows.Forms.MenuItem();
            this.ExitmenuItem = new System.Windows.Forms.MenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.QueryTabPage = new System.Windows.Forms.TabPage();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.ClassesTabPage = new System.Windows.Forms.TabPage();
            this.treeView2 = new System.Windows.Forms.TreeView();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.ConnectionLabel = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.QueryTabPage.SuspendLayout();
            this.ClassesTabPage.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // QueryButton
            // 
            this.QueryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.QueryButton.Location = new System.Drawing.Point(333, 7);
            this.QueryButton.Name = "QueryButton";
            this.QueryButton.Size = new System.Drawing.Size(67, 24);
            this.QueryButton.TabIndex = 1;
            this.QueryButton.Text = "&Query";
            this.QueryButton.Click += new System.EventHandler(this.QueryButton_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 56);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(494, 10);
            this.progressBar1.TabIndex = 5;
            // 
            // StopButton
            // 
            this.StopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.StopButton.Enabled = false;
            this.StopButton.Location = new System.Drawing.Point(408, 7);
            this.StopButton.Name = "StopButton";
            this.StopButton.Size = new System.Drawing.Size(67, 24);
            this.StopButton.TabIndex = 2;
            this.StopButton.Text = "Stop!";
            this.StopButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // QueryTextBox
            // 
            this.QueryTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.QueryTextBox.Location = new System.Drawing.Point(0, 0);
            this.QueryTextBox.Name = "QueryTextBox";
            this.QueryTextBox.Size = new System.Drawing.Size(494, 21);
            this.QueryTextBox.TabIndex = 0;
            this.QueryTextBox.Text = "select * from CIM_System";
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1});
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 0;
            this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.LoginMenuItem,
            this.menuItem2,
            this.BasicTreemenuItem,
            this.ExitmenuItem});
            this.menuItem1.Text = "&Options";
            // 
            // LoginMenuItem
            // 
            this.LoginMenuItem.Index = 0;
            this.LoginMenuItem.Text = "&Login";
            this.LoginMenuItem.Click += new System.EventHandler(this.LoginMenuItem_Click);
            // 
            // menuItem2
            // 
            this.menuItem2.Index = 1;
            this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.SavemenuItem,
            this.LoadmenuItem,
            this.ClearmenuItem});
            this.menuItem2.Text = "&History";
            // 
            // SavemenuItem
            // 
            this.SavemenuItem.Index = 0;
            this.SavemenuItem.Text = "&Save";
            this.SavemenuItem.Click += new System.EventHandler(this.SavemenuItem_Click);
            // 
            // LoadmenuItem
            // 
            this.LoadmenuItem.Index = 1;
            this.LoadmenuItem.Text = "L&oad";
            this.LoadmenuItem.Click += new System.EventHandler(this.LoadmenuItem_Click);
            // 
            // ClearmenuItem
            // 
            this.ClearmenuItem.Index = 2;
            this.ClearmenuItem.Text = "&Clear";
            this.ClearmenuItem.Click += new System.EventHandler(this.ClearmenuItem_Click);
            // 
            // BasicTreemenuItem
            // 
            this.BasicTreemenuItem.Index = 2;
            this.BasicTreemenuItem.Text = "Load Static Class Tree";
            this.BasicTreemenuItem.Click += new System.EventHandler(this.BasicTreemenuItem_Click);
            // 
            // ExitmenuItem
            // 
            this.ExitmenuItem.Index = 3;
            this.ExitmenuItem.Text = "E&xit";
            this.ExitmenuItem.Click += new System.EventHandler(this.ExitmenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.QueryTabPage);
            this.tabControl1.Controls.Add(this.ClassesTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(494, 354);
            this.tabControl1.TabIndex = 3;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // QueryTabPage
            // 
            this.QueryTabPage.Controls.Add(this.treeView1);
            this.QueryTabPage.Location = new System.Drawing.Point(4, 22);
            this.QueryTabPage.Name = "QueryTabPage";
            this.QueryTabPage.Size = new System.Drawing.Size(486, 328);
            this.QueryTabPage.TabIndex = 0;
            this.QueryTabPage.Text = "Query";
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(486, 328);
            this.treeView1.TabIndex = 3;
            this.treeView1.DoubleClick += new System.EventHandler(this.treeView1_DoubleClick);
            // 
            // ClassesTabPage
            // 
            this.ClassesTabPage.Controls.Add(this.treeView2);
            this.ClassesTabPage.Location = new System.Drawing.Point(4, 22);
            this.ClassesTabPage.Name = "ClassesTabPage";
            this.ClassesTabPage.Size = new System.Drawing.Size(486, 328);
            this.ClassesTabPage.TabIndex = 1;
            this.ClassesTabPage.Text = "Classes";
            // 
            // treeView2
            // 
            this.treeView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView2.Location = new System.Drawing.Point(0, 0);
            this.treeView2.Name = "treeView2";
            treeNode1.Name = "";
            treeNode1.Text = "To load-> Double Click";
            this.treeView2.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.treeView2.Size = new System.Drawing.Size(486, 328);
            this.treeView2.TabIndex = 4;
            this.treeView2.DoubleClick += new System.EventHandler(this.treeView2_DoubleClick);
            this.treeView2.Click += new System.EventHandler(this.treeView2_Click);
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(4, 7);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(67, 24);
            this.ConnectButton.TabIndex = 6;
            this.ConnectButton.Text = "Connect...";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1.Controls.Add(this.QueryTextBox);
            this.splitContainer1.Panel1.Controls.Add(this.progressBar1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(494, 424);
            this.splitContainer1.SplitterDistance = 66;
            this.splitContainer1.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.ConnectionLabel);
            this.panel1.Controls.Add(this.QueryButton);
            this.panel1.Controls.Add(this.StopButton);
            this.panel1.Controls.Add(this.ConnectButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 21);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(494, 35);
            this.panel1.TabIndex = 7;
            // 
            // ConnectionLabel
            // 
            this.ConnectionLabel.AutoSize = true;
            this.ConnectionLabel.Location = new System.Drawing.Point(88, 13);
            this.ConnectionLabel.Name = "ConnectionLabel";
            this.ConnectionLabel.Size = new System.Drawing.Size(115, 13);
            this.ConnectionLabel.TabIndex = 7;
            this.ConnectionLabel.Text = "\\\\localhost\\root\\cimv2";
            // 
            // WMIControl
            // 
            this.Controls.Add(this.splitContainer1);
            this.Name = "WMIControl";
            this.Size = new System.Drawing.Size(494, 424);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.tabControl1.ResumeLayout(false);
            this.QueryTabPage.ResumeLayout(false);
            this.ClassesTabPage.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }
        #endregion


        public void Form1_Resize(object sender, System.EventArgs e)
        {
            //this.tabControl1.Height = this.Height - 50;
            //this.StopButton.Left = this.Width - QueryButton.Width - 15;
            //QueryButton.Left = StopButton.Left - QueryButton.Width - 15;
            //this.ConnectButton.Left = QueryButton.Left - StopButton.Left - QueryButton.Width - StopButton.Width - 15;
            
            //this.QueryTextBox.Width = QueryButton.Left - 15;
            progressBar1.Width = this.Width - 10;
            //progressBar1.Left=this.Width-progressBar1.Width-25;
            //progressBar1.Top=this.Height-progressBar1.Height-33;
        }

        private void IncrementBar()
        {
            if (progressBar1.Value >= this.progressBar1.Maximum) progressBar1.Value = 0;
            progressBar1.Value++;
        }
        private bool StilRunning = true;

        private void AddToHistory()
        {
            if (!history.Contains(this.QueryTextBox.Text))
            {
                this.QueryTextBox.Items.Add(this.QueryTextBox.Text);
                this.history.Add(this.QueryTextBox.Text);
            }
        }
        private string HistoryPath
        {
            get { return Application.StartupPath + "\\WMITestClientHistory.txt"; }
        }
        private void SaveHistory()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (string historyItem in this.history)
            {
                if (historyItem != "") sb.Append(historyItem + "\r\n");
            }
            System.IO.StreamWriter sw = new System.IO.StreamWriter(HistoryPath, false);
            sw.Write(sb.ToString());
            sw.Close();

        }
        private void LoadHistory(string path)
        {
            string realPath = path;
            if (realPath == null || realPath == "") realPath = HistoryPath;
            if (System.IO.File.Exists(realPath))
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(realPath);
                string contents = sr.ReadToEnd();
                sr.Close();
                string[] items = System.Text.RegularExpressions.Regex.Split(contents, "\r\n");
                foreach (string item in items)
                {
                    if (item != "")
                    {
                        history.Add(item);
                        this.QueryTextBox.Items.Add(item);
                    }
                }
            }
        }

        private void QueryButton_Click(object sender, System.EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            AddToHistory();
            StilRunning = true;
            this.QueryButton.Enabled = false;
            this.StopButton.Enabled = true;
            try
            {
                treeView1.Nodes.Clear();
                if (this.QueryTextBox.Text != "")
                {
                    string qry = QueryTextBox.Text;
                    System.Management.ManagementObjectSearcher searcher;
                    System.Management.ObjectQuery query = new System.Management.ObjectQuery(qry);

                    if (Username != "" && Password != "" && Computer != "" && !Computer.StartsWith(@"\\localhost"))
                    {
                        System.Management.ConnectionOptions oConn = new System.Management.ConnectionOptions();
                        oConn.Username = Username;
                        oConn.Password = Password;
                        System.Management.ManagementScope oMs = new System.Management.ManagementScope(Computer, oConn);
                        
                        searcher = new System.Management.ManagementObjectSearcher(oMs, query);
                    }
                    else
                    {
                        searcher = new System.Management.ManagementObjectSearcher(query);
                    }
                    System.Windows.Forms.TreeNode root = new TreeNode(qry);
                    root.Tag = "RootNode";
                    treeView1.Nodes.Add(root);
                    string path = "";
                    foreach (System.Management.ManagementObject share in searcher.Get())
                    {
                        System.Windows.Forms.TreeNode item = new TreeNode(share.ClassPath.ClassName);
                        item.Tag = "ClassNode";
                        root.Nodes.Add(item);
                        path = "Enumerating:" + share.ClassPath.ClassName;
                        foreach (System.Management.PropertyData p in share.Properties)
                        {
                            bool isLocal = p.IsLocal;
                            string type = p.Type.ToString();
                            string origin = p.Origin;
                            string name = p.Name;
                            path = "Enumerating:" + share.ClassPath.ClassName + "," + name;
                            Application.DoEvents();

                            bool IsArray = p.IsArray;
                            string val = "NULL";
                            if (p.Value != null) val = p.Value.ToString();
                            System.Windows.Forms.TreeNode node = new TreeNode(name);
                            node.Tag = "PropertyNode";
                            string display = "";
                            if (type.ToLower() == "string")
                            {
                                display = "Value='" + val + "'";
                            }
                            else
                            {
                                display = "Value=" + val;
                            }
                            System.Windows.Forms.TreeNode ValueNode = new TreeNode(display);
                            ValueNode.Tag = "ValueNode";
                            System.Windows.Forms.TreeNode TypeNode = new TreeNode("Type='" + type + "'");
                            TypeNode.Tag = "ValueNode";
                            System.Windows.Forms.TreeNode localNode = new TreeNode("IsLocal=" + isLocal);
                            localNode.Tag = "ValueNode";
                            System.Windows.Forms.TreeNode OriginNode = new TreeNode("Origin='" + origin + "'");
                            OriginNode.Tag = "ValueNode";
                            System.Windows.Forms.TreeNode IsArrayNode = new TreeNode("IsArray=" + IsArray);
                            IsArrayNode.Tag = "ValueNode";

                            node.Nodes.Add(ValueNode);
                            node.Nodes.Add(TypeNode);
                            node.Nodes.Add(localNode);
                            node.Nodes.Add(OriginNode);
                            node.Nodes.Add(IsArrayNode);

                            if (IsArray && p.Value != null)
                            {
                                System.Array a = (System.Array)p.Value;
                                for (int x = 0; x < a.Length; x++)
                                {
                                    string v = "";
                                    if (a.GetValue(x) != null) v = a.GetValue(x).ToString();
                                    System.Windows.Forms.TreeNode arrayNode = new TreeNode(name + "[" + x + "]=" + v);
                                    arrayNode.Tag = "ArrayNode";
                                    IsArrayNode.Nodes.Add(arrayNode);
                                    IncrementBar();
                                }
                            }
                            IncrementBar();

                            item.Nodes.Add(node);
                            Application.DoEvents();
                            if (!StilRunning) break;

                        }
                        if (!StilRunning) break;

                    }

                }
            }
            catch (Exception exc)
            {
                Terminals.Logging.Log.Info("", exc);
                System.Windows.Forms.MessageBox.Show("Error Thrown:" + exc.Message);
            }
            this.progressBar1.Value = 0;
            this.QueryButton.Enabled = true;
            this.StopButton.Enabled = false;
        }

        private void treeView1_DoubleClick(object sender, System.EventArgs e)
        {
            //
            //select * from win32_process where caption like 'k%'

            //work our way up the tree till the root
            if (sender != null)
            {
                string queryString = "";
                System.Windows.Forms.TreeNode n = ((System.Windows.Forms.TreeView)sender).SelectedNode;
                if (n != null)
                {
                    string nodeType = "";
                    if (n.Tag != null) nodeType = n.Tag.ToString();
                    string ntext = n.Text;
                    if (ntext.IndexOf("(") > 0) ntext = ntext.Substring(0, ntext.IndexOf("(") - 1).Trim();

                    switch (nodeType)
                    {
                        case "ClassNode":
                            queryString = "select * from " + ntext;
                            break;
                        case "PropertyNode":
                            System.Windows.Forms.TreeNode p = n.Parent;
                            string pnPText = p.Text;
                            if (pnPText.IndexOf("(") > 0) pnPText = pnPText.Substring(0, pnPText.IndexOf("(") - 1).Trim();
                            queryString = "select " + ntext + " from " + pnPText;
                            break;
                        case "ValueNode":
                            System.Windows.Forms.TreeNode p1 = n.Parent;
                            System.Windows.Forms.TreeNode pp = p1.Parent;
                            string ppText = pp.Text;
                            string p1Text = p1.Text;
                            if (ppText.IndexOf("(") > 0) ppText = ppText.Substring(0, ppText.IndexOf("(") - 1).Trim();
                            if (p1Text.IndexOf("(") > 0) p1Text = p1Text.Substring(0, p1Text.IndexOf("(") - 1).Trim();

                            if (ntext.Substring(0, 5).ToLower() == "value")
                            {
                                queryString = "select * from " + ppText + " where " + p1Text + "=" + ntext.Replace("Value=", "");
                            }
                            else
                            {
                                queryString = "select " + p1Text + " from " + ppText;// + " where " + n.Text;
                            }
                            break;
                        case "RootNode":
                            if (treeView1.Nodes == null || treeView1.Nodes.Count == 0) treeView1.Nodes.Add("Query");
                            System.Windows.Forms.TreeNode root = treeView1.Nodes[0];
                            queryString = root.Text;
                            break;
                        default:
                            if (nodeType != null && nodeType != "") System.Windows.Forms.MessageBox.Show(nodeType);
                            break;
                    }
                    this.QueryTextBox.Text = queryString;

                    //Clipboard.SetDataObject(queryString);
                    //System.Windows.Forms.MessageBox.Show("\""+queryString + "\" was copied to the clipboard.");
                }
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            StilRunning = false;
            this.QueryButton.Enabled = true;
            this.StopButton.Enabled = false;

        }

        private void Form1_Load(object sender, System.EventArgs e)
        {
            this.QueryTextBox.Items.Clear();
            this.QueryTextBox.Text = "select * from CIM_System";
            this.LoadHistory(null);
            BasicTreemenuItem_Click(null, null);
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.SaveHistory();
        }

        private void ExitmenuItem_Click(object sender, System.EventArgs e)
        {
            this.SaveHistory();
            Application.Exit();

        }

        private void SavemenuItem_Click(object sender, System.EventArgs e)
        {
            SaveHistory();
            System.Windows.Forms.MessageBox.Show("History Saved.");
        }

        private void ClearmenuItem_Click(object sender, System.EventArgs e)
        {
            this.QueryTextBox.Items.Clear();
            this.history = new ArrayList();
            System.IO.File.Delete(this.HistoryPath);
            System.Windows.Forms.MessageBox.Show("History Cleared.");
        }

        private void LoadmenuItem_Click(object sender, System.EventArgs e)
        {
            this.openFileDialog1.CheckFileExists = true;
            this.openFileDialog1.CheckPathExists = true;
            this.openFileDialog1.DefaultExt = "*.txt";
            this.openFileDialog1.InitialDirectory = Application.StartupPath;
            this.openFileDialog1.Multiselect = false;
            this.openFileDialog1.Title = "Locate History File...";
            System.Windows.Forms.DialogResult result = this.openFileDialog1.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                this.LoadHistory(this.openFileDialog1.FileName);

            }
        }

        public string Username = "";
        public string Password = "";
        public string Computer = "";

        private void LoginMenuItem_Click(object sender, System.EventArgs e)
        {
            LoginForm frm = new LoginForm();
            frm.UsernameTextBox.Text = this.Username;
            frm.PasswordTextBox.Text = this.Password;
            frm.MachineNameTextBox.Text = this.Computer;

            frm.ShowDialog(this);
            if (!frm.Cancelled)
            {
                this.Username = frm.UsernameTextBox.Text;
                this.Password = frm.PasswordTextBox.Text;
                this.Computer = frm.MachineNameTextBox.Text;
            }

        }

        private void LoadNode(System.Xml.XmlNode n, System.Windows.Forms.TreeNode tn)
        {
            if (n != null)
            {

                string nText;
                string nAlt;
                string nType;
                try
                {
                    nText = n.Attributes["Text"].Value;
                    this.IncrementBar();
                    Application.DoEvents();

                    try { nAlt = n.Attributes["Alt"].Value; }
                    catch (Exception exc) { Terminals.Logging.Log.Info("", exc); nAlt = nText; }
                    try { nType = n.Attributes["Type"].Value; }
                    catch (Exception exc) { Terminals.Logging.Log.Info("", exc); nType = nAlt; }

                    if (nAlt == "" || nAlt == null)
                    {
                        tn.Text = nText;
                    }
                    else
                    {
                        tn.Text = nText + " (" + nAlt + ")";
                    }
                    tn.Tag = nType;
                    if (n.HasChildNodes)
                    {
                        foreach (System.Xml.XmlNode newXmlNode in n.ChildNodes)
                        {
                            System.Windows.Forms.TreeNode child = new TreeNode();
                            LoadNode(newXmlNode, child);
                            tn.Nodes.Add(child);
                        }
                    }
                }
                catch (Exception ee)
                {
                    Terminals.Logging.Log.Info("", ee);
                    ee.ToString();
                }
            }

        }
        private void BasicTreemenuItem_Click(object sender, System.EventArgs e)
        {
            //string path = Application.StartupPath + "\\BasicTree.xml";
            //System.IO.StreamReader rdr = new System.IO.StreamReader(path);
            //string xml = rdr.ReadToEnd();
            //rdr.Close();

            LoadBasicTree(Terminals.Properties.Resources.BasicTree);
        }

        private void LoadBasicTree(string xml)
        {

            tabControl1.SelectedIndex = 1;

            this.progressBar1.Value = 0;

            this.IncrementBar();
            treeView2.Nodes.Clear();
            Application.DoEvents();
            this.IncrementBar();
            Application.DoEvents();

            this.IncrementBar();
            Application.DoEvents();
            System.Xml.XmlDocument x = new System.Xml.XmlDocument();
            try
            {
                x.LoadXml(xml);
            }
            catch (Exception xexc)
            {
                Terminals.Logging.Log.Info("", xexc);
                xexc.ToString();
            }
            System.Xml.XmlNode n = x.SelectSingleNode("/tree");
            System.Windows.Forms.TreeNode root = new System.Windows.Forms.TreeNode();
            this.treeView2.Nodes.Add(root);
            Application.DoEvents();

            LoadNode(n, root);


            this.progressBar1.Value = 0;
        }

        private void treeView2_DoubleClick(object sender, System.EventArgs e)
        {

            if (this.treeView2.Nodes[0].Text == "To load-> Double Click")
            {
                BasicTreemenuItem_Click(null, null);
            }
            else
            {
                treeView1.Nodes.Clear();
                tabControl1.SelectedIndex = 0;
                Application.DoEvents();
                treeView1_DoubleClick(treeView2, null);
                Application.DoEvents();
                QueryButton_Click(null, null);
                Application.DoEvents();
            }

        }

        private void tabControl1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            /*
                        if(tabControl1.SelectedTab.Name==processesTab.Name) {
                System.Diagnostics.Process[] procs = System.Diagnostics.Process.GetProcesses();
                treeView3.Nodes.Clear();
                foreach(System.Diagnostics.Process p in procs) {
                    treeView3.Nodes.Add(new System.Windows.Forms.TreeNode(p.ProcessName));
                    Application.DoEvents();          
                }
            }

   
        */
        }
        private void treeView2_Click(object sender, System.EventArgs e)
        {
            //System.Windows.Forms.TreeNode rootNode = treeView2.SelectedNode;
            //if (rootNode != null)
            //{
            //    if (rootNode.Nodes == null || rootNode.Nodes.Count <= 0)
            //    {
            //        if (rootNode.Tag != null && Convert.ToString(rootNode.Tag) == "ClassNode")
            //        {
            //            string ntext = rootNode.Text;
            //            if (ntext.IndexOf("(") > 0) ntext = ntext.Substring(0, ntext.IndexOf("(") - 1).Trim();
            //            string qry = "select * from " + ntext;
            //            Application.DoEvents();
            //            System.Management.ManagementObjectSearcher searcher;
            //            System.Management.ObjectQuery query = new System.Management.ObjectQuery(qry);

            //            if (Username != "" && Password != "" && Computer != "")
            //            {
            //                System.Management.ConnectionOptions oConn = new System.Management.ConnectionOptions();
            //                oConn.Username = Username;
            //                oConn.Password = Password;
            //                System.Management.ManagementScope oMs = new System.Management.ManagementScope(Computer, oConn);
            //                searcher = new System.Management.ManagementObjectSearcher(oMs, query);
            //            }
            //            else
            //            {
            //                searcher = new System.Management.ManagementObjectSearcher(query);
            //            }
            //            System.Windows.Forms.TreeNode root = new TreeNode(qry);
            //            root.Tag = "RootNode";
            //            rootNode.Nodes.Add(root);
            //            root.Expand();
            //            rootNode.Expand();
            //            string path = "";
            //            Application.DoEvents();

            //            foreach (System.Management.ManagementObject share in searcher.Get())
            //            {
            //                System.Windows.Forms.TreeNode item = new TreeNode(share.ClassPath.ClassName);
            //                item.Tag = "ClassNode";
            //                root.Nodes.Add(item);
            //                path = "Enumerating:" + share.ClassPath.ClassName;
            //                foreach (System.Management.PropertyData p in share.Properties)
            //                {
            //                    bool isLocal = p.IsLocal;
            //                    string type = p.Type.ToString();
            //                    string origin = p.Origin;
            //                    string name = p.Name;
            //                    path = "Enumerating:" + share.ClassPath.ClassName + "," + name;
            //                    Application.DoEvents();

            //                    bool IsArray = p.IsArray;
            //                    string val = "NULL";
            //                    if (p.Value != null) val = p.Value.ToString();
            //                    System.Windows.Forms.TreeNode node = new TreeNode(name);
            //                    node.Tag = "PropertyNode";
            //                    string display = "";
            //                    if (type.ToLower() == "string")
            //                    {
            //                        display = "Value='" + val + "'";
            //                    }
            //                    else
            //                    {
            //                        display = "Value=" + val;
            //                    }
            //                    System.Windows.Forms.TreeNode ValueNode = new TreeNode(display);
            //                    ValueNode.Tag = "ValueNode";
            //                    System.Windows.Forms.TreeNode TypeNode = new TreeNode("Type='" + type + "'");
            //                    TypeNode.Tag = "ValueNode";
            //                    System.Windows.Forms.TreeNode localNode = new TreeNode("IsLocal=" + isLocal);
            //                    localNode.Tag = "ValueNode";
            //                    System.Windows.Forms.TreeNode OriginNode = new TreeNode("Origin='" + origin + "'");
            //                    OriginNode.Tag = "ValueNode";
            //                    System.Windows.Forms.TreeNode IsArrayNode = new TreeNode("IsArray=" + IsArray);
            //                    IsArrayNode.Tag = "ValueNode";

            //                    node.Nodes.Add(ValueNode);
            //                    node.Nodes.Add(TypeNode);
            //                    node.Nodes.Add(localNode);
            //                    node.Nodes.Add(OriginNode);
            //                    node.Nodes.Add(IsArrayNode);
            //                    Application.DoEvents();
            //                    if (IsArray && p.Value != null)
            //                    {
            //                        System.Array a = (System.Array)p.Value;
            //                        for (int x = 0; x < a.Length; x++)
            //                        {
            //                            string v = "";
            //                            if (a.GetValue(x) != null) v = a.GetValue(x).ToString();
            //                            System.Windows.Forms.TreeNode arrayNode = new TreeNode(name + "[" + x + "]=" + v);
            //                            arrayNode.Tag = "ArrayNode";
            //                            IsArrayNode.Nodes.Add(arrayNode);
            //                            IncrementBar();
            //                        }
            //                    }
            //                    IncrementBar();

            //                    item.Nodes.Add(node);
            //                    Application.DoEvents();
            //                    if (!StilRunning) break;

            //                }
            //                if (!StilRunning) break;
            //            }
            //        }
            //    }
            //}
        }

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            this.LoginMenuItem_Click(null, null);
            ConnectionLabel.Text = this.Computer;
        }

    }



}