using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using AxVMRCClientControlLib;

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
                // vmrc.BringToFront();
                this.BringToFront();
                //vmrc.Parent = base.TerminalTabPage;
                this.Parent = TerminalTabPage;

                //vmrc.CtlAutoSize = true;

                string domainName = Favorite.DomainName;
                if(domainName == null || domainName == "") domainName = Settings.DefaultDomain;

                string pass = Favorite.Password;
                if(pass == null || pass == "") pass = Settings.DefaultPassword;


                string userName = Favorite.UserName;
                if(userName == null || userName == "") userName = Settings.DefaultUsername;


                // vmrc.Dock = DockStyle.Fill;
                vmrc.UserName = userName;
                vmrc.ServerAddress = Favorite.ServerName;
                vmrc.ServerPort = Favorite.Port;
                vmrc.UserDomain = domainName;
                vmrc.UserPassword = pass;
                vmrc.AdministratorMode = Favorite.VMRCAdministratorMode;
                vmrc.ReducedColorsMode = Favorite.VMRCReducedColorsMode;

                int height = 0, width = 0;
                ConnectionManager.GetSize(out height, out width, this, Favorite.DesktopSize);

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
            string f = "";
            //if(e.state == VMRCClientControlLib.VMRCState.) this.connected = true;
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