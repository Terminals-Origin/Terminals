using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace TerminalsUpdater {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }
        private void Update(object state) {
            Mutex mtx = new Mutex(false, "TerminalsMutex");
            bool isAppRunning = true;

            while(isAppRunning) {
                isAppRunning = !mtx.WaitOne(0, false);
            }
            //wait for the process to completely end
            System.Threading.Thread.Sleep(5000);

            string source = Program.Args[0];
            string destination = Program.Args[1];

            System.IO.DirectoryInfo dir = new DirectoryInfo(source);

            //System.Diagnostics.Debugger.Break();
            foreach(System.IO.FileInfo file in dir.GetFiles()) {
                string dest = System.IO.Path.Combine(destination, file.Name);
                System.IO.FileInfo fi = new FileInfo(dest);
                if(fi.CreationTime != file.CreationTime) file.CopyTo(dest, true);
            }

            System.Diagnostics.Process.Start(System.IO.Path.Combine(destination, "Terminals.exe"));
            //System.IO.Directory.Move(destination, destination + "_temp");
            //System.IO.Directory.Move(source, destination);        
            Application.Exit();
        }
        private void Form1_Load(object sender, EventArgs e) {
            System.Threading.ThreadPool.QueueUserWorkItem(new WaitCallback(Update), null);

        }

        private void button1_Click(object sender, EventArgs e) {
            Application.Exit();
        }
    }
}