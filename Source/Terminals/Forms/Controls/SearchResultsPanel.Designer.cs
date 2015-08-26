using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    partial class SearchResultsPanel
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
            this.components = new System.ComponentModel.Container();
            this.protocolsImageList = new System.Windows.Forms.ImageList(this.components);
            this.resultsListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // protocolsImageList
            // 
            this.protocolsImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.protocolsImageList.ImageSize = new System.Drawing.Size(16, 16);
            this.protocolsImageList.TransparentColor = System.Drawing.Color.Magenta;
            // 
            // resultsListView
            // 
            this.resultsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.resultsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.resultsListView.LargeImageList = this.protocolsImageList;
            this.resultsListView.Location = new System.Drawing.Point(0, 0);
            this.resultsListView.Name = "resultsListView";
            this.resultsListView.ShowItemToolTips = true;
            this.resultsListView.Size = new System.Drawing.Size(218, 318);
            this.resultsListView.SmallImageList = this.protocolsImageList;
            this.resultsListView.TabIndex = 3;
            this.resultsListView.UseCompatibleStateImageBehavior = false;
            this.resultsListView.View = System.Windows.Forms.View.Details;
            this.resultsListView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ResultsListView_MouseUp);
            // 
            // SearchResultsPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.resultsListView);
            this.Name = "SearchResultsPanel";
            this.Size = new System.Drawing.Size(218, 318);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList protocolsImageList;
        private System.Windows.Forms.ListView resultsListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}
