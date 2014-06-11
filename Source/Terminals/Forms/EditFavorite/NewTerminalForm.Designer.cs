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
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
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
            this.btnSaveDefault = new Terminals.Forms.Controls.SplitButton();
            this.btnSave = new Terminals.Forms.Controls.SplitButton();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.favoritePropertiesControl1 = new Terminals.Forms.EditFavorite.FavoritePropertiesControl();
            this.contextMenuStripDefaults.SuspendLayout();
            this.contextMenuStripSave.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(620, 39);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(72, 27);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
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
            this.groupBox7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox7.Location = new System.Drawing.Point(0, 396);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(704, 78);
            this.groupBox7.TabIndex = 10;
            this.groupBox7.TabStop = false;
            // 
            // btnSaveDefault
            // 
            this.btnSaveDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveDefault.AutoSize = true;
            this.btnSaveDefault.ContextMenuStrip = this.contextMenuStripDefaults;
            this.btnSaveDefault.Location = new System.Drawing.Point(400, 39);
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
            this.btnSave.Location = new System.Drawing.Point(494, 39);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 27);
            this.btnSave.SplitMenuStrip = this.contextMenuStripSave;
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save && Close";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // favoritePropertiesControl1
            // 
            this.favoritePropertiesControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.favoritePropertiesControl1.Location = new System.Drawing.Point(0, 0);
            this.favoritePropertiesControl1.Name = "favoritePropertiesControl1";
            this.favoritePropertiesControl1.Size = new System.Drawing.Size(704, 396);
            this.favoritePropertiesControl1.TabIndex = 11;
            // 
            // NewTerminalFormCopy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(704, 474);
            this.Controls.Add(this.favoritePropertiesControl1);
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
            this.contextMenuStripDefaults.ResumeLayout(false);
            this.contextMenuStripSave.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolTip toolTip1;

        //private System.Windows.Forms.GroupBox groupBox2;
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
        private System.Windows.Forms.ErrorProvider errorProvider;
        private Forms.EditFavorite.FavoritePropertiesControl favoritePropertiesControl1;
    }
}
