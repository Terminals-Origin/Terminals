using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using Terminals.Data;

namespace Terminals.Forms.Controls
{
    partial class SearchPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchPanel));
            this.protocolsImageList = new System.Windows.Forms.ImageList(this.components);
            this.searchTextBox = new Terminals.Forms.Controls.SearchTextBox();
            this.resultsListView = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // protocolsImageList
            // 
            this.protocolsImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("protocolsImageList.ImageStream")));
            this.protocolsImageList.TransparentColor = System.Drawing.Color.Magenta;
            this.protocolsImageList.Images.SetKeyName(0, "treeIcon_rdp.png");
            this.protocolsImageList.Images.SetKeyName(1, "treeIcon_http.png");
            this.protocolsImageList.Images.SetKeyName(2, "treeIcon_vnc.png");
            this.protocolsImageList.Images.SetKeyName(3, "treeIcon_ssh.png");
            this.protocolsImageList.Images.SetKeyName(4, "treeIcon_telnet.png");
            this.protocolsImageList.Images.SetKeyName(5, "terminalsicon.png");
            // 
            // searchTextBox
            // 
            this.searchTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.searchTextBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.searchTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.searchTextBox.Location = new System.Drawing.Point(0, 0);
            this.searchTextBox.MinimumSize = new System.Drawing.Size(30, 22);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(218, 22);
            this.searchTextBox.TabIndex = 0;
            this.searchTextBox.Start += new System.EventHandler<Terminals.Forms.Controls.SearchEventArgs>(this.SearchTextBoxStart);
            this.searchTextBox.Cancel += new System.EventHandler(this.SearchTextBoxCancel);
            // 
            // resultsListView
            // 
            this.resultsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultsListView.LargeImageList = this.protocolsImageList;
            this.resultsListView.Location = new System.Drawing.Point(0, 22);
            this.resultsListView.Name = "resultsListView";
            this.resultsListView.ShowItemToolTips = true;
            this.resultsListView.Size = new System.Drawing.Size(218, 296);
            this.resultsListView.SmallImageList = this.protocolsImageList;
            this.resultsListView.TabIndex = 3;
            this.resultsListView.UseCompatibleStateImageBehavior = false;
            this.resultsListView.View = System.Windows.Forms.View.SmallIcon;
            // 
            // SearchPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.resultsListView);
            this.Controls.Add(this.searchTextBox);
            this.Name = "SearchPanel";
            this.Size = new System.Drawing.Size(218, 318);
            this.ResumeLayout(false);

        }

        #endregion

        private SearchTextBox searchTextBox;
        private System.Windows.Forms.ImageList protocolsImageList;
        private System.Windows.Forms.ListView resultsListView;
    }
}
