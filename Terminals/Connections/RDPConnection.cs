using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Terminals.Properties;
using System.Diagnostics;
using System.Runtime.InteropServices;

using MSTSC = MSTSCLib;
using AxMSTSCLib;
using System.IO;

using TabControl;

namespace Terminals.Connections
{
    public class RDPConnection : Connection
    {
        private MSTSCLib.IMsRdpClientNonScriptable _nonScriptable;
        private AxMsRdpClient2 _axMsRdpClient2 = null;

        public delegate void Disconnected(RDPConnection Connection);
        public event Disconnected OnDisconnected;
        public delegate void ConnectionEstablish(RDPConnection Connection);
        public event ConnectionEstablish OnConnected;
        
        public AxMsRdpClient2 AxMsRdpClient2
        {
            get { return _axMsRdpClient2; }
        }

        private void SHCopyFiles(string[] sourceFiles, string destinationFolder)
        {
            SHFileOperationWrapper fo = new SHFileOperationWrapper();
            List<string> destinationFiles = new List<string>();

            foreach (string sourceFile in sourceFiles)
            {
                destinationFiles.Add(Path.Combine(destinationFolder, Path.GetFileName(sourceFile)));
            }

            fo.Operation = SHFileOperationWrapper.FileOperations.FO_COPY;
            fo.OwnerWindow = this.Handle;
            fo.SourceFiles = sourceFiles;
            fo.DestFiles = destinationFiles.ToArray();
            fo.DoOperation();
        }
        #region IConnection Members
        public override void ChangeDesktopSize(Terminals.DesktopSize Size)
        {



            int height = Favorite.DesktopSizeHeight, width = Favorite.DesktopSizeWidth;
            ConnectionManager.GetSize(ref height, ref width, this, Favorite.DesktopSize);

            try
            {

                if(Size == DesktopSize.AutoScale)
                {
                    _axMsRdpClient2.AdvancedSettings3.SmartSizing = true;
                    _axMsRdpClient2.DesktopWidth = width;
                    _axMsRdpClient2.DesktopHeight = height;
                    return;
                }
                if(Size == DesktopSize.FullScreen)
                {
                    _axMsRdpClient2.FullScreen = true;
                    _axMsRdpClient2.DesktopWidth = width;
                    _axMsRdpClient2.DesktopHeight = height;
                    return;
                }
                _axMsRdpClient2.DesktopWidth = width;
                _axMsRdpClient2.DesktopHeight = height;

            }
            catch(Exception exc)
            {
                Terminals.Logging.Log.Error("Error trying to set the desktop dimensions",exc);
            }
        }                
        public override bool Connect()
        {
            try
            {
                _axMsRdpClient2 = new AxMsRdpClient2();                
                Controls.Add(_axMsRdpClient2);
                _axMsRdpClient2.BringToFront();
                this.BringToFront();
                _axMsRdpClient2.Parent = base.TerminalTabPage;
                this.Parent = TerminalTabPage;
                _axMsRdpClient2.AllowDrop = true;

                ((Control)_axMsRdpClient2).DragEnter += new DragEventHandler(axMsRdpClient2_DragEnter);
                ((Control)_axMsRdpClient2).DragDrop += new DragEventHandler(axMsRdpClient2_DragDrop);
                _axMsRdpClient2.OnConnected += new EventHandler(axMsRdpClient2_OnConnected);
                _axMsRdpClient2.Dock = DockStyle.Fill;

                ChangeDesktopSize(Favorite.DesktopSize);
                try
                {
                    //if(Favorite.DesktopSize == DesktopSize.AutoScale) axMsRdpClient2.AdvancedSettings3.SmartSizing = true;
                    //axMsRdpClient2.DesktopWidth = width;
                    //axMsRdpClient2.DesktopHeight = height;

                    switch(Favorite.Colors)
                    {
                        case Colors.Bits8:
                            _axMsRdpClient2.ColorDepth = 8;
                            break;
                        case Colors.Bit16:
                            _axMsRdpClient2.ColorDepth = 16;
                            break;
                        case Colors.Bits24:
                            _axMsRdpClient2.ColorDepth = 24;
                            break;
                        case Colors.Bits32:
                            if(Settings.SupportsRDP6)
                                _axMsRdpClient2.ColorDepth = 32;
                            else
                                _axMsRdpClient2.ColorDepth = 24;
                            break;
                    }

                    _axMsRdpClient2.ConnectingText = "Connecting. Please wait...";
                    _axMsRdpClient2.DisconnectedText = "Disconnecting...";
                    _axMsRdpClient2.AdvancedSettings3.RedirectDrives = Favorite.RedirectDrives;

                    //advanced settings
                    //bool, 0 is false, other is true
                    if(Favorite.AllowBackgroundInput) 
                        _axMsRdpClient2.AdvancedSettings.allowBackgroundInput = -1;
                    
                    if(Favorite.BitmapPeristence) 
                        _axMsRdpClient2.AdvancedSettings.BitmapPeristence = -1;
                    
                    if(Favorite.EnableCompression)
                        _axMsRdpClient2.AdvancedSettings.Compress = -1;

                    if(Favorite.AcceleratorPassthrough) 
                        _axMsRdpClient2.AdvancedSettings2.AcceleratorPassthrough = -1;

                    if(Favorite.DisableControlAltDelete) 
                        _axMsRdpClient2.AdvancedSettings2.DisableCtrlAltDel = -1;

                    if(Favorite.DisplayConnectionBar) 
                        _axMsRdpClient2.AdvancedSettings2.DisplayConnectionBar = true;

                    if(Favorite.DoubleClickDetect) 
                        _axMsRdpClient2.AdvancedSettings2.DoubleClickDetect = -1;

                    if(Favorite.DisableWindowsKey) 
                        _axMsRdpClient2.AdvancedSettings2.EnableWindowsKey = -1;
                    
                    if(Favorite.EnableEncryption) 
                        _axMsRdpClient2.AdvancedSettings2.EncryptionEnabled = -1;


                    if(Favorite.GrabFocusOnConnect) 
                        _axMsRdpClient2.AdvancedSettings2.GrabFocusOnConnect = true;

                        if(Favorite.EnableSecuritySettings)
                        {
                            if(Favorite.SecurityFullScreen) 
                                _axMsRdpClient2.SecuredSettings2.FullScreen = -1;
                            
                            _axMsRdpClient2.SecuredSettings2.StartProgram = Favorite.SecurityStartProgram;
                            _axMsRdpClient2.SecuredSettings2.WorkDir = Favorite.SecurityWorkingFolder;
                        }

                    _axMsRdpClient2.AdvancedSettings2.MinutesToIdleTimeout = Favorite.IdleTimeout;

                    try {
                        int timeout = Favorite.OverallTimeout;
                        if(timeout > 600) timeout = 10;
                        if(timeout <= 0) timeout = 10;
                        _axMsRdpClient2.AdvancedSettings2.overallConnectionTimeout = timeout;
                        timeout = Favorite.ConnectionTimeout;
                        if(timeout > 600) timeout = 10;
                        if(timeout <= 0) timeout = 10;
                        
                        _axMsRdpClient2.AdvancedSettings2.singleConnectionTimeout = timeout;

                        timeout = Favorite.ShutdownTimeout;
                        if(timeout > 600) timeout = 10;
                        if(timeout <= 0) timeout = 10;
                        _axMsRdpClient2.AdvancedSettings2.shutdownTimeout = timeout;
                        

                        //axMsRdpClient2.AdvancedSettings2.PinConnectionBar;
                        //axMsRdpClient2.AdvancedSettings2.TransportType;
                        //axMsRdpClient2.AdvancedSettings2.WinceFixedPalette;
                        //axMsRdpClient2.AdvancedSettings3.CanAutoReconnect = Favorite.CanAutoReconnect;
                    } catch(Exception exc) {
                        Terminals.Logging.Log.Error("Error when trying to set timeout values.", exc);
                    }

                    _axMsRdpClient2.AdvancedSettings3.RedirectPorts = Favorite.RedirectPorts;
                    _axMsRdpClient2.AdvancedSettings3.RedirectPrinters = Favorite.RedirectPrinters;
                    _axMsRdpClient2.AdvancedSettings3.RedirectSmartCards = Favorite.RedirectSmartCards;
                    _axMsRdpClient2.AdvancedSettings3.PerformanceFlags = Favorite.PerformanceFlags;
                    _nonScriptable = (_axMsRdpClient2.GetOcx() as MSTSCLib.IMsRdpClientNonScriptable);
                        
                    /*
                    TS_PERF_DISABLE_CURSOR_SHADOW
                    0x00000020
                     No shadow is displayed for the cursor.
                    TS_PERF_DISABLE_CURSORSETTINGS
                    0x00000040
                     Cursor blinking is disabled.
                    TS_PERF_DISABLE_FULLWINDOWDRAG
                    0x00000002
                     Full-window drag is disabled; only the window outline is displayed when the window is moved.
                    TS_PERF_DISABLE_MENUANIMATIONS
                    0x00000004
                     Menu animations are disabled.
                    TS_PERF_DISABLE_NOTHING
                    0x00000000
                     No features are disabled.
                    TS_PERF_DISABLE_THEMING
                    0x00000008
                     Themes are disabled.
                    TS_PERF_DISABLE_WALLPAPER
                    0x00000001
                     Wallpaper on the desktop is not displayed.
                 
                    TS_PERF_ENABLE_FONT_SMOOTHING 0x00000080
                    TS_PERF_ENABLE_DESKTOP_COMPOSITION 0x00000100
                    */
                    if (Settings.SupportsRDP6)
                    {
                        MSTSCLib6.IMsRdpClientAdvancedSettings5 advancedSettings5 = (_axMsRdpClient2.AdvancedSettings3 as MSTSCLib6.IMsRdpClientAdvancedSettings5);
                        if (advancedSettings5 != null)
                        {
                            advancedSettings5.RedirectClipboard = Favorite.RedirectClipboard;
                            advancedSettings5.RedirectDevices = Favorite.RedirectDevices;
                            advancedSettings5.ConnectionBarShowMinimizeButton = false;
                            advancedSettings5.ConnectionBarShowPinButton = false;
                            advancedSettings5.ConnectionBarShowRestoreButton = false;

                            if (Favorite.EnableTLSAuthentication)
                                advancedSettings5.AuthenticationLevel = 2;
                        }
                    }
                    _axMsRdpClient2.SecuredSettings2.AudioRedirectionMode = (int)Favorite.Sounds;
                                        
                    string domainName = Favorite.DomainName;
                    if(string.IsNullOrEmpty(domainName)) 
                        domainName = Settings.DefaultDomain;

                    string pass = Favorite.Password;
                    if (string.IsNullOrEmpty(pass))
                        pass = Settings.DefaultPassword;

                    string userName = Favorite.UserName;
                    if (string.IsNullOrEmpty(userName))
                        userName = Settings.DefaultUsername;

                    _axMsRdpClient2.UserName = userName;
                    _axMsRdpClient2.Domain = domainName;
                    try {
                        if(!String.IsNullOrEmpty(pass)) {
                            if(_nonScriptable != null)
                                _nonScriptable.ClearTextPassword = pass;
                        }
                    } catch(Exception exc) {
                        Terminals.Logging.Log.Error("Error when trying to set the ClearTextPassword on the nonScriptable mstsc object", exc);
                    }

                    _axMsRdpClient2.Server = Favorite.ServerName;
                    _axMsRdpClient2.AdvancedSettings3.RDPPort = Favorite.Port;
                    _axMsRdpClient2.AdvancedSettings3.ContainerHandledFullScreen = -1;
                    _axMsRdpClient2.AdvancedSettings3.DisplayConnectionBar = Favorite.DisplayConnectionBar;

                    // Use ConnectToServerConsole or ConnectToAdministerServer based on implementation
                    if (_axMsRdpClient2.AdvancedSettings3 is MSTSCLib6.IMsRdpClientAdvancedSettings6)
                        ((MSTSCLib6.IMsRdpClientAdvancedSettings6)_axMsRdpClient2.AdvancedSettings3).ConnectToAdministerServer = Favorite.ConnectToConsole;
                    else
                        _axMsRdpClient2.AdvancedSettings3.ConnectToServerConsole = Favorite.ConnectToConsole;

                    _axMsRdpClient2.OnRequestGoFullScreen += new EventHandler(axMsTscAx_OnRequestGoFullScreen);
                    _axMsRdpClient2.OnRequestLeaveFullScreen += new EventHandler(axMsTscAx_OnRequestLeaveFullScreen);
                    _axMsRdpClient2.OnDisconnected += new IMsTscAxEvents_OnDisconnectedEventHandler(axMsTscAx_OnDisconnected);
                    _axMsRdpClient2.OnWarning += new IMsTscAxEvents_OnWarningEventHandler(axMsRdpClient2_OnWarning);
                    _axMsRdpClient2.OnFatalError += new IMsTscAxEvents_OnFatalErrorEventHandler(axMsRdpClient2_OnFatalError);

                    Text = "Connecting to RDP Server...";
                    _axMsRdpClient2.FullScreen = true;
                }
                catch(Exception exc)
                {
                    Terminals.Logging.Log.Warn("There was an exception setting an RDP Value.", exc);
                }
                _axMsRdpClient2.Connect();
                return true;
            }
            catch(Exception exc)
            {
                Terminals.Logging.Log.Fatal("Connecting to RDP", exc);
                return false;
            }
        }        
        public override void Disconnect()
        {
            try
            {
                
                _axMsRdpClient2.Disconnect();
            }
            catch(Exception e)
            {
                Terminals.Logging.Log.Info("", e);
            }
        }
        public override bool Connected { get { return Convert.ToBoolean(_axMsRdpClient2.Connected); } }
        private void axMsRdpClient2_OnConnected(object sender, EventArgs e)
        {
            if (OnConnected != null) OnConnected(this);
        }
        #endregion
        #region private event
        private void axMsRdpClient2_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string desktopShare = ParentForm.GetDesktopShare();
            if(String.IsNullOrEmpty(desktopShare))
            {
                MessageBox.Show(this, "A Desktop Share was not defined for this connection.\n" +
                    "Please define a share in the connection properties window (under the Local Resources tab)."
                    , "Terminals", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
                SHCopyFiles(files, desktopShare);
        }

        private void axMsRdpClient2_DragEnter(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop, false))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }
        private void axMsTscAx_OnRequestLeaveFullScreen(object sender, EventArgs e)
        {
            ParentForm.tsbGrabInput.Checked = false;
            ParentForm.UpdateControls();
            NativeApi.PostMessage(new HandleRef(this, this.Handle), MainForm.WM_LEAVING_FULLSCREEN, IntPtr.Zero, IntPtr.Zero);
        }

