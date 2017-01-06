using System;
using System.Windows.Forms;
using Terminals.Configuration;

namespace Terminals.Forms
{
    internal partial class FavoritesOptionPanel : UserControl, IOptionPanel
    {
        private readonly Settings settings = Settings.Instance;

        public FavoritesOptionPanel()
        {
            InitializeComponent();
        }

        public void LoadSettings()
        {
            this.chkAutoCaseTags.Checked = settings.AutoCaseTags;
            this.chkAutoExapandTagsPanel.Checked = settings.AutoExapandTagsPanel;
            this.chkEnableFavoritesPanel.Checked = settings.EnableFavoritesPanel;

            switch (settings.DefaultSortProperty)
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
            settings.AutoCaseTags = this.chkAutoCaseTags.Checked;
            settings.AutoExapandTagsPanel = this.chkAutoExapandTagsPanel.Checked;
            settings.EnableFavoritesPanel = this.chkEnableFavoritesPanel.Checked;

            if (this.ServerNameRadio.Checked)
                settings.DefaultSortProperty = SortProperties.ServerName;
            else if (this.NoneRadioButton.Checked)
                settings.DefaultSortProperty = SortProperties.None;
            else if (this.ConnectionNameRadioButton.Checked)
                settings.DefaultSortProperty = SortProperties.ConnectionName;
            else
                settings.DefaultSortProperty = SortProperties.Protocol;
        }
    }
}
