using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ZedGraph;

namespace Terminals.Network.CPU
{
    public partial class CPU : UserControl
    {
        public CPU()
        {
            InitializeComponent();
        }
        private List<long> cpuHistory = new List<long>();
        private void timer1_Tick(object sender, EventArgs e)
        {
            cpuHistory.Add(cpuQuery.Query());
            UpdateGraph();
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
            foreach (int p in cpuHistory)
            {
                if (p > yMax) yMax = p;
                list.Add(x, p);

                sum += p;

                avgList.Add(x, (int)(sum / x));
                x++;
            }

            // Manually set the axis range
            myPane.YAxis.Scale.Min = 0;
            myPane.YAxis.Scale.Max = yMax;
            myPane.XAxis.Scale.Min = 0;
            myPane.XAxis.Scale.Max = x;

            myPane.CurveList.Clear();
            LineItem myCurve = myPane.AddCurve(destination, list, Color.Blue, SymbolType.Diamond);
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
        private Org.Mentalis.Utilities.CpuUsage cpuQuery;
        private void CPU_Load(object sender, EventArgs e)
        {
            cpuQuery = Org.Mentalis.Utilities.CpuUsage.Create();
            myPane = zg1.GraphPane;
            // Set the titles and axis labels
            myPane.Title.Text = "CPU History";
            myPane.XAxis.Title.Text = "Time";
            myPane.YAxis.Title.Text = "Useage";


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

            this.timer1.Enabled = true;
            this.timer1.Start();
        }

        private void CPU_Resize(object sender, EventArgs e)
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
            catch (Exception) { }
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


    }
}
