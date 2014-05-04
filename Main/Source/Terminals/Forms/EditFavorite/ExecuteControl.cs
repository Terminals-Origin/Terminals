using System.Windows.Forms;
using Terminals.Data;

namespace Terminals.Forms.EditFavorite
{
    public partial class ExecuteControl : UserControl
    {
        public ExecuteControl()
        {
            InitializeComponent();
        }

        private void FillFavoriteExecuteBeforeOptions(IFavorite favorite)
        {
            IBeforeConnectExecuteOptions exucutionOptions = favorite.ExecuteBeforeConnect;
            exucutionOptions.Execute = this.chkExecuteBeforeConnect.Checked;
            exucutionOptions.Command = this.txtCommand.Text;
            exucutionOptions.CommandArguments = this.txtArguments.Text;
            exucutionOptions.InitialDirectory = this.txtInitialDirectory.Text;
            exucutionOptions.WaitForExit = this.chkWaitForExit.Checked;
        }

        private void FillExecuteBeforeControls(IFavorite favorite)
        {
            this.chkExecuteBeforeConnect.Checked = favorite.ExecuteBeforeConnect.Execute;
            this.txtCommand.Text = favorite.ExecuteBeforeConnect.Command;
            this.txtArguments.Text = favorite.ExecuteBeforeConnect.CommandArguments;
            this.txtInitialDirectory.Text = favorite.ExecuteBeforeConnect.InitialDirectory;
            this.chkWaitForExit.Checked = favorite.ExecuteBeforeConnect.WaitForExit;
        }
    }
}
