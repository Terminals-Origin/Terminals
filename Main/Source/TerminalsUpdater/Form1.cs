using System;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace TerminalsUpdater
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Update(object state)
        {
            Mutex mtx = new Mutex(false, "TerminalsMutex");
            bool isAppRunning = true;

            while (isAppRunning)
            {
                isAppRunning = !mtx.WaitOne(0, false);
            }
            //wait for the process to completely end
            Thread.Sleep(5000);

            string source = Program.Args[0];
            string destination = Program.Args[1];

            DirectoryInfo dir = new DirectoryInfo(source);

            //System.Diagnostics.Debugger.Break();
            foreach (FileInfo file in dir.GetFiles())
            {
                string dest = Path.Combine(destination, file.Name);
                FileInfo fi = new FileInfo(dest);
                if (fi.CreationTime != file.CreationTime) file.CopyTo(dest, true);
            }

            System.Diagnostics.Process.Start(Path.Combine(destination, "Terminals.exe"));
            //System.IO.Directory.Move(destination, destination + "_temp");
            //System.IO.Directory.Move(source, destination);        
            Application.Exit();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(Update), null);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}