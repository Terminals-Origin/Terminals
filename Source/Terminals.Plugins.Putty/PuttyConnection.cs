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
    // TODO IConnectionExtra Shouldnt be implemented here.
    internal class PuttyConnection : Connection, IConnectionExtra
    {
        internal const string PUTTY_BINARY = "putty.exe";

        private bool windowCaptured;
        private bool fullScreen;
        private Process puttyProcess;

        private bool IsPuttyReady
        {
            get
            {
                return this.windowCaptured && null != this.puttyProcess && !this.puttyProcess.HasExited;
            }
        }

        public override bool Connected
        {
            get
            {
                return this.IsPuttyReady && this.puttyProcess != null && !this.puttyProcess.HasExited;
            }
        }

        public bool FullScreen
        {
            get
            {
                return this.fullScreen;
            }

            set
            {
                this.fullScreen = value;
                if (this.fullScreen)
                    this.SendFocusToPutty();
            }
        }

        public string Server
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string UserName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Domain
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool ConnectToConsole
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public PuttyConnection()
        {
            this.Resize += this.PuttyConnection_Resize;
            this.ParentChanged += this.PuttyConnection_ParentChanged;
            this.GotFocus += this.PuttyConnection_GotFocus;
        }

        private void PuttyConnection_GotFocus(object sender, EventArgs e)
        {
            this.SendFocusToPutty();
        }

        private void PuttyConnection_ParentChanged(object sender, EventArgs e)
        {
            this.RegisterResizeEndEventHandler();
        }

        private void PuttyConnection_Resize(object sender, EventArgs e)
        {
            this.Invoke(new ThreadStart(this.ClipPutty));
        }

        private void PuttyConnection_ResizeEnd(object sender, EventArgs e)
        {
            this.Invoke(new ThreadStart(this.ClipPutty));
        }

        private void ClipPutty()
        {
            if (this.IsPuttyReady)
            {
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
            this.RegisterResizeEndEventHandler();
            this.Dock = DockStyle.Fill;
            this.LaunchPutty();

            return true; // TODO Return correct connection state
        }

        private void RegisterResizeEndEventHandler()
        {
            // TODO Verify, if the registration isnt executed twise to prevent memory leak and add unregister.
            var parentForm = this.ParentForm as Form;
            if (parentForm != null)
                parentForm.ResizeEnd += this.PuttyConnection_ResizeEnd;
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

            IGuardedSecurity credentials = this.ResolveFavoriteCredentials();
            this.puttyProcess.StartInfo.Arguments = new ArgumentsBuilder(credentials, this.Favorite).Build();
            this.puttyProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;

            this.puttyProcess.Start();
            this.puttyProcess.WaitForInputIdle();

            this.AdjustWindowStyle(this.puttyProcess.MainWindowHandle);
            Methods.SetParent(this.puttyProcess.MainWindowHandle, this.Handle);

            this.windowCaptured = true;

            this.ClipPutty();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing && this.puttyProcess != null && !this.puttyProcess.HasExited)
                this.ClosePutty();
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

        void IConnectionExtra.Focus()
        {
            this.SendFocusToPutty();
        }

        private void SendFocusToPutty()
        {
            if (this.IsPuttyReady)
            {
                this.Invoke(new ThreadStart(() =>
                {
                    Methods.SetForegroundWindow(this.puttyProcess.MainWindowHandle);
                    Methods.SetFocus(this.puttyProcess.MainWindowHandle);
                }));
            }
        }
    }
}
