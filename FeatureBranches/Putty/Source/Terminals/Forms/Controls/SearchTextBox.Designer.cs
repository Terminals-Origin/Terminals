namespace Terminals.Forms.Controls
{
  partial class SearchTextBox
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
      if (disposing && (this.components != null))
      {
        this.components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.searchPicture = new System.Windows.Forms.PictureBox();
            this.valueTextBox = new System.Windows.Forms.TextBox();
            this.searchButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.searchPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // searchPicture
            // 
            this.searchPicture.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchPicture.Image = global::Terminals.Properties.Resources.search_glyph;
            this.searchPicture.Location = new System.Drawing.Point(156, 2);
            this.searchPicture.Name = "searchPicture";
            this.searchPicture.Size = new System.Drawing.Size(18, 16);
            this.searchPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.searchPicture.TabIndex = 1;
            this.searchPicture.TabStop = false;
            this.searchPicture.Visible = false;
            // 
            // valueTextBox
            // 
            this.valueTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.valueTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.valueTextBox.Location = new System.Drawing.Point(2, 3);
            this.valueTextBox.Name = "valueTextBox";
            this.valueTextBox.Size = new System.Drawing.Size(150, 13);
            this.valueTextBox.TabIndex = 2;
            this.valueTextBox.Text = "Search ...";
            this.valueTextBox.TextChanged += new System.EventHandler(this.ValueTextBoxTextChanged);
            this.valueTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.ValueTextBoxKeyUp);
            this.valueTextBox.Leave += new System.EventHandler(this.ValueTextBox_Leave);
            this.valueTextBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ValueTextBox_MouseUp);
            // 
            // searchButton
            // 
            this.searchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.searchButton.FlatAppearance.BorderColor = System.Drawing.SystemColors.Window;
            this.searchButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.searchButton.Image = global::Terminals.Properties.Resources.search_glyph;
            this.searchButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.searchButton.Location = new System.Drawing.Point(152, -2);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(24, 22);
            this.searchButton.TabIndex = 3;
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.SearchButtonClick);
            // 
            // SearchTextBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.searchButton);
            this.Controls.Add(this.valueTextBox);
            this.Controls.Add(this.searchPicture);
            this.MinimumSize = new System.Drawing.Size(30, 22);
            this.Name = "SearchTextBox";
            this.Size = new System.Drawing.Size(176, 20);
            ((System.ComponentModel.ISupportInitialize)(this.searchPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.PictureBox searchPicture;
    private System.Windows.Forms.TextBox valueTextBox;
    private System.Windows.Forms.Button searchButton;
  }
}
