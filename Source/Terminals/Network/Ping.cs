using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using ZedGraph;

namespace Terminals.Network
{
    public partial class Ping : UserControl
    {
        #region Fields
        
        private Int32 currentDelay = 0;
        private Int64 counter;
        private Boolean pingRunning = false;
        private Boolean pingReady = false;
        private System.Threading.Timer timer;
        private MethodInvoker DoUpdateForm;
        private AutoResetEvent waiter = new AutoResetEvent(false);
        private List<PingReplyData> pingList = new List<PingReplyData>();
        private Object threadLocker = new Object();
        private GraphPane myPane;
        private System.Net.NetworkInformation.Ping pingSender;
        private Byte[] buffer;
        private PingOptions packetOptions;
        private String hostName = String.Empty;
        private String destination = String.Empty;

        #endregion

        #region Constructors
        
        public Ping()
        {
            InitializeComponent();
            this.DoUpdateForm = new MethodInvoker(this.UpdateForm);

            // Create a buffer of 32 bytes of data to be transmitted.
            this.buffer = Encoding.ASCII.GetBytes(new String('.', 32));
            // Jump though 50 routing nodes tops, and don't fragment the packet
            this.packetOptions = new PingOptions(50, true);

            this.InitializeGraph();
        }

        #endregion

        #region Form Events

        private void Ping_Load(object sender, EventArgs e)
        {
            this.TextHost.Focus();
        }

        private void Ping_Resize(object sender, EventArgs e)
        {
            this.SetSize();
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.TextHost.Text.Trim()))
            {
                this.TextHost.Focus();
                return;
            }
            else
            {
                this.TextHost.Text = this.TextHost.Text.Trim();
            }

            this.ButtonStart.Enabled = false;
            this.TextHost.Enabled = false;
            Application.DoEvents();

            if (!this.pingRunning)
            {
                IPAddress[] list = null;
                String msg = String.Empty;
                try
                {
                    list = System.Net.Dns.GetHostAddresses(this.TextHost.Text);
                    if (list != null)
                    {
                        this.destination = list[0].ToString();

                        IPAddress ip;
                        this.hostName = (IPAddress.TryParse(this.TextHost.Text, out ip)) ? this.destination : Dns.GetHostEntry(this.TextHost.Text).HostName;

                        this.counter = 1;
                        this.currentDelay = (Int32)this.DelayNumericUpDown.Value;
                        this.pingList = new List<PingReplyData>();

                        if (this.pingSender == null)
                        {
                            this.pingSender = new System.Net.NetworkInformation.Ping();
                            this.pingSender.PingCompleted += new PingCompletedEventHandler(pingSender_PingCompleted);
                        }

                        this.pingRunning = true;
                        this.pingReady = true;

                        // Making sure previous timer is cleared before starting a new one
                        if (this.timer != null)
                        {
                            this.timer.Dispose();
                            this.timer = null;
                        }

                        // Start thread timer to start TrySend method for every ms in the specified delay updown box
                        TimerCallback callback = new TimerCallback(this.TryPing);
                        timer = new System.Threading.Timer(callback, null, this.currentDelay, this.currentDelay);
                    }
                }
                catch (System.Net.Sockets.SocketException)
                {
                    msg = String.Format("Could not resolve address: {0}", this.TextHost.Text);
                    this.pingRunning = false;
                }
                catch (ArgumentException)
                {
                    msg = String.Format("Hostname or IP-Address is invalid: {0}", this.TextHost.Text);
                    this.pingRunning = false;
                }
                catch (Exception ex)
                {
                    msg = String.Format("An error occured trying to ping {0}", this.TextHost.Text);
                    Terminals.Logging.Log.Info(msg, ex);
                    this.pingRunning = false;
                }
                finally
                {
                    if (!this.pingRunning)
                    {
                        MessageBox.Show(msg, "Ping Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.ResetForm();
                    }
                }
            }
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            if (this.pingRunning)
            {
                this.pingRunning = false;
                this.pingReady = false;

                if (this.pingSender != null)
                {
                    this.pingSender.PingCompleted -= pingSender_PingCompleted;
                    this.pingSender.SendAsyncCancel();
                    this.pingSender.Dispose();
                    this.pingSender = null;
                }

                this.timer.Dispose();
                this.timer = null;
            }

            this.ResetForm();
        }

