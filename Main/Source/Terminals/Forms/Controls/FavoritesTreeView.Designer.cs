namespace Terminals.Forms.Controls
{
    partial class FavoritesTreeView
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FavoritesTreeView));
      this.imageListIcons = new System.Windows.Forms.ImageList(this.components);
      this.SuspendLayout();
      // 
      // imageListIcons
      // 
      this.imageListIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListIcons.ImageStream")));
      this.imageListIcons.TransparentColor = System.Drawing.Color.Magenta;
      this.imageListIcons.Images.SetKeyName(0, "XPfolder_closed.bmp");
      this.imageListIcons.Images.SetKeyName(1, "XPfolder_Open.bmp");
      this.imageListIcons.Images.SetKeyName(2, "treeIcon_rdp.png");
      this.imageListIcons.Images.SetKeyName(3, "treeIcon_http.png");
      this.imageListIcons.Images.SetKeyName(4, "treeIcon_vnc.png");
      this.imageListIcons.Images.SetKeyName(5, "treeIcon_telnet.png");
      this.imageListIcons.Images.SetKeyName(6, "treeIcon_ssh.png");
      this.imageListIcons.Images.SetKeyName(7, "terminalsicon.ico");
      // 
      // FavoritesTreeView
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
