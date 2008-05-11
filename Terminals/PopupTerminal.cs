using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Terminals {
    public partial class PopupTerminal : Form {
        public PopupTerminal() {
            InitializeComponent();
            
        }
        private System.Windows.Forms.Timer timerHover;
        private System.Windows.Forms.Timer closeTimer;

        private MainForm mainForm;
        public MainForm MainForm {
            get {
                return mainForm;
            }
            set {
                mainForm = value;
            }
        }

        public void AddTerminal(TerminalTabControlItem TabControlItem) {


            
            this.tabControl1.AddTab(TabControlItem);
            
            Terminals.Connections.Connection b = (TabControlItem.Connection as Terminals.Connections.Connection);
            this.Text = TabControlItem.Connection.Favorite.Name;


        }

        void timerHover_Tick(object sender, EventArgs e) {
            if(timerHover.Enabled) {
                timerHover.Enabled = false;
                tabControl1.ShowTabs = true;
            }
        }


        private void tabControl1_TabControlItemClosing(TabControl.TabControlItemClosingEventArgs e) {
            if(this.tabControl1.Items.Count <= 1) {
                this.Close();
            }
        }


        private void tcTerminals_MouseHover(object sender, EventArgs e) {
            if(tabControl1 != null) {
                if(!tabControl1.ShowTabs) {
                    timerHover.Enabled = true;
                }
            }
        }
        private void tcTerminals_MouseLeave(object sender, EventArgs e) {
            timerHover.Enabled = false;
            if(FullScreen && tabControl1.ShowTabs && !tabControl1.MenuOpen) {
                tabControl1.ShowTabs = false;
            }
           
        }
        private bool fullScreen = false;
        public bool FullScreen {
            get {
                return fullScreen;
            }
            set {
                fullScreen = value;
                if(value) {
                    this.FormBorderStyle = FormBorderStyle.None;
                    this.WindowState = FormWindowState.Maximized;
                } else {
                    this.FormBorderStyle = FormBorderStyle.Sizable;
                    this.WindowState = FormWindowState.Normal;
                }
            }
        }

        private void tabControl1_DoubleClick(object sender, EventArgs e) {
            FullScreen = !fullScreen;
        }

        private void PopupTerminal_Load(object sender, EventArgs e) {
            timerHover = new Timer();
            timerHover.Interval = 200;
            timerHover.Tick += new EventHandler(timerHover_Tick);
            timerHover.Start();

            closeTimer = new Timer();
            closeTimer.Interval = 500;
            closeTimer.Tick += new EventHandler(closeTimer_Tick);
            closeTimer.Start();
        }
        object synLock = new object();
        void closeTimer_Tick(object sender, EventArgs e) {
            lock(synLock) {
                closeTimer.Enabled = false;
                System.Collections.Generic.List<TabControl.TabControlItem> removeableTabs = new List<TabControl.TabControlItem>();
                foreach(TabControl.TabControlItem tab in this.tabControl1.Items) {
                    if(tab.Controls.Count > 0) {
                        if(!(tab.Controls[0] as Terminals.Connections.IConnection).Connected) {
                            removeableTabs.Add(tab);
                        }
                    }
                }
                foreach(TabControl.TabControlItem tab in removeableTabs) {
                    tabControl1.CloseTab(tab);
                    tab.Dispose();
                }
                closeTimer.Enabled = true;
            }
        }

        private void tabControl1_MouseDown(object sender, MouseEventArgs e) {            
            
        }

        private void attachToTerminalsToolStripMenuItem_Click(object sender, EventArgs e) {
            if(mainForm != null && this.tabControl1.SelectedItem!=null) {
                mainForm.AddTerminal((TerminalTabControlItem) this.tabControl1.SelectedItem);
                //this.tabControl1.RemoveTab(this.tabControl1.SelectedItem);
                this.tabControl1.CloseTab(this.tabControl1.SelectedItem);
            }
        }
    }
}
