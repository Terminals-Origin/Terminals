using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AxMSTSCLib;
using System.IO;
using System.Runtime.InteropServices;
using Terminals.Configuration;
using Terminals.Data;
using Terminals.Forms;
using Terminals.Forms.Controls;

//http://msdn.microsoft.com/en-us/library/aa381172(v=vs.85).aspx
//http://msdn.microsoft.com/en-us/library/aa380838(v=VS.85).aspx
//http://msdn.microsoft.com/en-us/library/aa380847(v=VS.85).aspx
//http://msdn.microsoft.com/en-us/library/bb892063(v=VS.85).aspx
//http://msdn.microsoft.com/en-us/library/ee338625(v=VS.85).aspx


namespace Terminals.Connections
{
    internal class RDPConnection : Connection
    {
        private readonly ReconnectingControl reconecting = new ReconnectingControl();
        private readonly ConnectionStateDetector connectionStateDetector = new ConnectionStateDetector();

        private MSTSCLib.IMsRdpClientNonScriptable4 nonScriptable;
        private AxMsRdpClient6NotSafeForScripting client = null;

        public delegate void Disconnected(RDPConnection Connection);
        public event Disconnected OnDisconnected;
        public delegate void ConnectionEstablish(RDPConnection Connection);
        public event ConnectionEstablish OnConnected;

        public AxMsRdpClient6 AxMsRdpClient
        {
            get
            {
                return null;
            }
        }

        #region IConnection Members

        public override bool Connected
        {
            get
            {
                // dont let the connection to close with running reconnection
                return Convert.ToBoolean(this.client.Connected) || this.connectionStateDetector.IsRunning;
            }
        }

        public override bool Connect()
        {
            try
            {
                if (!this.InitializeClientControl())
                    return false;
                this.ConfigureCientUserControl();
                ChangeDesktopSize(Favorite.Display.DesktopSize);

                try
                {
                    this.client.ConnectingText = "Connecting. Please wait...";
                    this.client.DisconnectedText = "Disconnecting...";

                    var rdpOptions = this.Favorite.ProtocolProperties as RdpOptions;
                    this.ConfigureColorsDepth();
                    this.ConfigureRedirectedDrives(rdpOptions);
                    this.ConfigureInterface(rdpOptions);
                    this.ConfigureStartBehaviour(rdpOptions);
                    this.ConfigureTimeouts(rdpOptions);
                    this.ConfigureRedirectOptions(rdpOptions);
                    this.ConfigureConnectionBar(rdpOptions);
                    this.ConfigureTsGateway(rdpOptions);
                    this.ConfigureSecurity(rdpOptions);
                    this.ConfigureConnection(rdpOptions);
                    this.AssignEventHandlers();

                    Text = "Connecting to RDP Server...";
                    this.client.FullScreen = true;
                }
                catch (Exception exc)
                {
                    Logging.Log.Info("There was an exception setting an RDP Value.", exc);
                }
                // if next line fails on Protected memory access exception,
                // some string property is set to null, which leads to this exception
                this.client.Connect();
                return true;
            }
            catch (Exception exc)
            {
                Logging.Log.Fatal("Connecting to RDP", exc);
                return false;
            }
        }

        private bool InitializeClientControl()
        {
            try
            {
                this.client = new AxMsRdpClient6NotSafeForScripting();
            }
            catch (Exception exception)
            {
                string message = "Please update your RDP client to at least version 6.";
                Logging.Log.Info(message, exception);
                MessageBox.Show(message);
                return false;
            }
            return true;
        }

        private void ConfigureCientUserControl()
        {
            var clientControl = (Control)this.client;
            this.Controls.Add(clientControl);
            this.client.CreateControl();
            nonScriptable = this.client.GetOcx() as MSTSCLib.IMsRdpClientNonScriptable4;
            this.client.BringToFront();
            this.BringToFront();
            this.client.Parent = this.TerminalTabPage;
            this.Parent = this.TerminalTabPage;
            this.client.AllowDrop = true;
            this.client.Dock = DockStyle.Fill;
            this.ConfigureReconnect();
        }

