using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Tamir.IPLib;


namespace WindowsFormsApplication2 {
    public partial class PacketCapture : UserControl {
        public PacketCapture() {
            InitializeComponent();
        }
        MethodInvoker updater;
        MethodInvoker stopUpdater;
        PcapDeviceList devices;
        PcapDevice dev;

        private void PacketCapture_Load(object sender, EventArgs e) {
            try {
                promiscuousCheckbox.Enabled = true;
                DumpToFileCheckbox.Enabled = true;
                StopCaptureButton.Enabled = false;
                AmberPicture.Visible = true;
                GreenPicture.Visible = false;
                RedPicture.Visible = false;
                updater = new MethodInvoker(UpdateUI);
                stopUpdater = new MethodInvoker(PcapStopped);
                devices = SharpPcap.GetAllDevices();
                foreach(PcapDevice device in devices) {
                    comboBox1.Items.Add(device.PcapDescription);
                }
                if(devices.Count > 0) comboBox1.SelectedIndex = 1;
                this.webBrowser1.DocumentStream = new System.IO.MemoryStream(System.Text.ASCIIEncoding.Default.GetBytes(Terminals.Properties.Resources.Filtering));
            } catch(Exception exc) {
                
                this.Enabled = false;
                if(exc is System.BadImageFormatException)
                {
                    Terminals.Logging.Log.Info("Terminals Packet Capture is not configured to work with this system (Bad Image Format Exception)", exc);
                    System.Windows.Forms.MessageBox.Show("Terminals Packet Capture is not configured to work with this system (Bad Image Format Exception)");
                }
                else if(exc is System.DllNotFoundException)
                {
                    Terminals.Logging.Log.Info("WinpPcap was not installed", exc);
                    if(System.Windows.Forms.MessageBox.Show("It appears that WinPcap is not installed.  In order to use this feature within Terminals you must first install that product.  Do you wish to visit the download location right now?", "Download WinPcap?", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        System.Diagnostics.Process.Start("http://www.winpcap.org/install/default.htm");
                    }
                }
                else
                {
                    Terminals.Logging.Log.Info("WinpPcap was not installed correctly", exc);
                }
            }
            this.PacketCapture_Resize(null, null);
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            int index = comboBox1.SelectedIndex;
            dev = devices[index];
            this.propertyGrid1.SelectedObject = (dev as NetworkDevice);

        }

        private void StartCapture(object state) {
            PcapDevice dev = (PcapDevice)state;
            dev.PcapOpen(promiscuousCheckbox.Checked);
            if(DumpToFileCheckbox.Checked) {
                dev.PcapDumpOpen(DumpFile);
            }
            try {
                dev.PcapSetFilter(this.FilterTextBox.Text);
            } catch(Exception exc) {
                System.Windows.Forms.MessageBox.Show("Failed to set the filter: " + this.FilterTextBox.Text);
                Terminals.Logging.Log.Info("Failed to set the filter: " + this.FilterTextBox.Text, exc);
            }
            dev.PcapStartCapture();

        }
        private string DumpFile = @"c:\Terminals.dump";

        private void CaptureButton_Click(object sender, EventArgs e) {
            DumpToFileCheckbox.Enabled = false;
            promiscuousCheckbox.Enabled = false;
            CaptureButton.Enabled = false;
            StopCaptureButton.Enabled = true;
            AmberPicture.Visible = false;
            GreenPicture.Visible = true;
            RedPicture.Visible = false;
            this.listBox1.Items.Clear();
            lock(packets) {
                packets = new List<Tamir.IPLib.Packets.Packet>();
                newpackets = new List<Tamir.IPLib.Packets.Packet>();
                dev.PcapOnPacketArrival += new SharpPcap.PacketArrivalEvent(dev_PcapOnPacketArrival);
                dev.PcapOnCaptureStopped += new SharpPcap.PcapCaptureStoppedEvent(dev_PcapOnCaptureStopped);
            }
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(StartCapture), dev);
        }

