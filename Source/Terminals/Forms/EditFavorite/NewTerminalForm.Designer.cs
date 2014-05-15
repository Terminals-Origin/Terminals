using Terminals.TerminalServices;

namespace Terminals
{
    partial class NewTerminalFormCopy
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewTerminalFormCopy));
            this.btnCancel = new System.Windows.Forms.Button();
            this.chkAddtoToolbar = new System.Windows.Forms.CheckBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.NewWindowCheckbox = new System.Windows.Forms.CheckBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label43 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.contextMenuStripDefaults = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveCurrentSettingsAsDefaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeSavedDefaultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripSave = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveConnectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveNewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveCopyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.ExecuteTabPage = new System.Windows.Forms.TabPage();
            this.TagsTabPage = new System.Windows.Forms.TabPage();
            this.RAStabPage = new System.Windows.Forms.TabPage();
            this.ICAtabPage = new System.Windows.Forms.TabPage();
            this.VMRCtabPage = new System.Windows.Forms.TabPage();
            this.SSHTabPage = new System.Windows.Forms.TabPage();
            this.ConsoleTabPage = new System.Windows.Forms.TabPage();
            this.VNCTabPage = new System.Windows.Forms.TabPage();
            this.RDPTabPage = new System.Windows.Forms.TabPage();
            this.GeneralTabPage = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnSaveDefault = new Terminals.Forms.Controls.SplitButton();
            this.btnSave = new Terminals.Forms.Controls.SplitButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.contextMenuStripDefaults.SuspendLayout();
            this.contextMenuStripSave.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tabControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(605, 39);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 27);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // chkAddtoToolbar
            // 
            this.chkAddtoToolbar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkAddtoToolbar.AutoSize = true;
            this.chkAddtoToolbar.Location = new System.Drawing.Point(21, 49);
            this.chkAddtoToolbar.Name = "chkAddtoToolbar";
            this.chkAddtoToolbar.Size = new System.Drawing.Size(97, 17);
            this.chkAddtoToolbar.TabIndex = 3;
            this.chkAddtoToolbar.Text = "Add to &Toolbar";
            this.chkAddtoToolbar.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = global::Terminals.Properties.Resources.terminalsbanner_left;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(84, 396);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // NewWindowCheckbox
            // 
            this.NewWindowCheckbox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NewWindowCheckbox.AutoSize = true;
            this.NewWindowCheckbox.Location = new System.Drawing.Point(21, 22);
            this.NewWindowCheckbox.Name = "NewWindowCheckbox";
            this.NewWindowCheckbox.Size = new System.Drawing.Size(128, 17);
            this.NewWindowCheckbox.TabIndex = 4;
            this.NewWindowCheckbox.Text = "&Open in New Window";
            this.NewWindowCheckbox.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(154, 132);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(160, 20);
            this.textBox3.TabIndex = 10;
            this.textBox3.Text = "Black";
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Location = new System.Drawing.Point(101, 108);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(31, 13);
            this.label43.TabIndex = 8;
            this.label43.Text = "Font:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(320, 130);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(31, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // contextMenuStripDefaults
            // 
            this.contextMenuStripDefaults.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contextMenuStripDefaults.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveCurrentSettingsAsDefaultToolStripMenuItem,
            this.removeSavedDefaultsToolStripMenuItem});
            this.contextMenuStripDefaults.Name = "contextMenuStripDefaults";
            this.contextMenuStripDefaults.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStripDefaults.ShowImageMargin = false;
            this.contextMenuStripDefaults.Size = new System.Drawing.Size(208, 48);
            // 
            // saveCurrentSettingsAsDefaultToolStripMenuItem
            // 
            this.saveCurrentSettingsAsDefaultToolStripMenuItem.Name = "saveCurrentSettingsAsDefaultToolStripMenuItem";
            this.saveCurrentSettingsAsDefaultToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.saveCurrentSettingsAsDefaultToolStripMenuItem.Text = "Save Current Settings as Default";
            this.saveCurrentSettingsAsDefaultToolStripMenuItem.Click += new System.EventHandler(this.SaveCurrentSettingsAsDefaultToolStripMenuItem_Click);
            // 
            // removeSavedDefaultsToolStripMenuItem
            // 
            this.removeSavedDefaultsToolStripMenuItem.Name = "removeSavedDefaultsToolStripMenuItem";
            this.removeSavedDefaultsToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.removeSavedDefaultsToolStripMenuItem.Text = "Remove Saved Defaults";
            this.removeSavedDefaultsToolStripMenuItem.Click += new System.EventHandler(this.RemoveSavedDefaultsToolStripMenuItem_Click);
            // 
            // contextMenuStripSave
            // 
            this.contextMenuStripSave.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contextMenuStripSave.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveConnectToolStripMenuItem,
            this.saveNewToolStripMenuItem,
            this.saveCopyToolStripMenuItem});
            this.contextMenuStripSave.Name = "contextMenuStripSave";
            this.contextMenuStripSave.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.contextMenuStripSave.Size = new System.Drawing.Size(152, 70);
            // 
            // saveConnectToolStripMenuItem
            // 
            this.saveConnectToolStripMenuItem.Name = "saveConnectToolStripMenuItem";
            this.saveConnectToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.saveConnectToolStripMenuItem.Text = "Save && Connect";
            this.saveConnectToolStripMenuItem.Click += new System.EventHandler(this.SaveConnectToolStripMenuItem_Click);
            // 
            // saveNewToolStripMenuItem
            // 
            this.saveNewToolStripMenuItem.Name = "saveNewToolStripMenuItem";
            this.saveNewToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.saveNewToolStripMenuItem.Text = "Save && New";
            this.saveNewToolStripMenuItem.Click += new System.EventHandler(this.SaveNewToolStripMenuItem_Click);
            // 
            // saveCopyToolStripMenuItem
            // 
            this.saveCopyToolStripMenuItem.Name = "saveCopyToolStripMenuItem";
            this.saveCopyToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.saveCopyToolStripMenuItem.Text = "Save && Copy";
            this.saveCopyToolStripMenuItem.Click += new System.EventHandler(this.SaveCopyToolStripMenuItem_Click);
            // 
            // groupBox7
            // 
            this.groupBox7.BackColor = System.Drawing.Color.White;
            this.groupBox7.Controls.Add(this.btnSaveDefault);
            this.groupBox7.Controls.Add(this.btnSave);
            this.groupBox7.Controls.Add(this.btnCancel);
            this.groupBox7.Controls.Add(this.chkAddtoToolbar);
            this.groupBox7.Controls.Add(this.NewWindowCheckbox);
            this.groupBox7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox7.Location = new System.Drawing.Point(0, 396);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(689, 78);
            this.groupBox7.TabIndex = 10;
            this.groupBox7.TabStop = false;
            // 
            // ExecuteTabPage
            // 
            this.ExecuteTabPage.Location = new System.Drawing.Point(4, 22);
            this.ExecuteTabPage.Name = "ExecuteTabPage";
            this.ExecuteTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ExecuteTabPage.Size = new System.Drawing.Size(597, 370);
            this.ExecuteTabPage.TabIndex = 3;
            this.ExecuteTabPage.Text = "Execute";
            this.ExecuteTabPage.UseVisualStyleBackColor = true;
            // 
            // TagsTabPage
            // 
            this.TagsTabPage.Location = new System.Drawing.Point(4, 22);
            this.TagsTabPage.Name = "TagsTabPage";
            this.TagsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.TagsTabPage.Size = new System.Drawing.Size(597, 370);
            this.TagsTabPage.TabIndex = 4;
            this.TagsTabPage.Text = "Groups";
            this.TagsTabPage.UseVisualStyleBackColor = true;
            // 
            // RAStabPage
            // 
            this.RAStabPage.Location = new System.Drawing.Point(4, 22);
            this.RAStabPage.Name = "RAStabPage";
            this.RAStabPage.Padding = new System.Windows.Forms.Padding(3);
            this.RAStabPage.Size = new System.Drawing.Size(597, 370);
            this.RAStabPage.TabIndex = 9;
            this.RAStabPage.Text = "RAS";
            this.RAStabPage.UseVisualStyleBackColor = true;
            // 
            // ICAtabPage
            // 
            this.ICAtabPage.Location = new System.Drawing.Point(4, 22);
            this.ICAtabPage.Name = "ICAtabPage";
            this.ICAtabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ICAtabPage.Size = new System.Drawing.Size(597, 370);
            this.ICAtabPage.TabIndex = 10;
            this.ICAtabPage.Text = "Citrix";
            this.ICAtabPage.UseVisualStyleBackColor = true;
            // 
            // VMRCtabPage
            // 
            this.VMRCtabPage.Location = new System.Drawing.Point(4, 22);
            this.VMRCtabPage.Name = "VMRCtabPage";
            this.VMRCtabPage.Padding = new System.Windows.Forms.Padding(3);
            this.VMRCtabPage.Size = new System.Drawing.Size(597, 370);
            this.VMRCtabPage.TabIndex = 7;
            this.VMRCtabPage.Text = "VMRC";
            this.VMRCtabPage.UseVisualStyleBackColor = true;
            // 
            // SSHTabPage
            // 
            this.SSHTabPage.Location = new System.Drawing.Point(4, 22);
            this.SSHTabPage.Name = "SSHTabPage";
            this.SSHTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.SSHTabPage.Size = new System.Drawing.Size(597, 370);
            this.SSHTabPage.TabIndex = 13;
            this.SSHTabPage.Text = "SSH";
            this.SSHTabPage.UseVisualStyleBackColor = true;
            // 
            // ConsoleTabPage
            // 
            this.ConsoleTabPage.Location = new System.Drawing.Point(4, 22);
            this.ConsoleTabPage.Name = "ConsoleTabPage";
            this.ConsoleTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ConsoleTabPage.Size = new System.Drawing.Size(597, 370);
            this.ConsoleTabPage.TabIndex = 14;
            this.ConsoleTabPage.Text = "Console";
            this.ConsoleTabPage.UseVisualStyleBackColor = true;
            // 
            // VNCTabPage
            // 
            this.VNCTabPage.Location = new System.Drawing.Point(4, 22);
            this.VNCTabPage.Name = "VNCTabPage";
            this.VNCTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.VNCTabPage.Size = new System.Drawing.Size(597, 370);
            this.VNCTabPage.TabIndex = 12;
            this.VNCTabPage.Text = "VNC";
            this.VNCTabPage.UseVisualStyleBackColor = true;
            // 
            // RDPTabPage
            // 
            this.RDPTabPage.Location = new System.Drawing.Point(4, 22);
            this.RDPTabPage.Name = "RDPTabPage";
            this.RDPTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.RDPTabPage.Size = new System.Drawing.Size(597, 370);
            this.RDPTabPage.TabIndex = 1;
            this.RDPTabPage.Text = "RDP";
            this.RDPTabPage.UseVisualStyleBackColor = true;
            // 
            // GeneralTabPage
            // 
            this.GeneralTabPage.Location = new System.Drawing.Point(4, 22);
            this.GeneralTabPage.Name = "GeneralTabPage";
            this.GeneralTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.GeneralTabPage.Size = new System.Drawing.Size(597, 370);
            this.GeneralTabPage.TabIndex = 0;
            this.GeneralTabPage.Text = "General";
            this.GeneralTabPage.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.GeneralTabPage);
            this.tabControl1.Controls.Add(this.RDPTabPage);
            this.tabControl1.Controls.Add(this.VNCTabPage);
            this.tabControl1.Controls.Add(this.ConsoleTabPage);
            this.tabControl1.Controls.Add(this.SSHTabPage);
            this.tabControl1.Controls.Add(this.VMRCtabPage);
            this.tabControl1.Controls.Add(this.ICAtabPage);
            this.tabControl1.Controls.Add(this.RAStabPage);
            this.tabControl1.Controls.Add(this.TagsTabPage);
            this.tabControl1.Controls.Add(this.ExecuteTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(84, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(605, 396);
            this.tabControl1.TabIndex = 0;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // btnSaveDefault
            // 
            this.btnSaveDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveDefault.AutoSize = true;
            this.btnSaveDefault.ContextMenuStrip = this.contextMenuStripDefaults;
            this.btnSaveDefault.Location = new System.Drawing.Point(385, 39);
            this.btnSaveDefault.Name = "btnSaveDefault";
            this.btnSaveDefault.Size = new System.Drawing.Size(88, 27);
            this.btnSaveDefault.SplitMenuStrip = this.contextMenuStripDefaults;
            this.btnSaveDefault.TabIndex = 5;
            this.btnSaveDefault.Text = "Defaults";
            this.btnSaveDefault.UseVisualStyleBackColor = true;
            this.btnSaveDefault.Click += new System.EventHandler(this.BtnSaveDefault_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.AutoSize = true;
            this.btnSave.ContextMenuStrip = this.contextMenuStripSave;
            this.btnSave.Location = new System.Drawing.Point(479, 39);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 27);
            this.btnSave.SplitMenuStrip = this.contextMenuStripSave;
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save && Close";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // NewTerminalFormCopy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(689, 474);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.groupBox7);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewTerminalFormCopy";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "New Connection";
            this.Load += new System.EventHandler(this.NewTerminalForm_Load);
            this.Shown += new System.EventHandler(this.NewTerminalForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.contextMenuStripDefaults.ResumeLayout(false);
            this.contextMenuStripSave.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox chkAddtoToolbar;
        private System.Windows.Forms.ToolTip toolTip1;

        //private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox NewWindowCheckbox;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Button button1;
        private Terminals.Forms.Controls.SplitButton btnSaveDefault;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripDefaults;
        private System.Windows.Forms.ToolStripMenuItem saveCurrentSettingsAsDefaultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeSavedDefaultsToolStripMenuItem;
        private Forms.Controls.SplitButton btnSave;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripSave;
        private System.Windows.Forms.ToolStripMenuItem saveConnectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveNewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveCopyToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TabPage ExecuteTabPage;
        private System.Windows.Forms.TabPage TagsTabPage;
        private System.Windows.Forms.TabPage RAStabPage;
        private System.Windows.Forms.TabPage ICAtabPage;
        private System.Windows.Forms.TabPage VMRCtabPage;
        private System.Windows.Forms.TabPage SSHTabPage;
        private System.Windows.Forms.TabPage ConsoleTabPage;
        private System.Windows.Forms.TabPage VNCTabPage;
        private System.Windows.Forms.TabPage RDPTabPage;
        private System.Windows.Forms.TabPage GeneralTabPage;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}
