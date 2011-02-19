using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Metro.TransportLayer.Icmp;
using System.Net;
using System.Net.Sockets;
using ZedGraph;
using Bdev.Net.Dns;
using Terminals.Network;

namespace Metro
{
    public partial class TraceRoute : UserControl
    {
        public TraceRoute()
        {
            InitializeComponent();
            miv = new MethodInvoker(UpdateRoute);


        }
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                nil = new Metro.NetworkInterfaceList();
            }
            catch (Exception exc)
            {
                Terminals.Logging.Log.Error("On Load", exc);
            }

            try
            {


                NetworkInterface validNic = nil.Interfaces[0];
                foreach (NetworkInterface nic in nil.Interfaces)
                {
                    if (nic.IsEnabled && !nic.isLoopback)
                    {
                        validNic = nic;
                    }
                }
                trace = new Metro.TransportLayer.Icmp.IcmpTraceRoute(validNic.Address);
                trace.RouteUpdate += new Metro.TransportLayer.Icmp.RouteUpdateHandler(trace_RouteUpdate);
                trace.TraceFinished += new Metro.TransportLayer.Icmp.TraceFinishedHandler(trace_TraceFinished);
            }
            catch (Exception exc)
            {
                Terminals.Logging.Log.Error(exc);
                //Terminals.Connections.TabbedTools tabbed = (this.Parent as Terminals.Connections.TabbedTools);
                //if (tabbed != null)
                //{
                //    tabbed.HideTab(1);
                //}
            }
            base.OnLoad(e);
        }
        Metro.NetworkInterfaceList nil = null;
        Metro.TransportLayer.Icmp.IcmpTraceRoute trace = null;
        System.Collections.Generic.List<RouteUpdate> RUList = new List<RouteUpdate>();

        public void ForceTrace(string Host)
        {
            this.textBox1.Text = Host;
            button1_Click(null, null);
        }

        private void UpdateRoute()
        {
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = RUList;
            UpdateGraph();
        }
        void trace_TraceFinished()
        {
            button1.Enabled = true;
            textBox1.Enabled = true;
            MessageBox.Show("Trace Route Finished.");
        }
        private void UpdateGraph()
        {

            // Make up some data points based on the Sine function
            PointPairList list = new PointPairList();
            PointPairList avgList = new PointPairList();
            int x = 1;
            int yMax = 0;
            int sum = 0;
            foreach (RouteUpdate p in RUList)
            {
                if (p.RoundTripTime > yMax) yMax = p.RoundTripTime;
                list.Add(x, p.RoundTripTime,p.Gateway.ToString());

                sum += p.RoundTripTime;

                avgList.Add(x, (int)(sum / x));
                x++;
            }

            // Manually set the axis range
            myPane.YAxis.Scale.Min = 0;
            myPane.YAxis.Scale.Max = yMax;
            myPane.XAxis.Scale.Min = 0;
            myPane.XAxis.Scale.Max = x;

            myPane.Title.Text = "Trace Route results for " + this.textBox1.Text;

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
        GraphPane myPane;



        void trace_RouteUpdate(System.Net.IPAddress gateway, int roundTripTime, byte currentHop)
        {
            lock(RUList)
            {
                RouteUpdate ru = new RouteUpdate();
                ru.Gateway = gateway;
                ru.ResolveHostName = this.ResolveCheckBox.Checked;
                ru.RoundTripTime = roundTripTime;
                ru.CurrentHop = currentHop;
                ru.OnHostnameResolved += new RouteUpdate.HostnameResolved(ru_OnHostnameResolved);
                this.RUList.Add(ru);
            }
            this.Invoke(miv);
        }

        void ru_OnHostnameResolved(RouteUpdate Update)
        {
            this.Invoke(miv);
        }
        MethodInvoker miv;
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            this.textBox1.Enabled = false;

            RUList = new List<RouteUpdate>();

            Trace();
        }

        private void Trace()
        {
            IPAddress[] list = null;
            try
            {
                list = System.Net.Dns.GetHostAddresses(this.textBox1.Text);
            }
            catch (Exception exc)
            {
                Terminals.Logging.Log.Info("Could not resolve address:" + this.textBox1.Text, exc);
                MessageBox.Show("Could not resolve address:" + this.textBox1.Text);
                button1.Enabled = true;
                this.textBox1.Enabled = true;

            }
            try
            {
                if (list != null) trace.TraceRoute(list[0], true, 2000, 30);
            }
            catch (Exception exc) { Terminals.Logging.Log.Error(exc); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = true;
            this.textBox1.Enabled = true;

            trace.CancelTrace();
        }

        private void TraceRoute_Load(object sender, EventArgs e)
        {
            myPane = zg1.GraphPane;
            // Set the titles and axis labels
            myPane.Title.Text = "Trace Route results";
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

        private void TraceRoute_Resize(object sender, EventArgs e)
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

                return pt.Tag.ToString() + " is " + pt.Y.ToString("f2") + " milliseconds at " + pt.X.ToString("f1");
            }
            catch (Exception ex) { Terminals.Logging.Log.Error(ex); }
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

        private void ResolveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            lock(RUList)
            {
                foreach(RouteUpdate ru in RUList)
                {
                    ru.ResolveHostName = ResolveCheckBox.Checked;
                }
            }
            if(ResolveCheckBox.Checked) this.Invoke(miv);
        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(null, null);
            }
        }
    }

    public class RouteUpdate
    {
        private System.Net.IPAddress gateway;
        private int roundTripTime;
        private byte currentHop;

        public delegate void HostnameResolved(RouteUpdate Update);
        public event HostnameResolved OnHostnameResolved;

        public bool ResolveHostName = false;

        private string hostName;

        private void UpdateHostName(object nullstate)
        {
            System.Net.IPHostEntry entry;
            try
            {
                entry = System.Net.Dns.GetHostEntry(this.gateway);
            }
            catch(Exception e)
            {
                Terminals.Logging.Log.Error(e);
                entry = null;
            }
            if(entry != null && entry.HostName != null)
            {
                hostName = entry.HostName;
                if(OnHostnameResolved != null) OnHostnameResolved(this);
            }
            else
            {
                hostName = " ";
            }
        }

        public string HostName
        {
            get {
                if(!ResolveHostName) return hostName;
                if(hostName == null || hostName == "")
                {
                    System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(UpdateHostName), null);
                }
                return hostName;

            }
        }
	

        public System.Net.IPAddress Gateway
        {
            get { return gateway; }
            set { gateway = value; }
        }
        public int RoundTripTime
        {
            get { return roundTripTime; }
            set { roundTripTime = value; }
        }
        public byte CurrentHop
        {
            get { return currentHop; }
            set { currentHop = value; }
        }

    }
}
