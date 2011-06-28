using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

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
            this.textBox1.Text = this.textBox1.Text + "\r\nConfig File:\r\n" + Program.ConfigurationFileLocation;
        }
    }
}