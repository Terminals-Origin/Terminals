using System;
using System.Linq;
using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Credentials
{
    internal partial class CredentialManager : Form
    {
        private static ICredentials Credentials
        {
            get { return Persistence.Instance.Credentials; }
        }

        internal CredentialManager()
        {
            InitializeComponent();
            Credentials.CredentialsChanged += new EventHandler(this.CredentialsChanged);
        }

        private void CredentialsChanged(object sender, EventArgs e)
        {
            BindList();
        }

        private void BindList()
        {
            this.gridCredentials.AutoGenerateColumns = false;
            this.gridCredentials.DataSource = new SortableList<ICredentialSet>(Credentials);
        }

        private void DoneButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CredentialManager_Load(object sender, EventArgs e)
        {
            BindList();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            EditCredential(null);
        }

        private ICredentialSet GetSelectedItemCredentials()
        {
            if (gridCredentials.SelectedRows.Count > 0)
                return gridCredentials.SelectedRows[0].DataBoundItem as ICredentialSet;
            return null;
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            this.EditSelectedCredential();
        }

        private void EditSelectedCredential()
        {
            ICredentialSet selected = this.GetSelectedItemCredentials();
            if (selected != null)
            {
                this.EditCredential(selected);
            }
        }

        private void EditCredential(ICredentialSet selected)
        {
            ManageCredentialForm mgr = new ManageCredentialForm(selected);
            if (mgr.ShowDialog() == DialogResult.OK)
                this.BindList();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            ICredentialSet toRemove = GetSelectedItemCredentials();
            if (toRemove != null)
            {
                if (MessageBox.Show("Are you sure you want to delete credential " + toRemove.Name + "?",
                                    "Credential manager", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Credentials.Remove(toRemove);
                    Credentials.Save();
                    BindList();
                }
            }
        }

        private void CredentialManager_FormClosed(object sender, FormClosedEventArgs e)
        {
            Credentials.CredentialsChanged -= CredentialsChanged;
        }

        private void gridCredentials_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridViewColumn lastSortedColumn = this.gridCredentials.FindLastSortedColumn();
            DataGridViewColumn column = this.gridCredentials.Columns[e.ColumnIndex];

            SortOrder newSortDirection = SortableUnboundGrid.GetNewSortDirection(lastSortedColumn, column);
            var data = this.gridCredentials.DataSource as SortableList<ICredentialSet>;
            this.gridCredentials.DataSource = data.SortByProperty(column.DataPropertyName, newSortDirection);
            column.HeaderCell.SortGlyphDirection = newSortDirection;
        }

        private void gridCredentials_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // dont allow double click on column row
                EditSelectedCredential();
        }
    }
}
