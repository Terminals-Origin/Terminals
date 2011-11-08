using System;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Terminals
{
    internal delegate void NewInstanceMessageEventHandler(object sender, object message);

    internal class SingleInstanceApplication
    {
        public static event NewInstanceMessageEventHandler NewInstanceMessage;

        private static SingleInstanceApplication _theInstance = new SingleInstanceApplication();

        public static bool AlreadyExists
        {
            get
            {
                return _theInstance.Exists;
            }
        }

        /// <summary>
        /// this is a uniqe id used to identify the application
        /// </summary>
        private string _id;

        /// <summary>
        /// This is a global counter for the # currently active of instances of the application.
        /// when the last application is shutdown the semaphore will be released automatically
        /// </summary>
        private Semaphore _instanceCounter;

        /// <summary>
        /// Is this the first instance?
        /// </summary>
        private bool _firstInstance;

        private SIANativeWindow _notifcationWindow;

        private bool Exists
        {
            get
            {
                return !_firstInstance;
            }
        }

        private SingleInstanceApplication()
        {
            _id = "SIA_" + GetAppId();
            _instanceCounter = new Semaphore(0, Int32.MaxValue, _id, out _firstInstance);
        }

        public static void Initialize()
        {
            _theInstance.Init();
        }
        
        private void Init()
        {
            _notifcationWindow = new SIANativeWindow();
        }

        public static void Close()
        {
            _theInstance.Dispose();
        }
        
        private void Dispose()
        {
            _instanceCounter.Close();
            if (_notifcationWindow != null)
                _notifcationWindow.DestroyHandle();
        }

        private static string GetAppId()
        {
            return Path.GetFileName(Environment.GetCommandLineArgs()[0]);
        }

        private void OnNewInstanceMessage(object message)
        {
            if (NewInstanceMessage != null)
                NewInstanceMessage(this, message);
        }
        
        public static bool NotifyExistingInstance()
        {
            return NotifyExistingInstance(null);
        }

        public static bool NotifyExistingInstance(object message)
        {
            if (_theInstance.Exists)
            {
                return _theInstance.NotifyPreviousInstance(message);
            }
            return false;
        }

        private bool NotifyPreviousInstance(object message)
        {
            Logging.Log.Info("NotifyPreviousInstance", null);
            IntPtr handle = NativeMethods.FindWindow(null, _id);
            if (handle != IntPtr.Zero)
            {
                GCHandle bufferHandle = new GCHandle();
                try
                {
                    NativeMethods.COPYDATASTRUCT data = new NativeMethods.COPYDATASTRUCT();
                    if (message != null)
                    {
                        byte[] buffer = Serialize(message);
                        bufferHandle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

                        data.dwData = IntPtr.Zero;
                        data.cbData = buffer.Length;
                        data.lpData = bufferHandle.AddrOfPinnedObject();
                    }

                    GCHandle dataHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
                    try
                    {
                        NativeMethods.SendMessage(handle, NativeMethods.WM_COPYDATA, IntPtr.Zero, dataHandle.AddrOfPinnedObject());
                        return true;
                    }
                    finally
                    {
                        dataHandle.Free();
                    }
                }
                finally
                {
                    if (bufferHandle.IsAllocated)
                        bufferHandle.Free();
                }
            }
            return false;
        }

        /// <summary>
        /// utility method for serialization\deserialization
        /// </summary>
        private static object Deserialize(ref byte[] buffer)
        {
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                return new BinaryFormatter().Deserialize(stream);
            }
        }

        private static byte[] Serialize(Object obj)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, obj);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// win32 translation of some needed APIs
        /// </summary>
        private class NativeMethods
        {
            [DllImport("user32.dll")]
            public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
            [DllImport("user32.dll")]
            public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

            public const short WM_COPYDATA = 74;

            public struct COPYDATASTRUCT
            {
                public IntPtr dwData;
                public int cbData;
                public IntPtr lpData;
            }
        }

        /// <summary>
        /// a utility window to communicate between application instances
        /// </summary>
        private class SIANativeWindow : NativeWindow
        {
            public SIANativeWindow()
            {
                CreateParams cp = new CreateParams();
                cp.Caption = _theInstance._id;
                CreateHandle(cp);
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == NativeMethods.WM_COPYDATA)
                {
                    NativeMethods.COPYDATASTRUCT data = (NativeMethods.COPYDATASTRUCT)Marshal.PtrToStructure(m.LParam, typeof(NativeMethods.COPYDATASTRUCT));
                    object obj = null;
                    if (data.cbData > 0 && data.lpData != IntPtr.Zero)
                    {
                        byte[] buffer = new byte[data.cbData];
                        Marshal.Copy(data.lpData, buffer, 0, buffer.Length);
                        obj = Deserialize(ref buffer);
                    }
                    _theInstance.OnNewInstanceMessage(obj);
                }
                else
                    base.WndProc(ref m);
            }
        }
    }
}
