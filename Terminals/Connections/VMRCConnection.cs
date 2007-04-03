using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Terminals.Connections {
    public class VMRCConnection : Connection {
        #region IConnection Members
        private bool connected = false;
        public override bool Connected { get { return connected; } }
        public override void ChangeDesktopSize(Terminals.DesktopSize Size) {
        }

        AxVMRCClientControlLib.AxVMRCClientControl vmrc = new AxVMRCClientControlLib.AxVMRCClientControl();
        public override bool Connect() {
            Controls.Add(vmrc);
            vmrc.BringToFront();
            this.BringToFront();
            vmrc.Parent = base.TerminalTabPage;
            this.Parent = TerminalTabPage;

            vmrc.CtlAutoSize = true;

            vmrc.Dock = DockStyle.Fill;
            vmrc.UserName = Favorite.UserName;
            vmrc.ServerAddress = Favorite.ServerName;
            vmrc.ServerPort = Favorite.Port;
            vmrc.UserDomain = Favorite.DomainName;
            vmrc.UserPassword = Favorite.Password;
            vmrc.AdministratorMode = Favorite.VMRCAdministratorMode;
            vmrc.ReducedColorsMode = Favorite.VMRCReducedColorsMode;

            int height = 0, width = 0;
            ConnectionManager.GetSize(out height, out width, this, Favorite.DesktopSize);
            
            //vmrc.ServerDisplayHeight = height;
            //vmrc.ServerDisplayWidth = width;

            vmrc.OnStateChanged += new AxVMRCClientControlLib._IVMRCClientControlEvents_OnStateChangedEventHandler(vmrc_OnStateChanged);
            
            //vmrc.ServerDisplayHeight;
            //vmrc.ServerDisplayWidth;

            vmrc.OnSwitchedDisplay += new AxVMRCClientControlLib._IVMRCClientControlEvents_OnSwitchedDisplayEventHandler(vmrc_OnSwitchedDisplay);

            Text = "Connecting to VMRC Server...";
            vmrc.Connect();
            vmrc.BringToFront();
            vmrc.Update();
            return true;
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

        void vmrc_OnSwitchedDisplay(object sender, AxVMRCClientControlLib._IVMRCClientControlEvents_OnSwitchedDisplayEvent e) {
            Text = e.displayName;
        }

        void vmrc_OnStateChanged(object sender, AxVMRCClientControlLib._IVMRCClientControlEvents_OnStateChangedEvent e) {
            if (e.state == VMRCClientControlLib.VMRCState.vmrcState_Connected) this.connected = true;

        }

        public override void Disconnect() {
            try {
                vmrc.Disconnect();
            } catch (Exception e) { }
        }

        #endregion
    }
}