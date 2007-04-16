using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AxMSTSCLib;
using MSTSC = MSTSCLib;
using Terminals.Properties;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TabControl;
using System.IO;

namespace Terminals.Connections {
    public class RDPConnection : Connection {
        #region IConnection Members
        public override void ChangeDesktopSize(Terminals.DesktopSize Size) {
            //int height; int width;
            //ConnectionManager.GetSize(out height, out width, this, Size);
            //axMsRdpClient2.DesktopWidth = width;
            //axMsRdpClient2.DesktopHeight = height;
            if(Size == DesktopSize.AutoScale) axMsRdpClient2.AdvancedSettings3.SmartSizing = true;
            if(Size == DesktopSize.FullScreen) axMsRdpClient2.FullScreen = true;
        }

        public override bool Connected { get { return Convert.ToBoolean(axMsRdpClient2.Connected); } }
        public AxMsRdpClient2 axMsRdpClient2 = null;
        public override bool Connect() {
            axMsRdpClient2 = new AxMsRdpClient2();
            Controls.Add(axMsRdpClient2);
            axMsRdpClient2.BringToFront();
            this.BringToFront();
            axMsRdpClient2.AllowDrop = true;
            axMsRdpClient2.Parent = base.TerminalTabPage;
            this.Parent = TerminalTabPage;

            ((Control)axMsRdpClient2).DragEnter += new DragEventHandler(axMsRdpClient2_DragEnter);
            ((Control)axMsRdpClient2).DragDrop += new DragEventHandler(axMsRdpClient2_DragDrop);
            axMsRdpClient2.Dock = DockStyle.Fill;


            int height = 0, width = 0;
            ConnectionManager.GetSize(out height, out width, this, Favorite.DesktopSize);
            if (Favorite.DesktopSize == DesktopSize.AutoScale) axMsRdpClient2.AdvancedSettings3.SmartSizing = true;
            axMsRdpClient2.DesktopWidth = width;
            axMsRdpClient2.DesktopHeight = height;

            switch (Favorite.Colors) {
                case Colors.Bits8:
                    axMsRdpClient2.ColorDepth = 8;
                    break;
                case Colors.Bit16:
                    axMsRdpClient2.ColorDepth = 16;
                    break;
                case Colors.Bits24:
                    axMsRdpClient2.ColorDepth = 24;
                    break;
                case Colors.Bits32:
                    if (Settings.SupportsRDP6)
                        axMsRdpClient2.ColorDepth = 32;
                    else
                        axMsRdpClient2.ColorDepth = 24;
                    break;
            }
            axMsRdpClient2.ConnectingText = "Connecting. Please wait...";
            axMsRdpClient2.DisconnectedText = "Disconnecting...";
            axMsRdpClient2.AdvancedSettings3.RedirectDrives = Favorite.RedirectDrives;
            axMsRdpClient2.AdvancedSettings3.RedirectPorts = Favorite.RedirectPorts;
            axMsRdpClient2.AdvancedSettings3.RedirectPrinters = Favorite.RedirectPrinters;
            axMsRdpClient2.AdvancedSettings3.RedirectSmartCards = Favorite.RedirectSmartCards;
            axMsRdpClient2.AdvancedSettings3.PerformanceFlags = Favorite.PerformanceFlags;
            if (Settings.SupportsRDP6) {
                MSTSCLib6.IMsRdpClientAdvancedSettings5 advancedSettings5 = (axMsRdpClient2.AdvancedSettings3 as MSTSCLib6.IMsRdpClientAdvancedSettings5);
                if (advancedSettings5 != null) {
                    advancedSettings5.RedirectClipboard = Favorite.RedirectClipboard;
                    advancedSettings5.RedirectDevices = Favorite.RedirectDevices;
                    advancedSettings5.ConnectionBarShowMinimizeButton = false;
                    advancedSettings5.ConnectionBarShowPinButton = false;
                    advancedSettings5.ConnectionBarShowRestoreButton = false;
                }
            }
            axMsRdpClient2.SecuredSettings2.AudioRedirectionMode = (int)Favorite.Sounds;
            axMsRdpClient2.Domain = Favorite.DomainName;
            axMsRdpClient2.Server = Favorite.ServerName;
            axMsRdpClient2.AdvancedSettings3.RDPPort = Favorite.Port;
            axMsRdpClient2.UserName = Favorite.UserName;
            axMsRdpClient2.AdvancedSettings3.ContainerHandledFullScreen = -1;
            axMsRdpClient2.AdvancedSettings3.DisplayConnectionBar = false;
            axMsRdpClient2.AdvancedSettings3.ConnectToServerConsole = Favorite.ConnectToConsole;
            axMsRdpClient2.OnRequestGoFullScreen += new EventHandler(axMsTscAx_OnRequestGoFullScreen);
            axMsRdpClient2.OnRequestLeaveFullScreen += new EventHandler(axMsTscAx_OnRequestLeaveFullScreen);
            axMsRdpClient2.OnDisconnected += new IMsTscAxEvents_OnDisconnectedEventHandler(axMsTscAx_OnDisconnected);
            axMsRdpClient2.OnWarning += new IMsTscAxEvents_OnWarningEventHandler(axMsRdpClient2_OnWarning);
            axMsRdpClient2.OnFatalError += new IMsTscAxEvents_OnFatalErrorEventHandler(axMsRdpClient2_OnFatalError);

            if (!String.IsNullOrEmpty(Favorite.Password)) {
                MSTSC.IMsTscNonScriptable nonScriptable = (MSTSC.IMsTscNonScriptable)axMsRdpClient2.GetOcx();
                nonScriptable.ClearTextPassword = Favorite.Password;
            }
            Text = "Connecting to RDP Server...";
            axMsRdpClient2.FullScreen = true;
            axMsRdpClient2.Connect();
            return true;
        }