        private void ConfigureReconnect()
        {
            // if not added to the client control controls collection, then it isnt visible
            var clientControl = (Control)this.client;
            clientControl.Controls.Add(this.reconecting);
            this.reconecting.Hide();
            this.reconecting.AbortReconnectRequested += new EventHandler(this.Recoonecting_AbortReconnectRequested);
            this.connectionStateDetector.AssignFavorite(this.Favorite);
            this.connectionStateDetector.ReconnectExpired += ConnectionStateDetectorOnReconnectExpired;
            this.connectionStateDetector.Reconnected += ConnectionStateDetectorOnReconnected;
        }

        private void ConnectionStateDetectorOnReconnected(object sender, EventArgs eventArgs)
        {
            if (this.reconecting.InvokeRequired)
            {
                this.reconecting.Invoke(new EventHandler(this.ConnectionStateDetectorOnReconnected), new object[] { sender, eventArgs });
            }
            else
            {
                this.connectionStateDetector.Stop();
                this.reconecting.Hide();
                this.client.Connect();
            }
        }

        private void ConnectionStateDetectorOnReconnectExpired(object sender, EventArgs eventArgs)
        {
            this.CancelReconnect();
        }

        private void Recoonecting_AbortReconnectRequested(object sender, EventArgs e)
        {
            this.CancelReconnect();
        }

        private void CancelReconnect()
        {
            if (this.reconecting.InvokeRequired)
            {
                this.reconecting.Invoke(new Action(this.CancelReconnect));
            }
            else
            {
                this.connectionStateDetector.Stop();
                this.reconecting.Hide();
                this.FinishDisconnect();
            }
        }

        public override void ChangeDesktopSize(DesktopSize desktopSize)
        {
            Size size = ConnectionManager.GetSize(this, this.Favorite);

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

        private void ConfigureColorsDepth()
        {
            switch (this.Favorite.Display.Colors)
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
        }

        private void ConfigureRedirectedDrives(RdpOptions rdpOptions)
        {
            if (rdpOptions.Redirect.Drives.Count > 0 && rdpOptions.Redirect.Drives[0].Equals("true"))
                this.client.AdvancedSettings2.RedirectDrives = true;
            else
            {
                for (int i = 0; i < nonScriptable.DriveCollection.DriveCount; i++)
                {
                    MSTSCLib.IMsRdpDrive drive = nonScriptable.DriveCollection.get_DriveByIndex((uint)i);
                    foreach (string str in rdpOptions.Redirect.Drives)
                    {
                        if (drive.Name.IndexOf(str) > -1)
                            drive.RedirectionState = true;
                    }
                }
            }
        }

        private void ConfigureInterface(RdpOptions rdpOptions)
        {
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

            this.ConfigureCustomReconnect();
        }

        private void ConfigureCustomReconnect()
        {
            this.client.AdvancedSettings3.EnableAutoReconnect = false;
            this.client.AdvancedSettings3.MaxReconnectAttempts = 0;
            this.client.AdvancedSettings3.keepAliveInterval = 0;
        }

        private void ConfigureStartBehaviour(RdpOptions rdpOptions)
        {
            if (rdpOptions.GrabFocusOnConnect)
                this.client.AdvancedSettings2.GrabFocusOnConnect = true;

            if (rdpOptions.Security.Enabled)
            {
                if (rdpOptions.FullScreen)
                    this.client.SecuredSettings2.FullScreen = -1;

                this.client.SecuredSettings2.StartProgram = rdpOptions.Security.StartProgram;
                this.client.SecuredSettings2.WorkDir = rdpOptions.Security.WorkingFolder;
            }
        }

        private void ConfigureTimeouts(RdpOptions rdpOptions)
        {
            try
            {
                this.client.AdvancedSettings2.MinutesToIdleTimeout = rdpOptions.TimeOuts.IdleTimeout;

                int timeout = rdpOptions.TimeOuts.OverallTimeout;
                if (timeout > 600)
                    timeout = 10;
                if (timeout <= 0)
                    timeout = 10;
                this.client.AdvancedSettings2.overallConnectionTimeout = timeout;
                timeout = rdpOptions.TimeOuts.ConnectionTimeout;
                if (timeout > 600)
                    timeout = 10;
                if (timeout <= 0)
                    timeout = 10;

                this.client.AdvancedSettings2.singleConnectionTimeout = timeout;

                timeout = rdpOptions.TimeOuts.ShutdownTimeout;
                if (timeout > 600)
                    timeout = 10;
                if (timeout <= 0)
                    timeout = 10;
                this.client.AdvancedSettings2.shutdownTimeout = timeout;
            }
            catch (Exception exc)
            {
                Logging.Log.Error("Error when trying to set timeout values.", exc);
            }
        }

        private void ConfigureRedirectOptions(RdpOptions rdpOptions)
        {
            this.client.AdvancedSettings3.RedirectPorts = rdpOptions.Redirect.Ports;
            this.client.AdvancedSettings3.RedirectPrinters = rdpOptions.Redirect.Printers;
            this.client.AdvancedSettings3.RedirectSmartCards = rdpOptions.Redirect.SmartCards;
            this.client.AdvancedSettings3.PerformanceFlags = rdpOptions.UserInterface.PerformanceFlags;
            this.client.AdvancedSettings6.RedirectClipboard = rdpOptions.Redirect.Clipboard;
            this.client.AdvancedSettings6.RedirectDevices = rdpOptions.Redirect.Devices;
        }

        private void ConfigureConnectionBar(RdpOptions rdpOptions)
        {
            this.client.AdvancedSettings6.ConnectionBarShowMinimizeButton = false;
            this.client.AdvancedSettings6.ConnectionBarShowPinButton = false;
            this.client.AdvancedSettings6.ConnectionBarShowRestoreButton = false;
            this.client.AdvancedSettings3.DisplayConnectionBar = rdpOptions.UserInterface.DisplayConnectionBar;
        }

        private void ConfigureTsGateway(RdpOptions rdpOptions)
        {
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
                this.client.TransportSettings2.GatewayUsername = this.Favorite.Security.UserName;
                this.client.TransportSettings2.GatewayPassword = this.Favorite.Security.Password;
            }
        }

