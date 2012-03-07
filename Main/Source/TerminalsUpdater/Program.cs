using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TerminalsUpdater {
    static class Program {

        public static string[] Args;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Args = args;

            Application.Run(new Form1());
        }
    }
}