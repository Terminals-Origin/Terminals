using System;
using System.Drawing;
using AxVMRCClientControlLib;
using Terminals.Configuration;

namespace Terminals.Connections
{
    internal class VMRCConnection : Connection
    {
        #region IConnection Members
        private bool connected = false;
        public override bool Connected { get { return connected; } }
        public override void ChangeDesktopSize(DesktopSize Size)
        {
        }

        AxVMRCClientControl vmrc;
        public override bool Connect()
        {
            try
            {
                vmrc = new AxVMRCClientControl();
                Controls.Add(vmrc);
                vmrc.BringToFront();
                this.BringToFront();
                vmrc.Parent = TerminalTabPage;
                this.Parent = TerminalTabPage;

                //vmrc.CtlAutoSize = true;

                string domainName = Favorite.DomainName;
                string pass = Favorite.Password;
                string userName = Favorite.UserName;

                if (string.IsNullOrEmpty(domainName)) domainName = Settings.DefaultDomain;
                if (string.IsNullOrEmpty(pass)) pass = Settings.DefaultPassword;
                if (string.IsNullOrEmpty(userName)) userName = Settings.DefaultUsername;

                // vmrc.Dock = DockStyle.Fill;
                vmrc.UserName = userName;
                vmrc.ServerAddress = Favorite.ServerName;
                vmrc.ServerPort = Favorite.Port;
                vmrc.UserDomain = domainName;
                vmrc.UserPassword = pass;
                vmrc.AdministratorMode = Favorite.VMRCAdministratorMode;
                vmrc.ReducedColorsMode = Favorite.VMRCReducedColorsMode;

                Size size = ConnectionManager.GetSize(this, Favorite);
                //vmrc.ServerDisplayHeight = size.Height;
                //vmrc.ServerDisplayWidth = size.Width;

                vmrc.OnStateChanged += new _IVMRCClientControlEvents_OnStateChangedEventHandler(vmrc_OnStateChanged);
                vmrc.OnSwitchedDisplay += new _IVMRCClientControlEvents_OnSwitchedDisplayEventHandler(vmrc_OnSwitchedDisplay);

                Text = "Connecting to VMRC Server...";
                vmrc.Connect();
                //vmrc.BringToFront();
                //vmrc.Update();
                return true;
            }
            catch (Exception exc)
            {
                Logging.Log.Fatal("Connecting to VMRC", exc);
                return false;
            }
        }

        private void vmrc_OnSwitchedDisplay(object sender, _IVMRCClientControlEvents_OnSwitchedDisplayEvent e)
        {
            Text = e.displayName;
        }

        private void vmrc_OnStateChanged(object sender, _IVMRCClientControlEvents_OnStateChangedEvent e)
        {
            if (e.state == VMRCClientControlLib.VMRCState.vmrcState_Connected)
                this.connected = true;
            else
            {
                connected = false;
                Logging.Log.Fatal("VMRC Connection Lost" + this.Favorite.Name);
                this.connected = false;

                if (ParentForm.InvokeRequired)
                {
                    InvokeCloseTabPage d = new InvokeCloseTabPage(CloseTabPage);
                    this.Invoke(d, new object[] { this.Parent });
                }
                else
                    CloseTabPage(this.Parent);
            }
        }

        public bool ViewOnlyMode
        {
            get
            {
                if (vmrc != null && connected) return vmrc.ViewOnlyMode;
                return false;
            }
            set
            {
                if (vmrc != null && connected) vmrc.ViewOnlyMode = value;
            }
        }
        public void SendKeySequence(string Keys)
        {
            if (vmrc != null && connected) vmrc.SendKeySequence(Keys);
        }

        public void AdminDisplay()
        {
            if (vmrc != null && connected)
            {
                vmrc.AdministratorMode = true;
                vmrc.AdminDisplay();
            }
        }


        public override void Disconnect()
        {
            try
            {
                vmrc.Disconnect();
            }
            catch (Exception e)
            {
                Logging.Log.Error("Disconnect", e);
            }
        }

        #endregion
    }
}