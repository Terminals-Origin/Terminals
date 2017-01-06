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
            // 
            // FavoritesTreeView
            // 
            this.ImageIndex = 0;
            this.ImageList = this.imageListIcons;
            this.SelectedImageIndex = 0;
            this.ShowNodeToolTips = true;
            this.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.FavoritesTreeView_ItemDrag);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FavsTree_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FavsTree_DragEnter);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.FavoritesTreeView_DragOver);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageListIcons;
    }
}
