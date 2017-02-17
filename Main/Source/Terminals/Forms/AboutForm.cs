using System;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Services;

namespace Terminals
{
    internal partial class AboutForm : Form
    {
        private readonly string persistenceName;

        public AboutForm(string persistenceName)
        {
            InitializeComponent();

            this.persistenceName = persistenceName;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LblTerminals_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ExternalLinks.ShowReleasePage();
        }

        private void LinkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ExternalLinks.OpenAuthorPage();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            this.titleLabel.Text += string.Format("({0})", Program.Info.Description);
            this.lblVersion.Text = Program.Info.GetAboutText(persistenceName);
            this.textBox1.Text = this.FormatDetails();
        }

        private string FormatDetails()
        {
            DateTime dt = Program.Info.BuildDate;
            const string DETAILS = "{0}\r\n" +
                                   "Config File:\r\n{1}\r\n\r\n" +
                                   "This version of terminals was build for you on {2} at {3}\r\n\r\n" +
                                   "Version: {4}\r\n" +
                                   "CommandLine: {5}\r\n" +
                                   "CurrentDirectory: {6}\r\n" +
                                   "MachineName: {7}\r\n" +
                                   "OSVersion: {8}\r\n" +
                                   "ProcessorCount: {9}\r\n" +
                                   "UserInteractive: {10}\r\n" +
                                   "Framework Version: {11}\r\n" +
                                   "WorkingSet: {12}\r\n" +
                                   "Is 64bit OS: {13}\r\n" +
                                   "Is 64bit Proces: {14}\r\n\r\n";

            return string.Format(DETAILS,
                                    this.textBox1.Text,
                                    Settings.Instance.FileLocations.Configuration,
                                    dt.ToLongDateString(),
                                    dt.ToLongTimeString(),
                                    Program.Info.DLLVersion,
                                    Environment.CommandLine,
                                    Environment.CurrentDirectory,
                                    Environment.MachineName,
                                    Environment.OSVersion,
                                    Environment.ProcessorCount,
                                    Environment.UserInteractive,
                                    Environment.Version,
                                    Environment.WorkingSet,
                                    Native.Wow.Is64BitOperatingSystem,
                                    Native.Wow.Is64BitProcess);
        }
    }
}