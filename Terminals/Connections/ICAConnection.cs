using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Terminals.Connections {
    public class ICAConnection : Connection {
        #region IConnection Members
        private bool connected = false;

        AxWFICALib.AxICAClient iIcaClient = new AxWFICALib.AxICAClient();

        public override void ChangeDesktopSize(Terminals.DesktopSize Size) {
        }

        public override bool Connected { get { return connected; } }
        

        public override bool Connect() {

            ((Control)iIcaClient).DragEnter += new DragEventHandler(ICAConnection_DragEnter);
            ((Control)iIcaClient).DragDrop += new DragEventHandler(ICAConnection_DragDrop);
            iIcaClient.Dock = DockStyle.Fill;


            Controls.Add(iIcaClient);
            if (Favorite.Password != null && Favorite.Password.Trim() != "") {
                icaPassword = Favorite.Password;
            }


            //rd.SendSpecialKeys(VncSharp.SpecialKeys);            
            iIcaClient.Parent = base.TerminalTabPage;
            this.Parent = TerminalTabPage;
            iIcaClient.Dock = DockStyle.Fill;

            iIcaClient.Address = Favorite.ServerName;
            switch(Favorite.Colors)
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


            iIcaClient.Domain = Favorite.DomainName;
            iIcaClient.Address = Favorite.ServerName;
            iIcaClient.Username = Favorite.UserName;

            if(Favorite.ApplicationName != "")
            {
                iIcaClient.ConnectionEntry = Favorite.ApplicationName;
                //iIcaClient.Application = favorite.applicationName;
                iIcaClient.InitialProgram = Favorite.ApplicationName;
                iIcaClient.WorkDirectory = Favorite.ApplicationWorkingFolder;
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

        void ICAConnection_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            string desktopShare = ParentForm.GetDesktopShare();
            if(String.IsNullOrEmpty(desktopShare))
            {
                MessageBox.Show(this, "A Desktop Share was not defined for this connection.\n" +
                    "Please define a share in the connection properties window (under the Local Resources tab)."
                    , "Terminals", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
                SHCopyFiles(files, desktopShare);
        }

        void ICAConnection_DragEnter(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop, false))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }

        }
        private string icaPassword = "";
        string ICAPassword()
        {
            return icaPassword;
        }


        public override void Disconnect() {
            try {
                connected = false;
                iIcaClient.Disconnect();
            } catch (Exception e) { }
        }
        private void SHCopyFiles(string[] sourceFiles, string destinationFolder)
        {
            SHFileOperationWrapper fo = new SHFileOperationWrapper();
            List<string> destinationFiles = new List<string>();

            foreach(string sourceFile in sourceFiles)
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