        private void axMsTscAx_OnRequestGoFullScreen(object sender, EventArgs e)
        {
            ParentForm.tsbGrabInput.Checked = true;
            ParentForm.UpdateControls();
        }

        private void axMsTscAx_OnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e)
        {
            AxMsRdpClient2 client = (AxMsRdpClient2)sender;

            string error = Functions.GetErrorMessage(e.discReason);
            if(error != null)
            {
//                MessageBox.Show(this, String.Format("Error connecting to {0} ({1})", client.Server, error), "Terminals " + Program.TerminalsVersion.ToString(),MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            TabControlItem selectedTabPage = (TabControlItem)(client.Parent);
            bool wasSelected = selectedTabPage.Selected;
            ParentForm.tcTerminals.RemoveTab(selectedTabPage);
            ParentForm.tcTerminals_TabControlItemClosed(null, EventArgs.Empty);
            if(wasSelected)
                NativeApi.PostMessage(new HandleRef(this, this.Handle), MainForm.WM_LEAVING_FULLSCREEN, IntPtr.Zero, IntPtr.Zero);
            ParentForm.UpdateControls();

            if(OnDisconnected != null) OnDisconnected(this);
        }
        private void axMsRdpClient2_OnFatalError(object sender, IMsTscAxEvents_OnFatalErrorEvent e)
        {
            //throw new Exception("The method or operation is not implemented.");

        }

        private void axMsRdpClient2_OnWarning(object sender, IMsTscAxEvents_OnWarningEvent e)
        {
            //throw new Exception("The method or operation is not implemented.");
        }
        #endregion        
    }
}
