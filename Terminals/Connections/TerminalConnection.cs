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

            term.BackColor = Color.FromName(Favorite.TelnetBackColor);
            term.Font = FontParser.ParseFontName(Favorite.TelnetFont);
            //term.ForeColor = Color.FromName(Favorite.TelnetTextColor);
            //term.CursorColor = Color.FromName(Favorite.TelnetCursorColor);
            //term.TextColor = Color.FromName(Favorite.TelnetTextColor);
            term.ForeColor = Color.FromName(Favorite.TelnetTextColor);
            
            term.Username = Favorite.UserName;
            term.Password = Favorite.Password;
            term.Hostname = Favorite.ServerName;
            //term.Port = Favorite.Port;
            term.Rows = Favorite.TelnetRows;
            
            term.Columns = Favorite.TelnetCols;
            
            if(Favorite.Telnet) 
                term.ConnectionType = WalburySoftware.TerminalEmulator.ConnectionTypes.Telnet;
            else
                term.ConnectionType = WalburySoftware.TerminalEmulator.ConnectionTypes.SSH2;

            Text = "Connecting to Telnet Server...";
            term.Connect();
            connected = true;
            return true;
        }

        public override void Disconnect() {
            try {
                //term.Disconnect();
            } catch (Exception e) { }
        }

        #endregion
        
    }
}
