using System;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms
{
    internal partial class PersistenceErrorForm : Form
    {
        private PersistenceErrorForm()
        {
            InitializeComponent();
        }

        internal static void RegisterDataEventHandler(DataDispatcher dispatcher)
        {
            dispatcher.ErrorOccurred += new EventHandler<DataErrorEventArgs>(DataErrorOccured);
        }

        private static void DataErrorOccured(object sender, DataErrorEventArgs args)
        {
            if (args.CallStackFull)
                Environment.Exit(-1);

            var form = new PersistenceErrorForm();
            form.textBoxDetail.Text = args.Message;
            // continue means, that user wants to retry
            if (form.ShowDialog() == DialogResult.Cancel)
                Environment.Exit(-1);
        }
    }
}
