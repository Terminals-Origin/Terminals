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

namespace Terminals.Connections
{
    public class TerminalConnection : Connection
    {
        #region IConnection Members
        private bool connected = false;
        public override bool Connected { get { return connected; } }
        public override void ChangeDesktopSize(Terminals.DesktopSize Size)
        {
        }

        public WalburySoftware.TerminalEmulator term;
        public override bool Connect()
        {
            try
            {
                Terminals.Logging.Log.Info("Connecting to a Telnet/SSH Connection");
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

                string domainName = Favorite.DomainName;
                if(domainName == null || domainName == "") domainName = Settings.DefaultDomain;
                string pass = Favorite.Password;
                if(pass == null || pass == "") pass = Settings.DefaultPassword;
                string userName = Favorite.UserName;
                if(userName == null || userName == "") userName = Settings.DefaultUsername;


                if(userName != null && userName != "") term.Username = userName;
                if(pass != null && pass != "") term.Password = pass;

                if(term.Username == null || term.Username == "" || term.Password == null || term.Password == "")
                {

                    Terminals.InputBoxResult result = Terminals.InputBox.Show("Please provider your User name", "Telnet/SSH User name");
                    if(result.ReturnCode == DialogResult.OK && result.Text != null && result.Text.Trim() != "")
                    {
                        term.Username = result.Text;
                        if(term.Password == null)
                        {

                            Terminals.InputBoxResult res = Terminals.InputBox.Show("Please provider your Password", "Telnet/SSH Password", '*');
                            if(res.ReturnCode == DialogResult.OK && res.Text != null && res.Text.Trim() != "")
                            {
                                term.Password = res.Text;
                            }
                        }

                    }
                }

                bool ForceClose = true;

                if(term.Username != null && term.Username != "" && term.Password != null && term.Password != "") ForceClose = false;


                if(ForceClose)
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

                    if(Favorite.Telnet)
                        term.ConnectionType = WalburySoftware.TerminalEmulator.ConnectionTypes.Telnet;
                    else
                        term.ConnectionType = WalburySoftware.TerminalEmulator.ConnectionTypes.SSH2;

                    Text = "Connecting to Telnet/SSH Server...";
                    term.Connect();
                    connected = true;
                    return true;
                }
            }
            catch(Exception exc)
            {
                Terminals.Logging.Log.Fatal("Connecting to Telnet/SSH Connection", exc);
                return false;
            }
        }

        void term_OnDisconnected(object Sender, string Message)
        {
            System.Windows.Forms.MessageBox.Show("There was an error with the Telnet/SSH connection.  It will now close.\r\nDetails:\r\n" + Message);
            this.ParentForm.tcTerminals.ForceCloseTab(this.TerminalTabPage);
        }

        public override void Disconnect()
        {
            try
            {
                //term.Disconnect();
            }
            catch(Exception e)
            {
                Terminals.Logging.Log.Info("", e);

            }
        }

        #endregion

    }
}