        private void ConfigureSecurity(RdpOptions rdpOptions)
        {
            if (rdpOptions.Security.EnableTLSAuthentication)
                this.client.AdvancedSettings5.AuthenticationLevel = 2;

            this.nonScriptable.EnableCredSspSupport = rdpOptions.Security.EnableNLAAuthentication;

            this.client.SecuredSettings2.AudioRedirectionMode = (int)rdpOptions.Redirect.Sounds;

            ISecurityOptions security = this.Favorite.Security.GetResolvedCredentials();

            this.client.UserName = security.UserName;
            this.client.Domain = security.Domain;
            try
            {
                if (!String.IsNullOrEmpty(security.Password))
                {
                    if (this.nonScriptable != null)
                        this.nonScriptable.ClearTextPassword = security.Password;
                }
            }
            catch (Exception exc)
            {
                Logging.Log.Error("Error when trying to set the ClearTextPassword on the nonScriptable mstsc object", exc);
            }
        }

        private void ConfigureConnection(RdpOptions rdpOptions)
        {
            this.client.Server = this.Favorite.ServerName;
            this.client.AdvancedSettings3.RDPPort = this.Favorite.Port;
            this.client.AdvancedSettings3.ContainerHandledFullScreen = -1;
            // Use ConnectToServerConsole or ConnectToAdministerServer based on implementation
            this.client.AdvancedSettings7.ConnectToAdministerServer = rdpOptions.ConnectToConsole;
            this.client.AdvancedSettings3.ConnectToServerConsole = rdpOptions.ConnectToConsole;
        }

        private void AssignEventHandlers()
        {
            this.client.OnRequestGoFullScreen += new EventHandler(this.client_OnRequestGoFullScreen);
            this.client.OnRequestLeaveFullScreen += new EventHandler(this.client_OnRequestLeaveFullScreen);
            this.client.OnDisconnected += new IMsTscAxEvents_OnDisconnectedEventHandler(this.client_OnDisconnected);
            this.client.OnWarning += new IMsTscAxEvents_OnWarningEventHandler(this.client_OnWarning);
            this.client.OnFatalError += new IMsTscAxEvents_OnFatalErrorEventHandler(this.client_OnFatalError);
            this.client.OnLogonError += new IMsTscAxEvents_OnLogonErrorEventHandler(this.client_OnLogonError);
            this.client.OnConnected += new EventHandler(this.client_OnConnected);
            // assign the drag and drop event handlers directly throws an exception
            var clientControl = (Control)this.client;
            clientControl.DragEnter += new DragEventHandler(this.client_DragEnter);
            clientControl.DragDrop += new DragEventHandler(this.client_DragDrop);
        }

