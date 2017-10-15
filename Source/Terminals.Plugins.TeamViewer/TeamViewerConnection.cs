using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Terminals.Connections;
using Terminals.Data;
using Terminals.Native;

namespace Terminals.Plugins.WinBox
{
    internal class TeamViewerConnection : Connection, IFocusable
    {
        private bool windowCaptured;
        private IntPtr TeamViewerHandle;
        private bool attached = false;
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        public override bool Connected
        {
            get
            {
                return this.windowCaptured && WindowRunning;
            }
        }

        private bool WindowRunning
        {
            get
            {
                return Methods.IsWindow(TeamViewerHandle);
            }
        }

        public TeamViewerConnection()
        {
            this.Resize += this.WinBoxConnection_Resize;
            this.GotFocus += this.TeamViewerConnection_GotFocus;
        }

        private void TeamViewerConnection_GotFocus(object sender, EventArgs e)
        {
            this.SendFocusToTeamViewer();
        }

        private void WinBoxConnection_Resize(object sender, EventArgs e)
        {
            Invoke(new ThreadStart(ClipTeamViewer));
        }

        private void ClipTeamViewer()
        {
            if (!this.Connected)
                return;

            Rectangle windowRect = Methods.GetWindowRect(this.TeamViewerHandle);

            Rectangle clientRect = Methods.GetClientRect(this.TeamViewerHandle);

            Point referencePoint0 = new Point(0, 0);
            Methods.ClientToScreen(this.TeamViewerHandle, ref referencePoint0);

            Point referencePoint1 = new Point(clientRect.Width, clientRect.Height);
            Methods.ClientToScreen(this.TeamViewerHandle, ref referencePoint1);

            int top = referencePoint0.Y - windowRect.Top;
            int left = referencePoint0.X - windowRect.Left;
            int bottom = windowRect.Bottom - referencePoint1.Y;

            int width = this.Width + left + left; // + right ( using doubled left )
            int height = this.Height + top + bottom;

            Methods.SetWindowPos(this.TeamViewerHandle, IntPtr.Zero, -left, -top, width, height, SetWindowPosFlags.FrameChanged | SetWindowPosFlags.DoNotActivate);
        }

        public override bool Connect()
        {
            this.Dock = DockStyle.Fill;
            this.LaunchTeamViewer();

            // Timer that checks if TeamViewer window is still open, and closes tab if it isn't
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();

            return true;
        }

        public void Timer_Tick(object sender, EventArgs e)
        {
            this.ClipTeamViewer();

            if (!Connected)
                this.FireDisconnected();
        }

        private void LaunchTeamViewer()
        {
            string FileName = "";

            try
            {
                FileName = (string)Registry.ClassesRoot.OpenSubKey(@".tvlink\DefaultIcon").GetValue(null);
            }
            catch
            {
                MessageBox.Show("Can't find TeamViewer executable. Error logged.");
                Logging.Error(@"Could not find HKEY_CLASSES_ROOT\.tvlink\DefaultIcon");
                return;
            }

            IGuardedSecurity credentials = this.ResolveFavoriteCredentials();

            var TeamViewerProcess = new Process();
            TeamViewerProcess.StartInfo.FileName = FileName;
            TeamViewerProcess.StartInfo.Arguments = string.Format("-i {0} -P {1}", this.Favorite.ServerName, credentials.Password);
            TeamViewerProcess.Start();
            TeamViewerProcess.WaitForInputIdle();

            var Start = DateTime.Now;

            while (!attached && (DateTime.Now - Start).TotalSeconds < 10)
                AttachTeamViewer();
        }

        public void AttachTeamViewer()
        {
            if (!attached)
            {
                foreach (var window in Methods.GetOpenWindows())
                {
                    IntPtr handle = window.Key;
                    string title = window.Value.Replace(" ", "");
                    string tvid = this.Favorite.ServerName.ToLower().Replace(" ", "");

                    if (title.ToLower().Contains(tvid))
                    {
                        TeamViewerHandle = handle;
                        Methods.SetParent(handle, this.Handle);
                        attached = true;
                        this.ClipTeamViewer();
                    }
                }
            }

            this.windowCaptured = true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.WindowRunning)
                this.CloseTeamViewer();

            base.Dispose(disposing);
        }

        private void CloseTeamViewer()
        {
            try
            {
                Methods.CloseWindow(TeamViewerHandle);
            }
            catch
            {
                Logging.Warn("Unable to close TeamViewer window.");
            }
        }

        void IFocusable.Focus()
        {
            this.SendFocusToTeamViewer();
        }

        private void SendFocusToTeamViewer()
        {
            if (!this.Connected)
                return;

            this.Invoke(new ThreadStart(() =>
            {
                IntPtr teamviewerWindow = this.TeamViewerHandle;
                Methods.SetForegroundWindow(teamviewerWindow);
                Methods.SetFocus(teamviewerWindow);
            }));
        }
    }
}
