using System;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using Metro.TransportLayer.Icmp;
using System.Net;
using System.Net.Sockets;
using ZedGraph;


namespace Metro
{
    public partial class Ping : UserControl
    {
        public Ping()
        {
            InitializeComponent();
            mivPing = new MethodInvoker(UpdatePing);
            string data = new string('x', 32);
            payload = System.Text.ASCIIEncoding.ASCII.GetBytes(data);
            currentDelay = (int)DelayNumericUpDown.Value;
            t = new System.Threading.Timer(new System.Threading.TimerCallback(TryPing), null, currentDelay, this.currentDelay);

        }
        public void ForcePing(string Host)
        {
            this.textBox1.Text = Host;
            button1_Click(null, null);
        }
        private void TryPing(object state)
        {
            if (pingRunning && pingReady)
            {
                SendPing();
            }
        }
        int currentDelay = 0;
        private void SendPing()
        {
            IPAddress[] list = null;
            try
            {
                list = System.Net.Dns.GetHostAddresses(this.textBox1.Text);
            }
            catch (Exception exc)
            {
                Terminals.Logging.Log.Info("", exc);
                MessageBox.Show("Could not resolve address:" + this.textBox1.Text);
                this.button1.Enabled = true;
                this.textBox1.Enabled = true;

            }
            try
            {
                if (list != null) ping.SendPing(list[0], payload, true, 2000);
            }
            catch (Exception exc) { Terminals.Logging.Log.Info("", exc); }

            lock(t)
            {
                if(this.DelayNumericUpDown.Value != this.currentDelay)
                {
                    currentDelay = (int)this.DelayNumericUpDown.Value;
                    t.Change(currentDelay, currentDelay);
                }
            }
            
        }
        System.Threading.Timer t;
        bool pingRunning = false;
        bool pingReady = false;
        byte[] payload;
        MethodInvoker mivPing;
        Metro.NetworkInterfaceList nil = new Metro.NetworkInterfaceList();
        Metro.TransportLayer.Icmp.IcmpPingManager ping;
        System.Collections.Generic.List<PingUpdate> PingList = new List<PingUpdate>();

        private void button1_Click(object sender, EventArgs e)
        {
            this.button1.Enabled = false;
            this.textBox1.Enabled = false;
            PingList = new List<PingUpdate>();
            if (ping == null)
            {
                ping = new Metro.TransportLayer.Icmp.IcmpPingManager(nil.Interfaces[0].Address);
                ping.PingReply += new Metro.TransportLayer.Icmp.IcmpPingReplyHandler(ping_PingReply);
                ping.PingTimeout += new Metro.TransportLayer.Icmp.IcmpPingTimeOutHandler(ping_PingTimeout);
            }
            if (!pingRunning)
            {
                pingRunning = true;
                pingReady = false;
                SendPing();
            }

        }
        object threadLocker = new object();
        void ping_PingTimeout()
        {
            lock(threadLocker)
            {
                ping.CancelPing();
                PingUpdate pu = new PingUpdate();
                pu.ipHeader = null;
                pu.icmpHeader = null;
                pu.RoundTripTime = 0;
                PingList.Add(pu);
                this.Invoke(mivPing);
                pingReady = true;
            }

        }

        void ping_PingReply(Metro.NetworkLayer.IpV4.IpV4Packet ipHeader, Metro.TransportLayer.Icmp.IcmpPacket icmpHeader, int roundTripTime)
        {
            lock(threadLocker)
            {
                PingUpdate pu = new PingUpdate();
                pu.ipHeader = ipHeader;
                pu.icmpHeader = icmpHeader;
                pu.RoundTripTime = roundTripTime;
                pu.dateReceived = DateTime.Now;
                PingList.Add(pu);
                pingReady = true;
                this.Invoke(mivPing);
            }
        }
        private void UpdatePing()
        {

            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = PingList;
            UpdateGraph();
            Application.DoEvents();
        }
        private void UpdateGraph()
        {

            // Make up some data points based on the Sine function
            PointPairList list = new PointPairList();
            PointPairList avgList = new PointPairList();
            int x = 1;
            int yMax = 0;
            string destination = "";
            int sum = 0;
            foreach (PingUpdate p in PingList)
            {
                if (p.RoundTripTime > yMax) yMax = p.RoundTripTime;
                list.Add(x, p.RoundTripTime);

                sum += p.RoundTripTime;

                avgList.Add(x, (int)(sum/x));
                x++;
            }

            // Manually set the axis range
            myPane.YAxis.Scale.Min = 0;
            myPane.YAxis.Scale.Max = yMax;
            myPane.XAxis.Scale.Min = 0;
            myPane.XAxis.Scale.Max = x;

            myPane.Title.Text = "Ping results for " + this.textBox1.Text;

            myPane.CurveList.Clear();
            LineItem myCurve = myPane.AddCurve(this.textBox1.Text, list, Color.Blue, SymbolType.Diamond);
            LineItem avgCurve = myPane.AddCurve("Average", avgList, Color.Red, SymbolType.Diamond);


            // Fill the symbols with white
            myCurve.Symbol.Fill = new Fill(Color.White);

            // Fill the symbols with white
            myCurve.Symbol.Fill = new Fill(Color.White);
            // Associate this curve with the Y2 axis
            myCurve.IsY2Axis = true;

            // Tell ZedGraph to calculate the axis ranges
            // Note that you MUST call this after enabling IsAutoScrollRange, since AxisChange() sets
            // up the proper scrolling parameters
            zg1.AxisChange();
            // Make sure the Graph gets redrawn
            zg1.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(pingRunning) ping.CancelPing();
            pingReady = false;
            pingRunning = false;
            this.button1.Enabled = true;
            this.textBox1.Enabled = true;

        }

