using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using AxWFICALib;
using Terminals.Configuration;
using Terminals.Data;

namespace Terminals.Connections
{
    internal class ICAConnection : Connection
    {
        #region IConnection Members
        private bool connected = false;

        private AxICAClient iIcaClient;

        public override bool Connected { get { return connected; } }

        public override bool Connect()
        {
            try
            {
                iIcaClient = new AxICAClient();
                ((Control)iIcaClient).DragEnter += new DragEventHandler(ICAConnection_DragEnter);
                ((Control)iIcaClient).DragDrop += new DragEventHandler(ICAConnection_DragDrop);
                iIcaClient.OnDisconnect += new EventHandler(iIcaClient_OnDisconnect);
                iIcaClient.Dock = DockStyle.Fill;


                Controls.Add(iIcaClient);

                ISecurityOptions security = this.Favorite.Security.GetResolvedCredentials();
                icaPassword = security.Password;

                //rd.SendSpecialKeys(VncSharp.SpecialKeys);            
                iIcaClient.Parent = base.TerminalTabPage;
                this.Parent = TerminalTabPage;
                iIcaClient.Dock = DockStyle.Fill;

                iIcaClient.Address = Favorite.ServerName;
                switch (Favorite.Display.Colors)
                {
                    case Colors.Bit16:
                        iIcaClient.SetProp("DesiredColor", "16");
                        break;
                    case Colors.Bits32:
                        iIcaClient.SetProp("DesiredColor", "32");
                        break;
                    case Colors.Bits8:
                        iIcaClient.SetProp("DesiredColor", "16");
                        break;
                    default:
                        iIcaClient.SetProp("DesiredColor", "24");
                        break;

                }
                //             iIcaClient.Application = "Terminals " + Program.TerminalsVersion.ToString();

                // This line causes the following misleading error.
                // To log on to this remote computer, you must have Terminal Server User Access permissions on this computer. 
                // By default, members of the Remote Desktop Users group have these permissions. If you are not a member of the 
                // Remote Desktop Users group or another group that has these permissions, or if the Remote Desktop User group
                // does not have these permissions, you must be granted these permissions manually."

                ICAOptions icaOptions = this.Favorite.ProtocolProperties as ICAOptions;
                iIcaClient.AppsrvIni = icaOptions.ServerINI;
                iIcaClient.WfclientIni = icaOptions.ClientINI;
                iIcaClient.Encrypt = icaOptions.EnableEncryption;
                string encryptLevel = "Encrypt";
                string specifiedLevel = icaOptions.EncryptionLevel.Trim();
                if (specifiedLevel.Contains(" "))
                {
                    encryptLevel = specifiedLevel.Substring(0, specifiedLevel.IndexOf(" ")).Trim();
                    if (encryptLevel != "") iIcaClient.EncryptionLevelSession = encryptLevel;
                }




                iIcaClient.Domain = security.Domain;
                iIcaClient.Address = Favorite.ServerName;
                iIcaClient.Username = security.UserName;
                iIcaClient.SetProp("ClearPassword", security.Password);
                if (icaOptions.ApplicationName != "")
                {
                    iIcaClient.ConnectionEntry = icaOptions.ApplicationName;
                    iIcaClient.InitialProgram = icaOptions.ApplicationName;
                    iIcaClient.Application = icaOptions.ApplicationPath;
                    iIcaClient.WorkDirectory = icaOptions.ApplicationWorkingFolder;
                }


                Text = "Connecting to ICA Server...";

                iIcaClient.Visible = true;

                iIcaClient.SetProp("ScalingMode", "3");
                iIcaClient.Launch = false;
                iIcaClient.TransportDriver = "TCP/IP";
                iIcaClient.Connect();
                iIcaClient.Focus();


                return true;
            }
            catch (Exception exc)
            {
                Logging.Fatal("Connecting to ICA", exc);
                return false;
            }
        }

        private void iIcaClient_OnDisconnect(object sender, EventArgs e)
        {
            Logging.Fatal("ICA Connection Lost" + this.Favorite.Name);
            this.connected = false;
            this.FireDisconnected();
        }

        private void ICAConnection_DragDrop(object sender, DragEventArgs e)
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
                SHCopyFiles(files, desktopShare);
        }

        private void ICAConnection_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }

        }

        private string icaPassword = "";
        
        private string ICAPassword()
        {
            return icaPassword;
        }

        public void Disconnect()
        {
            try
            {
                connected = false;
                iIcaClient.Disconnect();
            }
            catch (Exception e)
            {
                Logging.Error("Error on Disconnect", e);
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
        #endregion
    }
}