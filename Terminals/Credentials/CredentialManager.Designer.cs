namespace Terminals.Credentials
{
    partial class CredentialManager
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CredentialManager));
            this.AddButton = new System.Windows.Forms.Button();
            this.EditButton = new System.Windows.Forms.Button();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.DoneButton = new System.Windows.Forms.Button();
            this.gridCredentials = new Terminals.SortableUnboundGrid();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDomain = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCredentials)).BeginInit();
            this.SuspendLayout();
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(12, 12);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(75, 23);
            this.AddButton.TabIndex = 1;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // EditButton
            // 
            this.EditButton.Location = new System.Drawing.Point(12, 41);
            this.EditButton.Name = "EditButton";
            this.EditButton.Size = new System.Drawing.Size(75, 23);
            this.EditButton.TabIndex = 2;
            this.EditButton.Text = "Edit";
            this.EditButton.UseVisualStyleBackColor = true;
            this.EditButton.Click += new System.EventHandler(this.EditButton_Click);
            // 
            // DeleteButton
            // 
            this.DeleteButton.Location = new System.Drawing.Point(12, 70);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(75, 23);
            this.DeleteButton.TabIndex = 3;
            this.DeleteButton.Text = "Delete";
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.DoneButton);
            this.panel1.Controls.Add(this.AddButton);
            this.panel1.Controls.Add(this.DeleteButton);
            this.panel1.Controls.Add(this.EditButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(352, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(100, 213);
            this.panel1.TabIndex = 4;
            // 
            // DoneButton
            // 
            this.DoneButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DoneButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.DoneButton.Location = new System.Drawing.Point(12, 180);
            this.DoneButton.Name = "DoneButton";
            this.DoneButton.Size = new System.Drawing.Size(75, 23);
            this.DoneButton.TabIndex = 4;
            this.DoneButton.Text = "Close";
            this.DoneButton.UseVisualStyleBackColor = true;
            this.DoneButton.Click += new System.EventHandler(this.DoneButton_Click);
            // 
            // gridCredentials
            // 
            this.gridCredentials.AllowUserToAddRows = false;
            this.gridCredentials.AllowUserToOrderColumns = true;
            this.gridCredentials.AllowUserToResizeRows = false;
            this.gridCredentials.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridCredentials.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridCredentials.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colUserName,
            this.colDomain});
            this.gridCredentials.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCredentials.GridColor = System.Drawing.SystemColors.Control;
            this.gridCredentials.Location = new System.Drawing.Point(0, 0);
            this.gridCredentials.Name = "gridCredentials";
            this.gridCredentials.RowHeadersVisible = false;
            this.gridCredentials.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCredentials.Size = new System.Drawing.Size(352, 213);
            this.gridCredentials.TabIndex = 5;
            this.gridCredentials.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.gridCredentials_CellDoubleClick);
            this.gridCredentials.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gridCredentials_ColumnHeaderMouseClick);
            // 
            // colName
            // 
            this.colName.DataPropertyName = "Name";
            this.colName.HeaderText = "Name";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colUserName
            // 
            this.colUserName.DataPropertyName = "UserName";
            this.colUserName.HeaderText = "User name";
            this.colUserName.Name = "colUserName";
            this.colUserName.ReadOnly = true;
            // 
            // colDomain
            // 
            this.colDomain.DataPropertyName = "Domain";
            this.colDomain.HeaderText = "Domain";
            this.colDomain.Name = "colDomain";
            this.colDomain.ReadOnly = true;
            // 
            // CredentialManager
            // 
            this.AcceptButton = this.DoneButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.DoneButton;
            this.ClientSize = new System.Drawing.Size(452, 213);
            this.Controls.Add(this.gridCredentials);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(460, 240);
            this.Name = "CredentialManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Terminals - Credential Manager";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CredentialManager_FormClosed);
            this.Load += new System.EventHandler(this.CredentialManager_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCredentials)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button EditButton;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button DoneButton;
        private Terminals.SortableUnboundGrid gridCredentials;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDomain;

    }
}