        GraphPane myPane;
        private void Ping_Load(object sender, EventArgs e)
        {
            myPane = zg1.GraphPane;
            // Set the titles and axis labels
            myPane.Title.Text = "Ping results";
            myPane.XAxis.Title.Text = "Counter";
            myPane.YAxis.Title.Text = "Time, Milliseconds";


            // Show the x axis grid
            myPane.XAxis.MajorGrid.IsVisible = true;

            // Make the Y axis scale red
            myPane.YAxis.Scale.FontSpec.FontColor = Color.Blue;
            myPane.YAxis.Title.FontSpec.FontColor = Color.Blue;
            // turn off the opposite tics so the Y tics don't show up on the Y2 axis
            myPane.YAxis.MajorTic.IsOpposite = false;
            myPane.YAxis.MinorTic.IsOpposite = false;
            // Don't display the Y zero line
            myPane.YAxis.MajorGrid.IsZeroLine = false;
            // Align the Y axis labels so they are flush to the axis
            myPane.YAxis.Scale.Align = AlignP.Inside;


            // Fill the axis background with a gradient
            myPane.Chart.Fill = new Fill(Color.White, Color.LightGray, 45.0f);

            // Add a text box with instructions
            TextObj text = new TextObj(
                "Zoom: left mouse & drag\nPan: middle mouse & drag\nContext Menu: right mouse",
                0.05f, 0.95f, CoordType.ChartFraction, AlignH.Left, AlignV.Bottom);
            text.FontSpec.StringAlignment = StringAlignment.Near;
            myPane.GraphObjList.Add(text);

            // Enable scrollbars if needed
            zg1.IsShowHScrollBar = true;
            zg1.IsShowVScrollBar = true;

            // OPTIONAL: Show tooltips when the mouse hovers over a point
            zg1.IsShowPointValues = true;
            zg1.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);

            // OPTIONAL: Add a custom context menu item
            zg1.ContextMenuBuilder += new ZedGraphControl.ContextMenuBuilderEventHandler(
                            MyContextMenuBuilder);

            // OPTIONAL: Handle the Zoom Event
            
            // Size the control to fit the window
            SetSize();



        }

        private void Ping_Resize(object sender, EventArgs e)
        {
            SetSize();
        }

        private void SetSize()
        {
            zg1.Location = new Point(10, 10);
            // Leave a small margin around the outside of the control
            zg1.Size = new Size(this.ClientRectangle.Width - 20,
                    this.ClientRectangle.Height - 20);
        }

        /// <summary>
        /// Display customized tooltips when the mouse hovers over a point
        /// </summary>
        private string MyPointValueHandler(ZedGraphControl control, GraphPane pane,
                        CurveItem curve, int iPt)
        {
            try
            {
                // Get the PointPair that is under the mouse
                PointPair pt = curve[iPt];

                return curve.Label.Text + " is " + pt.Y.ToString("f2") + " milliseconds at " + pt.X.ToString("f1");
            }
            catch (Exception ex) { Terminals.Logging.Log.Info("", ex); }
            return "";
        }

        /// <summary>
        /// Customize the context menu by adding a new item to the end of the menu
        /// </summary>
        private void MyContextMenuBuilder(ZedGraphControl control, ContextMenuStrip menuStrip,
                        Point mousePt, ZedGraphControl.ContextMenuObjectState objState)
        {
            //ToolStripMenuItem item = new ToolStripMenuItem();
            //item.Name = "add-beta";
            //item.Tag = "add-beta";
            //item.Text = "Add a new Beta Point";
            //item.Click += new System.EventHandler(AddBetaPoint);

            //menuStrip.Items.Add(item);
        }

        /// 
    }



    public class PingUpdate
    {
        public Metro.NetworkLayer.IpV4.IpV4Packet ipHeader;
        public Metro.TransportLayer.Icmp.IcmpPacket icmpHeader;
        private int roundTripTime;

        public DateTime dateReceived = DateTime.Now;

        public string DateReceived
        {
            get { return dateReceived.ToLongTimeString(); }
        }
	
        
        public string Counter
        {
            get
            {
                if (ipHeader != null)
                {
                    return ipHeader.Identification.ToString();
                }
                else
                {
                    return "*";
                }
            }
        }

        public string Destination
        {
            get {
                if (ipHeader != null)
                {
                    return ipHeader.SourceAddress.ToString();
                }
                else
                {
                    return "*";
                }
            }
        }
        public string Source
        {
            get {
                if (ipHeader != null)
                {

                    return ipHeader.DestinationAddress.ToString();
                }
                else
                {
                    return "*";
                }
            }
        }
    
        
        public string TimeToLive 
        {
            get {
                if (ipHeader != null)
                {
                    return ipHeader.TimeToLive.ToString();
                }
                else
                {
                    return "*";
                }
            }
        }
        public int RoundTripTime
        {
            get { return roundTripTime; }
            set { roundTripTime = value; }
        }





    }
}
