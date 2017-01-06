using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Terminals.Connections;
using Terminals.Data;
using Terminals.Native;

namespace Terminals.Plugins.Putty
{
    internal class PuttyConnection : Connection, IConnectionExtra
    {
        const string PUTTY_BINARY = "putty.exe";

        private bool windowCaptured = false;
        private Process puttyProcess;

        private bool IsPuttyReady
        {
            get
            {
                return windowCaptured && null != puttyProcess && !puttyProcess.HasExited;
            }
        }

        public override bool Connected
        {
            get
            {
                return (IsPuttyReady && puttyProcess != null && !puttyProcess.HasExited);
            }
        }

        public bool FullScreen
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                if (value)
                    SendFocusToPutty();
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
                this.Invoke(new ThreadStart(() =>
                {
                    Methods.SetForegroundWindow(puttyProcess.MainWindowHandle);
                    Methods.SetFocus(puttyProcess.MainWindowHandle);
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
                Rectangle windowRect = Methods.GetWindowRect(puttyProcess.MainWindowHandle);

                Rectangle clientRect = Methods.GetClientRect(puttyProcess.MainWindowHandle);

                Point referencePoint0 = new Point(0, 0);
                Methods.ClientToScreen(puttyProcess.MainWindowHandle, ref referencePoint0);

                Point referencePoint1 = new Point(clientRect.Width, clientRect.Height);
                Methods.ClientToScreen(puttyProcess.MainWindowHandle, ref referencePoint1);

                int top = (referencePoint0.Y - windowRect.Top);
                int left = (referencePoint0.X - windowRect.Left);

                int right = windowRect.Right - referencePoint1.X; // right contains the width of the scrool that should be shown
                int bottom = windowRect.Bottom - referencePoint1.Y;

                int width = this.Width + left + left; // + right ( using doubled left )
                int height = this.Height + top + bottom;

                Methods.SetWindowPos(puttyProcess.MainWindowHandle, IntPtr.Zero, -left, -top, width, height, SetWindowPosFlags.FrameChanged | SetWindowPosFlags.DoNotActivate);
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
            ((Form)this.ParentForm).ResizeEnd += PuttyConnection_ResizeEnd;
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
            Methods.SetParent(puttyProcess.MainWindowHandle, this.Handle);

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
