using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Terminals.Connections;
using Terminals.Data;
using Terminals.Native;

namespace Terminals.Plugins.WinBox
{
    internal class WinBoxConnection : Connection, IFocusable
    {
        private bool windowCaptured;
        private Process WinBoxProcess;
        private IntPtr WinBoxHandle;

        public override bool Connected
        {
            get
            {
                return this.windowCaptured && this.ProcessRunning;
            }
        }

        private bool ProcessRunning
        {
            get
            {
                return this.WinBoxProcess != null && !this.WinBoxProcess.HasExited;
            }
        }

        public WinBoxConnection()
        {
            this.Resize += this.WinBoxConnection_Resize;
            this.GotFocus += this.WinBoxConnection_GotFocus;
        }

        private void WinBoxConnection_GotFocus(object sender, EventArgs e)
        {
            this.SendFocusToWinBox();
        }

        private void WinBoxConnection_Resize(object sender, EventArgs e)
        {
            Invoke(new ThreadStart(ClipWinBox));
        }

        private void ClipWinBox()
        {
            if (!this.Connected)
                return;

            Rectangle windowRect = Methods.GetWindowRect(this.WinBoxHandle);

            Rectangle clientRect = Methods.GetClientRect(this.WinBoxHandle);

            Point referencePoint0 = new Point(0, 0);
            Methods.ClientToScreen(this.WinBoxHandle, ref referencePoint0);

            Point referencePoint1 = new Point(clientRect.Width, clientRect.Height);
            Methods.ClientToScreen(this.WinBoxHandle, ref referencePoint1);

            int top = referencePoint0.Y - windowRect.Top;
            int left = referencePoint0.X - windowRect.Left;
            int bottom = windowRect.Bottom - referencePoint1.Y;

            int width = this.Width + left + left; // + right ( using doubled left )
            int height = this.Height + top + bottom;

            Methods.SetWindowPos(this.WinBoxHandle, IntPtr.Zero, -left, -top, width, height, SetWindowPosFlags.FrameChanged | SetWindowPosFlags.DoNotActivate);
        }

        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        public override bool Connect()
        {
            this.Dock = DockStyle.Fill;
            this.LaunchWinBoxProcess();

            // Timer is needed to reattach WinBox when it somehow changes MainWindow (It does this)
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();

            return true; // Not connected WinBox fires later proces exited.
        }

        public void Timer_Tick(object sender, EventArgs e)
        {
            AttachWinBox();
        }

        private void LaunchWinBoxProcess()
        {
            IGuardedSecurity credentials = this.ResolveFavoriteCredentials();

            this.WinBoxProcess = new Process();
            this.WinBoxProcess.StartInfo.FileName = Executables.GetWinBoxBinaryPath();
            this.WinBoxProcess.StartInfo.Arguments = string.Format("{0}:{1} {2} {3}", this.Favorite.ServerName, this.Favorite.Port, credentials.UserName, credentials.Password);
            this.WinBoxProcess.EnableRaisingEvents = true;
            this.WinBoxProcess.Exited += this.WinBoxProcessOnExited;
            this.WinBoxProcess.Start();
            this.WinBoxProcess.WaitForInputIdle();

            AttachWinBox();
        }

        public void AttachWinBox()
        {
            var CurrentProcess = new Process();
            try { CurrentProcess = Process.GetProcessById(WinBoxProcess.Id); } catch { return; } // For some reason this is needed
            var NewHandle = CurrentProcess.MainWindowHandle;

            var windowplacement = new WINDOWPLACEMENT();
            Methods.GetWindowPlacement(NewHandle, ref windowplacement);

            if (Methods.GetParent(NewHandle) != this.Handle && windowplacement.ShowCmd == ShowWindowCommands.Normal)
            {
                WinBoxHandle = NewHandle;
                IntPtr WinBoxWindow = this.WinBoxHandle;
                Methods.SetParent(WinBoxWindow, this.Handle);
                this.windowCaptured = true;
            }

            this.ClipWinBox();
        }

        private void WinBoxProcessOnExited(object sender, EventArgs eventArgs)
        {
            this.FireDisconnected();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.ProcessRunning)
                this.CloseWinBox();

            base.Dispose(disposing);
        }

        private void CloseWinBox()
        {
            try
            {
                this.WinBoxProcess.Kill();
            }
            catch
            {
                Logging.Warn("Unable to close WinBox process.");
            }
            finally
            {
                this.WinBoxProcess.Dispose();
            }
        }

        void IFocusable.Focus()
        {
            this.SendFocusToWinBox();
        }

        private void SendFocusToWinBox()
        {
            if (!this.Connected)
                return;

            this.Invoke(new ThreadStart(() =>
            {
                IntPtr WinBoxWindow = this.WinBoxHandle;
                Methods.SetForegroundWindow(WinBoxWindow);
                Methods.SetFocus(WinBoxWindow);
            }));
        }
    }
}