        public override void Disconnect()
        {
            try
            {
                this.connectionStateDetector.Disable();
                this.client.Disconnect();
            }
            catch (Exception e)
            {
                Logging.Log.Info("Error on Disconnect RDP", e);
            }
        }

        #endregion

        #region Eventing

        private void client_OnConnected(object sender, EventArgs e)
        {
            if (this.OnConnected != null)
                this.OnConnected(this);
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
            if (DecideToReconnect(e))
            {
                this.Reconnect();
            }
            else
            {
                this.ShowDisconnetMessageBox(e);
                this.FinishDisconnect();
            }
        }

        private static bool DecideToReconnect(IMsTscAxEvents_OnDisconnectedEvent e)
        {
            // 516 reason in case of reconnect expired
            // 2308 connection lost
            // 2 - reboot or shutdown
            if (e.discReason != 2308 && e.discReason != 2)
                return false;

            if (!Settings.AskToReconnect)
                return true;

            return AskToReconnect();
        }

        private static bool AskToReconnect()
        {
            const string MESSAGE = "Do you want to try reconnect?";
            YesNoDisableResult answer = YesNoDisableForm.ShowDialog("Connection to server lost", MESSAGE);
            if (answer.Disable)
                Settings.AskToReconnect = false;
            return answer.Result == DialogResult.Yes;
        }

        private void Reconnect()
        {
            this.reconecting.Show();
            this.reconecting.BringToFront();
            this.connectionStateDetector.Start();
        }

        private void FinishDisconnect()
        {
            this.CloseTabPageOnParent();
            this.FireDisconnected();
        }

        private void ShowDisconnetMessageBox(IMsTscAxEvents_OnDisconnectedEvent e)
        {
            int reason = e.discReason;
            string error = RdpClientErrorMessages.ToDisconnectMessage(this.client, reason);

            if (!string.IsNullOrEmpty(error))
            {
                string message = String.Format("Error connecting to {0}\n\n{1}", this.client.Server, error);
                MessageBox.Show(this, message, Program.Info.TitleVersion, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void CloseTabPageOnParent()
        {
            if (this.ParentForm.InvokeRequired)
            {
                this.Invoke(new InvokeCloseTabPage(this.CloseTabPage), new object[] { this.client.Parent });
            }
            else
            {
                this.CloseTabPage(this.client.Parent);
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
            string message = RdpClientErrorMessages.ToFatalErrorMessage(errorCode);
            string finalMsg = string.Format("There was a fatal error returned from the RDP Connection, details:\n\nError Code:{0}\n\nError Description:{1}", errorCode, message);
            MessageBox.Show(finalMsg);
            Logging.Log.Fatal(finalMsg);
        }

        private void client_OnWarning(object sender, IMsTscAxEvents_OnWarningEvent e)
        {
            int warningCode = e.warningCode;
            string message = RdpClientErrorMessages.ToWarningMessage(warningCode);
            string finalMsg = string.Format("There was a warning returned from the RDP Connection, details:\n\nWarning Code:{0}\n\nWarning Description:{1}", warningCode, message);
            Logging.Log.Warn(finalMsg);
        }

        private void client_OnLogonError(object sender, IMsTscAxEvents_OnLogonErrorEvent e)
        {
            int errorCode = e.lError;
            string message = RdpClientErrorMessages.ToLogonMessage(errorCode);
            string finalMsg = string.Format("There was a logon error returned from the RDP Connection, details:\n\nLogon Code:{0}\n\nLogon Description:{1}", errorCode, message);
            Logging.Log.Error(finalMsg);
        }

        #endregion
    }
}
