using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Terminals.Connections;
using Terminals.Data;

namespace Terminals.Plugins.Putty
{
    internal class PuttyConnection : Connection, IConnectionExtra
    {
        const string PUTTY_BINARY = "putty.exe";

        private bool windowCaptured = false;
        private Process puttyProcess;

        private bool IsPuttyReady {
            get {
                return windowCaptured && null != puttyProcess && !puttyProcess.HasExited;
            }
        }

        public PuttyConnection()
        {
            this.Resize += PuttyConnection_Resize;
            this.ParentChanged += PuttyConnection_ParentChanged;
            this.GotFocus += PuttyConnection_GotFocus;
        }

        private void PuttyConnection_Resize(object sender, EventArgs e)
        {
            this.Invoke(new ThreadStart(this.ClipPutty));
        }

        private void SendFocusToPutty()
        {
            if (IsPuttyReady)
            {
                this.Invoke(new ThreadStart(() => {
                    NativeMethods.SetForegroundWindow(puttyProcess.MainWindowHandle);
                    NativeMethods.SetFocus(puttyProcess.MainWindowHandle);
                }));
                
            }
        }

        private void PuttyConnection_GotFocus(object sender, EventArgs e)
        {
            SendFocusToPutty();
        }

        private void PuttyConnection_ParentChanged(object sender, EventArgs e)
        {
            if (null != this.ParentForm)
                ((Form)this.ParentForm).ResizeEnd += PuttyConnection_ResizeEnd;
        }
        

        private void ClipPutty()
        {
            if (IsPuttyReady)
            {
                NativeMethods.RECT windowRect;
                NativeMethods.GetWindowRect(puttyProcess.MainWindowHandle, out windowRect);

                NativeMethods.RECT clientRect;
                NativeMethods.GetClientRect(puttyProcess.MainWindowHandle, out clientRect);

                Point referencePoint0 = new Point(0, 0);
                NativeMethods.ClientToScreen(puttyProcess.MainWindowHandle, ref referencePoint0);

                Point referencePoint1 = new Point(clientRect.Width, clientRect.Height);
                NativeMethods.ClientToScreen(puttyProcess.MainWindowHandle, ref referencePoint1);

                var top = (referencePoint0.Y - windowRect.Top);
                var left = (referencePoint0.X - windowRect.Left);

                var right = windowRect.Right - referencePoint1.X; // right contains the width of the scrool that should be shown
                var bottom = windowRect.Bottom - referencePoint1.Y;

                var width = this.Width + left + left; // + right ( using doubled left )
                var height = this.Height + top + bottom;

                NativeMethods.SetWindowPos(puttyProcess.MainWindowHandle, IntPtr.Zero, -left, -top, width, height, NativeMethods.SetWindowPosFlags.FrameChanged | NativeMethods.SetWindowPosFlags.DoNotActivate);
            }
        }


        private void AdjustWindowStyle(IntPtr handle)
        {
            uint lStyle = NativeMethods.GetWindowLong(handle, (int) NativeMethods.WindowLongParam.GWL_STYLE);
            NativeMethods.WindowStyles flagsToDisable = ~(NativeMethods.WindowStyles.WS_CAPTION | NativeMethods.WindowStyles.WS_THICKFRAME | NativeMethods.WindowStyles.WS_MINIMIZE | NativeMethods.WindowStyles.WS_MAXIMIZE | NativeMethods.WindowStyles.WS_SYSMENU);
            lStyle &= (uint) flagsToDisable;
            NativeMethods.SetWindowLong(handle, (int)NativeMethods.WindowLongParam.GWL_STYLE, lStyle);

            uint lExStyle = NativeMethods.GetWindowLong(handle, (int)NativeMethods.WindowLongParam.GWL_EXSTYLE);
            NativeMethods.WindowExStyles flagsToDisableEx = ~(NativeMethods.WindowExStyles.WS_EX_DLGMODALFRAME | NativeMethods.WindowExStyles.WS_EX_CLIENTEDGE | NativeMethods.WindowExStyles.WS_EX_STATICEDGE);
            lExStyle &= (uint)flagsToDisableEx;
            NativeMethods.SetWindowLong(handle, (int)NativeMethods.WindowLongParam.GWL_EXSTYLE, lExStyle);
        }


        public override bool Connected {
            get {
                return (IsPuttyReady && puttyProcess != null && !puttyProcess.HasExited);
            }
        }

        public bool FullScreen {
            get {
                throw new NotImplementedException();
            }

            set {
                if (value)
                    SendFocusToPutty();
            }
        }

        public string Server {
            get {
                throw new NotImplementedException();
            }
        }

        public string UserName {
            get {
                throw new NotImplementedException();
            }
        }

        public string Domain {
            get {
                throw new NotImplementedException();
            }
        }

        public bool ConnectToConsole {
            get {
                throw new NotImplementedException();
            }
        }

        public override bool Connect()
        {
            ((Form)this.ParentForm).ResizeEnd += PuttyConnection_ResizeEnd;
            var o = this.Favorite.ProtocolProperties;
            this.Dock = DockStyle.Fill;
            
            LaunchPutty();

            return true;
        }

        private void PuttyConnection_ResizeEnd(object sender, EventArgs e)
        {
            this.Invoke(new ThreadStart(this.ClipPutty));
        }

        internal string GetPuttyBinaryPath()
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), 
                "Resources", PUTTY_BINARY);
        }

        private void LaunchPutty()
        {
            puttyProcess = new Process();
            puttyProcess.StartInfo.FileName = GetPuttyBinaryPath();

            IGuardedSecurity credentials = this.ResolveFavoriteCredentials();
            puttyProcess.StartInfo.Arguments = new ArgumentsBuilder(credentials, this.Favorite).Build();
            puttyProcess.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;

            puttyProcess.Start();
            puttyProcess.WaitForInputIdle();

            AdjustWindowStyle(puttyProcess.MainWindowHandle);
            NativeMethods.SetParent(puttyProcess.MainWindowHandle, this.Handle);

            windowCaptured = true;

            ClipPutty();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (null != puttyProcess && !puttyProcess.HasExited)
                puttyProcess.Kill();
        }

        void IConnectionExtra.Focus()
        {
            SendFocusToPutty();
        }
    }
}
