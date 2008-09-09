using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Terminals.CaptureManager
{
    public class CaptureManager
    {
        
        public static string CaptureRoot
        {
            get
            {
                return Settings.CaptureRoot;
            }
            set
            {
                Settings.CaptureRoot = value;
            }
        }
        public static Captures LoadCaptures(string Path)
        {
            Captures c = new Captures();
            System.IO.DirectoryInfo dir =new System.IO.DirectoryInfo(Path);
            foreach(System.IO.FileInfo cap in dir.GetFiles("*.png"))
            {
                Capture newCap = new Capture(cap.FullName);
                c.Add(newCap);
            }
            return c;
        }
        public static Capture PerformScreenCapture(TabControl.TabControl tab)
        {
            ScreenCapture sc = new ScreenCapture();
            string filename = DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss");
            string tempFile = System.IO.Path.Combine(CaptureManager.CaptureRoot, string.Format("{0}.png", filename));

            using(sc.CaptureControl(tab, tempFile, ImageFormatHandler.ImageFormatTypes.imgPNG));
            
            //System.Diagnostics.Process.Start(tempFile);
            return null;
        }

        public static List<System.IO.DirectoryInfo> LoadCaptureFolder(string Path)
        {
            
            if(!System.IO.Directory.Exists(Path)) return null;

            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(Path);
            return new List<System.IO.DirectoryInfo>(dir.GetDirectories());
        }

    }
}
