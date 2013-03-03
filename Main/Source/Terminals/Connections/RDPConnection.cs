using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AxMSTSCLib;
using System.IO;
using System.Runtime.InteropServices;
using Terminals.Data;

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
        private AxMsRdpClient6 client = null;

        public delegate void Disconnected(RDPConnection Connection);
        public event Disconnected OnDisconnected;
        public delegate void ConnectionEstablish(RDPConnection Connection);
        public event ConnectionEstablish OnConnected;

        public AxMsRdpClient6 AxMsRdpClient
        {
            get
            {
                return this.client;
            }
        }

        #region IConnection Members

        public override bool Connected
        {
            get
            {
                return Convert.ToBoolean(this.client.Connected);
            }
        }

        public override void ChangeDesktopSize(DesktopSize desktopSize)
        {
            Size size = ConnectionManager.GetSize(this, Favorite);

            try
            {
                switch (desktopSize)
                {
                    case DesktopSize.AutoScale:
                    case DesktopSize.FitToWindow:
                        this.client.AdvancedSettings3.SmartSizing = true;
                        break;
                    case DesktopSize.FullScreen:
                        this.client.FullScreen = true;
                        break;
                }

                this.client.DesktopWidth = size.Width;
                this.client.DesktopHeight = size.Height;
            }
            catch (Exception exc)
            {
                Logging.Log.Error("Error trying to set the desktop dimensions", exc);
            }
        }

        public override bool Connect()
        {
            try
            {
                // todo replace the client with not safe for scripting prototype
                //var client = new AxMsRdpClient6NotSafeForScripting();
                //Control control = (Control)client;
                //Controls.Add(control);
                //client.Connect();
                //client.CreateControl();
                this.client = new AxMsRdpClient6();
            }
            catch (Exception exc)
            {
                String msg = "Please update your RDP client to at least version 6.";
                Logging.Log.Info(msg, exc);
                MessageBox.Show(msg);
                return false;
            }

            try
            {
                Controls.Add(this.client);
                this.client.BringToFront();
                this.BringToFront();
                this.client.Parent = TerminalTabPage;
                this.Parent = TerminalTabPage;
                this.client.AllowDrop = true;

                ((Control)this.client).DragEnter += new DragEventHandler(this.client_DragEnter);
                ((Control)this.client).DragDrop += new DragEventHandler(this.client_DragDrop);
                this.client.OnConnected += new EventHandler(this.client_OnConnected);
                this.client.Dock = DockStyle.Fill;
                _nonScriptable = (this.client.GetOcx() as MSTSCLib.IMsRdpClientNonScriptable4);

                ChangeDesktopSize(Favorite.Display.DesktopSize);
                try
                {
                    //if(Favorite.DesktopSize == DesktopSize.AutoScale) axMsRdpClient2.AdvancedSettings3.SmartSizing = true;
                    //axMsRdpClient2.DesktopWidth = width;
                    //axMsRdpClient2.DesktopHeight = height;

                    switch (Favorite.Display.Colors)
                    {
                        case Colors.Bits8:
                            this.client.ColorDepth = 8;
                            break;
                        case Colors.Bit16:
                            this.client.ColorDepth = 16;
                            break;
                        case Colors.Bits24:
                            this.client.ColorDepth = 24;
                            break;
                        case Colors.Bits32:
                            this.client.ColorDepth = 32;
                            break;
                    }

                    this.client.ConnectingText = "Connecting. Please wait...";
                    this.client.DisconnectedText = "Disconnecting...";

                    var rdpOptions = this.Favorite.ProtocolProperties as RdpOptions;
                    if (rdpOptions.Redirect.Drives.Count > 0 && rdpOptions.Redirect.Drives[0].Equals("true"))
                    {
                        this.client.AdvancedSettings2.RedirectDrives = true;
                    }
                    else
                    {
                        for (int i = 0; i < _nonScriptable.DriveCollection.DriveCount; i++)
                        {
                            MSTSCLib.IMsRdpDrive drive = _nonScriptable.DriveCollection.get_DriveByIndex((uint)i);
                            foreach (string str in rdpOptions.Redirect.Drives)
                                if (drive.Name.IndexOf(str) > -1)
                                    drive.RedirectionState = true;
                        }
                    }

                    //advanced settings
                    //bool, 0 is false, other is true
                    if (rdpOptions.UserInterface.AllowBackgroundInput)
                        this.client.AdvancedSettings.allowBackgroundInput = -1;

                    if (rdpOptions.UserInterface.BitmapPeristence)
                        this.client.AdvancedSettings.BitmapPeristence = -1;

                    if (rdpOptions.UserInterface.EnableCompression)
                        this.client.AdvancedSettings.Compress = -1;

                    if (rdpOptions.UserInterface.AcceleratorPassthrough)
                        this.client.AdvancedSettings2.AcceleratorPassthrough = -1;

                    if (rdpOptions.UserInterface.DisableControlAltDelete)
                        this.client.AdvancedSettings2.DisableCtrlAltDel = -1;

                    if (rdpOptions.UserInterface.DisplayConnectionBar)
                        this.client.AdvancedSettings2.DisplayConnectionBar = true;

                    if (rdpOptions.UserInterface.DoubleClickDetect)
                        this.client.AdvancedSettings2.DoubleClickDetect = -1;

                    if (rdpOptions.UserInterface.DisableWindowsKey)
                        this.client.AdvancedSettings2.EnableWindowsKey = -1;

                    if (rdpOptions.Security.EnableEncryption)
                        this.client.AdvancedSettings2.EncryptionEnabled = -1;


                    if (rdpOptions.GrabFocusOnConnect)
                        this.client.AdvancedSettings2.GrabFocusOnConnect = true;

                    if (rdpOptions.Security.Enabled)
                    {
                        if (rdpOptions.FullScreen)
                            this.client.SecuredSettings2.FullScreen = -1;

                        this.client.SecuredSettings2.StartProgram = rdpOptions.Security.StartProgram;
                        this.client.SecuredSettings2.WorkDir = rdpOptions.Security.WorkingFolder;
                    }

                    this.client.AdvancedSettings2.MinutesToIdleTimeout = rdpOptions.TimeOuts.IdleTimeout;

                    try
                    {
                        int timeout = rdpOptions.TimeOuts.OverallTimeout;
                        if (timeout > 600) timeout = 10;
                        if (timeout <= 0) timeout = 10;
                        this.client.AdvancedSettings2.overallConnectionTimeout = timeout;
                        timeout = rdpOptions.TimeOuts.ConnectionTimeout;
                        if (timeout > 600) timeout = 10;
                        if (timeout <= 0) timeout = 10;

                        this.client.AdvancedSettings2.singleConnectionTimeout = timeout;

                        timeout = rdpOptions.TimeOuts.ShutdownTimeout;
                        if (timeout > 600) timeout = 10;
                        if (timeout <= 0) timeout = 10;
                        this.client.AdvancedSettings2.shutdownTimeout = timeout;


                        //axMsRdpClient2.AdvancedSettings2.PinConnectionBar;
                        //axMsRdpClient2.AdvancedSettings2.TransportType;
                        //axMsRdpClient2.AdvancedSettings2.WinceFixedPalette;
                        //axMsRdpClient2.AdvancedSettings3.CanAutoReconnect = Favorite.CanAutoReconnect;
                    }
                    catch (Exception exc)
                    {
                        Logging.Log.Error("Error when trying to set timeout values.", exc);
                    }

                    this.client.AdvancedSettings3.RedirectPorts = rdpOptions.Redirect.Ports;
                    this.client.AdvancedSettings3.RedirectPrinters = rdpOptions.Redirect.Printers;
                    this.client.AdvancedSettings3.RedirectSmartCards = rdpOptions.Redirect.SmartCards;
                    this.client.AdvancedSettings3.PerformanceFlags = rdpOptions.UserInterface.PerformanceFlags;

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
                    this.client.AdvancedSettings6.RedirectClipboard = rdpOptions.Redirect.Clipboard;
                    this.client.AdvancedSettings6.RedirectDevices = rdpOptions.Redirect.Devices;
                    this.client.AdvancedSettings6.ConnectionBarShowMinimizeButton = false;
                    this.client.AdvancedSettings6.ConnectionBarShowPinButton = false;
                    this.client.AdvancedSettings6.ConnectionBarShowRestoreButton = false;

                    // Terminal Server Gateway Settings
                    this.client.TransportSettings.GatewayUsageMethod = (uint)rdpOptions.TsGateway.UsageMethod;
                    this.client.TransportSettings.GatewayCredsSource = (uint)rdpOptions.TsGateway.CredentialSource;
                    this.client.TransportSettings.GatewayHostname = rdpOptions.TsGateway.HostName;
                    this.client.TransportSettings2.GatewayDomain = rdpOptions.TsGateway.Security.Domain;
                    this.client.TransportSettings2.GatewayProfileUsageMethod = 1;
                    if (rdpOptions.TsGateway.SeparateLogin)
                    {
                        this.client.TransportSettings2.GatewayUsername = rdpOptions.TsGateway.Security.UserName;
                        this.client.TransportSettings2.GatewayPassword = rdpOptions.TsGateway.Security.Password;
                    }
                    else
                    {
                        this.client.TransportSettings2.GatewayUsername = Favorite.Security.UserName;
                        this.client.TransportSettings2.GatewayPassword = Favorite.Security.Password;
                    }

                    if (rdpOptions.Security.EnableTLSAuthentication)
                        this.client.AdvancedSettings5.AuthenticationLevel = 2;

                    _nonScriptable.EnableCredSspSupport = rdpOptions.Security.EnableNLAAuthentication;

                    this.client.SecuredSettings2.AudioRedirectionMode = (int)rdpOptions.Redirect.Sounds;

                    ISecurityOptions security = Favorite.Security.GetResolvedCredentials();

                    this.client.UserName = security.UserName;
                    this.client.Domain = security.Domain;
                    try
                    {
                        if (!String.IsNullOrEmpty(security.Password))
                        {
                            if (_nonScriptable != null)
                                _nonScriptable.ClearTextPassword = security.Password;
                        }
                    }
                    catch (Exception exc)
                    {
                        Logging.Log.Error("Error when trying to set the ClearTextPassword on the nonScriptable mstsc object", exc);
                    }

                    this.client.Server = Favorite.ServerName;
                    this.client.AdvancedSettings3.RDPPort = Favorite.Port;
                    this.client.AdvancedSettings3.ContainerHandledFullScreen = -1;
                    this.client.AdvancedSettings3.DisplayConnectionBar = rdpOptions.UserInterface.DisplayConnectionBar;

                    // Use ConnectToServerConsole or ConnectToAdministerServer based on implementation
                    this.client.AdvancedSettings7.ConnectToAdministerServer = rdpOptions.ConnectToConsole;
                    this.client.AdvancedSettings3.ConnectToServerConsole = rdpOptions.ConnectToConsole;

                    this.client.OnRequestGoFullScreen += new EventHandler(this.client_OnRequestGoFullScreen);
                    this.client.OnRequestLeaveFullScreen += new EventHandler(this.client_OnRequestLeaveFullScreen);
                    this.client.OnDisconnected += new IMsTscAxEvents_OnDisconnectedEventHandler(this.client_OnDisconnected);
                    this.client.OnWarning += new IMsTscAxEvents_OnWarningEventHandler(this.client_OnWarning);
                    this.client.OnFatalError += new IMsTscAxEvents_OnFatalErrorEventHandler(this.client_OnFatalError);
                    this.client.OnLogonError += new IMsTscAxEvents_OnLogonErrorEventHandler(this.client_OnLogonError);

                    Text = "Connecting to RDP Server...";
                    this.client.FullScreen = true;
                }
                catch (Exception exc)
                {
                    Logging.Log.Info("There was an exception setting an RDP Value.", exc);
                }
                this.client.Connect();
                return true;
            }
            catch (Exception exc)
            {
                Logging.Log.Fatal("Connecting to RDP", exc);
                return false;
            }
        }

        public override void Disconnect()
        {
            try
            {
                this.client.Disconnect();
            }
            catch (Exception e)
            {
                Logging.Log.Info("Error on Disconnect RDP", e);
            }
        }

        #endregion

        #region Private event

        private void client_OnConnected(object sender, EventArgs e)
        {
            if (this.OnConnected != null) this.OnConnected(this);
        }

        private void client_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string desktopShare = ParentForm.GetDesktopShare();
            if (String.IsNullOrEmpty(desktopShare))
            {
                MessageBox.Show(this, "A Desktop Share was not defined for this connection.\n" +
                    "Please define a share in the connection properties window (under the Local Resources tab)."
                    , "Terminals", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                SHCopyFiles(files, desktopShare);
            }
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

        private void client_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void client_OnRequestLeaveFullScreen(object sender, EventArgs e)
        {
            ParentForm.tsbGrabInput.Checked = false;
            ParentForm.UpdateControls();
            Native.Methods.PostMessage(new HandleRef(this, this.Handle), MainForm.WM_LEAVING_FULLSCREEN, IntPtr.Zero, IntPtr.Zero);
        }

        private void client_OnRequestGoFullScreen(object sender, EventArgs e)
        {
            ParentForm.tsbGrabInput.Checked = true;
            ParentForm.UpdateControls();
        }

        private void client_OnDisconnected(object sender, IMsTscAxEvents_OnDisconnectedEvent e)
        {
            var client = (AxMsRdpClient6)sender;
            this.ShowDisconnetMessageBox(e, client);
            this.CloseTabPageOnParent(client);
            this.FireDisconnected();
        }

        private void ShowDisconnetMessageBox(IMsTscAxEvents_OnDisconnectedEvent e, AxMsRdpClient6 client)
        {
            int reason = e.discReason;
            string error = ToDisconnectMessage(client, reason);
            
            if (!string.IsNullOrEmpty(error))
            {
                string message = String.Format("Error connecting to {0}\n\n{1}", client.Server, error);
                MessageBox.Show(this, message, Program.Info.TitleVersion, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private static string ToDisconnectMessage(AxMsRdpClient6 client, int reason)
        {
            switch (reason)
            {
                case 1:
                case 2:
                case 3:
                    // These are normal disconnects and not considered errors.
                    return string.Empty;

                default:
                    return client.GetErrorDescription((uint)reason, (uint)client.ExtendedDisconnectReason);
            }
        }

        private void CloseTabPageOnParent(AxMsRdpClient6 client)
        {
            if (this.ParentForm.InvokeRequired)
            {
                this.Invoke(new InvokeCloseTabPage(this.CloseTabPage), new object[] {client.Parent});
            }
            else
            {
                this.CloseTabPage(client.Parent);
            }
        }

        private void FireDisconnected()
        {
            if (this.OnDisconnected != null)
                this.OnDisconnected(this);
        }

        private void client_OnFatalError(object sender, IMsTscAxEvents_OnFatalErrorEvent e)
        {
            int errorCode = e.errorCode;
            string message = ToFatalErrorMessage(errorCode);
            string finalMsg = string.Format("There was a fatal error returned from the RDP Connection, details:\n\nError Code:{0}\n\nError Description:{1}", errorCode, message);
            MessageBox.Show(finalMsg);
            Logging.Log.Fatal(finalMsg);
        }

        private static string ToFatalErrorMessage(int errorCode)
        {
            switch (errorCode)
            {
                case 0:
                    return "An unknown error has occurred.";
                case 1:
                    return "Internal error code 1.";
                case 2:
                    return "An out-of-memory error has occurred.";
                case 3:
                    return "A window-creation error has occurred.";
                case 4:
                    return "Internal error code 2.";
                case 5:
                    return "Internal error code 3. This is not a valid state.";
                case 6:
                    return "Internal error code 4.";
                case 7:
                    return "An unrecoverable error has occurred during client connection.";
                case 100:
                    return "Winsock initialization error.";
                default:
                    return "An unknown error.";
            }
        }

        private void client_OnWarning(object sender, IMsTscAxEvents_OnWarningEvent e)
        {
            int warningCode = e.warningCode;
            string message = ToWarningMessage(warningCode);
            string finalMsg = string.Format("There was a warning returned from the RDP Connection, details:\n\nWarning Code:{0}\n\nWarning Description:{1}", warningCode, message);
            Logging.Log.Warn(finalMsg);
        }

        private static string ToWarningMessage(int warningCode)
        {
            switch (warningCode)
            {
                case 1:
                    return "Bitmap cache is corrupt.";
                default:
                    return "An unknown warning";
            }
        }

        private void client_OnLogonError(object sender, IMsTscAxEvents_OnLogonErrorEvent e)
        {
            int errorCode = e.lError;
            string message = ToLogonMessage(errorCode);
            string finalMsg = string.Format("There was a logon error returned from the RDP Connection, details:\n\nLogon Code:{0}\n\nLogon Description:{1}", errorCode, message);
            Logging.Log.Error(finalMsg);
        }

        private static string ToLogonMessage(int errorCode)
        {
            switch (errorCode)
            {
                case -5:
                    return "Winlogon is displaying the Session Contention dialog box.";
                case -2:
                    return "Winlogon is continuing with the logon process.";
                case -3:
                    return "Winlogon is ending silently.";
                case -6:
                    return "Winlogon is displaying the No Permissions dialog box.";
                case -7:
                    return "Winlogon is displaying the Disconnect Refused dialog box.";
                case -4:
                    return "Winlogon is displaying the Reconnect dialog box.";
                case -1:
                    return "The user was denied access.";
                case 0:
                    return "The logon failed because the logon credentials are not valid.";
                case 2:
                    return "Another logon or post-logon error occurred. The Remote Desktop client displays a logon screen to the user.";
                case 1:
                    return "The password is expired. The user must update their password to continue logging on.";
                case 3:
                    return "The Remote Desktop client displays a dialog box that contains important information for the user.";
                case -1073741714:
                    return "The user name and authentication information are valid, but authentication was blocked due to restrictions on the user account, such as time-of-day restrictions.";
                case -1073741715:
                    return "The attempted logon is not valid. This is due to either an incorrect user name or incorrect authentication information.";
                case -1073741276:
                    return "The password is expired. The user must update their password to continue logging on.";
                default:
                    return "An unknown error.";
            }
        }

        #endregion
    }
}
