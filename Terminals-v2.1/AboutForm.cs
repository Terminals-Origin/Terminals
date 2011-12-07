using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Terminals.Configuration;

namespace Terminals
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();            
        }

        private void lblTerminals_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Program.Resources.GetString("TerminalsURL"));
        }

        private void lblEyal_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.epocalipse.com/blog/");
        }

        private void lblDudu_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://notsosmartbuilder.blogspot.com/");
        }

        private void linkHiro_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.nbc.com/Heroes/cast/hiro.shtml");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.nbc.com/Heroes/cast/peter.shtml");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.epocalipse.com/blog/");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("http://weblogs.asp.net/rchartier/");
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            lblVersion.Text = Program.Info.AboutText;
            DateTime dt = Terminals.Program.Info.BuildDate;
            this.textBox1.Text = string.Format("{0}\r\nConfig File:\r\n{1}\r\n\r\n{2}\r\n\r\nVersion: {3}\r\n{4}\r\n{5}\r\n{6}\r\n{7}\r\n{8}\r\n{9}\r\n{10}\r\n{11}\r\n\r\n",
                this.textBox1.Text, Settings.ConfigurationFileLocation,
                string.Format("This version of terminals was build for you on {0} at {1}", dt.ToLongDateString(), dt.ToLongTimeString()),
                Program.Info.DLLVersion,
                String.Format("CommandLine:{0}", Environment.CommandLine),
                String.Format("CurrentDirectory:{0}", Environment.CurrentDirectory),
                String.Format("MachineName:{0}", Environment.MachineName),
                String.Format("OSVersion:{0}", Environment.OSVersion),
                String.Format("ProcessorCount:{0}", Environment.ProcessorCount),
                String.Format("UserInteractive:{0}", Environment.UserInteractive),
                String.Format("Framework Version:{0}", Environment.Version),
                String.Format("WorkingSet:{0}", Environment.WorkingSet)
                );



        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}