using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Terminals.Connections;
using Terminals.Data;
using Terminals.Native;

namespace Terminals.Plugins.Putty
{
    internal class PuttyConnection : Connection
    {
        internal const string PUTTY_BINARY = "putty.exe";

        private bool windowCaptured;
        private Process puttyProcess;

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
                return this.puttyProcess != null && !this.puttyProcess.HasExited;
            }
        }

        public PuttyConnection()
        {
            this.Resize += this.PuttyConnection_Resize;
            this.GotFocus += this.PuttyConnection_GotFocus;
        }

        private void PuttyConnection_GotFocus(object sender, EventArgs e)
        {
            this.SendFocusToPutty();
        }

        private void PuttyConnection_Resize(object sender, EventArgs e)
        {
            this.Invoke(new ThreadStart(this.ClipPutty));
        }

        private void ClipPutty()
        {
            if (!this.Connected)
                return;

            Rectangle windowRect = Methods.GetWindowRect(this.puttyProcess.MainWindowHandle);

            Rectangle clientRect = Methods.GetClientRect(this.puttyProcess.MainWindowHandle);

            Point referencePoint0 = new Point(0, 0);
            Methods.ClientToScreen(this.puttyProcess.MainWindowHandle, ref referencePoint0);

            Point referencePoint1 = new Point(clientRect.Width, clientRect.Height);
            Methods.ClientToScreen(this.puttyProcess.MainWindowHandle, ref referencePoint1);

            int top = referencePoint0.Y - windowRect.Top;
            int left = referencePoint0.X - windowRect.Left;

            int right = windowRect.Right - referencePoint1.X; // TODO VERIFY: right contains the width of the scrool that should be shown
            int bottom = windowRect.Bottom - referencePoint1.Y;

            int width = this.Width + left + left; // + right ( using doubled left )
            int height = this.Height + top + bottom;

            Methods.SetWindowPos(this.puttyProcess.MainWindowHandle, IntPtr.Zero, -left, -top, width, height, SetWindowPosFlags.FrameChanged | SetWindowPosFlags.DoNotActivate);
        }

        private void AdjustWindowStyle(IntPtr handle)
        {
            uint lStyle = Methods.GetWindowLong(handle, (int) WindowLongParam.GWL_STYLE);
            WindowStyles flagsToDisable = ~(WindowStyles.WS_CAPTION | WindowStyles.WS_THICKFRAME | WindowStyles.WS_MINIMIZE | WindowStyles.WS_MAXIMIZE | WindowStyles.WS_SYSMENU);
            lStyle &= (uint)flagsToDisable;
            Methods.SetWindowLong(handle, (int)WindowLongParam.GWL_STYLE, lStyle);

            uint lExStyle = Methods.GetWindowLong(handle, (int)WindowLongParam.GWL_EXSTYLE);
            WindowExStyles flagsToDisableEx = ~(WindowExStyles.WS_EX_DLGMODALFRAME | WindowExStyles.WS_EX_CLIENTEDGE | WindowExStyles.WS_EX_STATICEDGE);
            lExStyle &= (uint)flagsToDisableEx;
            Methods.SetWindowLong(handle, (int)WindowLongParam.GWL_EXSTYLE, lExStyle);
        }

        public override bool Connect()
        {
            this.Dock = DockStyle.Fill;
            this.LaunchPutty();

            return true; // TODO Return correct connection state
        }

        internal string GetPuttyBinaryPath()
        {
            return this.GetPuttyBinaryPath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        }

        internal string GetPuttyBinaryPath(string baseLocation)
        {
            return Path.Combine(baseLocation, "Resources", PUTTY_BINARY);
        }

        private void LaunchPutty()
        {
            this.puttyProcess = new Process();
            this.puttyProcess.StartInfo.FileName = this.GetPuttyBinaryPath();

            // security issue: password visible in taskbar
            IGuardedSecurity credentials = this.ResolveFavoriteCredentials();
            this.puttyProcess.StartInfo.Arguments = new ArgumentsBuilder(credentials, this.Favorite).Build();
            this.puttyProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
            this.puttyProcess.EnableRaisingEvents = true;
            this.puttyProcess.Exited += this.PuttyProcessOnExited;
            this.puttyProcess.Start();
            this.puttyProcess.WaitForInputIdle();

            IntPtr puttyWindow = this.puttyProcess.MainWindowHandle;
            this.AdjustWindowStyle(puttyWindow);
            Methods.SetParent(puttyWindow, this.Handle);
            this.windowCaptured = true;

            this.ClipPutty();
        }

        private void PuttyProcessOnExited(object sender, EventArgs eventArgs)
        {
            this.FireDisconnected();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.ProcessRunning)
                this.ClosePutty();

            base.Dispose(disposing);
        }

        private void ClosePutty()
        {
            try
            {
                this.puttyProcess.Kill();
            }
            catch
            {
                Logging.Warn("Unable to close putty process.");
            }
            finally
            {
                this.puttyProcess.Dispose();
            }
        }

        private void SendFocusToPutty()
        {
            if (!this.Connected)
                return;

            this.Invoke(new ThreadStart(() =>
            {
                IntPtr puttyWindow = this.puttyProcess.MainWindowHandle;
                Methods.SetForegroundWindow(puttyWindow);
                Methods.SetFocus(puttyWindow);
            }));
        }
    }
}
