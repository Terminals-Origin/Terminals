namespace Terminals
{
    partial class OrganizeFavoritesToolbarForm
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
            this.lvFavoriteButtons = new System.Windows.Forms.ListView();
            this.chName = new System.Windows.Forms.ColumnHeader();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.tsTop = new System.Windows.Forms.ToolStrip();
            this.tsbMoveToFirst = new System.Windows.Forms.ToolStripButton();
            this.tsbMoveUp = new System.Windows.Forms.ToolStripButton();
            this.tsbMoveDown = new System.Windows.Forms.ToolStripButton();
            this.tsbMoveToLast = new System.Windows.Forms.ToolStripButton();
            this.tsTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvFavoriteButtons
            // 
            this.lvFavoriteButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lvFavoriteButtons.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName});
            this.lvFavoriteButtons.FullRowSelect = true;
            this.lvFavoriteButtons.HideSelection = false;
            this.lvFavoriteButtons.Location = new System.Drawing.Point(8, 32);
            this.lvFavoriteButtons.MultiSelect = false;
            this.lvFavoriteButtons.Name = "lvFavoriteButtons";
            this.lvFavoriteButtons.Size = new System.Drawing.Size(254, 177);
            this.lvFavoriteButtons.TabIndex = 0;
            this.lvFavoriteButtons.UseCompatibleStateImageBehavior = false;
            this.lvFavoriteButtons.View = System.Windows.Forms.View.Details;
            this.lvFavoriteButtons.SelectedIndexChanged += new System.EventHandler(this.lvFavoriteButtons_SelectedIndexChanged);
            // 
            // chName
            // 
            this.chName.Text = "Name";
            this.chName.Width = 221;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(182, 217);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(94, 217);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // tsTop
            // 
            this.tsTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbMoveToFirst,
            this.tsbMoveUp,
            this.tsbMoveDown,
            this.tsbMoveToLast});
            this.tsTop.Location = new System.Drawing.Point(0, 0);
            this.tsTop.Name = "tsTop";
            this.tsTop.Size = new System.Drawing.Size(271, 25);
            this.tsTop.TabIndex = 7;
            this.tsTop.Text = "toolStrip1";
            // 
            // tsbMoveToFirst
            // 
            this.tsbMoveToFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbMoveToFirst.Enabled = false;
            this.tsbMoveToFirst.Image = global::Terminals.Properties.Resources.application_get;
            this.tsbMoveToFirst.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMoveToFirst.Name = "tsbMoveToFirst";
            this.tsbMoveToFirst.Size = new System.Drawing.Size(23, 22);
            this.tsbMoveToFirst.ToolTipText = "Move To First";
            this.tsbMoveToFirst.Click += new System.EventHandler(this.tsbMoveToFirst_Click);
            // 
            // tsbMoveUp
            // 
            this.tsbMoveUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbMoveUp.Enabled = false;
            this.tsbMoveUp.Image = global::Terminals.Properties.Resources.arrow_up;
            this.tsbMoveUp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMoveUp.Name = "tsbMoveUp";
            this.tsbMoveUp.Size = new System.Drawing.Size(23, 22);
            this.tsbMoveUp.Text = "toolStripButton2";
            this.tsbMoveUp.ToolTipText = "Move Up";
            this.tsbMoveUp.Click += new System.EventHandler(this.tsbMoveUp_Click);
            // 
            // tsbMoveDown
            // 
            this.tsbMoveDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbMoveDown.Enabled = false;
            this.tsbMoveDown.Image = global::Terminals.Properties.Resources.arrow_down;
            this.tsbMoveDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMoveDown.Name = "tsbMoveDown";
            this.tsbMoveDown.Size = new System.Drawing.Size(23, 22);
            this.tsbMoveDown.Text = "toolStripButton3";
            this.tsbMoveDown.ToolTipText = "Move Down";
            this.tsbMoveDown.Click += new System.EventHandler(this.tsbMoveDown_Click);
            // 
            // tsbMoveToLast
            // 
            this.tsbMoveToLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbMoveToLast.Enabled = false;
            this.tsbMoveToLast.Image = global::Terminals.Properties.Resources.application_put;
            this.tsbMoveToLast.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMoveToLast.Name = "tsbMoveToLast";
            this.tsbMoveToLast.Size = new System.Drawing.Size(23, 22);
            this.tsbMoveToLast.Text = "toolStripButton4";
            this.tsbMoveToLast.ToolTipText = "Move To Last";
            this.tsbMoveToLast.Click += new System.EventHandler(this.tsbMoveToLast_Click);
            // 
            // OrganizeFavoritesToolbarForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(271, 251);
            this.Controls.Add(this.tsTop);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lvFavoriteButtons);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrganizeFavoritesToolbarForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Organize Favorites Toolbar";
            this.tsTop.ResumeLayout(false);
            this.tsTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvFavoriteButtons;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ToolStrip tsTop;
        private System.Windows.Forms.ToolStripButton tsbMoveToFirst;
        private System.Windows.Forms.ToolStripButton tsbMoveUp;
        private System.Windows.Forms.ToolStripButton tsbMoveDown;
        private System.Windows.Forms.ToolStripButton tsbMoveToLast;
    }
}