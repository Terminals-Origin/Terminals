using System;
using System.Drawing;
using AxVMRCClientControlLib;
using Terminals.Configuration;
using Terminals.Data;

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

                vmrc.ServerAddress = Favorite.ServerName;
                vmrc.ServerPort = Favorite.Port;
                ISecurityOptions security = this.Favorite.Security.GetResolvedCredentials();
                vmrc.UserName = security.UserName;
                vmrc.UserDomain = security.Domain;
                vmrc.UserPassword = security.Password;

                var options = this.Favorite.ProtocolProperties as VMRCOptions;
                vmrc.AdministratorMode = options.AdministratorMode;
                vmrc.ReducedColorsMode = options.ReducedColorsMode;

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
                Logging.Fatal("Connecting to VMRC", exc);
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
                Logging.Fatal("VMRC Connection Lost" + this.Favorite.Name);
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
                Logging.Error("Disconnect", e);
            }
        }

        #endregion
    }
}