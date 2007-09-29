using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Microsoft.VMRCClientControl;


namespace Terminals.Connections {
    public class VMRCConnection : Connection {
        #region IConnection Members
        private bool connected = false;
        public override bool Connected { get { return connected; } }
        public override void ChangeDesktopSize(Terminals.DesktopSize Size) {
        }
        Microsoft.VMRCClientControl.IVMRCClientControl vmrc = new VMRCClientControl();
        //AxVMRCClientControlLib.AxVMRCClientControl vmrc = new AxVMRCClientControlLib.AxVMRCClientControl();
        public override bool Connect() {
            Controls.Add((System.Windows.Forms.Control)vmrc);
           // vmrc.BringToFront();
            this.BringToFront();
            //vmrc.Parent = base.TerminalTabPage;
            this.Parent = TerminalTabPage;

            //vmrc.CtlAutoSize = true;

           // vmrc.Dock = DockStyle.Fill;
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
            (vmrc as VMRCClientControl).OnStateChanged += new _IVMRCClientControlEvents_OnStateChangedEventHandler(VMRCConnection_OnStateChanged);
            
            //vmrc.ServerDisplayHeight;
            //vmrc.ServerDisplayWidth;

            (vmrc as VMRCClientControl).OnSwitchedDisplay += new _IVMRCClientControlEvents_OnSwitchedDisplayEventHandler(VMRCConnection_OnSwitchedDisplay);

            Text = "Connecting to VMRC Server...";
            vmrc.Connect();
            //vmrc.BringToFront();
            //vmrc.Update();
            return true;
        }

        void VMRCConnection_OnSwitchedDisplay(string displayName) {
            Text = displayName;
        }

        void VMRCConnection_OnStateChanged(VMRCState State) {
            if(State == VMRCState.vmrcState_Connected) this.connected = true;
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
            } catch (Exception e) { }
        }

        #endregion
    }
}