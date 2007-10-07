using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AxMSTSCLib;
using MSTSC = MSTSCLib;
using Terminals.Properties;
using System.Diagnostics;
using System.Runtime.InteropServices;
using TabControl;
using System.IO;

namespace Terminals.Connections {
    public class TerminalConnection : Connection {
        #region IConnection Members
        private bool connected = false;
        public override bool Connected { get { return connected; } }
        public override void ChangeDesktopSize(Terminals.DesktopSize Size) {
        }

        public WalburySoftware.TerminalEmulator term;
        public override bool Connect() {
            term = new WalburySoftware.TerminalEmulator();

            Controls.Add(term);
            term.BringToFront();
            this.BringToFront();

            term.Parent = base.TerminalTabPage;
            this.Parent = TerminalTabPage;
            term.Dock = DockStyle.Fill;

            term.OnDisconnected += new WalburySoftware.TerminalEmulator.Disconnected(term_OnDisconnected);
            term.BackColor = Color.FromName(Favorite.TelnetBackColor);
            term.Font = FontParser.ParseFontName(Favorite.TelnetFont);
            term.ForeColor = Color.FromName(Favorite.TelnetTextColor);
            
            if(Favorite.UserName!=null && Favorite.UserName.Trim()!="") term.Username = Favorite.UserName;
            if (Favorite.Password != null && Favorite.Password.Trim() != "") term.Password = Favorite.Password;

            bool ForceClose = true;

            if (term.Username == null)
            {
                
                Terminals.InputBoxResult result = Terminals.InputBox.Show("Please provider your User name", "SSH User name");
                if (result.ReturnCode == DialogResult.OK && result.Text != null && result.Text.Trim() != "")
                {
                    term.Username = result.Text;
                    if (term.Password == null)
                    {

                        Terminals.InputBoxResult res = Terminals.InputBox.Show("Please provider your Password", "SSH Password", '*');
                        if (res.ReturnCode == DialogResult.OK && res.Text != null && res.Text.Trim() != "")
                        {
                            term.Password = res.Text;
                            ForceClose = false;
                        }
                    }

                }
            }
            if (ForceClose)
            {
                this.ParentForm.tcTerminals.ForceCloseTab(this.TerminalTabPage);
                connected = false;
                return false;
            }
            else
            {

                term.Hostname = Favorite.ServerName;
                //term.Port = Favorite.Port;
                term.Rows = Favorite.TelnetRows;

                term.Columns = Favorite.TelnetCols;

                if (Favorite.Telnet)
                    term.ConnectionType = WalburySoftware.TerminalEmulator.ConnectionTypes.Telnet;
                else
                    term.ConnectionType = WalburySoftware.TerminalEmulator.ConnectionTypes.SSH2;

                Text = "Connecting to Telnet Server...";
                term.Connect();
                connected = true;
                return true;
            }
        }

        void term_OnDisconnected(object Sender, string Message)
        {
            System.Windows.Forms.MessageBox.Show("There was an error with the Terminals connection.  It will now close.");
            this.ParentForm.tcTerminals.ForceCloseTab(this.TerminalTabPage);
        }

        public override void Disconnect() {
            try {
                //term.Disconnect();
            } catch (Exception e) { }
        }

        #endregion
        
    }
}
