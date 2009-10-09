using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.CaptureManager
{
    public class Capture
    {

        public Capture()
        {
        }
        public void Delete()
        {
            try
            {
                if(System.IO.File.Exists(this.FilePath)) System.IO.File.Delete(this.FilePath);
                if(System.IO.File.Exists(this.CommentsFilename)) System.IO.File.Delete(this.CommentsFilename);
            }
            catch(Exception ec)
            {
                Terminals.Logging.Log.Info("", ec);
            }
        }
        public void PostToFlickr() {
            if(Settings.FlickrToken != "") {
                FlickrNet.Flickr flckr = new FlickrNet.Flickr(Program.FlickrAPIKey, Program.FlickrSharedSecretKey);
                flckr.AuthToken = Settings.FlickrToken;
                string c = this.Comments;
                if(c == null) c = "";
                using(System.IO.FileStream fs = new System.IO.FileStream(FilePath, System.IO.FileMode.Open)) {
                    flckr.UploadPicture(fs, System.IO.Path.GetFileName(FilePath), c, "screenshot Terminals", 1, 1, 1, FlickrNet.ContentType.Screenshot, FlickrNet.SafetyLevel.Safe, FlickrNet.HiddenFromSearch.Visible);
                }
            } else {
                throw new Exception("You must authorize with Flickr prior to posting.  In Terminals, go to Tools, Options and then select the Flickr tab.");
            }
        }
        public void Move(string Destination)
        {
            try
            {

                if(System.IO.File.Exists(Destination))
                {
                    System.IO.File.Delete(FilePath);
                }
                else
                {
                    System.IO.File.Move(FilePath, Destination);

                }
                Destination = Destination + ".comments";
                if(System.IO.File.Exists(Destination))
                {
                    System.IO.File.Delete(CommentsFilename);
                }
                else
                {
                    System.IO.File.Move(CommentsFilename, Destination);

                }
            }
            catch(Exception exc)
            {
                Terminals.Logging.Log.Info("Error trying to call Move (file)", exc);                
            }
            finally
            {
            }
        }
        private string CommentsFilename
        {
            get
            {
                return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.FilePath), string.Format("{0}.comments", System.IO.Path.GetFileName(this.filepath)));
            }
        }
        public void Save()
        {
            if(System.IO.File.Exists(CommentsFilename)) System.IO.File.Delete(CommentsFilename);
            if(Comments != null && Comments.Trim() != "")
            {
                System.IO.File.WriteAllText(CommentsFilename, this.comments);
            }
        }
        public void Show()
        {
            System.Diagnostics.Process.Start(this.FilePath);
        }
        public Capture(string FilePath)
        {
            this.FilePath = FilePath;
        }

        public string Name
        {
            get { return System.IO.Path.GetFileName(this.FilePath);  }
        }	
	
        private System.Drawing.Image image;

        public System.Drawing.Image Image
        {
            get {
                if(image == null)
                {
                    string copy = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.IO.Path.GetTempFileName()), System.IO.Path.GetFileName(this.filepath));
                    if(!System.IO.File.Exists(copy)) System.IO.File.Copy(this.filepath, copy);
                    using(System.Drawing.Image i = System.Drawing.Image.FromFile(copy))
                    {
                        image = (System.Drawing.Image)i.Clone();
                    }
                }
                return image; 
            }
        }
	

        private string comments;

        public string Comments
        {
            get {
                if(comments == null)
                {
                    if(System.IO.File.Exists(CommentsFilename))
                    {
                        string copy = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.IO.Path.GetTempFileName()), System.IO.Path.GetFileName(this.CommentsFilename));
                        if(!System.IO.File.Exists(copy)) System.IO.File.Copy(CommentsFilename, copy);
                        comments = System.IO.File.ReadAllText(copy);
                    }
                }
                return comments; }
            set { comments = value; }
        }
	
        private string filepath;

        public string FilePath
        {
            get { return filepath; }
            set { filepath = value; }
        }
	
    }
}
