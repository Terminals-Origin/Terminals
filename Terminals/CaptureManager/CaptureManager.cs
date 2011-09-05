using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Terminals.Configuration;

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
            DirectoryInfo dir = new DirectoryInfo(Path);
            foreach (FileInfo cap in dir.GetFiles("*.png"))
            {
                Capture newCap = new Capture(cap.FullName);
                c.Add(newCap);
            }

            return c;
        }

        public static Capture PerformScreenCapture(TabControl.TabControl tab)
        {
            string filename = DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss");
            string tempFile = Path.Combine(CaptureRoot, string.Format("{0}.png", filename));
            ScreenCapture sc = new ScreenCapture();
            Bitmap bmp = sc.CaptureControl(tab, tempFile, ImageFormatTypes.imgPNG);

            if (Settings.EnableCaptureToClipboard)
                Clipboard.SetImage(bmp);

            return null;
        }

        public static List<DirectoryInfo> LoadCaptureFolder(string Path)
        {
            if (!Directory.Exists(Path)) 
                return null;

            DirectoryInfo dir = new DirectoryInfo(Path);
            return new List<DirectoryInfo>(dir.GetDirectories());
        }
    }
}
