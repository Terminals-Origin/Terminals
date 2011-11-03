using System;
using System.IO;
using FlickrNet;
using Terminals.Configuration;

namespace Terminals.CaptureManager
{
    public class Capture
    {
        internal static string FlickrAPIKey = "9362619635c6f6c20e7c14fe4b67c2a0";
        internal static string FlickrSharedSecretKey = "ac8f3c60be0812b6";
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
                if (File.Exists(this.FilePath)) File.Delete(this.FilePath);
                if (File.Exists(this.CommentsFilename)) File.Delete(this.CommentsFilename);
            }
            catch (Exception ec)
            {
                Logging.Log.Error("Error trying to Delete", ec);
            }
        }

        internal static Flickr CreateFlickerInstance()
        {
            return new Flickr(FlickrAPIKey, FlickrSharedSecretKey);
        }

        public void PostToFlickr()
        {
            if (Settings.FlickrToken != string.Empty)
            {
                Flickr flckr = CreateFlickerInstance();
                flckr.AuthToken = Settings.FlickrToken;
                string c = this.Comments;
                if (c == null) c = string.Empty;
                using (FileStream fs = new FileStream(this.FilePath, FileMode.Open))
                {
                    flckr.UploadPicture(fs, Path.GetFileName(this.FilePath), c, "screenshot Terminals", 1, 1, 1,
                        ContentType.Screenshot, SafetyLevel.Safe, HiddenFromSearch.Visible);
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
                if (File.Exists(Destination))
                {
                    File.Delete(this.FilePath);
                }
                else
                {
                    File.Move(this.FilePath, Destination);
                }

                Destination = Destination + ".comments";
                if (File.Exists(Destination))
                {
                    File.Delete(this.CommentsFilename);
                }
                else
                {
                    File.Move(this.CommentsFilename, Destination);
                }
            }
            catch (Exception exc)
            {
                Logging.Log.Error("Error trying to call Move (file)", exc);                
            }
        }

        public void Save()
        {
            if (File.Exists(this.CommentsFilename)) File.Delete(this.CommentsFilename);
            if (this.Comments != null && this.Comments.Trim() != string.Empty)
            {
                File.WriteAllText(this.CommentsFilename, this.comments);
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
                return Path.Combine(Path.GetDirectoryName(this.FilePath), string.Format("{0}.comments", Path.GetFileName(this.filepath)));
            }
        }

        public string Name
        {
            get 
            {
                return Path.GetFileName(this.FilePath);
            }
        }

        public System.Drawing.Image Image
        {
            get
            {
                if (this.image == null)
                {
                    string copy = Path.Combine(Path.GetDirectoryName(Path.GetTempFileName()), Path.GetFileName(this.filepath));
                    if (!File.Exists(copy)) File.Copy(this.filepath, copy);
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
                    if (File.Exists(this.CommentsFilename))
                    {
                        string copy = Path.Combine(Path.GetDirectoryName(Path.GetTempFileName()), Path.GetFileName(this.CommentsFilename));
                        if (!File.Exists(copy))
                            File.Copy(this.CommentsFilename, copy);

                        this.comments = File.ReadAllText(copy);
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
