namespace Terminals.Forms.Controls
{
    partial class HistoryTreeView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HistoryTreeView));
            this.imageListIcons = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // imageListIcons
            // 
            this.imageListIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListIcons.ImageStream")));
            this.imageListIcons.TransparentColor = System.Drawing.Color.Magenta;
            this.imageListIcons.Images.SetKeyName(0, "history_icon_today.png");
            this.imageListIcons.Images.SetKeyName(1, "history_icon_yesterday.png");
            this.imageListIcons.Images.SetKeyName(2, "history_icon_week.png");
            this.imageListIcons.Images.SetKeyName(3, "history_icon_twoweeks.png");
            this.imageListIcons.Images.SetKeyName(4, "history_icon_month.png");
            this.imageListIcons.Images.SetKeyName(5, "history_icon_overmonth.png");
            this.imageListIcons.Images.SetKeyName(6, "history_icon_halfyear.png");
            this.imageListIcons.Images.SetKeyName(7, "history_icon_year.png");
            this.imageListIcons.Images.SetKeyName(8, "treeIcon_vnc.png");
            this.imageListIcons.Images.SetKeyName(9, "treeIcon_http.png");
            this.imageListIcons.Images.SetKeyName(10, "treeIcon_rdp.png");
            this.imageListIcons.Images.SetKeyName(11, "treeIcon_ssh.png");
            this.imageListIcons.Images.SetKeyName(12, "treeIcon_telnet.png");
            // 
            // HistoryTreeView
            // 
            this.ImageIndex = 0;
            this.ImageList = this.imageListIcons;
            this.SelectedImageIndex = 0;
            this.ShowNodeToolTips = true;
            this.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.OnTreeViewExpand);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageListIcons;
    }
}
