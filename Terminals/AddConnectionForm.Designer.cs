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
            this.gbFavorites = new System.Windows.Forms.GroupBox();
            this.lvFavorites = new System.Windows.Forms.ListView();
            this.chFavoriteName = new System.Windows.Forms.ColumnHeader();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbFavorites.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbFavorites
            // 
            this.gbFavorites.Controls.Add(this.lvFavorites);
            this.gbFavorites.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbFavorites.Location = new System.Drawing.Point(0, 0);
            this.gbFavorites.Name = "gbFavorites";
            this.gbFavorites.Size = new System.Drawing.Size(221, 240);
            this.gbFavorites.TabIndex = 0;
            this.gbFavorites.TabStop = false;
            this.gbFavorites.Text = "Favorites";
            // 
            // lvFavorites
            // 
            this.lvFavorites.CheckBoxes = true;
            this.lvFavorites.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chFavoriteName});
            this.lvFavorites.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvFavorites.FullRowSelect = true;
            this.lvFavorites.Location = new System.Drawing.Point(3, 17);
            this.lvFavorites.Name = "lvFavorites";
            this.lvFavorites.Size = new System.Drawing.Size(215, 220);
            this.lvFavorites.TabIndex = 1;
            this.lvFavorites.UseCompatibleStateImageBehavior = false;
            this.lvFavorites.View = System.Windows.Forms.View.Details;
            this.lvFavorites.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvFavorites_ItemChecked);
            // 
            // chFavoriteName
            // 
            this.chFavoriteName.Text = "Favorite name";
            this.chFavoriteName.Width = 186;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 240);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(221, 47);
            this.panel1.TabIndex = 1;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Enabled = false;
            this.btnOk.Location = new System.Drawing.Point(56, 11);
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
            this.btnCancel.Location = new System.Drawing.Point(136, 11);
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
            this.ClientSize = new System.Drawing.Size(221, 287);
            this.Controls.Add(this.gbFavorites);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddConnectionForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Connection";
            this.gbFavorites.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbFavorites;
        private System.Windows.Forms.ListView lvFavorites;
        private System.Windows.Forms.ColumnHeader chFavoriteName;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
    }
}