        void dev_PcapOnCaptureStopped(object sender, bool error) {
            this.Invoke(stopUpdater);
        }
        void PcapStopped() {
            CaptureButton.Enabled = true;
            StopCaptureButton.Enabled = false;
            RedPicture.Visible = false;
            GreenPicture.Visible = false;
            AmberPicture.Visible = true;
            DumpToFileCheckbox.Enabled = true;
            promiscuousCheckbox.Enabled = true;
        }
        void UpdateUI() {
            lock(packets) {
                GreenPicture.Visible = false;
                Application.DoEvents();
                foreach(Tamir.IPLib.Packets.Packet packet in newpackets) {
                    this.listBox1.Items.Add(packet);                   
                    newpackets = new List<Tamir.IPLib.Packets.Packet>();
                }
                Application.DoEvents(); 
                GreenPicture.Visible = true;
            }
        }

        List<Tamir.IPLib.Packets.Packet> packets = new List<Tamir.IPLib.Packets.Packet>();
        List<Tamir.IPLib.Packets.Packet> newpackets = new List<Tamir.IPLib.Packets.Packet>();

        void dev_PcapOnPacketArrival(object sender, Tamir.IPLib.Packets.Packet packet) {
            lock(packets) {
                packets.Add(packet);
                newpackets.Add(packet);
            }
            if(dev.PcapDumpOpened) {
                dev.PcapDump(packet);
            }
            this.Invoke(updater);
        }
        private void StopCapture(object state) {
            PcapDevice dev = (PcapDevice)state;
            dev.PcapStopCapture();
            dev.PcapClose();
            dev.PcapDumpFlush();
            dev.PcapDumpClose();
        }
        private void StopCaptureButton_Click(object sender, EventArgs e) {
            CaptureButton.Enabled = false;
            StopCaptureButton.Enabled = false;
            RedPicture.Visible = true;
            GreenPicture.Visible = false;
            AmberPicture.Visible = false;
            DumpToFileCheckbox.Enabled = true;
            promiscuousCheckbox.Enabled = true;
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(StopCapture), dev);
            if(DumpToFileCheckbox.Checked) {
                if(System.Windows.Forms.MessageBox.Show("Open dump file in notepad?") == DialogResult.OK) {
                    System.Diagnostics.Process.Start("notepad.exe", this.DumpFile);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {
            if(listBox1.SelectedIndex > 0) {
                Tamir.IPLib.Packets.Packet packet = packets[listBox1.SelectedIndex];
                Be.Windows.Forms.DynamicByteProvider provider = new Be.Windows.Forms.DynamicByteProvider(packet.Data);
                this.hexBox1.ByteProvider = provider;
                this.textBox1.Text = System.Text.ASCIIEncoding.Default.GetString(packet.Data);
                this.treeView1.Nodes.Clear();
                TreeNode header = this.treeView1.Nodes.Add("Header");
                header.Nodes.Add(string.Format("Length:{0}", packet.Header.Length));
                System.Text.StringBuilder sb = new StringBuilder();
                foreach(byte b in packet.Header) {
                    sb.Append(b.ToString("00"));
                    sb.Append(" ");
                }
                header.Nodes.Add(string.Format("Data:{0}", sb.ToString()));
                header.Nodes.Add(string.Format("Capture Length:{0}", packet.PcapHeader.CaptureLength.ToString()));
                header.Nodes.Add(string.Format("Packet Length:{0}", packet.PcapHeader.PacketLength.ToString()));
                header.Nodes.Add(string.Format("Date:{0}", packet.PcapHeader.Date.ToString()));
                header.Nodes.Add(string.Format("Microseconds:{0}", packet.PcapHeader.MicroSeconds.ToString()));
                header.Nodes.Add(string.Format("Seconds:{0}", packet.PcapHeader.Seconds.ToString()));

                this.treeView1.ExpandAll();
            }
        }

        private void PacketCapture_Resize(object sender, EventArgs e) {
            if(hexBox1.Width > 640) {
                hexBox1.BytesPerLine = 16;
            } else {
                hexBox1.BytesPerLine = 8;
            }
        }

    }
}
