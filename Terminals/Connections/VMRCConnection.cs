using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using AxVMRCClientControlLib;
using System.Runtime.InteropServices;
using TabControl;
using System.IO;

namespace Terminals.Connections {
    public class VMRCConnection : Connection {
        #region IConnection Members
        private bool connected = false;
        public override bool Connected { get { return connected; } }
        public override void ChangeDesktopSize(Terminals.DesktopSize Size) {
        }

        AxVMRCClientControlLib.AxVMRCClientControl vmrc;
        public override bool Connect() {
            try
            {
                vmrc = new AxVMRCClientControlLib.AxVMRCClientControl();
                Controls.Add((System.Windows.Forms.Control)vmrc);
                vmrc.BringToFront();
                this.BringToFront();
                vmrc.Parent = base.TerminalTabPage;
                this.Parent = TerminalTabPage;

                //vmrc.CtlAutoSize = true;

                string domainName = Favorite.DomainName;
                string pass = Favorite.Password;
                string userName = Favorite.UserName;

                if(string.IsNullOrEmpty(domainName)) domainName = Settings.DefaultDomain;
                if(string.IsNullOrEmpty(pass)) pass = Settings.DefaultPassword;
                if(string.IsNullOrEmpty(userName)) userName = Settings.DefaultUsername;

                // vmrc.Dock = DockStyle.Fill;
                vmrc.UserName = userName;
                vmrc.ServerAddress = Favorite.ServerName;
                vmrc.ServerPort = Favorite.Port;
                vmrc.UserDomain = domainName;
                vmrc.UserPassword = pass;
                vmrc.AdministratorMode = Favorite.VMRCAdministratorMode;
                vmrc.ReducedColorsMode = Favorite.VMRCReducedColorsMode;

                int height = Favorite.DesktopSizeHeight, width = Favorite.DesktopSizeWidth;
                ConnectionManager.GetSize(ref height, ref width, this, Favorite.DesktopSize);

                //vmrc.ServerDisplayHeight = height;
                //vmrc.ServerDisplayWidth = width;
                vmrc.OnStateChanged += new _IVMRCClientControlEvents_OnStateChangedEventHandler(vmrc_OnStateChanged);

                //vmrc.ServerDisplayHeight;
                //vmrc.ServerDisplayWidth;

                vmrc.OnSwitchedDisplay += new _IVMRCClientControlEvents_OnSwitchedDisplayEventHandler(vmrc_OnSwitchedDisplay);

                Text = "Connecting to VMRC Server...";
                vmrc.Connect();
                //vmrc.BringToFront();
                //vmrc.Update();
                return true;
            }
            catch(Exception exc)
            {
                Terminals.Logging.Log.Fatal("Connecting to VMRC", exc);
                return false;
            }
        }

        void vmrc_OnSwitchedDisplay(object sender, _IVMRCClientControlEvents_OnSwitchedDisplayEvent e)
        {
            Text = e.displayName;
        }

        void vmrc_OnStateChanged(object sender, _IVMRCClientControlEvents_OnStateChangedEvent e)
        {
            if(e.state == VMRCClientControlLib.VMRCState.vmrcState_Connected) 
                this.connected = true;
            else if(e.state == VMRCClientControlLib.VMRCState.vmrcState_ConnectionFailed) {
                connected = false;
                Terminals.Logging.Log.Fatal("VMRC Connection Lost" + this.Favorite.Name);
                this.connected = false;

                TabControlItem selectedTabPage = (TabControlItem)(this.Parent);
                bool wasSelected = selectedTabPage.Selected;
                ParentForm.tcTerminals.RemoveTab(selectedTabPage);
                ParentForm.CloseTabControlItem();
                if(wasSelected)
                    NativeApi.PostMessage(new HandleRef(this, this.Handle), MainForm.WM_LEAVING_FULLSCREEN, IntPtr.Zero, IntPtr.Zero);
                ParentForm.UpdateControls();
            } else if(e.state == VMRCClientControlLib.VMRCState.vmrcState_NotConnected) {
                connected = false;
            }
        }

        public bool ViewOnlyMode  {
            get {
                if (vmrc != null && connected) return vmrc.ViewOnlyMode ;
                return false;
            }
            set {
                if (vmrc != null && connected) vmrc.ViewOnlyMode = value;
            }
        }
        public void SendKeySequence(string Keys) {
            if (vmrc != null && connected) vmrc.SendKeySequence(Keys);
        }
        
        public void AdminDisplay() {
            if (vmrc != null && connected) {
                vmrc.AdministratorMode = true;
                vmrc.AdminDisplay();
            }
        }


        public override void Disconnect() {
            try {
                vmrc.Disconnect();
            } catch (Exception e) {
                Terminals.Logging.Log.Info("", e);
            }
        }

        #endregion
    }
}