using System;
using System.Windows.Forms;
using Terminals.Configuration;

namespace Terminals.Forms
{
    internal partial class FavoritesOptionPanel : UserControl, IOptionPanel
    {
        public FavoritesOptionPanel()
        {
            InitializeComponent();
        }

        public void LoadSettings()
        {
            this.chkAutoCaseTags.Checked = Settings.AutoCaseTags;
            this.chkAutoExapandTagsPanel.Checked = Settings.AutoExapandTagsPanel;
            this.chkEnableFavoritesPanel.Checked = Settings.EnableFavoritesPanel;

            switch (Settings.DefaultSortProperty)
            {
                case SortProperties.ServerName:
                    this.ServerNameRadio.Checked = true;
                    break;
                case SortProperties.ConnectionName:
                    this.ConnectionNameRadioButton.Checked = true;
                    break;
                case SortProperties.Protocol:
                    this.ProtocolRadionButton.Checked = true;
                    break;
                case SortProperties.None:
                    this.NoneRadioButton.Checked = true;
                    break;
            }
        }

        public void SaveSettings()
        {
            Settings.AutoCaseTags = this.chkAutoCaseTags.Checked;
            Settings.AutoExapandTagsPanel = this.chkAutoExapandTagsPanel.Checked;
            Settings.EnableFavoritesPanel = this.chkEnableFavoritesPanel.Checked;

            if (this.ServerNameRadio.Checked)
                Settings.DefaultSortProperty = SortProperties.ServerName;
            else if (this.NoneRadioButton.Checked)
                Settings.DefaultSortProperty = SortProperties.None;
            else if (this.ConnectionNameRadioButton.Checked)
                Settings.DefaultSortProperty = SortProperties.ConnectionName;
            else
                Settings.DefaultSortProperty = SortProperties.Protocol;
        }
    }
}
