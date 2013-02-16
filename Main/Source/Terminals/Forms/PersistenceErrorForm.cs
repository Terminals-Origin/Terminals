using System;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms
{
    internal partial class PersistenceErrorForm : Form
    {
        internal PersistenceErrorForm()
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
                Application.Exit();

            var form = new PersistenceErrorForm();
            form.detailLabel.Text = args.Message;
            form.ShowDialog();
            // continue means, that user wants to retry
        }

        private void ReTryButtonClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ExitButtonClick(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
