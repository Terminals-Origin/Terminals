using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Terminals.Configuration;

namespace Terminals.CaptureManager
{
    internal class CaptureManager
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
            foreach (FileInfo cap in GetFiles(dir))
            {
                Capture newCap = new Capture(cap.FullName);
                c.Add(newCap);
            }

            return c;
        }

        private static FileInfo[] GetFiles(DirectoryInfo dir)
        {
            try
            {
                return dir.GetFiles("*.png");
            }
            catch (Exception)
            {
                return new FileInfo[0];
            }
        }

        public static Capture PerformScreenCapture(TabControl.TabControl tab)
        {
            TerminalTabControlItem activeTab = tab.SelectedItem as TerminalTabControlItem;
            string name = "";
            if (activeTab != null && activeTab.Favorite != null && !string.IsNullOrEmpty(activeTab.Favorite.Name))
            {
                name = activeTab.Favorite.Name + "-";
            }
            string filename = DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss");
            string tempFile = Path.Combine(CaptureRoot, string.Format("{0}{1}.png", name, filename));
            ScreenCapture sc = new ScreenCapture();
            Bitmap bmp = sc.CaptureControl(tab, tempFile, ImageFormatTypes.imgPNG);

            if (Settings.EnableCaptureToClipboard)
                Clipboard.SetImage(bmp);

            return null;
        }

        public static List<DirectoryInfo> LoadCaptureFolder(string Path)
        {
            if (!Directory.Exists(Path))
                return new List<DirectoryInfo>();

            DirectoryInfo dir = new DirectoryInfo(Path);
            return new List<DirectoryInfo>(dir.GetDirectories());
        }

        internal static void EnsureRoot()
        {
            try
            {
                TryEnsureRoot();
            }
            catch (Exception exception)
            {
                string logMessage = String.Format("Capture root could not be created, set it to the default value : {0}", CaptureRoot);
                Logging.Error(logMessage, exception);
            }
        }

        private static void TryEnsureRoot()
        {
            string root = CaptureRoot;
            if (Directory.Exists(root))
                return;

            Logging.Info(String.Format("Capture root folder does not exist:{0}. Lets try to create it now.", root));
            Directory.CreateDirectory(root);
        }
    }
}
