using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AxMSTSCLib;
using System.IO;
using System.Runtime.InteropServices;
using Terminals.Configuration;

//http://msdn.microsoft.com/en-us/library/aa381172(v=vs.85).aspx
//http://msdn.microsoft.com/en-us/library/aa380838(v=VS.85).aspx
//http://msdn.microsoft.com/en-us/library/aa380847(v=VS.85).aspx
//http://msdn.microsoft.com/en-us/library/bb892063(v=VS.85).aspx
//http://msdn.microsoft.com/en-us/library/ee338625(v=VS.85).aspx


namespace Terminals.Connections
{
    internal class RDPConnection : Connection
    {
        private MSTSCLib.IMsRdpClientNonScriptable4 _nonScriptable;
        private AxMsRdpClient6 _axMsRdpClient = null;

        public delegate void Disconnected(RDPConnection Connection);
        public event Disconnected OnDisconnected;
        public delegate void ConnectionEstablish(RDPConnection Connection);
        public event ConnectionEstablish OnConnected;
        
        public AxMsRdpClient6 AxMsRdpClient
        {
            get { return _axMsRdpClient; }
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
        public override void ChangeDesktopSize(DesktopSize Size)
        {
            Size size = ConnectionManager.GetSize(this, Favorite);

            try
            {
                switch (Size)
                {
                  case DesktopSize.AutoScale:
                  case DesktopSize.FitToWindow:
                    this._axMsRdpClient.AdvancedSettings3.SmartSizing = true;
                    break;
                  case DesktopSize.FullScreen:
                    _axMsRdpClient.FullScreen = true;
                    break;
                }

                _axMsRdpClient.DesktopWidth = size.Width;
                _axMsRdpClient.DesktopHeight = size.Height;
            }
            catch(Exception exc)
            {
                Logging.Log.Error("Error trying to set the desktop dimensions",exc);
            }
        }
                
        public override bool Connect()
        {
            try
            {
                _axMsRdpClient = new AxMsRdpClient6();
            }
            catch (Exception exc)
            {
                Logging.Log.Info("Please Update your RDP client to at least version 6.", exc);
                MessageBox.Show("Please Update your RDP client to at least version 6.");
                return false;
            }

            try
            {
                Controls.Add(_axMsRdpClient);
                _axMsRdpClient.BringToFront();
                this.BringToFront();
                _axMsRdpClient.Parent = TerminalTabPage;
                this.Parent = TerminalTabPage;
                _axMsRdpClient.AllowDrop = true;

                ((Control)_axMsRdpClient).DragEnter += new DragEventHandler(axMsRdpClient2_DragEnter);
                ((Control)_axMsRdpClient).DragDrop += new DragEventHandler(axMsRdpClient2_DragDrop);
                _axMsRdpClient.OnConnected += new EventHandler(axMsRdpClient2_OnConnected);
                _axMsRdpClient.Dock = DockStyle.Fill;
                _nonScriptable = (_axMsRdpClient.GetOcx() as MSTSCLib.IMsRdpClientNonScriptable4);
                    
                ChangeDesktopSize(Favorite.DesktopSize);
                try
                {
                    //if(Favorite.DesktopSize == DesktopSize.AutoScale) axMsRdpClient2.AdvancedSettings3.SmartSizing = true;
                    //axMsRdpClient2.DesktopWidth = width;
                    //axMsRdpClient2.DesktopHeight = height;

                    switch(Favorite.Colors)
                    {
                        case Colors.Bits8:
                            _axMsRdpClient.ColorDepth = 8;
                            break;
                        case Colors.Bit16:
                            _axMsRdpClient.ColorDepth = 16;
                            break;
                        case Colors.Bits24:
                            _axMsRdpClient.ColorDepth = 24;
                            break;
                        case Colors.Bits32:
                            _axMsRdpClient.ColorDepth = 32;
                            break;
                    }

                    _axMsRdpClient.ConnectingText = "Connecting. Please wait...";
                    _axMsRdpClient.DisconnectedText = "Disconnecting...";

                    if (Favorite.RedirectedDrives.Count > 0 && Favorite.RedirectedDrives[0].Equals("true"))
                    {
                        _axMsRdpClient.AdvancedSettings2.RedirectDrives = true;
                    }
                    else
                    {
                        for (int i = 0; i < _nonScriptable.DriveCollection.DriveCount; i++)
                        {
                            MSTSCLib.IMsRdpDrive drive = _nonScriptable.DriveCollection.get_DriveByIndex((uint)i);
                            foreach (string str in Favorite.RedirectedDrives)
                                if (drive.Name.IndexOf(str) > -1)
                                    drive.RedirectionState = true;
                        }
                    }

                    //advanced settings
                    //bool, 0 is false, other is true
                    if(Favorite.AllowBackgroundInput) 
                        _axMsRdpClient.AdvancedSettings.allowBackgroundInput = -1;
                    
                    if(Favorite.BitmapPeristence) 
                        _axMsRdpClient.AdvancedSettings.BitmapPeristence = -1;
                    
                    if(Favorite.EnableCompression)
                        _axMsRdpClient.AdvancedSettings.Compress = -1;

                    if(Favorite.AcceleratorPassthrough) 
                        _axMsRdpClient.AdvancedSettings2.AcceleratorPassthrough = -1;

                    if(Favorite.DisableControlAltDelete) 
                        _axMsRdpClient.AdvancedSettings2.DisableCtrlAltDel = -1;

                    if(Favorite.DisplayConnectionBar) 
                        _axMsRdpClient.AdvancedSettings2.DisplayConnectionBar = true;

                    if(Favorite.DoubleClickDetect) 
                        _axMsRdpClient.AdvancedSettings2.DoubleClickDetect = -1;

                    if(Favorite.DisableWindowsKey) 
                        _axMsRdpClient.AdvancedSettings2.EnableWindowsKey = -1;
                    
                    if(Favorite.EnableEncryption) 
                        _axMsRdpClient.AdvancedSettings2.EncryptionEnabled = -1;


                    if(Favorite.GrabFocusOnConnect) 
                        _axMsRdpClient.AdvancedSettings2.GrabFocusOnConnect = true;

                        if(Favorite.EnableSecuritySettings)
                        {
                            if(Favorite.SecurityFullScreen) 
                                _axMsRdpClient.SecuredSettings2.FullScreen = -1;
                            
                            _axMsRdpClient.SecuredSettings2.StartProgram = Favorite.SecurityStartProgram;
                            _axMsRdpClient.SecuredSettings2.WorkDir = Favorite.SecurityWorkingFolder;
                        }

                    _axMsRdpClient.AdvancedSettings2.MinutesToIdleTimeout = Favorite.IdleTimeout;

                    try {
                        int timeout = Favorite.OverallTimeout;
                        if(timeout > 600) timeout = 10;
                        if(timeout <= 0) timeout = 10;
                        _axMsRdpClient.AdvancedSettings2.overallConnectionTimeout = timeout;
                        timeout = Favorite.ConnectionTimeout;
                        if(timeout > 600) timeout = 10;
                        if(timeout <= 0) timeout = 10;
                        
                        _axMsRdpClient.AdvancedSettings2.singleConnectionTimeout = timeout;

                        timeout = Favorite.ShutdownTimeout;
                        if(timeout > 600) timeout = 10;
                        if(timeout <= 0) timeout = 10;
                        _axMsRdpClient.AdvancedSettings2.shutdownTimeout = timeout;
                        

                        //axMsRdpClient2.AdvancedSettings2.PinConnectionBar;
                        //axMsRdpClient2.AdvancedSettings2.TransportType;
                        //axMsRdpClient2.AdvancedSettings2.WinceFixedPalette;
                        //axMsRdpClient2.AdvancedSettings3.CanAutoReconnect = Favorite.CanAutoReconnect;
                    } catch(Exception exc) {
                        Logging.Log.Error("Error when trying to set timeout values.", exc);
                    }

                    _axMsRdpClient.AdvancedSettings3.RedirectPorts = Favorite.RedirectPorts;
                    _axMsRdpClient.AdvancedSettings3.RedirectPrinters = Favorite.RedirectPrinters;
                    _axMsRdpClient.AdvancedSettings3.RedirectSmartCards = Favorite.RedirectSmartCards;
                    _axMsRdpClient.AdvancedSettings3.PerformanceFlags = Favorite.PerformanceFlags;
                        
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
                    _axMsRdpClient.AdvancedSettings6.RedirectClipboard = Favorite.RedirectClipboard;
                    _axMsRdpClient.AdvancedSettings6.RedirectDevices = Favorite.RedirectDevices;
                    _axMsRdpClient.AdvancedSettings6.ConnectionBarShowMinimizeButton = false;
                    _axMsRdpClient.AdvancedSettings6.ConnectionBarShowPinButton = false;
                    _axMsRdpClient.AdvancedSettings6.ConnectionBarShowRestoreButton = false;

                    // Terminal Server Gateway Settings
                    _axMsRdpClient.TransportSettings.GatewayUsageMethod = (uint)Favorite.TsgwUsageMethod;
                    _axMsRdpClient.TransportSettings.GatewayCredsSource = (uint)Favorite.TsgwCredsSource;
                    _axMsRdpClient.TransportSettings.GatewayHostname = Favorite.TsgwHostname;
                    _axMsRdpClient.TransportSettings2.GatewayDomain = Favorite.TsgwDomain;
                    _axMsRdpClient.TransportSettings2.GatewayProfileUsageMethod = 1;
                    if (Favorite.TsgwSeparateLogin)
                    {
                        _axMsRdpClient.TransportSettings2.GatewayUsername = Favorite.TsgwUsername;
                        _axMsRdpClient.TransportSettings2.GatewayPassword = Favorite.TsgwPassword;
                    }
                    else
                    {
                        _axMsRdpClient.TransportSettings2.GatewayUsername = Favorite.UserName;
                        _axMsRdpClient.TransportSettings2.GatewayPassword = Favorite.Password;
                    }

                    if (Favorite.EnableTLSAuthentication)
                        _axMsRdpClient.AdvancedSettings5.AuthenticationLevel = 2;

                    _nonScriptable.EnableCredSspSupport = Favorite.EnableNLAAuthentication;

                    _axMsRdpClient.SecuredSettings2.AudioRedirectionMode = (int)Favorite.Sounds;

                    string domainName = Favorite.DomainName;
                    if(string.IsNullOrEmpty(domainName)) 
                        domainName = Settings.DefaultDomain;

                    string pass = Favorite.Password;
                    if (string.IsNullOrEmpty(pass))
                        pass = Settings.DefaultPassword;

                    string userName = Favorite.UserName;
                    if (string.IsNullOrEmpty(userName))
                        userName = Settings.DefaultUsername;

                    _axMsRdpClient.UserName = userName;
                    _axMsRdpClient.Domain = domainName;
                    try {
                        if(!String.IsNullOrEmpty(pass)) {
                            if(_nonScriptable != null)
                                _nonScriptable.ClearTextPassword = pass;
                        }
                    } catch(Exception exc) {
                        Logging.Log.Error("Error when trying to set the ClearTextPassword on the nonScriptable mstsc object", exc);
                    }

                    _axMsRdpClient.Server = Favorite.ServerName;
                    _axMsRdpClient.AdvancedSettings3.RDPPort = Favorite.Port;
                    _axMsRdpClient.AdvancedSettings3.ContainerHandledFullScreen = -1;
                    _axMsRdpClient.AdvancedSettings3.DisplayConnectionBar = Favorite.DisplayConnectionBar;

                    // Use ConnectToServerConsole or ConnectToAdministerServer based on implementation
                    _axMsRdpClient.AdvancedSettings7.ConnectToAdministerServer = Favorite.ConnectToConsole;
                    _axMsRdpClient.AdvancedSettings3.ConnectToServerConsole = Favorite.ConnectToConsole;

                    _axMsRdpClient.OnRequestGoFullScreen += new EventHandler(axMsTscAx_OnRequestGoFullScreen);
                    _axMsRdpClient.OnRequestLeaveFullScreen += new EventHandler(axMsTscAx_OnRequestLeaveFullScreen);
                    _axMsRdpClient.OnDisconnected += new IMsTscAxEvents_OnDisconnectedEventHandler(axMsTscAx_OnDisconnected);
                    _axMsRdpClient.OnWarning += new IMsTscAxEvents_OnWarningEventHandler(axMsRdpClient2_OnWarning);
                    _axMsRdpClient.OnFatalError += new IMsTscAxEvents_OnFatalErrorEventHandler(axMsRdpClient2_OnFatalError);

                    Text = "Connecting to RDP Server...";
                    _axMsRdpClient.FullScreen = true;
                }
                catch(Exception exc)
                {
                    Logging.Log.Info("There was an exception setting an RDP Value.", exc);
                }
                _axMsRdpClient.Connect();
                return true;
            }
            catch(Exception exc)
            {
                Logging.Log.Fatal("Connecting to RDP", exc);
                return false;
            }
        }        
        public override void Disconnect()
        {
            try
            {
                
                _axMsRdpClient.Disconnect();
            }
            catch(Exception e)
            {
                Logging.Log.Info("Error on Disconnect RDP", e);
            }
        }
        public override bool Connected { get { return Convert.ToBoolean(_axMsRdpClient.Connected); } }
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
            Native.Methods.PostMessage(new HandleRef(this, this.Handle), MainForm.WM_LEAVING_FULLSCREEN, IntPtr.Zero, IntPtr.Zero);
        }

        private void axMsTscAx_OnRequestGoFullScreen(object sender, EventArgs e)
        {
            ParentForm.tsbGrabInput.Checked = true;
            ParentForm.UpdateControls();
        }

        private void axMsTscAx_OnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e)
        {
            AxMsRdpClient6 client = (AxMsRdpClient6)sender;

            switch (e.discReason)
            {
                case 1:
                case 2:
                case 3:
                    // These are normal disconnects and not considered errors.
                    break;

                default:
                    string error = client.GetErrorDescription((uint)e.discReason, (uint)client.ExtendedDisconnectReason);
                    if ((error != null) && (error.Length > 0))
                        MessageBox.Show(this, String.Format("Error connecting to {0}\n\n{1}", client.Server, error), Program.Info.TitleVersion, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;
            }

            if (ParentForm.InvokeRequired)
            {
                InvokeCloseTabPage d = new InvokeCloseTabPage(CloseTabPage);
                this.Invoke(d, new object[] { client.Parent });
            }
            else
                CloseTabPage(client.Parent);

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
