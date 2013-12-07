namespace Terminals.Forms.Controls
{
    internal partial class SearchPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.searchBox = new Terminals.Forms.Controls.FavoritesSearchBox();
            this.searchResultsPanel = new Terminals.Forms.Controls.SearchResultsPanel();
            this.SuspendLayout();
            // 
            // searchBox
            // 
            this.searchBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.searchBox.Location = new System.Drawing.Point(0, 0);
            this.searchBox.Name = "searchBox";
            this.searchBox.Size = new System.Drawing.Size(270, 22);
            this.searchBox.TabIndex = 2;
            this.searchBox.Found += new System.EventHandler<Terminals.Forms.Controls.FavoritesFoundEventArgs>(this.FavoritesSearchFound);
            this.searchBox.Canceled += new System.EventHandler(this.SearchBox_Canceled);
            // 
            // searchResultsPanel
            // 
            this.searchResultsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchResultsPanel.LabelEdit = false;
            this.searchResultsPanel.Location = new System.Drawing.Point(0, 22);
            this.searchResultsPanel.Name = "searchResultsPanel";
            this.searchResultsPanel.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.searchResultsPanel.ResultsContextMenu = null;
            this.searchResultsPanel.Size = new System.Drawing.Size(270, 219);
            this.searchResultsPanel.TabIndex = 3;
            // 
            // SearchPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.searchResultsPanel);
            this.Controls.Add(this.searchBox);
            this.Name = "SearchPanel";
            this.Size = new System.Drawing.Size(270, 241);
            this.ResumeLayout(false);

        }

        #endregion

        private FavoritesSearchBox searchBox;
        private SearchResultsPanel searchResultsPanel;

    }
}