        public override void Disconnect() {
            try {
                axMsRdpClient2.Disconnect();
            } catch (Exception e) { }
        }

        #endregion

        void axMsRdpClient2_DragDrop(object sender, DragEventArgs e) {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string desktopShare = ParentForm.GetDesktopShare();
            if (String.IsNullOrEmpty(desktopShare)) {
                MessageBox.Show(this, "A Desktop Share was not defined for this connection.\n" +
                    "Please define a share in the connection properties window (under the Local Resources tab)."
                    , "Terminals", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            } else
                SHCopyFiles(files, desktopShare);
        }

        void axMsRdpClient2_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false)) {
                e.Effect = DragDropEffects.Copy;
            } else {
                e.Effect = DragDropEffects.None;
            }
        }
        void axMsTscAx_OnRequestLeaveFullScreen(object sender, EventArgs e) {
            ParentForm.tsbGrabInput.Checked = false;
            ParentForm.UpdateControls();
            NativeApi.PostMessage(new HandleRef(this, this.Handle), MainForm.WM_LEAVING_FULLSCREEN, IntPtr.Zero, IntPtr.Zero);
        }

        void axMsTscAx_OnRequestGoFullScreen(object sender, EventArgs e) {
            ParentForm.tsbGrabInput.Checked = true;
            ParentForm.UpdateControls();
        }

        void axMsTscAx_OnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e) {
            AxMsRdpClient2 client = (AxMsRdpClient2)sender;

            string error = Functions.GetErrorMessage(e.discReason);
            if (error != null) {
                MessageBox.Show(this, String.Format("error connecting to {0} ({1})", client.Server, error), "Terminals",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            TabControlItem selectedTabPage = (TabControlItem)(client.Parent);
            bool wasSelected = selectedTabPage.Selected;
            ParentForm.tcTerminals.RemoveTab(selectedTabPage);
            ParentForm.tcTerminals_TabControlItemClosed(null, EventArgs.Empty);
            if (wasSelected)
                NativeApi.PostMessage(new HandleRef(this, this.Handle), MainForm.WM_LEAVING_FULLSCREEN, IntPtr.Zero, IntPtr.Zero);
            ParentForm.UpdateControls();
        }
        void axMsRdpClient2_OnFatalError(object sender, IMsTscAxEvents_OnFatalErrorEvent e) {
            //throw new Exception("The method or operation is not implemented.");
        }

        void axMsRdpClient2_OnWarning(object sender, IMsTscAxEvents_OnWarningEvent e) {
            //throw new Exception("The method or operation is not implemented.");
        }
        private void SHCopyFiles(string[] sourceFiles, string destinationFolder) {
            SHFileOperationWrapper fo = new SHFileOperationWrapper();
            List<string> destinationFiles = new List<string>();

            foreach (string sourceFile in sourceFiles) {
                destinationFiles.Add(Path.Combine(destinationFolder, Path.GetFileName(sourceFile)));
            }

            fo.Operation = SHFileOperationWrapper.FileOperations.FO_COPY;
            fo.OwnerWindow = this.Handle;
            fo.SourceFiles = sourceFiles;
            fo.DestFiles = destinationFiles.ToArray();

            fo.DoOperation();
        }
    }
}
