using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Terminals
{
    public partial class FirstRunWizard : Form
    {
        public FirstRunWizard()
        {
            InitializeComponent();
        }

        private void FirstRunWizard_Load(object sender, EventArgs e)
        {
            Wizard.IntroForm frm = new Terminals.Wizard.IntroForm();
            frm.Dock = DockStyle.Fill;
            this.panel1.Controls.Add(frm);
        }

        private void nextButton_Click(object sender, EventArgs e)
        {

            
            if(nextButton.Text == "Finished!")
            {
                this.Hide();
            }
            else
            {
                nextButton.Text = "Finished!";
                this.panel1.Controls.Clear();
                Wizard.AddExistingRDPConnections rdp = new Terminals.Wizard.AddExistingRDPConnections();
                rdp.Dock = DockStyle.Fill;
                this.panel1.Controls.Add(rdp);

            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}