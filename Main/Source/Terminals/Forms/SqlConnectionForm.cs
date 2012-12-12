using System.Data.SqlClient;
using System.Windows.Forms;

namespace Terminals.Forms
{
    /// <summary>
    /// Handles detailed possibility to configure all properties of MS SQL connection using property grid.
    /// Reused idea from Visual Studio Server Explorer.
    /// </summary>
    internal partial class SqlConnectionForm : Form
    {
        internal SqlConnectionStringBuilder DataSource
        {
            get
            {
                return this.pGridConnection.SelectedObject as SqlConnectionStringBuilder;
            }
            set
            {
                if (value == null)
                    this.pGridConnection.SelectedObject = new SqlConnectionStringBuilder();
                else
                    this.pGridConnection.SelectedObject = value;

                this.UpdateConnectionStringTextBox();
            }
        }

        internal SqlConnectionForm()
        {
            InitializeComponent();
        }

        private void PropertyGridConnectionPropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            this.UpdateConnectionStringTextBox();
        }

        private void UpdateConnectionStringTextBox()
        {
            if (this.DataSource != null)
            {
                this.txtCurrenConnectionString.Text = this.DataSource.ToString();
            }
        }
    }
}
