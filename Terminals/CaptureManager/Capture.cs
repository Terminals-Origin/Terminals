using System;
using System.Collections.Generic;
using System.Text;

namespace Terminals.CaptureManager
{
    public class Capture
    {
        private System.Drawing.Image image;
        private string comments;
        private string filepath;

        public Capture()
        {
        }

        public void Delete()
        {
            try
            {
                if (System.IO.File.Exists(this.FilePath)) System.IO.File.Delete(this.FilePath);
                if (System.IO.File.Exists(this.CommentsFilename)) System.IO.File.Delete(this.CommentsFilename);
            }
            catch (Exception ec)
            {
                Terminals.Logging.Log.Error("Error trying to Delete", ec);
            }
        }

        public void PostToFlickr()
        {
            if (Settings.FlickrToken != string.Empty)
            {
                FlickrNet.Flickr flckr = new FlickrNet.Flickr(Program.FlickrAPIKey, Program.FlickrSharedSecretKey);
                flckr.AuthToken = Settings.FlickrToken;
                string c = this.Comments;
                if (c == null) c = string.Empty;
                using (System.IO.FileStream fs = new System.IO.FileStream(this.FilePath, System.IO.FileMode.Open))
                {
                    flckr.UploadPicture(fs, System.IO.Path.GetFileName(this.FilePath), c, "screenshot Terminals", 1, 1, 1, FlickrNet.ContentType.Screenshot, FlickrNet.SafetyLevel.Safe, FlickrNet.HiddenFromSearch.Visible);
                }
            } 
            else
            {
                throw new Exception("You must authorize with Flickr prior to posting.  In Terminals, go to Tools, Options and then select the Flickr tab.");
            }
        }

        public void Move(string Destination)
        {
            try
            {
                if (System.IO.File.Exists(Destination))
                {
                    System.IO.File.Delete(this.FilePath);
                }
                else
                {
                    System.IO.File.Move(this.FilePath, Destination);
                }

                Destination = Destination + ".comments";
                if (System.IO.File.Exists(Destination))
                {
                    System.IO.File.Delete(this.CommentsFilename);
                }
                else
                {
                    System.IO.File.Move(this.CommentsFilename, Destination);
                }
            }
            catch (Exception exc)
            {
                Terminals.Logging.Log.Error("Error trying to call Move (file)", exc);                
            }
        }

        public void Save()
        {
            if (System.IO.File.Exists(this.CommentsFilename)) System.IO.File.Delete(this.CommentsFilename);
            if (this.Comments != null && this.Comments.Trim() != string.Empty)
            {
                System.IO.File.WriteAllText(this.CommentsFilename, this.comments);
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

        private string CommentsFilename
        {
            get
            {
                return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(this.FilePath), string.Format("{0}.comments", System.IO.Path.GetFileName(this.filepath)));
            }
        }

        public string Name
        {
            get 
            {
                return System.IO.Path.GetFileName(this.FilePath);
            }
        }

        public System.Drawing.Image Image
        {
            get
            {
                if (this.image == null)
                {
                    string copy = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.IO.Path.GetTempFileName()), System.IO.Path.GetFileName(this.filepath));
                    if (!System.IO.File.Exists(copy)) System.IO.File.Copy(this.filepath, copy);
                    using (System.Drawing.Image i = System.Drawing.Image.FromFile(copy))
                    {
                        this.image = (System.Drawing.Image)i.Clone();
                    }
                }

                return this.image; 
            }
        }

        public string Comments
        {
            get 
            {
                if (this.comments == null)
                {
                    if (System.IO.File.Exists(this.CommentsFilename))
                    {
                        string copy = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.IO.Path.GetTempFileName()), System.IO.Path.GetFileName(this.CommentsFilename));
                        if (!System.IO.File.Exists(copy))
                            System.IO.File.Copy(this.CommentsFilename, copy);

                        this.comments = System.IO.File.ReadAllText(copy);
                    }
                }

                return this.comments;
            }

            set 
            {
                this.comments = value;
            }
        }

        public string FilePath
        {
            get
            {
                return this.filepath;
            }

            set
            {
                this.filepath = value;
            }
        }
    }
}
