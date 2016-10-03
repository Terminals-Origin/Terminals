using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Terminals.Configuration;
using Terminals.Connections;

namespace Terminals.Forms.OptionPanels
{
    public partial class PluginsOptionPanel : UserControl, IOptionPanel
    {
        private readonly PluginsSelection pluginsSelection;

        public PluginsOptionPanel()
        {
            InitializeComponent();

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
            this.UpdatePluginsFromUi();
            List<SelectedPlugin> plugins = this.pluginsListbox.Items
                .OfType<SelectedPlugin>()
                .ToList();
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
    }
}
