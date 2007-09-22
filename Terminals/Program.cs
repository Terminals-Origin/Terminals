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
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if(ReuseExistingInstance() && SingleInstanceApplication.NotifyExistingInstance(Environment.GetCommandLineArgs()))
                return;

            SingleInstanceApplication.Initialize();

            if(Settings.TerminalsPassword != "")
            {
                Security.RequestPassword rp = new Terminals.Security.RequestPassword();
                DialogResult result = rp.ShowDialog();
                if(result == DialogResult.Cancel)
                {
                    Application.Exit();
                }
            }
            Application.Run(new MainForm());
            SingleInstanceApplication.Close();

        }

        private static bool ReuseExistingInstance()
        {
            if(Settings.SingleInstance)
                return true;
            string[] cmdLineArgs = Environment.GetCommandLineArgs();
            return (cmdLineArgs.Length > 1 && cmdLineArgs[1] == "/reuse");
        }
    }
}