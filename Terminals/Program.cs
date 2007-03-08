using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Text;
using System.Runtime.InteropServices;

namespace Terminals
{
    static class Program
    {
        //This is a global counter for the # of instances of the application that are currently active
        //when the last application is shutdown the semaphore will be releaed automatically
        static Semaphore instanceCounter;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool firstInstance;
            instanceCounter = new Semaphore(0, Int32.MaxValue, "Terminals", out firstInstance);
            try
            {
                if (!firstInstance && ReuseExistingInstance())
                {
                    //Find a windows of an existing instance
                    IntPtr handle = NativeApi.FindWindow(null, "Terminals");
                    //now send the parameters to the other instance
                    if (handle != IntPtr.Zero)
                    {
                        string args = String.Join(">", Environment.GetCommandLineArgs());
                        SendArgsToHandle(handle, args);
                        //exit
                        return;
                    }

                }
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            finally
            {
                instanceCounter.Release();
            }
        }

        private static void SendArgsToHandle(IntPtr handle, string args)
        {
            byte[] buffer = Encoding.Unicode.GetBytes(args);

            NativeApi.COPYDATASTRUCT data;
            data.lpData = NativeApi.PinObject(buffer);
            data.dwData = 0;
            data.cbData = buffer.Length;

            IntPtr dataPtr = NativeApi.PinObject(data);
            HandleRef r=new HandleRef(null, handle);

            NativeApi.SendMessage(r, NativeApi.WM_COPYDATA, IntPtr.Zero, dataPtr);
        }

        private static bool ReuseExistingInstance()
        {
            if (Settings.SingleInstance)
                return true;
            string[] cmdLineArgs = Environment.GetCommandLineArgs();
            return (cmdLineArgs.Length > 1 && cmdLineArgs[1] == "/reuse");
        }
    }
}