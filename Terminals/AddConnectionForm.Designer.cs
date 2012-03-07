namespace Terminals
{
    partial class AddConnectionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddConnectionForm));
            this.gbFavorites = new System.Windows.Forms.GroupBox();
            this.gridFavorites = new System.Windows.Forms.DataGridView();
            this.colFavoriteName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbFavorites.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFavorites)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbFavorites
            // 
            this.gbFavorites.Controls.Add(this.gridFavorites);
            this.gbFavorites.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbFavorites.Location = new System.Drawing.Point(0, 0);
            this.gbFavorites.Name = "gbFavorites";
            this.gbFavorites.Size = new System.Drawing.Size(349, 315);
            this.gbFavorites.TabIndex = 0;
            this.gbFavorites.TabStop = false;
            this.gbFavorites.Text = "Favorites";
            // 
            // gridFavorites
            // 
            this.gridFavorites.AllowUserToAddRows = false;
            this.gridFavorites.AllowUserToDeleteRows = false;
            this.gridFavorites.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridFavorites.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFavorites.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFavoriteName});
            this.gridFavorites.Location = new System.Drawing.Point(7, 21);
            this.gridFavorites.Name = "gridFavorites";
            this.gridFavorites.ReadOnly = true;
            this.gridFavorites.RowHeadersVisible = false;
            this.gridFavorites.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridFavorites.Size = new System.Drawing.Size(336, 288);
            this.gridFavorites.TabIndex = 0;
            this.gridFavorites.SelectionChanged += new System.EventHandler(this.gridFavorites_SelectionChanged);
            // 
            // colFavoriteName
            // 
            this.colFavoriteName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colFavoriteName.DataPropertyName = "Name";
            this.colFavoriteName.HeaderText = "Favorite name";
            this.colFavoriteName.Name = "colFavoriteName";
            this.colFavoriteName.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 315);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(349, 47);
            this.panel1.TabIndex = 1;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Enabled = false;
            this.btnOk.Location = new System.Drawing.Point(184, 11);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 25);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(264, 11);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // AddConnectionForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(349, 362);
            this.Controls.Add(this.gbFavorites);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddConnectionForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select favorite to add";
            this.gbFavorites.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridFavorites)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbFavorites;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridView gridFavorites;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFavoriteName;
    }
}