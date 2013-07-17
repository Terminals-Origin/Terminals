using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;

namespace Terminals.Network
{
    internal partial class TraceRouteControl : UserControl
    {
        #region Fields
        
        private Boolean traceRunning;
        private TraceRoute tracert;
        private List<TraceRouteHopData> hopList = new List<TraceRouteHopData>();
        private readonly MethodInvoker doUpdateForm;
        private GraphPane myPane;

        #endregion

        public TraceRouteControl()
        {
            InitializeComponent();
            this.doUpdateForm = new MethodInvoker(this.UpdateForm);

            this.InitializeGraph();
        }

        #region Form Events

        private void TraceRoute_Load(object sender, EventArgs e)
        {
            this.TextHost.Focus();
        }

        private void TraceRoute_Resize(object sender, EventArgs e)
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
            
            this.TextHost.Text = this.TextHost.Text.Trim();
            this.ButtonStart.Enabled = false;
            this.TextHost.Enabled = false;

            if (!this.traceRunning)
            {
                if (this.tracert == null)
                {
                    this.tracert = new TraceRoute();
                    this.tracert.Completed += new EventHandler(this.Tracert_Completed);
                    this.tracert.RouteHopFound += new EventHandler<RouteHopFoundEventArgs>(this.Tracert_RouteHopFound);
                }

                tracert.Destination = this.TextHost.Text;
                tracert.ResolveNames = this.ResolveCheckBox.Checked;
                tracert.Start();
                this.traceRunning = true;
            }

            this.hopList = new List<TraceRouteHopData>();
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            this.tracert.Cancel();
            this.ResetForm();
        }

        private void TextHost_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                e.Handled = true;
                if (!this.traceRunning)
                    this.ButtonStart.PerformClick();
            }
        }

        #endregion

        #region Developer made methods

        private void Tracert_Completed(object sender, EventArgs e)
        {
            this.traceRunning = false;
            this.ResetForm();
        }

        private void Tracert_RouteHopFound(object sender, RouteHopFoundEventArgs e)
        {
            hopList = this.tracert.Hops;
            this.Invoke(this.doUpdateForm);
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

        public void ForceTrace(String hostName)
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
            this.dataGridView1.DataSource = this.hopList;

            if (this.dataGridView1.Rows.Count > 1)
                this.dataGridView1.FirstDisplayedScrollingRowIndex = this.dataGridView1.Rows.Count - 1;

            this.dataGridView1.ResumeLayout(true);
            this.UpdateGraph();
            Application.DoEvents();
        }

        #endregion

        #region Graph Control

        private void InitializeGraph()
        {
            myPane = this.ZGraph.GraphPane;
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
                0.02f, 0.15f, CoordType.ChartFraction, AlignH.Left, AlignV.Bottom);
            text.FontSpec.Size = 8;
            text.FontSpec.StringAlignment = StringAlignment.Near;
            myPane.GraphObjList.Add(text);

            // Enable scrollbars if needed
            this.ZGraph.IsShowHScrollBar = true;
            this.ZGraph.IsShowVScrollBar = true;

            // OPTIONAL: Show tooltips when the mouse hovers over a point
            this.ZGraph.IsShowPointValues = true;
            this.ZGraph.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);

            // OPTIONAL: Add a custom context menu item
            this.ZGraph.ContextMenuBuilder += new ZedGraphControl.ContextMenuBuilderEventHandler(MyContextMenuBuilder);

            // OPTIONAL: Handle the Zoom Event

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

            foreach (TraceRouteHopData p in hopList)
            {
                if (p.RoundTripTime > yMax)
                    yMax = p.RoundTripTime;

                list.Add(x, p.RoundTripTime, p.Address.ToString());

                sum += p.RoundTripTime;
                avgList.Add(x, (Int32)(sum / x));
                x++;
            }

            myPane.Title.Text = String.Format("Trace Route results for {0}", this.TextHost.Text);

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

        /// <summary>
        /// Display customized tooltips when the mouse hovers over a point
        /// </summary>
        private string MyPointValueHandler(ZedGraphControl control, GraphPane pane, CurveItem curve, int iPt)
        {
            try
            {
                // Get the PointPair that is under the mouse
                PointPair pt = curve[iPt];

                return String.Format("{0} is {1:f2} milliseconds at {2:f1}", pt.Tag, pt.Y, pt.X);
            }
            catch (Exception ex)
            {
                Logging.Log.Error(ex);
            }

            return String.Empty;
        }

        private void SetSize()
        {
            ZGraph.Location = new Point(10, 10);
            // Leave a small margin around the outside of the control
            ZGraph.Size = new Size(this.ClientRectangle.Width - 20, this.ClientRectangle.Height - 20);
        }

        /// <summary>
        /// Customize the context menu by adding a new item to the end of the menu
        /// </summary>
        private void MyContextMenuBuilder(ZedGraphControl control, ContextMenuStrip menuStrip, Point mousePt, ZedGraphControl.ContextMenuObjectState objState)
        {
            //ToolStripMenuItem item = new ToolStripMenuItem();
            //item.Name = "add-beta";
            //item.Tag = "add-beta";
            //item.Text = "Add a new Beta Point";
            //item.Click += new System.EventHandler(AddBetaPoint);

            //menuStrip.Items.Add(item);
        }

        #endregion
    }
}
