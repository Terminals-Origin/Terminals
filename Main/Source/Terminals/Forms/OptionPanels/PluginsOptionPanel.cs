using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Terminals.Connections;
using Terminals.Properties;
using Settings = Terminals.Configuration.Settings;

namespace Terminals.Forms.OptionPanels
{
    public partial class PluginsOptionPanel : UserControl, IOptionPanel
    {
        private readonly PluginsSelection pluginsSelection;

        public PluginsOptionPanel()
        {
            this.InitializeComponent();

            this.pluginsListbox.CheckOnClick = true;
            this.pluginsSelection = new PluginsSelection(Settings.Instance, new PluginsLoader());
        }

        public void LoadSettings()
        {
            foreach (SelectedPlugin plugin in this.pluginsSelection.LoadPlugins())
            {
                this.pluginsListbox.Items.Add(plugin, plugin.Enabled);
            }
        }

        public void SaveSettings()
        {
            List<SelectedPlugin> plugins = this.GetPluginsFromUI();
            this.pluginsSelection.SaveSelected(plugins);
        }

        private void UpdatePluginsFromUi()
        {
            for (int index = 0; index < this.pluginsListbox.Items.Count; index++)
            {
                var plugin = this.pluginsListbox.Items[index] as SelectedPlugin;
                plugin.Enabled = this.pluginsListbox.GetItemChecked(index);
            }
        }

        
        protected override void OnValidating(CancelEventArgs e)
        {
            base.OnValidating(e);
            e.Cancel = !this.GetPluginsFromUI()
                .Any(p => p.Enabled);

            string errorMessage = string.Empty;
            if (e.Cancel)
                errorMessage = Resources.PluginSelectionErrorMessage;

            this.errorProvider.SetError(this.pluginsListbox, errorMessage);
        }

        private List<SelectedPlugin> GetPluginsFromUI()
        {
            this.UpdatePluginsFromUi();
            return this.pluginsListbox.Items
                .OfType<SelectedPlugin>()
                .ToList();
        }
    }
}