        private void TextHost_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                e.Handled = true;
                if (!this.pingRunning)
                    this.ButtonStart.PerformClick();
            }
        }

        #endregion

        #region Developer made methods

        private void TryPing(Object state)
        {
            if (this.pingRunning && this.pingReady)
            {
                this.SendPing();
            }
        }

        private void SendPing()
        {
            try
            {
                this.pingSender.SendAsync(this.destination, 4000, this.buffer, this.packetOptions, this.waiter);
                this.waiter.WaitOne(0);

                lock (this.timer)
                {
                    if (this.DelayNumericUpDown.Value != this.currentDelay)
                    {
                        this.currentDelay = (Int32)this.DelayNumericUpDown.Value;
                        this.timer.Change(this.currentDelay, this.currentDelay);
                    }
                }
            }
            catch (InvalidOperationException)
            {
                // Error: An asynchronous call is already in progress
                // Overflow of SendAsync calls. Just let it go, or does someone know how to handle this better?
            }
            catch (Exception ex)
            {
                Terminals.Logging.Log.Info(String.Empty, ex);
            }
        }

        private void pingSender_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            try
            {
                if (Thread.CurrentThread.Name == null)
                    Thread.CurrentThread.Name = "Ping Completed";

                ((AutoResetEvent)e.UserState).Set();

                if (e.Reply.Status == IPStatus.Success)
                {
                    lock (this.threadLocker)
                    {
                        PingReplyData pd = new PingReplyData(
                            this.counter++,
                            "Reply from: ",
                            this.hostName,
                            this.destination,
                            e.Reply.Buffer.Length,
                            e.Reply.Options.Ttl,
                            e.Reply.RoundtripTime);

                        lock (this.pingList)
                        {
                            this.pingList.Add(pd);
                        }

                        this.Invoke(this.DoUpdateForm);
                        this.pingReady = true;
                    }
                }
                else if (!e.Cancelled)
                {
                    String status = String.Empty;
                    switch (e.Reply.Status)
                    {
                        case IPStatus.TimedOut:
                            status = "Request timed out.";
                            break;

                        case IPStatus.DestinationHostUnreachable:
                            status = "Destination host unreachable.";
                            break;
                    }

                    lock (this.threadLocker)
                    {
                        this.pingSender.SendAsyncCancel();
                        PingReplyData pd = new PingReplyData(
                            this.counter++, status, String.Empty, String.Empty, 0, 0, 0);

                        lock (this.pingList)
                        {
                            this.pingList.Add(pd);
                        }

                        this.Invoke(this.DoUpdateForm);
                        this.pingReady = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Terminals.Logging.Log.Info("Error on Ping.PingCompleted", ex);
            }
            finally
            {
                ((AutoResetEvent)e.UserState).Set();
            }
        }

        public void ForcePing(String hostName)
        {
            this.TextHost.Text = hostName;
            this.ButtonStart.PerformClick();
        }

        /// <summary>
        /// Update form control with new data.
        /// </summary>
        private void UpdateForm()
        {
            this.dataGridView1.SuspendLayout();

            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = this.pingList;

            if (this.dataGridView1.Rows.Count > 1)
                this.dataGridView1.FirstDisplayedScrollingRowIndex = this.dataGridView1.Rows.Count - 1;

            this.dataGridView1.ResumeLayout(true);
            this.UpdateGraph();
            Application.DoEvents();
        }

        /// <summary>
        /// Reset the form control to start properties.
        /// </summary>
        private void ResetForm()
        {
            this.ButtonStart.Enabled = true;
            this.TextHost.Enabled = true;
            this.TextHost.Focus();
            this.TextHost.SelectAll();
        }

        #endregion

        #region Graph Control

        private void InitializeGraph()
        {
            myPane = this.ZGraph.GraphPane;
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
                0.02f, 0.15f, CoordType.ChartFraction, AlignH.Left, AlignV.Bottom);
            text.FontSpec.Size = 8;
            text.FontSpec.StringAlignment = StringAlignment.Near;
            myPane.GraphObjList.Add(text);

            // Enable scrollbars if needed
            this.ZGraph.IsShowHScrollBar = true;
            this.ZGraph.IsShowVScrollBar = true;

            // OPTIONAL: Show tooltips when the mouse hovers over a point
            this.ZGraph.IsShowPointValues = true;
            this.ZGraph.PointValueEvent += new ZedGraphControl.PointValueHandler(this.MyPointValueHandler);

            // OPTIONAL: Add a custom context menu item
            //this.ZGraph.ContextMenuBuilder += new ZedGraphControl.ContextMenuBuilderEventHandler(this.MyContextMenuBuilder);

            // Size the control to fit the window
            this.SetSize();
        }

        private void UpdateGraph()
        {
            // Make up some data points based on the Sine function
            PointPairList list = new PointPairList();
            PointPairList avgList = new PointPairList();
            Int32 x = 1;
            Int64 yMax = 0;
            Int64 sum = 0;

            foreach (PingReplyData p in pingList)
            {
                if (p.RoundTripTime > yMax)
                    yMax = p.RoundTripTime;

                list.Add(x, p.RoundTripTime);

                sum += p.RoundTripTime;
                avgList.Add(x, (Int32)(sum / x));
                x++;
            }

            myPane.Title.Text = String.Format("Ping results for {0}", this.TextHost.Text);

            // Manually set the axis range
            myPane.YAxis.Scale.Min = 0;
            myPane.YAxis.Scale.Max = yMax;
            myPane.XAxis.Scale.Min = 0;
            myPane.XAxis.Scale.Max = x;

            myPane.CurveList.Clear();
            LineItem myCurve = myPane.AddCurve(this.TextHost.Text, list, Color.Blue, SymbolType.Diamond);
            LineItem avgCurve = myPane.AddCurve("Average", avgList, Color.Red, SymbolType.Diamond);

            // Fill the symbols with white
            myCurve.Symbol.Fill = new Fill(Color.White);
            // Associate this curve with the Y2 axis
            myCurve.IsY2Axis = true;

            // Tell ZedGraph to calculate the axis ranges
            // Note that you MUST call this after enabling IsAutoScrollRange, since AxisChange() sets
            // up the proper scrolling parameters
            ZGraph.AxisChange();
            // Make sure the Graph gets redrawn
            ZGraph.Invalidate();
        }
       
        private void SetSize()
        {
            ZGraph.Location = new Point(10, 10);
            // Leave a small margin around the outside of the control
            ZGraph.Size = new Size(this.ClientRectangle.Width - 20, this.ClientRectangle.Height - 20);
        }

        /// <summary>
        /// Display customized tooltips when the mouse hovers over a point
        /// </summary>
        private string MyPointValueHandler(ZedGraphControl control, GraphPane pane, CurveItem curve, int iPt)
        {
            try
            {
                // Get the PointPair that is under the mouse
                PointPair pt = curve[iPt];

                return String.Format("{0} is {1:f2} milliseconds at {2:f1}", curve.Label.Text, pt.Y, pt.X);
            }
            catch (Exception ex)
            {
                Terminals.Logging.Log.Info(String.Empty, ex);
            }
            
            return String.Empty;
        }

        #endregion
    }
 }
