using System;
using System.Windows.Forms;
using NetTools;

namespace Terminals.Network
{
    internal partial class WakeOnLan : UserControl
    {
        public WakeOnLan()
        {
            InitializeComponent();
        }

        private void SendButton_Click(object sender, EventArgs e)
        {
            try
            {
                MagicPacket wakeUpPacket = new MagicPacket(this.MACTextbox.Text);
                int byteSend = wakeUpPacket.WakeUp();
                this.ResultsLabel.Text = string.Format("{0} bytes sent to {1}", byteSend, wakeUpPacket.macAddress);
            }
            catch (Exception exc)
            {
                Logging.Log.Info("Error sending Magic Packet", exc);
                MessageBox.Show("There was an error sending the Magic Packet" + exc.Message);
            }
        }

        private void MACTextbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendButton_Click(null, null);
            }
        }
    }
}
